#!/usr/bin/env python

import numpy as np
import pandas as pd
from tqdm import tqdm
import time

import tickers
import pricedata


def main():

	tickers.screener()

# Create file to write results to
	with open("week-30-Scan.txt", "a") as r:

		with open ("tickers.txt") as f:
			tickerdata = f.readlines()
			for i in tqdm(tickerdata):
				ticker = (i.strip()) 
				pricedata.price_data(ticker)
				time.sleep(1)
					
				# Read in the data
				data = pd.read_csv("price-data.csv")
				
				# Clean up the data removing Nan and the last price field that is no relavaint
				data = data[:-1] 
				data = data.dropna()
				data = data.reset_index(drop=True)
				
				# Store in an array to process the data
				o = data["Open"]
				h = data["High"]
				l = data["Low"]
				c = data["Close"]
				
				# Get the latest closing price
				latestClose = c.iloc[-1]
				
				# Bullish Price Flip = When the close is less than the close 4 bars ealier, immediately
				# followed by by a close greater than the close 4 bars ago
				
				# Sell Setup Phase
				sSetupCount = 0
				sSetupLow = []
				
				for i in range(5, len(c)-1):
				
					bullishPF = c[i-1] < c[i-5] and c[i] > c[i-4]

					if sSetupCount < 9:
						if sSetupCount >= 1 and not c[i] > c[i-4]:
							sSetupCount = 0
							sSetupLow.clear()
				
						if sSetupCount == 0 and bullishPF: 
							sSetupCount += 1
							sSetupLow.append(l[i])
				
						elif sSetupCount >= 1:
							if c[i] > c[i-4]:
								sSetupLow.append(l[i])
								sSetupCount += 1
				
						if sSetupCount == 9:
							TDSTs = (min(sSetupLow))
							sSetupCount = 0
							sSetupLow.clear()
							#r.write("TDST Support level is %s\n" % (TDSTs))
				
				#if sSetupCount >= 1:
				#	print(f"On Bar {sSetupCount}, of Sell Count")	
				
				# Setup Phase
				bSetupCount = 0
				bSetupHigh = []
				
				for i in range(5, len(c)-1):
				
					bearishPF = c[i-1] > c[i-5] and c[i] < c[i-4]

					if bSetupCount < 9:
						if bSetupCount >= 1 and not c[i] < c[i-4]:
							bSetupCount = 0
							bSetupHigh.clear()
				
						if bSetupCount == 0 and bearishPF: 
							bSetupCount += 1
							bSetupHigh.append(h[i])
				
						elif bSetupCount >= 1:
							if c[i] < c[i-4]:
								bSetupHigh.append(h[i])
								bSetupCount += 1
				
						if bSetupCount == 9:
							bSetup = True
							TDSTr = (max(bSetupHigh))
							bSetupCount = 0
							bSetupHigh.clear()
				
				#Trade potential filter
							if bSetup and latestClose < TDSTr and sSetupCount >= 1:
								r.write(ticker + "\n")
								r.write("On Bar %s of Sell Count\n" % (sSetupCount))
								r.write("The latest close is %.2f\n" % (latestClose))
								r.write("TDST Resistence level is %.2f\n\n" % (TDSTr))	
				
				#if bSetupCount >= 1:
				#	r.write("On Bar %s, of Buy Count\n" % (bSetupCount))
					

if __name__ == "__main__":
	main()
