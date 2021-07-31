#!/usr/bin/env python

import numpy as np
import pandas as pd
from finvizfinance.screener.ticker import Ticker


COUNTRY = 'USA'
EXCHANGES = ["NYSE", "NASDAQ"] # list of exchanges to use
MARKETCAPS = ['Mega ($200bln and more)', 'Large ($10bln to $200bln)', 'Mid ($2bln to $10bln)']

data = []

def screener():
	tickers = Ticker()
	for exchange in EXCHANGES:
		for marketcap in MARKETCAPS:
			filters_dict = {'Exchange' : exchange, 'Country' : COUNTRY, 'Market Cap.' : marketcap}	
			tickers.set_filter(filters_dict=filters_dict)
			df = tickers.ScreenerView()
			data.extend(df)	
			with open ("tickers.txt", "w") as f:
				for ticker in data:
					f.write("%s\n" % ticker)	

def main():
	
	screener()

if __name__ == "__main__":
	main()
