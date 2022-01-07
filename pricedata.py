#!/usr/bin/env python

import yfinance as yf
import pandas as pd
import requests_cache


PERIOD = "1y"
INTERVAL = "1wk"

def price_data(ticker):
		symbol = yf.Ticker(ticker)
		data = symbol.history(period=PERIOD, interval=INTERVAL)	
		data = data[['Open', 'High', 'Low', 'Close']]
		data.to_csv(f'price-data.csv')

def main():

	price_data()

if __name__ == "__main__":
	main()
