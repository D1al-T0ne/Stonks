#!/usr/bin/env python

import yfinance as yf
import pandas as pd
import requests_cache


PERIOD = "1y"
INTERVAL = "1wk"

def price_data(ticker):
		session = requests_cache.CachedSession('yfinance.cache')
		session.headers['User-Agent'] = 'Mozilla/5.0 (Windows NT 6.1; rv:40.0) Gecko/20100101 Firefox/40.0'
		symbol = yf.Ticker(ticker)
		data = symbol.history(period=PERIOD, interval=INTERVAL)	
		data = data[['Open', 'High', 'Low', 'Close']]
		data.to_csv(f'price-data.csv')

def main():

	price_data()

if __name__ == "__main__":
	main()
