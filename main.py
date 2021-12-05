#!/usr/bin/env python

import numpy as np
import pandas as pd
import mplfinance as mpf
import yfinance as yf

yticker = yf.Ticker("B")
data = yticker.history(period="1y",interval="1wk")
	
				
# Clean up the data removing Nan and the last price field that is no relavaint
data = data[:-1] 
data = data.dropna()
#data = data.reset_index(drop=True)

# Store in an array to process the data
o = data["Open"]
h = data["High"]
l = data["Low"]
c = data["Close"]

# Get the latest closing price
latest_close = c.iloc[-1]

buy_setup = 0
sell_setup = 0
buy_setup_first_bar = 0
sell_setup_first_bar = 0
completed_buys = []
completed_sells = []
buy_setup_high = []
sell_setup_low = []
buy_setup_values = {}
sell_setup_values = {}
TDST_resistance = []
TDST_support = []

#Buy Setup
for i in range(5, len(c)-1):

    bearish_price_flip = c[i-1] > c[i-5] and c[i] < c[i-4]

    #Confirm Price Flip and first Bar for Buy Setup
    if c[i] < c[i-4] and bearish_price_flip:
        buy_setup += 1
        buy_setup_first_bar = i
        buy_setup_high.append(h[i])
    
    #Buy Setup Bar 2 through to 9
    elif c[i] < c[i-4] and buy_setup > 0 and buy_setup < 9:
        buy_setup += 1
        buy_setup_high.append(h[i]) 

        if buy_setup == 9:
            TDST_resistance = (max(buy_setup_high))
            sell_setup = 0
            print("We had a Buy Setup")
            print(f"TDST Resistance is {TDST_resistance}")
            print(f"The First Bar was {buy_setup_first_bar}")

    #Cancel Buy Setup
    elif c[i] < c[i-4] and buy_setup != 9 and buy_setup != 0:
        buy_setup = 0
        buy_setup_first_bar = 0
        buy_setup_high.clear()
        

#Sell Setup
for i in range(5, len(c)-1):

    bullish_price_flip = c[i-1] < c[i-5] and c[i] > c[i-4]

    #Confirm Price Flip and first Bar for Sell Setup
    if c[i] > c[i-4] and bullish_price_flip: 
        sell_setup += 1
        sell_setup_first_bar = i
        sell_setup_low.append(l[i])
    
    #Sell Setup Bar 2 through to 9
    elif c[i] > c[i-4] and sell_setup > 0 and sell_setup < 9:
        sell_setup += 1
        sell_setup_low.append(l[i]) 

        if sell_setup == 9:
            TDST_support = (min(sell_setup_low))
            sell_setup = 0
            print("We had a Sell Setup")
            print(f"TDST Support is {TDST_support}")
            print(f"The First Bar was {sell_setup_first_bar}")

    #Cancel Sell Setup
    elif c[i] > c[i-4] and buy_setup != 9 and buy_setup != 0:
        sell_setup = 0
        sell_setup_first_bar = 0
        sell_setup_low.clear()


#Trade Filter
if buy_setup and latest_close < TDST_resistance and sell_setup >= 1:
    print(f"On Bar {sell_setup}")
    print(f"The latest close is  {latest_close}")
    print(f"TDST Resistance level is {TDST_resistance}")


#Charting
mpf.plot(data,hlines=dict(hlines=[TDST_resistance,TDST_support],colors=["r","g"],linestyle="-."))
