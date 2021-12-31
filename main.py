#!/usr/bin/env python

import numpy as np
import pandas as pd
import mplfinance as mpf
import yfinance as yf
from tqdm import tqdm
import time
from datetime import date

import tickers
import pricedata

def scan_number():
    weekk_number = (date.today().isocalendar()[1])
    return(f"week-{weekk_number}-scan.txt")

def ticker_handler():
    with open ("tickers.txt") as f:
        tickerdata = f.readlines()
        return tickerdata

def main():

    tickers.screener()

    #tickers = ["MSFT", "GIS", "TSLA","HI", "EBS", "PPG", "LMT", "FICO", "HPE", "D", "CLX", "CL"]
    for i in tqdm(ticker_handler()):
        ticker = (i.strip())
        pricedata.price_data(ticker)
        time.sleep(0.50)

        #yticker = yf.Ticker(ticker) 
        #data = yticker.history(period="1y",interval="1wk")
        data = pd.read_csv("price-data.csv")

        # Clean up the data removing Nan and the last price field that is no relavaint
        data = data[:-1] 
        data = data.dropna()
        data = data.round(2)
        #data.index.name = "Date"
        data = data.reset_index(drop=True)

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
        buy_setup_complete = False
        sell_setup_complete = False
        buy_setup_high = []
        sell_setup_low = []
        TDST_resistance = []
        TDST_support = []

        #Buy Setup
        for i in range(5, len(c)-1):

            bearish_price_flip = c[i-1] > c[i-5] and c[i] < c[i-4]

            #Confirm Price Flip and first bar for Buy Setup
            if buy_setup < 9:
                if c[i] < c[i-4] and bearish_price_flip:
                    buy_setup += 1
                    buy_setup_first_bar = i
                    buy_setup_high.append(h[i])

                #Buy Setup Bar 2 through to 9
                elif c[i] < c[i-4] and buy_setup >= 1:
                    buy_setup += 1
                    buy_setup_high.append(h[i])

                    if buy_setup == 9:
                        TDST_resistance = (max(buy_setup_high))
                        buy_setup = 0
                        buy_setup_complete = True
                        completed_buy_first_bar = buy_setup_first_bar
                        buy_setup_first_bar = 0

                elif buy_setup >= 1 and not c[i] < c[i-4]:
                    buy_setup = 0
                    buy_setup_first_bar = 0
                    buy_setup_high.clear()

        #Sell Sellup
        for i in range(5, len(c)-1):

            bullish_price_flip = c[i-1] < c[i-5] and c[i] > c[i-4]

            #Confirm Price Flip and first bar for Sell Setup
            if sell_setup < 9:
                if c[i] > c[i-4] and bullish_price_flip:
                 sell_setup += 1
                 sell_setup_first_bar = i
                 sell_setup_low.append(l[i])

                #Sell Setup Bar 2 through to 9
                elif c[i] > c[i-4] and sell_setup >= 1:
                    sell_setup += 1
                    sell_setup_low.append(l[i])

                    if sell_setup == 9:
                        TDST_support = (min(sell_setup_low))
                        sell_setup = 0
                        sell_setup_complete = True
                        completed_sell_first_bar = sell_setup_first_bar
                        sell_setup_first_bar = 0

                elif sell_setup >= 1 and not c[i] > c[i-4]:
                    sell_setup = 0
                    sell_setup_first_bar = 0
                    sell_setup_low.clear()

        #Filters
        pre_trades = {}
        potential_trades = {}

        #Pre-Trade Filter
        if buy_setup_complete and latest_close < TDST_resistance and sell_setup >= 1:
            ticker_symbol = {"Name": ticker, "TDST": TDST_resistance, "Sell": sell_setup, "Close": latest_close}
            pre_trades[ticker] = ticker_symbol

        #Trade Filter
        if buy_setup_complete and latest_close > TDST_resistance and sell_setup >= 1:
            ticker_symbol = {"Name": ticker, "TDST": TDST_resistance, "Sell": sell_setup, "Close": latest_close}
            potential_trades[ticker] = ticker_symbol

        with open("test-1.txt", "a") as f:
            for k in pre_trades.items():
                f.write(str(k) + "\n")
            for k in potential_trades.items():
                f.write(str(k) + "\n")

if __name__ == "__main__":
    main()
