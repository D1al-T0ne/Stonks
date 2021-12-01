#region Using declarations
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using NinjaTrader;
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.Gui.Chart;
#endregion

// This namespace holds all indicators and is required. Do not change it.
namespace NinjaTrader.Indicator
{
	/// <summary>
	/// Countdown
	/// </summary>
	[Description("Countdown")]
	public class IGTDSequential2 : Indicator
	{
		#region Variables
		// Wizard generated variables
		
		// User defined variables (add any user defined variables below)
		
		private bool _boolBearishTDPriceFlipOccurred = false;
		private bool _boolBullishTDPriceFlipOccurred = false;
		private bool _boolIsComboCountdownBuyInProgress = false; 
		private bool _boolIsComboCountdownSellInProgress = false; 
		private bool _boolIsSequentialCountdownBuyInProgress = false; 
		private bool _boolIsSequentialCountdownSellInProgress = false; 
		private bool _boolDrawPerfectSignalArrow = true;
		private bool _boolOR = false;
		private bool _boolPlotComboCountdownAfterMinBars = false; // todo
		private bool _boolPlotComboCountdownBuy = false;
		private bool _boolPlotComboCountdownSell = false;
		private bool _boolPlotSequentialCountdownAfterMinBars = false; // todo
		private bool _boolPlotSequentialCountdownBuy = true;
		private bool _boolPlotSequentialCountdownSell = true;
		private bool _boolPlotSetupCountDownBuy = true;
		private bool _boolPlotSetupCountDownSell = true;
		private bool _boolPlotSetupCountDownAfterMinBars = true;
		private bool _boolPlotTDST = true;
		private bool _boolQ1 = false;
		private bool _boolSetupCancelHaHC_LbLC = false; // New Market Timing Techniques (page 50)
		private bool _boolSetupCancelHaHH_LbLL = false; // New Market Timing Techniques (page 50)
		private bool _boolSetupCancelCaHC_CbLC = false; // New Market Timing Techniques (page 50)
		private bool _boolSetupCancelCaHH_CbLL = false; // New Market Timing Techniques (page 50)
		private bool _boolSetupCancelCaHTH_CbLTL = false; // New Market Timing Techniques (page 50)
		
		private bool _boolSetupQ1ORBuy = true;
		private bool _boolSetupQ1ORSell = true;
		
		private bool _boolSequentialBar8VersusBar5Qualifier = true;
		private bool _boolSequentialBar13VersusBar8Qualifier = true;
		private bool _boolSequentialCancelReverseSetup = true; 
		private bool _boolSequentialCancelTDST = true; 
		private bool _boolWriteLOGOn = false; // todo
		
		private Color _colorComboBackgroundColorBar1 = Color.Yellow;
		private Color _colorComboBackgroundColorBar13 = Color.Yellow;
		private Color _colorComboBackgroundColorBarRecycle = Color.Yellow;
		private Color _colorComboFontColor = Color.Black;
		private Color _colorComboFontColorBar1 = Color.Black;
		private Color _colorComboFontColorBar13 = Color.Black;
		private Color _colorComboFontColorBarRecycle = Color.Black;
		
		private Color _colorPerfectSignalBuyColor = Color.Green;
		private Color _colorPerfectSignalSellColor = Color.Red;
		
		private Color _colorSequentialBackgroundColorBar1 = Color.Yellow;
		private Color _colorSequentialBackgroundColorBar13 = Color.Yellow;
		private Color _colorSequentialBackgroundColorBarRecycle = Color.Yellow;
		private Color _colorSequentialFontColor = Color.Blue;
		private Color _colorSequentialFontColorBar1 = Color.Blue; 
		private Color _colorSequentialFontColorBar13 = Color.Blue; 
		private Color _colorSequentialFontColorBarRecycle = Color.Blue;
		
		private Color _colorSetupBackgroundColorBar1 = Color.Yellow; 
		private Color _colorSetupBackgroundColorBar9 = Color.Yellow; 
		private Color _colorSetupFontColor = Color.Green;
		private Color _colorSetupFontColorBar1 = Color.Green; 
		private Color _colorSetupFontColorBar9 = Color.Green; 
		private Color _colorSetupTDSTResistance = Color.Red;
		private Color _colorSetupTDSTSupport = Color.Green;
		
		private DashStyle _dsTDST = DashStyle.Dash;
		
		private DataSeries _dsTrueHigh;
		private DataSeries _dsTrueLow;
		private DataSeries _dsYHigh;
		private DataSeries _dsYLow;
		private DataSeries _dsYPixelOffsetHigh;
		private DataSeries _dsYPixelOffsetLow;
		
		private double _doublePointOffset = 0.0; 
		private double _doubleSetupCountdownBar1High = 0.0; 
		private double _doubleTrueHigh = 0.0; 
		private double _doubleTrueLow = 0.0; 
		private double _doubleSetupCountdownBar1Low = 0.0; 
		private double _doubleYLocation = 0.0;
		private double _doubleTerminationPrice = 0.0;
		
		private int _intArrowCount = 0;
		
		private int _intComboBars = 13; 
		private int _intComboFontSize = 8;
		private int _intComboFontSizeBar1 = 10;
		private int _intComboFontSizeBar13 = 10;
		private int _intComboFontSizeBarRecycle = 13;
		private int _intComboLookBackBars = 2; 
		private int _intComboRecycleBars = 22; 
		private int _intComboRecycleCount = 0; 

		private int _intID = 0;
		private int _intIndex = 0;
		private int _intPixelOffset = 13; 
		private int _intObjectCount = 0;
		
		private int _intSequentialBars = 13; 
		private int _intSequentialFontSize = 8;
		private int _intSequentialFontSizeBar1 = 10;
		private int _intSequentialFontSizeBar13 = 10;
		private int _intSequentialFontSizeBarRecycle = 13;
		private int _intSequentialLookBackBars = 2; 
		private int _intSequentialRecycleBars = 22;

		private int _intSetupBars = 9; // Default setting for SetupBars
		private int _intSetupCount = 0;
		private int _intSetupFontSize = 8;
		private int _intSetupFontSizeBar1 = 10;
		private int _intSetupFontSizeBar9 = 10;
		private int _intSetupLookBackBars = 4; // Default setting for SetupLookBackBars
		private int _intSetupRecycleCount = 0;
		private int _intSetupTDSTMaxNumber = 0;
		private int _intSetupTDSTWidth = 0;
		private int _intSetupWidth = 2;
		
		private int _intYPixelOffset = 0;

		private List<TDSetupHelper2> _alTDSetupHelper;
		
		private List<TDSTHelper2> _alTDSTHelperBuy2; 
		private List<TDSTHelper2> _alTDSTHelperSell2; 
		
		private static int _intIDtoFind = 0;
		private string _strPerfectSignalText = "Perfect Signal";
		
		private TDSetupHelper2 _TDSetupHelper2ToRecycle = new TDSetupHelper2(-1, TDType2.Null, false);
		
		private TDSequentialPlotType2 _TDSequentialPlotType2 = TDSequentialPlotType2.DrawText;
		
		private TDTerminationCount2 _TDTerminationCount2 = TDTerminationCount2.Close;
	
		private TDType2 _tdType2Previous = TDType2.Null;
		#endregion

		/// <summary>
		/// This method is used to configure the indicator and is called once before any bar data is loaded.
		/// </summary>
		protected override void Initialize()
		{
			this.CalculateOnBarClose = true;
			this.Overlay = true;
			this.PriceTypeSupported	= false;
			
			this._alTDSetupHelper = new List<TDSetupHelper2>();
			this._alTDSTHelperBuy2 = new List<TDSTHelper2>();
			this._alTDSTHelperSell2 = new List<TDSTHelper2>();
			this._dsTrueHigh = new DataSeries(this);
			this._dsTrueLow = new DataSeries(this);
			this._dsYHigh = new DataSeries(this);
			this._dsYLow = new DataSeries(this);
			this._dsYPixelOffsetHigh = new DataSeries(this);
			this._dsYPixelOffsetLow = new DataSeries(this);
		}

		/// <summary>
		/// Called on each bar update event (incoming tick).
		/// </summary>
		protected override void OnBarUpdate()
		{
			if (CurrentBar < this.SetupBars*2) return;
			
			this.HouseKeeping();
			
			foreach (TDSetupHelper2 tdsh in this._alTDSetupHelper.ToArray()) // iterate through copy of the array
			{
				if (tdsh.Cancelled == false) this.Process(tdsh);	
			}
			
			foreach (TDSetupHelper2 tdsh in this._alTDSetupHelper.ToArray()) // iterate through copy of the array
			{
				if (tdsh.Tags != null) this.Cleanup(tdsh);	
			}
			
			// TDST
			if (this.PlotTDST) this.ProcessTDST();
		}
		
//		#region Private Methods
		
		#region HouseKeeping
		private void HouseKeeping()
		{
			//if (this._doublePointOffset == 0) this._doublePointOffset = SMA(Range(), this.SetupBars)[0] * 0.21;
			this._doublePointOffset = SMA(Range(), this.SetupBars)[0] * 0.21;
			if (this._doublePointOffset < 0.002 && this.Instrument.MasterInstrument.InstrumentType == InstrumentType.Currency) this._doublePointOffset = 0.002;
			if (this._doublePointOffset < 0.34 && this.Instrument.MasterInstrument.InstrumentType == InstrumentType.Future) this._doublePointOffset = 0.34;
			if (this._doublePointOffset < 0.34 && this.Instrument.MasterInstrument.InstrumentType == InstrumentType.Stock) this._doublePointOffset = 0.34;
			this._boolBearishTDPriceFlipOccurred = this.IsBearishFlip;
			this._boolBullishTDPriceFlipOccurred = this.IsBullishFlip;
			this._boolIsComboCountdownBuyInProgress = false;
			this._boolIsComboCountdownSellInProgress = false;
			this._boolIsSequentialCountdownBuyInProgress = false;
			this._boolIsSequentialCountdownSellInProgress = false;

			if (this._boolBearishTDPriceFlipOccurred)
			{
				this._intID++;
				this._alTDSetupHelper.Add(new TDSetupHelper2(this._intID, TDType2.Buy, this._boolPlotSetupCountDownAfterMinBars));
			}
			else if (this._boolBullishTDPriceFlipOccurred)
			{
				this._intID++;
				this._alTDSetupHelper.Add(new TDSetupHelper2(this._intID, TDType2.Sell, this._boolPlotSetupCountDownAfterMinBars));
			}
			
			if (this.CurrentBar == 0)
			{
				this._dsTrueHigh.Set(0);
				this._dsTrueLow.Set(0);
			}
			else
			{
				this._dsTrueHigh.Set(this.High[0] > this.Close[1] ? this.High[0] : this.Close[1]);
				this._dsTrueLow.Set(this.Low[0] < this.Close[1] ? this.Low[0] : this.Close[1]);
			}
			
			this._dsYHigh.Set(this.High[0]);
			this._dsYLow.Set(this.Low[0]);
			this._dsYPixelOffsetHigh.Set(0);
			this._dsYPixelOffsetLow.Set(0);
		}
		#endregion
		
		#region Cleanup
		private void Cleanup(TDSetupHelper2 tdsh)
		{
			if (tdsh.Cancelled ||
				(tdsh.Completed && tdsh.SearchForPerfectSignal == false && tdsh.PlotSetupCountDownAfterMinBars == false && 
				(tdsh.SequentialCountdownCancelled || tdsh.SequentialCountdownCompleted) && (tdsh.ComboCountdownCancelled || tdsh.ComboCountdownCompleted)))
			{
				Int32 int1 = this._alTDSetupHelper.IndexOf(tdsh);
				if (tdsh.Cancelled)
				{
					for (int int2 = 0; int2 < tdsh.Tags.Length; int2++)
					{
						if (tdsh.Tags[int2] != null)
						{
							RemoveDrawObject(tdsh.Tags[int2]); // remove td setup from chart
							if (tdsh.Type == this._tdType2Previous) this._intSetupRecycleCount--;
						}
						else
							break;
					}
					this._alTDSetupHelper.RemoveAt(int1);
				}
				else
				{
					TDSetupHelper2 tdsh2 = this._alTDSetupHelper[int1];
					tdsh2.Tags = null;
					this._alTDSetupHelper[int1] = tdsh2;
				}
			}
		}
		#endregion
		
		#region Process
		private void Process(TDSetupHelper2 tdsh)
		{
			_intIDtoFind = tdsh.ID;
			this._intIndex = this._alTDSetupHelper.FindIndex(FindByID);

			if (tdsh.Type == TDType2.Buy)
			{
				this._doubleYLocation = Low[0];
			}
			else if (tdsh.Type == TDType2.Sell)
			{
				this._doubleYLocation = High[0];
			}

			// td setup cancellation rules
			if (tdsh.InProgress)
			{
				tdsh.Cancelled = (tdsh.Type == TDType2.Buy && this._boolBullishTDPriceFlipOccurred) || // bullish td price flip (page 4)
					(tdsh.Type == TDType2.Sell && this._boolBearishTDPriceFlipOccurred); // bearish td price flip (page 2)
			
				tdsh.Cancelled = tdsh.Cancelled || 
					(tdsh.Type == TDType2.Buy && this.Close[0] >= this.Close[this.SetupLookBackBars]) || // td buy setup failed (page 3)
					(tdsh.Type == TDType2.Sell && this.Close[0] <= Close[this.SetupLookBackBars]); // td sell setup failed (page 5)
				
				if (this.CancelHaHC_LbLC && tdsh.Count > 1) // New Market Timing Techniques (page 50)
				{
					tdsh.Cancelled = tdsh.Cancelled || 
					(tdsh.Type == TDType2.Buy && this.High[0] > MAX(this.Close, tdsh.Count)[0]) || 
					(tdsh.Type == TDType2.Sell && this.Low[0] < MIN(this.Close, tdsh.Count)[0]);
				}
				
				if (this.CancelHaHH_LbLL && tdsh.Count > 1) // New Market Timing Techniques (page 50)
				{
					tdsh.Cancelled = tdsh.Cancelled || 
					(tdsh.Type == TDType2.Buy && this.High[0] > MAX(this.High, tdsh.Count)[0]) || 
					(tdsh.Type == TDType2.Sell && this.Low[0] < MIN(this.Low, tdsh.Count)[0]);
				}
				
				if (this.CancelCaHC_CbLC && tdsh.Count > 1) // New Market Timing Techniques (page 50)
				{
					tdsh.Cancelled = tdsh.Cancelled || 
					(tdsh.Type == TDType2.Buy && this.Close[0] > MAX(this.Close, tdsh.Count)[0]) || 
					(tdsh.Type == TDType2.Sell && this.Close[0] < MIN(this.Close, tdsh.Count)[0]);
				}
				
				if (this.CancelCaHH_CbLL && tdsh.Count > 1) // New Market Timing Techniques (page 50)
				{
					tdsh.Cancelled = tdsh.Cancelled || 
					(tdsh.Type == TDType2.Buy && this.Close[0] > MAX(this.High, tdsh.Count)[0]) || 
					(tdsh.Type == TDType2.Sell && this.Close[0] < MIN(this.Low, tdsh.Count)[0]);
				}
				
				if (this.CancelCaHTH_CbLTL && tdsh.Count > 1) // New Market Timing Techniques (page 50)
				{
					tdsh.Cancelled = tdsh.Cancelled || 
					(tdsh.Type == TDType2.Buy && this.Close[0] > MAX(this._dsTrueHigh, tdsh.Count)[0]) || 
					(tdsh.Type == TDType2.Sell && this.Close[0] < MIN(this._dsTrueLow, tdsh.Count)[0]);
				}
			}
			else if (tdsh.Completed && tdsh.PlotSetupCountDownAfterMinBars)
			{
				tdsh.PlotSetupCountDownAfterMinBars = (tdsh.Type == TDType2.Buy && this._boolBullishTDPriceFlipOccurred) || // bullish td price flip (page 4)
					(tdsh.Type == TDType2.Sell && this._boolBearishTDPriceFlipOccurred); // bearish td price flip (page 2)
			
				tdsh.PlotSetupCountDownAfterMinBars = tdsh.PlotSetupCountDownAfterMinBars || 
					(tdsh.Type == TDType2.Buy && this.Close[0] >= this.Close[this.SetupLookBackBars]) || // td buy setup failed (page 3)
					(tdsh.Type == TDType2.Sell && this.Close[0] <= Close[this.SetupLookBackBars]); // td sell setup failed (page 5)
				
				tdsh.PlotSetupCountDownAfterMinBars = !tdsh.PlotSetupCountDownAfterMinBars;
			}
			
			if (tdsh.Cancelled == false && (tdsh.InProgress || (tdsh.Completed && tdsh.PlotSetupCountDownAfterMinBars)))
			{
				tdsh = this.ProcessSetupCountDown(tdsh);
				//if (this._intSetupCount >= 2 && this._intSetupRecycleCount >= this._intSequentialRecycleBars) // (page 22 of Bloomberg DeMark Indicators book)
				if (this._intSetupCount >= 2 && tdsh.Count >= this._intSequentialRecycleBars) // (Kay Way)
				{
					//this.RecycleSequentialCountdown(this._TDSetupHelper2ToRecycle);
					this.RecycleSequentialCountdown(tdsh.CountdownToRecycle as TDSetupHelper2);
					this._intSetupCount--;
					this._intSetupRecycleCount = this._intSetupRecycleCount - tdsh.Count;
				}
			} 
			
			if (tdsh.Completed) 
			{
				if (tdsh.SearchForPerfectSignal) tdsh = this.PerfectSignalTest(tdsh);
				if ((tdsh.Type == TDType2.Buy && this.PlotSequentialCountdownBuy) || (tdsh.Type == TDType2.Sell && this.PlotSequentialCountdownSell)) tdsh = this.ProcessSequentialCountdown(tdsh);
				if ((tdsh.Type == TDType2.Buy && this.PlotComboCountdownBuy) || (tdsh.Type == TDType2.Sell && this.PlotComboCountdownSell)) tdsh = this.ProcessComboCountdown(tdsh);

				this._boolIsComboCountdownBuyInProgress = this._boolIsComboCountdownBuyInProgress == false && tdsh.Type == TDType2.Buy && tdsh.ComboCountdownInProgress;
				this._boolIsComboCountdownSellInProgress = this._boolIsComboCountdownSellInProgress == false && tdsh.Type == TDType2.Sell && tdsh.ComboCountdownInProgress;;
				this._boolIsSequentialCountdownBuyInProgress = this._boolIsSequentialCountdownBuyInProgress == false && tdsh.Type == TDType2.Buy && tdsh.SequentialCountdownInProgress;
				this._boolIsSequentialCountdownSellInProgress = this._boolIsSequentialCountdownSellInProgress == false && tdsh.Type == TDType2.Sell && tdsh.SequentialCountdownInProgress;
			}
			
			if (this._boolWriteLOGOn)
			{
				string strMessage = this._intIndex.ToString() + "-" +
					tdsh.Type.ToString() + 
					"- INTERNAL: " + this._boolIsComboCountdownBuyInProgress.ToString() + " " + 
									 this._boolIsComboCountdownSellInProgress.ToString() + " " + 
									 this._boolIsSequentialCountdownBuyInProgress.ToString() + " " + 
									 this._boolIsSequentialCountdownSellInProgress.ToString() + " " +
									 this._intSetupRecycleCount.ToString() + " " +
					"- SETUP: " + tdsh.Cancelled.ToString() + " " + tdsh.Completed.ToString() + " " + tdsh.InProgress.ToString() + " Count " + tdsh.Count.ToString() +
					"- SEQUENTIAL: " + tdsh.SequentialCountdownCancelled.ToString() + " " + tdsh.SequentialCountdownCompleted.ToString() + " " + tdsh.SequentialCountdownInProgress.ToString() + " bar8 " + tdsh.SequentialBar8Close.ToString() +
					"- COMBO: " + tdsh.ComboCountdownCancelled.ToString() + " " + tdsh.ComboCountdownCompleted.ToString() + " " + tdsh.ComboCountdownInProgress.ToString() + " Count " + tdsh.ComboCount.ToString() +
					"";
				this.Log(strMessage, LogLevel.Information);
			}
			
			this._alTDSetupHelper[this._intIndex] = tdsh;
		}
		
		// Explicit predicate delegate 
		private static bool FindByID(TDSetupHelper2 tdsh)
		{
			return tdsh.ID == _intIDtoFind;
		}	
		#endregion
		
		#region Setup Countdown
		/// <summary>
		/// (page 9 of Bloomberg DeMark Indicators book)
		/// </summary>
		private TDSetupHelper2 PerfectSignalTest(TDSetupHelper2 tdsh)
		{
			if (tdsh.Type == TDType2.Buy)
			{
				if ((this.Low[0] <= tdsh.Bar6Low && this.Low[0] <= tdsh.Bar7Low) || 
					(tdsh.Bar8or9Low <= tdsh.Bar6Low && tdsh.Bar8or9Low <= tdsh.Bar7Low)) 
				{
					DrawArrowUp(this._intArrowCount++.ToString(), false, 0, this._dsYLow[0] + this._doublePointOffset * -1.5, this._colorPerfectSignalBuyColor);
					tdsh.SearchForPerfectSignal = false;
				}
			}
			else
			{
				if ((this.High[0] >= tdsh.Bar6High && this.High[0] >= tdsh.Bar7High) || 
					(tdsh.Bar8or9High >= tdsh.Bar6High && tdsh.Bar8or9High >= tdsh.Bar7High)) 
				{
					DrawArrowDown(this._intArrowCount++.ToString(), false, 0, this._dsYHigh[0] + this._doublePointOffset * 1.5, this._colorPerfectSignalSellColor);
					tdsh.SearchForPerfectSignal = false;
				}
			}
			return tdsh;
		}
		
		private TDSetupHelper2 ProcessSetupCountDown(TDSetupHelper2 tdsh)
		{
			tdsh.Count++;
			if (tdsh.Type == this._tdType2Previous) this._intSetupRecycleCount++;
			// Bar 1
			if (tdsh.Count == 1) 
			{
				this.TDSetupDrawText(tdsh, this._colorSetupBackgroundColorBar1, this._colorSetupFontColorBar1, this._intSetupFontSizeBar1);
				tdsh.Tags.SetValue(this._intObjectCount.ToString(), tdsh.Count-1);
				tdsh.Bar1High = High[0];
				tdsh.Bar1Low = Low[0];
			}
			// Bar N (normally 9)
			else if (tdsh.Count == this._intSetupBars) 
			{
				this.TDSetupDrawText(tdsh, this._colorSetupBackgroundColorBar9, this._colorSetupFontColorBar9, this._intSetupFontSizeBar9);
				tdsh.Tags.SetValue(this._intObjectCount.ToString(), tdsh.Count-1);
				
				if (tdsh.Type == TDType2.Buy)
				{
					tdsh.Bar6Low = this.Low[3];
					tdsh.Bar7Low = this.Low[2];
					
					if (this.Low[0] < this.Low[1])
						tdsh.Bar8or9Low = this.Low[0]; // bar N low (normally 9)
					else
						tdsh.Bar8or9Low = this.Low[1]; // bar N-1 low (normally 8)
				}
				else
				{
					tdsh.Bar6High = this.High[3];
					tdsh.Bar7High = this.High[2];
					
					if (this.High[0] > this.High[1])
						tdsh.Bar8or9High = this.High[0]; // bar N high (normally 9)
					else
						tdsh.Bar8or9High = this.High[1]; // bar N-1 high (normally 8)
				}
				
				// TDST Helper
				if (this.PlotTDST && ((tdsh.Type == TDType2.Buy && this._boolPlotSetupCountDownBuy) || (tdsh.Type == TDType2.Sell && this._boolPlotSetupCountDownSell)))
				{
					if (this.TDSTMaxNumber > 0)
					{
						if (tdsh.Type == TDType2.Buy && this._alTDSTHelperBuy2.Count >= this.TDSTMaxNumber)
						{
                    		this.RemoveDrawObject(this._alTDSTHelperBuy2[this._alTDSTHelperBuy2.Count - this.TDSTMaxNumber].Tag); // remove oldest line from chart
						}
						if (tdsh.Type == TDType2.Sell && this._alTDSTHelperSell2.Count >= this.TDSTMaxNumber)
						{
                    		this.RemoveDrawObject(this._alTDSTHelperSell2[this._alTDSTHelperSell2.Count - this.TDSTMaxNumber].Tag); // remove oldest line from chart
						}
					}

					if (tdsh.Type == TDType2.Buy)
					{
						this._alTDSTHelperBuy2.Add(new TDSTHelper2(tdsh.Type,
							false,
							"TDSTBuyBar1" + this.CurrentBar.ToString(), 
							tdsh.StartBar, // - intBar, // start bar
							tdsh.Type == TDType2.Buy ? tdsh.TrueHigh : tdsh.TrueLow, // start Y
							tdsh.StartBar, // - intBar, // end bar
							tdsh.Type == TDType2.Buy ? tdsh.TrueHigh : tdsh.TrueLow, // end Y
							(tdsh.Type == TDType2.Buy ? this.TDSTResistanceColor : this.TDSTSupportColor),
							this.DashStyle,
							this.Width));
					}
					else if (tdsh.Type == TDType2.Sell)
					{
						this._alTDSTHelperSell2.Add(new TDSTHelper2(tdsh.Type,
							false,
							"TDSTSellBar1" + this.CurrentBar.ToString(), 
							tdsh.StartBar, // - intBar, // start bar
							tdsh.Type == TDType2.Buy ? tdsh.TrueHigh : tdsh.TrueLow, // start Y
							tdsh.StartBar, // - intBar, // end bar
							tdsh.Type == TDType2.Buy ? tdsh.TrueHigh : tdsh.TrueLow, // end Y
							(tdsh.Type == TDType2.Buy ? this.TDSTResistanceColor : this.TDSTSupportColor),
							this.DashStyle,
							this.Width));
					}
				}
			
				tdsh.Completed = true;
				tdsh.InProgress = false;
				tdsh.SearchForPerfectSignal = this._boolDrawPerfectSignalArrow && ((tdsh.Type == TDType2.Buy && this.PlotSetupCountDownBuy) || (tdsh.Type == TDType2.Sell && this.PlotSetupCountDownSell));
				
				tdsh.SequentialCountdownCancelled = (tdsh.Type == TDType2.Buy && this.PlotSequentialCountdownBuy == false) || (tdsh.Type == TDType2.Sell && this.PlotSequentialCountdownSell == false);
				tdsh.SequentialCountdownCompleted = (tdsh.Type == TDType2.Buy && this.PlotSequentialCountdownBuy == false) || (tdsh.Type == TDType2.Sell && this.PlotSequentialCountdownSell == false);
				tdsh.SequentialCountdownInProgress = (tdsh.Type == TDType2.Buy && this.PlotSequentialCountdownBuy) || (tdsh.Type == TDType2.Sell && this.PlotSequentialCountdownSell);
				
				tdsh.ComboCountdownCancelled = (tdsh.Type == TDType2.Buy && this.PlotComboCountdownBuy == false) || (tdsh.Type == TDType2.Sell && this.PlotComboCountdownSell == false);
				tdsh.ComboCountdownCompleted = (tdsh.Type == TDType2.Buy && this.PlotComboCountdownBuy == false) || (tdsh.Type == TDType2.Sell && this.PlotComboCountdownSell == false);
				tdsh.ComboCountdownInProgress = (tdsh.Type == TDType2.Buy && this.PlotComboCountdownBuy) || (tdsh.Type == TDType2.Sell && this.PlotComboCountdownSell);
				
				if (tdsh.ComboCountdownInProgress) tdsh = this.StartComboCountdown(tdsh);

                // Cancel Reverse Setup - Start
                // When a setup which is in the opposite direction to a countdown has reached the minimum required count. 
                // This cancellation works only when "REVERSE SETUP CANCEL" is true. So in cases where "reverse setup cancel" is true and 
                // a buy setup has reached the minimum required count (default count is 9), ALL sell countdowns in progress are cancelled. 
                // Likewise when a sell setup has reached the minimum required count, ALL buy countdowns in progress are cancelled.
				if (this._boolSequentialCancelReverseSetup)
				{
					if (this._boolPlotSequentialCountdownBuy && tdsh.Type == TDType2.Sell) this.CancelSequentialCountdownBuys(tdsh.ID);
					if (this._boolPlotSequentialCountdownSell && tdsh.Type == TDType2.Buy) this.CancelSequentialCountdownSells(tdsh.ID);
					if (this._boolPlotComboCountdownBuy && tdsh.Type == TDType2.Sell) this.CancelComboCountdownBuys(tdsh.ID);
					if (this._boolPlotComboCountdownSell && tdsh.Type == TDType2.Buy) this.CancelComboCountdownSells(tdsh.ID);
				}
                // Cancel Reverse Setup - End
				
				if (tdsh.Type != this._tdType2Previous)
				{
					this._intSetupCount = 0;
					this._intSetupRecycleCount = this._intSetupBars;
					this._tdType2Previous = tdsh.Type;
				}
				tdsh.CountdownToRecycle = this._TDSetupHelper2ToRecycle;
				this._TDSetupHelper2ToRecycle = tdsh;
				this._intSetupCount++;
			}
			// Bar 2 - N-1
			else
			{
				if (this._TDSequentialPlotType2 == TDSequentialPlotType2.DrawDots)
					this.TDSetupDrawDot(tdsh);
				else
					this.TDSetupDrawText(tdsh, this._colorSetupFontColor, this._intSetupFontSize);
				tdsh.Tags.SetValue(this._intObjectCount.ToString(), tdsh.Count-1);
			}
			
			if (tdsh.InProgress)
			{
				if (this.High[0] > tdsh.HighestHigh) tdsh.HighestHigh = this.High[0];
				if (this.Low[0] < tdsh.LowestLow) tdsh.LowestLow = this.Low[0];
				
				this._doubleTrueHigh = this.High[0] > this.Close[1] ? this.High[0] : this.Close[1];
				if (tdsh.Type == TDType2.Buy && this._doubleTrueHigh > tdsh.TrueHigh)
				{
					tdsh.StartBar = this.CurrentBar;
					tdsh.TrueHigh = this._doubleTrueHigh;
				}
				this._doubleTrueLow = this.Low[0] < this.Close[1] ? this.Low[0] : this.Close[1];
				if (tdsh.Type == TDType2.Sell && this._doubleTrueLow < tdsh.TrueLow)
				{
					tdsh.StartBar = this.CurrentBar;
					tdsh.TrueLow = this._doubleTrueLow;
				}
			}
			
			return tdsh;
		}

		private void TDSetupDrawDot(TDSetupHelper2 tdsh)
		{
			this._intObjectCount++;
			if (tdsh.Type == TDType2.Buy && this.PlotSetupCountDownBuy)
			{
				this._dsYLow.Set(this._dsYLow[0] + this._doublePointOffset * tdsh.Multiplier);
				DrawDot(this._intObjectCount.ToString(), true, 0, this._dsYLow[0], Color.Lime);
			}
			else if (tdsh.Type == TDType2.Sell && this.PlotSetupCountDownSell)
			{
				this._dsYHigh.Set(this._dsYHigh[0] + this._doublePointOffset * tdsh.Multiplier);
				DrawDot(this._intObjectCount.ToString(), true, 0, this._dsYHigh[0], Color.Red);
			}
		}
		
		private void TDSetupDrawText(TDSetupHelper2 tdsh, Color colorColor, int intFontSize)
		{
			this._intObjectCount++;
			if (tdsh.Type == TDType2.Buy && this.PlotSetupCountDownBuy == false) return;
			if (tdsh.Type == TDType2.Sell && this.PlotSetupCountDownSell == false) return;
			if (tdsh.Type == TDType2.Buy)
			{
				this._dsYLow.Set(this._dsYLow[0] + this._doublePointOffset * tdsh.Multiplier);
				this._doubleYLocation = this.Low[0];
				this._dsYPixelOffsetLow.Set(this._dsYPixelOffsetLow[0] + this._intPixelOffset * tdsh.Multiplier);
				this._intYPixelOffset = (int)this._dsYPixelOffsetLow[0];
			}
			else
			{
				this._dsYHigh.Set(this._dsYHigh[0] + this._doublePointOffset * tdsh.Multiplier);
				this._doubleYLocation = this.High[0];
				this._dsYPixelOffsetHigh.Set(this._dsYPixelOffsetHigh[0] + this._intPixelOffset * tdsh.Multiplier);
				this._intYPixelOffset = (int)this._dsYPixelOffsetHigh[0];
			}

			DrawText(this._intObjectCount.ToString(),
				false, 
				tdsh.Count.ToString(), 
				0, 
				this._doubleYLocation, 
				this._intYPixelOffset, // yPixelOffset
				colorColor, 
				new Font(FontFamily.GenericSansSerif, intFontSize, FontStyle.Bold, GraphicsUnit.Pixel), 
				StringAlignment.Center,
				Color.Transparent,
				Color.Transparent,
				0); // areaOpacity
		}
		
		private void TDSetupDrawText(TDSetupHelper2 tdsh, Color colorBackground, Color colorColor, int intFontSize)
		{
			this._intObjectCount++;
			if (tdsh.Type == TDType2.Buy && this.PlotSetupCountDownBuy == false) return;
			if (tdsh.Type == TDType2.Sell && this.PlotSetupCountDownSell == false) return;
			if (tdsh.Type == TDType2.Buy)
			{
				this._dsYLow.Set(this._dsYLow[0] + this._doublePointOffset * tdsh.Multiplier);
				this._doubleYLocation = this.Low[0];
				this._dsYPixelOffsetLow.Set(this._dsYPixelOffsetLow[0] + this._intPixelOffset * tdsh.Multiplier);
				this._intYPixelOffset = (int)this._dsYPixelOffsetLow[0];
			}
			else
			{
				this._dsYHigh.Set(this._dsYHigh[0] + this._doublePointOffset * tdsh.Multiplier);
				this._doubleYLocation = this.High[0];
				this._dsYPixelOffsetHigh.Set(this._dsYPixelOffsetHigh[0] + this._intPixelOffset * tdsh.Multiplier);
				this._intYPixelOffset = (int)this._dsYPixelOffsetHigh[0];
			}

			DrawText(this._intObjectCount.ToString(),
				false, 
				tdsh.Count.ToString(), 
				0, 
				this._doubleYLocation, 
				this._intYPixelOffset, // yPixelOffset
				colorColor, 
				new Font(FontFamily.GenericSansSerif, intFontSize, FontStyle.Bold, GraphicsUnit.Pixel), 
				StringAlignment.Center,
				colorBackground,
				colorBackground,
				8); // areaOpacity
		}
		#endregion
		
		#region Setup Process TDST
		private void ProcessTDST()
		{
			TDSTHelper2 tdst;

            Int32 intStart = this._alTDSTHelperBuy2.Count > 2 ? this._alTDSTHelperBuy2.Count - 2 : 0;

            for (Int32 int1 = intStart; int1 < this._alTDSTHelperBuy2.Count; int1++)
			{
				tdst = this._alTDSTHelperBuy2[int1];
				if (tdst.Completed == false)
				{
					this._boolQ1 = this.Close[2] < this.Close[3] &&
						this.Close[1] > this.Close[2] &&
						this.Close[1] > tdst.StartY &&
						this.High[1] > this.Open[1];
					this._boolOR = this.High[0] > this.Open[0] &&
						this.Open[0] > this.Close[1];
					
					if (this._boolSetupQ1ORBuy && this._boolQ1 && this._boolOR) 
					{
						this.DrawLine(tdst.Tag, false, this.CurrentBar - tdst.StartBar, tdst.StartY, 0, tdst.EndY, tdst.Color, DashStyle.Solid, tdst.Width);
						tdst.Completed = true;
						this._alTDSTHelperBuy2[int1] = tdst;
					}
					else
					{
						this.DrawLine(tdst.Tag, false, this.CurrentBar - tdst.StartBar, tdst.StartY, 0, tdst.EndY, tdst.Color, tdst.DashStyle, tdst.Width);
					}
				}
			}

            intStart = this._alTDSTHelperSell2.Count > 2 ? this._alTDSTHelperSell2.Count - 2 : 0;

            for (Int32 int1 = intStart; int1 < this._alTDSTHelperSell2.Count; int1++)
			{
				tdst = this._alTDSTHelperSell2[int1];
				if (tdst.Completed == false)
				{
					this._boolQ1 = this.Close[2] > this.Close[3] &&
						this.Close[1] < this.Close[2] &&
						this.Close[1] < tdst.StartY &&
						this.Low[1] < this.Open[1];
					this._boolOR = this.Low[0] < this.Open[0] &&
						this.Open[0] < this.Close[1];
					
					if (this._boolSetupQ1ORSell && this._boolQ1 && this._boolOR) 
					{
						this.DrawLine(tdst.Tag, false, this.CurrentBar - tdst.StartBar, tdst.StartY, 0, tdst.EndY, tdst.Color, DashStyle.Solid, tdst.Width);
						tdst.Completed = true;
						this._alTDSTHelperSell2[int1] = tdst;
					}
					else
					{
						this.DrawLine(tdst.Tag, false, this.CurrentBar - tdst.StartBar, tdst.StartY, 0, tdst.EndY, tdst.Color, tdst.DashStyle, tdst.Width);
					}
				}
			}
		}
		#endregion
		
		#region Sequential Countdown
		/// <summary>
		/// 
		/// </summary>
		/// <param name="intID"></param>
		private void CancelSequentialCountdownBuys(Int32 intID)
		{
			Int32 int1 = 0;
			TDSetupHelper2 tdsh2;
			foreach (TDSetupHelper2 tdsh in this._alTDSetupHelper.ToArray())
			{
				if (tdsh.ID > intID) break;
				if (tdsh.Type == TDType2.Buy)
				{
					tdsh2 = (TDSetupHelper2)this._alTDSetupHelper[int1];
					if (tdsh.SequentialCountdownInProgress)
					{
						this.RemoveCurrentSequentialCountdown(tdsh); // page 21 of Bloomberg DeMark Indicators book
						tdsh2.SequentialCountdownCancelled = true;
						tdsh2.SequentialCountdownInProgress = false;
					}
					tdsh2.SearchForPerfectSignal = false; // setup in the opposite direction, stop any perfection search
					this._alTDSetupHelper[int1] = tdsh2;
				}
				int1++;
			}
		}
		
		private void CancelSequentialCountdownSells(Int32 intID)
		{
			Int32 int1 = 0;
			TDSetupHelper2 tdsh2;
			foreach (TDSetupHelper2 tdsh in this._alTDSetupHelper.ToArray())
			{
				if (tdsh.ID > intID) break;
				if (tdsh.Type == TDType2.Sell)
				{
					tdsh2 = (TDSetupHelper2)this._alTDSetupHelper[int1];
					if (tdsh.SequentialCountdownInProgress)
					{						
						this.RemoveCurrentSequentialCountdown(tdsh); // page 34 of Bloomberg DeMark Indicators book
						tdsh2.SequentialCountdownCancelled = true;
						tdsh2.SequentialCountdownInProgress = false;
					}
					tdsh2.SearchForPerfectSignal = false; // setup in the opposite direction, stop any perfection search					
					this._alTDSetupHelper[int1] = tdsh2;
				}
				int1++;
			}
		}
		
		private TDSetupHelper2 ProcessSequentialCountdown(TDSetupHelper2 tdsh)
		{
			if (tdsh.SequentialCountdownCancelled || tdsh.SequentialCountdownCompleted) return tdsh;

            // Cancel TDST - Start
            // Each setup which reaches the minimum required count gives rise to 1 countdown and 1 TDST Line. 
            // In the case of a buy setup which has reached the minimum required count, a TDST Buy Line is drawn and a Buy Countdown commences. 
            // At any point during the progress of this Buy Countdown, a true low is formed above the TDST Buy Line that commenced 
            // drawing at the same time as the Buy Countdown, the Buy Countdown is cancelled BUT all other Buy Countdowns which may be in 
            // progress continue on. With the sell countdown, a true high must be formed below the TDST Sell Line for 
            // the sell countdown in question to be cancelled. In this case, countdowns cancellations refer to a specific countdown.
			if (this._boolSequentialCancelTDST)
			{
				if (tdsh.Type == TDType2.Buy)
				{
					this._doubleTrueLow = this.Low[0] < this.Close[1] ? this.Low[0] : this.Close[1];
					if (this._doubleTrueLow > tdsh.TrueHigh)
					{
						this.RemoveCurrentSequentialCountdown(tdsh);
						_intIDtoFind = tdsh.ID;
						this._intIndex = this._alTDSetupHelper.FindIndex(FindByID);
						tdsh.SequentialCountdownCancelled = true;
						tdsh.SequentialCountdownInProgress = false;
						this._alTDSetupHelper[this._intIndex] = tdsh;
					}
				}
				else
				{
					this._doubleTrueHigh = this.High[0] > this.Close[1] ? this.High[0] : this.Close[1];
					if (this._doubleTrueHigh < tdsh.TrueLow)
					{
						this.RemoveCurrentSequentialCountdown(tdsh);
						_intIDtoFind = tdsh.ID;
						this._intIndex = this._alTDSetupHelper.FindIndex(FindByID);
						tdsh.SequentialCountdownCancelled = true;
						tdsh.SequentialCountdownInProgress = false;
						this._alTDSetupHelper[this._intIndex] = tdsh;
					}
				}
			}
            // Cancel TDST - End

			if ( this._TDTerminationCount2 == TDTerminationCount2.Open && tdsh.SequentialCount == this._intSequentialBars - 1)
			{		
				if (tdsh.Type == TDType2.Buy)
				{
					this._doubleTerminationPrice = this.Open[0] > this.Close[0] ? this.Close[0] : this.Open[0];
				}
				else
				{
					this._doubleTerminationPrice = this.Open[0] > this.Close[0] ? this.Open[0] : this.Close[0];
				}
			}
			else
			{
				this._doubleTerminationPrice = this.Close[0];
			}

            if ((tdsh.Type == TDType2.Buy && _doubleTerminationPrice <= Low[this._intSequentialLookBackBars]) || // czpiaor
                (tdsh.Type == TDType2.Sell && _doubleTerminationPrice >= High[this._intSequentialLookBackBars])) // czpiaor
			{
				if (tdsh.SequentialCount < this._intSequentialBars - 1)
				{
					if (this._boolSequentialBar8VersusBar5Qualifier && tdsh.SequentialCount == 7)
					{
						if ((tdsh.Type == TDType2.Buy && this.Low[0] <= tdsh.SequentialBar5Close) ||
							(tdsh.Type == TDType2.Sell && this.High[0] >= tdsh.SequentialBar5Close))
						{
							tdsh.SequentialCount++;
							tdsh.SequentialBar8Close = this.Close[0];
						}
						if ((tdsh.Type == TDType2.Buy && this._boolIsSequentialCountdownBuyInProgress == false) ||
							(tdsh.Type == TDType2.Sell && this._boolIsSequentialCountdownSellInProgress == false))
						{
							if (tdsh.SequentialCount == 8)
							{
								this.TDSequentialDrawText(tdsh, this._colorSequentialFontColor, this._intSequentialFontSize);
								tdsh.SequentialTags.SetValue(this._intObjectCount.ToString(), tdsh.SequentialTagsIndex++); 
							}
							else
							{
								this.TDSequentialDrawText(tdsh, "+", this._colorSequentialFontColor, this._intSequentialFontSizeBar13);
								tdsh.SequentialTags.SetValue(this._intObjectCount.ToString(), tdsh.SequentialTagsIndex++); 
							}
						}
					}
					else
					{
						tdsh.SequentialCount++;
						if ((tdsh.Type == TDType2.Buy && this._boolIsSequentialCountdownBuyInProgress == false) ||
							(tdsh.Type == TDType2.Sell && this._boolIsSequentialCountdownSellInProgress == false))
						{
							if (tdsh.SequentialCount == 1)
							{
								this.TDSequentialDrawText(tdsh, this._colorSequentialBackgroundColorBar1, this._colorSequentialFontColorBar1, this._intSequentialFontSizeBar1);
								tdsh.SequentialTags.SetValue(this._intObjectCount.ToString(), tdsh.SequentialTagsIndex++); 
							}
							else
							{
								this.TDSequentialDrawText(tdsh, this._colorSequentialFontColor, this._intSequentialFontSize);
								tdsh.SequentialTags.SetValue(this._intObjectCount.ToString(), tdsh.SequentialTagsIndex++); 
							}
						}
						if (tdsh.SequentialCount == 5) tdsh.SequentialBar5Close = this.Close[0];
						if (tdsh.SequentialCount == 8) tdsh.SequentialBar8Close = this.Close[0];
					}
				}
				else if (tdsh.SequentialCount == this._intSequentialBars - 1) // additional requirement for bar 13 (page 19 of Bloomberg DeMark Indicators book)
				{
					if (this._boolSequentialBar13VersusBar8Qualifier)
					{
						if ((tdsh.Type == TDType2.Buy && Low[0] <= tdsh.SequentialBar8Close) ||
							(tdsh.Type == TDType2.Sell && High[0] >= tdsh.SequentialBar8Close))
						{
							tdsh.SequentialCount++;
							if ((tdsh.Type == TDType2.Buy && this._boolIsSequentialCountdownBuyInProgress == false) ||
								(tdsh.Type == TDType2.Sell && this._boolIsSequentialCountdownSellInProgress == false))
							{
								tdsh = this.TDSequentialDrawText(tdsh, this._colorSequentialBackgroundColorBar1, this._colorSequentialFontColorBar13, this._intSequentialFontSizeBar13);
								tdsh.SequentialTags.SetValue(this._intObjectCount.ToString(), tdsh.SequentialTagsIndex++); 
							}
							tdsh.SequentialCountdownCompleted = true;
							tdsh.SequentialCountdownInProgress = false;
						}
						else
						{
							if ((tdsh.Type == TDType2.Buy && this._boolIsSequentialCountdownBuyInProgress == false) ||
								(tdsh.Type == TDType2.Sell && this._boolIsSequentialCountdownSellInProgress == false))
							{
								this.TDSequentialDrawText(tdsh, "+", this._colorSequentialFontColor, this._intSequentialFontSizeBar13);
								tdsh.SequentialTags.SetValue(this._intObjectCount.ToString(), tdsh.SequentialTagsIndex++); 
							}
						}
					}
					else
					{
						tdsh.SequentialCount++;
						if ((tdsh.Type == TDType2.Buy && this._boolIsSequentialCountdownBuyInProgress == false) ||
							(tdsh.Type == TDType2.Sell && this._boolIsSequentialCountdownSellInProgress == false))
						{
							tdsh = this.TDSequentialDrawText(tdsh, this._colorSequentialBackgroundColorBar1, this._colorSequentialFontColorBar13, this._intSequentialFontSizeBar13);
							tdsh.SequentialTags.SetValue(this._intObjectCount.ToString(), tdsh.SequentialTagsIndex++); 
						}
						tdsh.SequentialCountdownCompleted = true;
						tdsh.SequentialCountdownInProgress = false;
					}
				}
			}

			return tdsh;
		}
		
		private string RemoveCurrentSequentialCountdown(TDSetupHelper2 tdsh)
		{
			string strTag13 = string.Empty;
			for (int int2 = 0; int2 < tdsh.SequentialTags.Length - 1; int2++)
			{
				if (tdsh.SequentialTags[int2] == null)
				{
					break;
				}
				else if (tdsh.SequentialTags[int2 + 1] == null && tdsh.SequentialCountdownCompleted)
				{
					strTag13 = tdsh.SequentialTags[int2];
					break;
				}
				RemoveDrawObject(tdsh.SequentialTags[int2]); // remove td sequential countdown from chart
			}
			return strTag13;
		}
		
		private void RecycleSequentialCountdown(TDSetupHelper2 tdsh)
		{
			if (tdsh == null || tdsh.ID < 0) return;
			string strBar13Tag = this.RemoveCurrentSequentialCountdown(tdsh);
			if (strBar13Tag.Length > 0)
			{
				DrawText(strBar13Tag,
					false, 
					"R", 
					this.CurrentBar - tdsh.SequentialBar13, 
					tdsh.SequentialBar13YLocation, 
					tdsh.SequentialBar13YPixelOffsetLow,
					this._colorSequentialFontColorBarRecycle,
					new Font(FontFamily.GenericSansSerif, this._intSequentialFontSizeBarRecycle, FontStyle.Bold, GraphicsUnit.Pixel), 
					StringAlignment.Center, 
					Color.Transparent, 
					Color.Transparent, 
					0); 
			}
			tdsh.SequentialCountdownCompleted = true;
			tdsh.SequentialCountdownInProgress = false;
		}
		
		private void TDSequentialDrawText(TDSetupHelper2 tdsh, Color colorColor, int intFontSize)
		{
			this._intObjectCount++;
			if (tdsh.Type == TDType2.Buy)
			{
				this._dsYLow.Set(this._dsYLow[0] + this._doublePointOffset * tdsh.Multiplier);
				this._doubleYLocation = this.Low[0];
				this._dsYPixelOffsetLow.Set(this._dsYPixelOffsetLow[0] + this._intPixelOffset * tdsh.Multiplier);
				this._intYPixelOffset = (int)this._dsYPixelOffsetLow[0];
			}
			else
			{
				this._dsYHigh.Set(this._dsYHigh[0] + this._doublePointOffset * tdsh.Multiplier);
				this._doubleYLocation = this.High[0];
				this._dsYPixelOffsetHigh.Set(this._dsYPixelOffsetHigh[0] + this._intPixelOffset * tdsh.Multiplier);
				this._intYPixelOffset = (int)this._dsYPixelOffsetHigh[0];
			}
			DrawText(this._intObjectCount.ToString(), 
				false, 
				tdsh.SequentialCount.ToString(), 
				0, 
				this._doubleYLocation, 
				this._intYPixelOffset,
				colorColor, 
				new Font(FontFamily.GenericSansSerif, intFontSize, FontStyle.Bold, GraphicsUnit.Pixel), 
				StringAlignment.Center, 
				Color.Transparent, 
				Color.Transparent, 
				0); 
		}
		
		private TDSetupHelper2 TDSequentialDrawText(TDSetupHelper2 tdsh, Color colorBackground, Color colorColor, int intFontSize)
		{
			this._intObjectCount++;
			if (tdsh.Type == TDType2.Buy)
			{
				this._dsYLow.Set(this._dsYLow[0] + this._doublePointOffset * tdsh.Multiplier);
				this._doubleYLocation = this.Low[0];
				this._dsYPixelOffsetLow.Set(this._dsYPixelOffsetLow[0] + this._intPixelOffset * tdsh.Multiplier);
				this._intYPixelOffset = (int)this._dsYPixelOffsetLow[0];
			}
			else
			{
				this._dsYHigh.Set(this._dsYHigh[0] + this._doublePointOffset * tdsh.Multiplier);
				this._doubleYLocation = this.High[0];
				this._dsYPixelOffsetHigh.Set(this._dsYPixelOffsetHigh[0] + this._intPixelOffset * tdsh.Multiplier);
				this._intYPixelOffset = (int)this._dsYPixelOffsetHigh[0];
			}
			DrawText(this._intObjectCount.ToString(), 
				false, 
				tdsh.SequentialCount.ToString(), 
				0, 
				this._doubleYLocation, 
				this._intYPixelOffset,
				colorColor, 
				new Font(FontFamily.GenericSansSerif, intFontSize, FontStyle.Bold, GraphicsUnit.Pixel), 
				StringAlignment.Center, 
				colorBackground, 
				colorBackground, 
				8); 
			tdsh.SequentialBar13 = this.CurrentBar;
			tdsh.SequentialBar13YLocation = this._doubleYLocation;
			tdsh.SequentialBar13YPixelOffsetLow = this._intYPixelOffset;
			return tdsh;
		}
		
		private void TDSequentialDrawText(TDSetupHelper2 tdsh, string strText, Color colorColor, int intFontSize)
		{
			this._intObjectCount++;
			if (tdsh.Type == TDType2.Buy)
			{
				this._dsYLow.Set(this._dsYLow[0] + this._doublePointOffset * tdsh.Multiplier);
				this._doubleYLocation = this.Low[0];
				this._dsYPixelOffsetLow.Set(this._dsYPixelOffsetLow[0] + this._intPixelOffset * tdsh.Multiplier);
				this._intYPixelOffset = (int)this._dsYPixelOffsetLow[0];
			}
			else
			{
				this._dsYHigh.Set(this._dsYHigh[0] + this._doublePointOffset * tdsh.Multiplier);
				this._doubleYLocation = this.High[0];
				this._dsYPixelOffsetHigh.Set(this._dsYPixelOffsetHigh[0] + this._intPixelOffset * tdsh.Multiplier);
				this._intYPixelOffset = (int)this._dsYPixelOffsetHigh[0];
			}
			DrawText(this._intObjectCount.ToString(), 
				false, 
				strText, 
				0, 
				this._doubleYLocation, 
				this._intYPixelOffset,
				colorColor, 
				new Font(FontFamily.GenericSansSerif, intFontSize, FontStyle.Bold, GraphicsUnit.Pixel), 
				StringAlignment.Center, 
				Color.Transparent, 
				Color.Transparent, 
				0); 
		}
		
		private void TDSequentialDrawText(TDSetupHelper2 tdsh, string strText, int intBarsAgo, Color colorBackground, Color colorColor, int intFontSize)
		{
			this._intObjectCount++;
			if (tdsh.Type == TDType2.Buy)
			{
				this._dsYLow.Set(this._dsYLow[0] + this._doublePointOffset * tdsh.Multiplier);
				this._doubleYLocation = this.Low[0];
				this._dsYPixelOffsetLow.Set(this._dsYPixelOffsetLow[0] + this._intPixelOffset * tdsh.Multiplier);
				this._intYPixelOffset = (int)this._dsYPixelOffsetLow[0];
			}
			else
			{
				this._dsYHigh.Set(this._dsYHigh[0] + this._doublePointOffset * tdsh.Multiplier);
				this._doubleYLocation = this.High[0];
				this._dsYPixelOffsetHigh.Set(this._dsYPixelOffsetHigh[0] + this._intPixelOffset * tdsh.Multiplier);
				this._intYPixelOffset = (int)this._dsYPixelOffsetHigh[0];
			}
			DrawText(this._intObjectCount.ToString(), 
				false, 
				strText, 
				intBarsAgo, 
				this._doubleYLocation, 
				this._intYPixelOffset, // yPixelOffset
				colorColor, 
				new Font(FontFamily.GenericSansSerif, intFontSize, FontStyle.Bold, GraphicsUnit.Pixel), 
				StringAlignment.Center, 
				colorBackground,
				colorBackground, 
				8); 
		}
		#endregion
	
		#region Combo Countdown
		private void CancelComboCountdownBuys(Int32 intID)
		{
			Int32 int1 = 0;
			TDSetupHelper2 tdsh2;
			foreach (TDSetupHelper2 tdsh in this._alTDSetupHelper.ToArray())
			{
				if (tdsh.ID > intID) break;
				if (tdsh.Type == TDType2.Buy)
				{
					tdsh2 = (TDSetupHelper2)this._alTDSetupHelper[int1];
					if (tdsh.ComboCountdownInProgress)
					{
						this.RemoveCurrentComboCountdown(tdsh); // page 21 of Bloomberg DeMark Indicators book
						tdsh2.ComboCountdownCancelled = true;
						tdsh2.ComboCountdownInProgress = false;
					}
					tdsh2.SearchForPerfectSignal = false; // setup in the opposite direction, stop any perfection search
					this._alTDSetupHelper[int1] = tdsh2;
				}
				int1++;
			}
		}
		
		private void CancelComboCountdownSells(Int32 intID)
		{
			Int32 int1 = 0;
			TDSetupHelper2 tdsh2;
			foreach (TDSetupHelper2 tdsh in this._alTDSetupHelper.ToArray())
			{
				if (tdsh.ID > intID) break;
				if (tdsh.Type == TDType2.Sell)
				{
					tdsh2 = (TDSetupHelper2)this._alTDSetupHelper[int1];
					if (tdsh.ComboCountdownInProgress)
					{						
						this.RemoveCurrentComboCountdown(tdsh); // page 34 of Bloomberg DeMark Indicators book
						tdsh2.ComboCountdownCancelled = true;
						tdsh2.ComboCountdownInProgress = false;
					}
					tdsh2.SearchForPerfectSignal = false; // setup in the opposite direction, stop any perfection search					
					this._alTDSetupHelper[int1] = tdsh2;
				}
				int1++;
			}
		}
		
		private TDSetupHelper2 ProcessComboCountdown(TDSetupHelper2 tdsh)
		{
			if (tdsh.ComboCountdownCancelled || tdsh.ComboCountdownCompleted) return tdsh;
			
			if ((tdsh.Type == TDType2.Buy && (Close[0] <= Low[this._intComboLookBackBars] && Low[0] <= tdsh.LastComboLow && Close[0] < tdsh.LastComboClose && Close[0] < Close[1])) ||
				(tdsh.Type == TDType2.Sell && (Close[0] >= High[this._intComboLookBackBars] && High[0] >= tdsh.LastComboHigh && Close[0] > tdsh.LastComboClose && Close[0] > Close[1])))
			{
				tdsh.ComboCount++;
				if (tdsh.ComboCount == 1)
				{
					if ((tdsh.Type == TDType2.Buy && this._boolIsComboCountdownBuyInProgress == false) ||
						(tdsh.Type == TDType2.Sell && this._boolIsComboCountdownSellInProgress == false))
					{
						this.TDComboDrawText(tdsh, 0, this._colorComboBackgroundColorBar1, this._colorComboFontColorBar1, this._intComboFontSizeBar1);
						tdsh.ComboTags.SetValue(this._intObjectCount.ToString(), tdsh.ComboCount-1); 
					}
				}
				else if (tdsh.ComboCount == this._intComboBars)
				{
					if (this._intSetupCount >= 2 && this._intSetupRecycleCount >= this._intComboRecycleBars) // (page 22 of Bloomberg DeMark Indicators book)
					{
						this.RecycleComboCountdown(tdsh);
						this._intSetupCount--;
						this._intSetupRecycleCount = this._intSetupRecycleCount - tdsh.Count;
						tdsh.ComboCountdownCompleted = true;
						tdsh.ComboCountdownInProgress = false;
					}
					else if ((tdsh.Type == TDType2.Buy && this._boolIsComboCountdownBuyInProgress == false) ||
							(tdsh.Type == TDType2.Sell && this._boolIsComboCountdownSellInProgress == false))
					{
						this.TDComboDrawText(tdsh, 0, this._colorComboBackgroundColorBar13, this._colorComboFontColorBar13, this._intComboFontSizeBar13);
						tdsh.ComboTags.SetValue(this._intObjectCount.ToString(), tdsh.ComboCount-1); 
					}
					tdsh.ComboCountdownCompleted = true;
					tdsh.ComboCountdownInProgress = false;
				}
				else
				{
					if ((tdsh.Type == TDType2.Buy && this._boolIsComboCountdownBuyInProgress == false) ||
						(tdsh.Type == TDType2.Sell && this._boolIsComboCountdownSellInProgress == false))
					{
						this.TDComboDrawText(tdsh, 0, this._colorComboFontColor, this._intComboFontSize);
						tdsh.ComboTags.SetValue(this._intObjectCount.ToString(), tdsh.ComboCount-1); 
					}
				}
				tdsh.LastComboClose = Close[0];
				tdsh.LastComboHigh = High[0];
				tdsh.LastComboLow = Low[0];
			}
			
			if (tdsh.Type == TDType2.Buy)
			{
				if (Low[0] > tdsh.Bar1High)
				{
					this.RemoveCurrentComboCountdown(tdsh);
					tdsh.ComboCountdownCancelled = true;
					tdsh.ComboCountdownInProgress = false;
				}
			}
			else
			{
				if (High[0] < tdsh.Bar1Low)
				{
					this.RemoveCurrentComboCountdown(tdsh);
					tdsh.ComboCountdownCancelled = true;
					tdsh.ComboCountdownInProgress = false;
				}
			}
			
			return tdsh;
		}
		
		private void RecycleComboCountdown(TDSetupHelper2 tdsh)
		{
			this.RemoveCurrentComboCountdown(tdsh);
			if ((tdsh.Type == TDType2.Buy && this._boolIsComboCountdownBuyInProgress == false) ||
				(tdsh.Type == TDType2.Sell && this._boolIsComboCountdownSellInProgress == false))
			{			
				this.TDComboDrawText(tdsh, "R", 0, this._colorComboBackgroundColorBarRecycle, this._colorComboFontColorBarRecycle, this._intComboFontSizeBarRecycle);
			}
		}

		private void RemoveCurrentComboCountdown(TDSetupHelper2 tdsh)
		{
			for (int int2 = 0; int2 < tdsh.ComboTags.Length; int2++)
			{
				if (tdsh.ComboTags[int2] != null)
					RemoveDrawObject(tdsh.ComboTags[int2]); // remove td setup from chart
				else
					break;
			}
		}

		private TDSetupHelper2 StartComboCountdown(TDSetupHelper2 tdsh)
		{
			for (int index = 8; index >= 1; index--) 
			{
				if (tdsh.Type == TDType2.Buy)
				{
					if ((index == 8 && Close[index] <= Low[index+this._intComboLookBackBars] && Close[index] < Close[index+this._intComboLookBackBars-1]) ||
						(Close[index] <= Low[index+this._intComboLookBackBars] && Low[index] <= tdsh.LastComboLow && Close[index] < tdsh.LastComboClose && Close[index] < Close[index+this._intComboLookBackBars-1]))
					{
						tdsh.ComboCount++;
						if ((tdsh.Type == TDType2.Buy && this._boolIsComboCountdownBuyInProgress == false) ||
							(tdsh.Type == TDType2.Sell && this._boolIsComboCountdownSellInProgress == false))
						{
							if (tdsh.ComboCount == 1)
							{
								this.TDComboDrawText(tdsh, index, this._colorComboBackgroundColorBar1, this._colorComboFontColorBar1, this._intComboFontSizeBar1);
							}
							else
							{
								this.TDComboDrawText(tdsh, index, this._colorComboFontColor, this._intComboFontSize);
							}
							tdsh.ComboTags.SetValue(this._intObjectCount.ToString(), tdsh.ComboCount-1); 
						}
						tdsh.LastComboClose = Close[index];
						tdsh.LastComboLow = Low[index];
					}
				}
				else
				{
					if ((index == 8 && Close[index] >= High[index+this._intComboLookBackBars] && Close[index] > Close[index+this._intComboLookBackBars-1]) ||
						(Close[index] >= High[index+this._intComboLookBackBars] && High[index] >= tdsh.LastComboHigh && Close[index] > tdsh.LastComboClose && Close[index] > Close[index+this._intComboLookBackBars-1]))
					{
						tdsh.ComboCount++;
						if ((tdsh.Type == TDType2.Buy && this._boolIsComboCountdownBuyInProgress == false) ||
							(tdsh.Type == TDType2.Sell && this._boolIsComboCountdownSellInProgress == false))
						{
							if (tdsh.ComboCount == 1)
							{
								this.TDComboDrawText(tdsh, index, this._colorComboBackgroundColorBar1, this._colorComboFontColorBar1, this._intComboFontSizeBar1);
							}
							else
							{
								this.TDComboDrawText(tdsh, index, this._colorComboFontColor, this._intComboFontSize);
							}
							tdsh.ComboTags.SetValue(this._intObjectCount.ToString(), tdsh.ComboCount-1); 
						}
						tdsh.LastComboClose = Close[index];
						tdsh.LastComboHigh = High[index];
					}
				}
			}
			return tdsh;
		}
		
		private void TDComboDrawText(TDSetupHelper2 tdsh, int intBarsAgo, Color colorColor, int intFontSize)
		{
			this._intObjectCount++;
			if (tdsh.Type == TDType2.Buy)
			{
				this._dsYLow.Set(intBarsAgo, this._dsYLow[intBarsAgo] + this._doublePointOffset * tdsh.Multiplier);
				this._doubleYLocation = this.Low[intBarsAgo];
				this._dsYPixelOffsetLow.Set(intBarsAgo, this._dsYPixelOffsetLow[intBarsAgo] + this._intPixelOffset * tdsh.Multiplier);
				this._intYPixelOffset = (int)this._dsYPixelOffsetLow[intBarsAgo];
			}
			else
			{
				this._dsYHigh.Set(intBarsAgo, this._dsYHigh[intBarsAgo] + this._doublePointOffset * tdsh.Multiplier);
				this._doubleYLocation = this.High[intBarsAgo];
				this._dsYPixelOffsetHigh.Set(intBarsAgo, this._dsYPixelOffsetHigh[intBarsAgo] + this._intPixelOffset * tdsh.Multiplier);
				this._intYPixelOffset = (int)this._dsYPixelOffsetHigh[intBarsAgo];
			}
			DrawText(this._intObjectCount.ToString(), 
				false, 
				tdsh.ComboCount.ToString(), 
				intBarsAgo, 
				this._doubleYLocation, 
				this._intYPixelOffset, // yPixelOffset
				colorColor, 
				new Font(FontFamily.GenericSansSerif, intFontSize, FontStyle.Bold, GraphicsUnit.Pixel), 
				StringAlignment.Center, 
				Color.Transparent, 
				Color.Transparent, 
				0); 
		}
		
		private void TDComboDrawText(TDSetupHelper2 tdsh, int intBarsAgo, Color colorBackground, Color colorColor, int intFontSize)
		{
			this._intObjectCount++;
			if (tdsh.Type == TDType2.Buy)
			{
				this._dsYLow.Set(intBarsAgo, this._dsYLow[intBarsAgo] + this._doublePointOffset * tdsh.Multiplier);
				this._doubleYLocation = this.Low[intBarsAgo];
				this._dsYPixelOffsetLow.Set(intBarsAgo, this._dsYPixelOffsetLow[intBarsAgo] + this._intPixelOffset * tdsh.Multiplier);
				this._intYPixelOffset = (int)this._dsYPixelOffsetLow[0];
			}
			else
			{
				this._dsYHigh.Set(intBarsAgo, this._dsYHigh[intBarsAgo] + this._doublePointOffset * tdsh.Multiplier);
				this._doubleYLocation = this.High[intBarsAgo];
				this._dsYPixelOffsetHigh.Set(this._dsYPixelOffsetHigh[intBarsAgo] + this._intPixelOffset * tdsh.Multiplier);
				this._intYPixelOffset = (int)this._dsYPixelOffsetHigh[intBarsAgo];
			}
			DrawText(this._intObjectCount.ToString(), 
				false, 
				tdsh.ComboCount.ToString(), 
				intBarsAgo, 
				this._doubleYLocation, 
				this._intYPixelOffset, // yPixelOffset
				colorColor, 
				new Font(FontFamily.GenericSansSerif, intFontSize, FontStyle.Bold, GraphicsUnit.Pixel), 
				StringAlignment.Center, 
				colorBackground, 
				colorBackground, 
				8); 
		}
		
		private void TDComboDrawText(TDSetupHelper2 tdsh, string strText, int intBarsAgo, Color colorBackground, Color colorColor, int intFontSize)
		{
			this._intObjectCount++;
			if (tdsh.Type == TDType2.Buy)
			{
				this._dsYLow.Set(intBarsAgo, this._dsYLow[intBarsAgo] + this._doublePointOffset * tdsh.Multiplier);
				this._doubleYLocation = this.Low[intBarsAgo];
				this._dsYPixelOffsetLow.Set(intBarsAgo, this._dsYPixelOffsetLow[intBarsAgo] + this._intPixelOffset * tdsh.Multiplier);
				this._intYPixelOffset = (int)this._dsYPixelOffsetLow[intBarsAgo];
			}
			else
			{
				this._dsYHigh.Set(intBarsAgo, this._dsYHigh[intBarsAgo] + this._doublePointOffset * tdsh.Multiplier);
				this._doubleYLocation = this.High[intBarsAgo];
				this._dsYPixelOffsetHigh.Set(intBarsAgo, this._dsYPixelOffsetHigh[intBarsAgo] + this._intPixelOffset * tdsh.Multiplier);
				this._intYPixelOffset = (int)this._dsYPixelOffsetHigh[intBarsAgo];
			}
			DrawText(this._intObjectCount.ToString(), 
				false, 
				strText, 
				intBarsAgo, 
				this._doubleYLocation, 
				this._intYPixelOffset, // yPixelOffset
				colorColor, 
				new Font(FontFamily.GenericSansSerif, intFontSize, FontStyle.Bold, GraphicsUnit.Pixel), 
				StringAlignment.Center, 
				colorBackground,
				colorBackground, 
				8); 
		}
		#endregion
		
//		#endregion
		
		#region Private Properties
		private bool BearishTDPriceFlipOccurred
		{
			get { return this._boolBearishTDPriceFlipOccurred; }
			set { this._boolBearishTDPriceFlipOccurred = value; }
		}
		private bool BullishTDPriceFlipOccurred
		{
			get { return this._boolBullishTDPriceFlipOccurred; }
			set { this._boolBullishTDPriceFlipOccurred = value; }
		}
		/// <summary>
		/// (page 2 of Bloomberg DeMark Indicators book)
		/// </summary>
		/// <returns></returns>
		private bool IsBearishFlip
		{
			get { return Close[0] < Close[this._intSetupLookBackBars] && Close[1] > Close[this._intSetupLookBackBars + 1]; }
		}
		/// <summary>
		/// (page 4 of Bloomberg DeMark Indicators book)
		/// </summary>
		/// <returns></returns>
		private bool IsBullishFlip
		{
			get { return Close[0] > Close[this._intSetupLookBackBars] && Close[1] < Close[this._intSetupLookBackBars + 1]; }
		}
		/// <summary>
		/// (page 3 of Bloomberg DeMark Indicators book)
		/// </summary>
		/// <returns></returns>
		private bool IsBarTDBuySetup
		{
			get
			{
				// need to be a for loop
				bool boolReturn = true;
				for (Int32 int1 = 0; int1 < this._intSetupBars; int1++)
				{
					boolReturn = Close[int1] >= Close[this._intSetupLookBackBars + int1];
					if (boolReturn == false) break;
				}
				return boolReturn;
				//return Close[0] < Close[this._intSetupLookBackBars] &&
				//	Close[1] < Close[this._intSetupLookBackBars + 1] &&
				//	Close[2] < Close[this._intSetupLookBackBars + 2] &&
				//	Close[3] < Close[this._intSetupLookBackBars + 3] &&
				//	Close[4] < Close[this._intSetupLookBackBars + 4] &&
				//	Close[5] < Close[this._intSetupLookBackBars + 5] &&
				//	Close[6] < Close[this._intSetupLookBackBars + 6] &&
				//	Close[7] < Close[this._intSetupLookBackBars + 7] &&
				//	Close[8] < Close[this._intSetupLookBackBars + 8];
			}
		}
		/// <summary>
		/// (page 4 of Bloomberg DeMark Indicators book)
		/// </summary>
		/// <returns></returns>
		private bool IsBarTDSellSetup
		{
			get
			{
				// need to be a for loop
				bool boolReturn = true;
				for (Int32 int1 = 0; int1 < this._intSetupBars; int1++)
				{
					boolReturn = Close[int1] <= Close[this._intSetupLookBackBars + int1];
					if (boolReturn == false) break;
				}
				return boolReturn;
				//return Close[0] > Close[this._intSetupLookBackBars] &&
				//	Close[1] > Close[this._intSetupLookBackBars + 1] &&
				//	Close[2] > Close[this._intSetupLookBackBars + 2] &&
				//	Close[3] > Close[this._intSetupLookBackBars + 3] &&
				//	Close[4] > Close[this._intSetupLookBackBars + 4] &&
				//	Close[5] > Close[this._intSetupLookBackBars + 5] &&
				//	Close[6] > Close[this._intSetupLookBackBars + 6] &&
				//	Close[7] > Close[this._intSetupLookBackBars + 7] &&
				//	Close[8] > Close[this._intSetupLookBackBars + 8];
			}
		}
		#endregion
		
		#region Public Properties
		
		#region Parameters
		//[Description("Draw price flip arrow.")]
		//[Category("1-Parameters")]
		//[Gui.Design.DisplayName("Draw price flip arrow")]
		//public bool DrawPriceFlipArrow
		//{
		//	get { return this._boolDrawPriceFlipArrow; }
		//	set { this._boolDrawPriceFlipArrow = value; }
		//}		
		
		[Description("Plot Setup Countdown Buy.")]
		[Category("1-Parameters")]
		[Gui.Design.DisplayName("a) Plot Setup Countdown Buy")]
		public bool PlotSetupCountDownBuy
		{
			get { return this._boolPlotSetupCountDownBuy; }
			set { this._boolPlotSetupCountDownBuy = value; }
		}
		
		[Description("Plot Setup Countdown Sell.")]
		[Category("1-Parameters")]
		[Gui.Design.DisplayName("b) Plot Setup Countdown Sell")]
		public bool PlotSetupCountDownSell
		{
			get { return this._boolPlotSetupCountDownSell; }
			set { this._boolPlotSetupCountDownSell = value; }
		}

		[Description("Plot TDST.")]
		[Category("1-Parameters")]
		[Gui.Design.DisplayName("c) Plot TDST")]
		public bool PlotTDST
		{
			get { return this._boolPlotTDST; }
			set { this._boolPlotTDST = value; }
		}

		[Description("Plot Sequential Countdown Buy.")]
		[Category("1-Parameters")]
		[Gui.Design.DisplayName("d) Plot Sequential Countdown Buy")]
		public bool PlotSequentialCountdownBuy
		{
			get { return this._boolPlotSequentialCountdownBuy; }
			set { this._boolPlotSequentialCountdownBuy = value; }
		}

		[Description("Plot Sequential Countdown Sell.")]
		[Category("1-Parameters")]
		[Gui.Design.DisplayName("e) Plot Sequential Countdown Sell")]
		public bool PlotSequentialCountdownSell
		{
			get { return this._boolPlotSequentialCountdownSell; }
			set { this._boolPlotSequentialCountdownSell = value; }
		}

		[Description("Plot Combo Countdown Buy.")]
		[Category("1-Parameters")]
		[Gui.Design.DisplayName("f) Plot Combo Countdown Buy")]
		public bool PlotComboCountdownBuy
		{
			get { return this._boolPlotComboCountdownBuy; }
			set { this._boolPlotComboCountdownBuy = value; }
		}

		[Description("Plot Combo Countdown Sell.")]
		[Category("1-Parameters")]
		[Gui.Design.DisplayName("g) Plot Combo Countdown Sell")]
		public bool PlotComboCountdownSell
		{
			get { return this._boolPlotComboCountdownSell; }
			set { this._boolPlotComboCountdownSell = value; }
		}

		[Description("Y Pixel Offset.")]
		[Category("1-Parameters")]
		[Gui.Design.DisplayName("h) Y Pixel Offset")]
		public int PixelOffset
		{
			get { return this._intPixelOffset; }
			set { this._intPixelOffset = value; }
		}
		#endregion
		
		#region Setup
		[Description("Number of bars needed (minimum of 5 bars).")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("a) Bars needed ")]
		public int SetupBars
		{
			get { return _intSetupBars; }
			set { _intSetupBars = Math.Max(5, value); }
		}

		[Description("Look back bars (minimum of 2 bars).")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("b) Look back bars")]
		public int SetupLookBackBars
		{
			get { return _intSetupLookBackBars; }
			set { _intSetupLookBackBars = Math.Max(2, value); }
		}
		
		[Description("Continue countdown after minimum bars.")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("c) Continue countdown after minimum bars")]
		public bool PlotSetupCountDownAfterMinBars
		{
			get { return this._boolPlotSetupCountDownAfterMinBars; }
			set { this._boolPlotSetupCountDownAfterMinBars = value; }
		}		
	
		[Description("Plot type.")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("d) Plot type")]
		public TDSequentialPlotType2 PlotType
		{
			get { return _TDSequentialPlotType2; }
			set { _TDSequentialPlotType2 = value; }
		}
	
		[XmlIgnore()]
		[Description("Font color.")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("e) Font color")]
		public Color SetupFontColor
		{
			get { return _colorSetupFontColor; }
			set { _colorSetupFontColor = value; }
		}
		[Browsable(false)]
		public string SetupFontColorSerialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(_colorSetupFontColor); }
			set { _colorSetupFontColor = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}
	
		[Description("Font size.")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("f) Font size")]
		public int SetupFontSize
		{
			get { return this._intSetupFontSize; }
			set { this._intSetupFontSize = Math.Min(Math.Max(5, value), 21); }
		}
		
		[Description("Background color bar 1.")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("g) BG color bar 1")]
		public Color SetupBackgroundColorBar1
		{
			get { return this._colorSetupBackgroundColorBar1; }
			set { this._colorSetupBackgroundColorBar1 = value; }
		}
		[Browsable(false)]
		public string SetupBackgroundColorBar1Serialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorSetupBackgroundColorBar1); }
			set { this._colorSetupBackgroundColorBar1 = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}
		
		[XmlIgnore()]
		[Description("Font color bar 1.")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("h) Font color bar 1")]
		public Color SetupFontColorBar1
		{
			get { return _colorSetupFontColorBar1; }
			set { _colorSetupFontColorBar1 = value; }
		}
		[Browsable(false)]
		public string SetupFontColorBar1Serialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(_colorSetupFontColorBar1); }
			set { _colorSetupFontColorBar1 = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}

		[Description("Font size.")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("i) Font size bar 1")]
		public int SetupFontSizeBar1
		{
			get { return this._intSetupFontSizeBar1; }
			set { this._intSetupFontSizeBar1 = Math.Min(Math.Max(5, value), 21); }
		}
		
		[Description("Background color bar 9.")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("j) BG color bar 9")]
		public Color SetupBackgroundColorBar9
		{
			get { return this._colorSetupBackgroundColorBar9; }
			set { this._colorSetupBackgroundColorBar9 = value; }
		}
		[Browsable(false)]
		public string SetupBackgroundColorBar9Serialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorSetupBackgroundColorBar9); }
			set { this._colorSetupBackgroundColorBar9 = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}

		[XmlIgnore()]
		[Description("Font color bar 9.")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("k) Font color bar 9")]
		public Color SetupFontColorBar9
		{
			get { return _colorSetupFontColorBar9; }
			set { _colorSetupFontColorBar9 = value; }
		}
		[Browsable(false)]
		public string SetupFontColorBar9Serialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(_colorSetupFontColorBar9); }
			set { _colorSetupFontColorBar9 = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}

		[Description("Font size.")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("l) Font size bar 9")]
		public int SetupFontSizeBar9
		{
			get { return this._intSetupFontSizeBar9; }
			set { this._intSetupFontSizeBar9 = Math.Min(Math.Max(5, value), 21); }
		}

		[Description("Draw perfect signal arrow.")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("m) Draw perfect signal arrow")]
		public bool DrawPerfectSignalArrow
		{
			get { return this._boolDrawPerfectSignalArrow; }
			set { this._boolDrawPerfectSignalArrow = value; }
		}		

		[XmlIgnore()]
		[Description("Perfect signal buy color.")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("n) Perfect signal buy color")]
		public Color PerfectSignalBuyColor
		{
			get { return this._colorPerfectSignalBuyColor; }
			set { this._colorPerfectSignalBuyColor = value; }
		}
		[Browsable(false)]
		public string PerfectSignalBuyColorSerialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorPerfectSignalBuyColor); }
			set { this._colorPerfectSignalBuyColor = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}

		[XmlIgnore()]
		[Description("Perfect signal sell color.")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("o) Perfect signal sell color")]
		public Color PerfectSignalSellColor
		{
			get { return this._colorPerfectSignalSellColor; }
			set { this._colorPerfectSignalSellColor = value; }
		}
		[Browsable(false)]
		public string PerfectSignalSellColorSerialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorPerfectSignalSellColor); }
			set { this._colorPerfectSignalSellColor = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}
		
		[Description("Cancel if High above Highest Close of the entire Buy Setup or Low below Lowest Close of the entire Sell Setup.")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("p) Cancel if HaHC or LbLC")]
		public bool CancelHaHC_LbLC
		{
			get { return this._boolSetupCancelHaHC_LbLC; }
			set { this._boolSetupCancelHaHC_LbLC = value; }
		}		
		
		[Description("Cancel if High above Highest High of the entire Buy Setup or Low below Lowest Low of the entire Sell Setup.")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("q) Cancel if HaHH or LbLL")]
		public bool CancelHaHH_LbLL
		{
			get { return this._boolSetupCancelHaHH_LbLL; }
			set { this._boolSetupCancelHaHH_LbLL = value; }
		}		
		
		[Description("Cancel if Close above Highest Close of the entire Buy Setup or Close below Lowest Close of the entire Sell Setup.")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("r) Cancel if CaHC or CbLC")]
		public bool CancelCaHC_CbLC
		{
			get { return this._boolSetupCancelCaHC_CbLC; }
			set { this._boolSetupCancelCaHC_CbLC = value; }
		}		
		
		[Description("Cancel if Close above Highest High of the entire Buy Setup or Close below Lowest Low of the entire Sell Setup.")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("s) Cancel if CaHH or CbLL")]
		public bool CancelCaHH_CbLL
		{
			get { return this._boolSetupCancelCaHH_CbLL; }
			set { this._boolSetupCancelCaHH_CbLL = value; }
		}		
		
		[Description("Cancel if Close above Highest True High of the entire Buy Setup or Close below Lowest True Low of the entire Sell Setup.")]
		[Category("2-Setup Countdown Parameters")]
		[Gui.Design.DisplayName("t) Cancel if CaHTH or CbLTL")]
		public bool CancelCaHTH_CbLTL
		{
			get { return this._boolSetupCancelCaHTH_CbLTL; }
			set { this._boolSetupCancelCaHTH_CbLTL = value; }
		}		
		#endregion
		
		#region Setup TDST
		[Description("Maximun number on chart, use 0 to plot all.")]
		[Category("3-Setup TDST Parameters")]
		[Gui.Design.DisplayName("a) Maximun number")]
		public int TDSTMaxNumber
		{
			get { return this._intSetupTDSTMaxNumber; }
			set { this._intSetupTDSTMaxNumber = Math.Min(Math.Max(0, value), Int32.MaxValue); }
		}

		[XmlIgnore()]
		[Description("Dash Style.")]
		[Category("3-Setup TDST Parameters")]
		[Gui.Design.DisplayName("b) Dash Style")]
		public DashStyle DashStyle
		{
			get { return this._dsTDST; }
			set { this._dsTDST = value; }
		}

		[XmlIgnore()]
		[Description("Resistance Color.")]
		[Category("3-Setup TDST Parameters")]
		[Gui.Design.DisplayName("c) Resistance Color")]
		public Color TDSTResistanceColor
		{
			get { return this._colorSetupTDSTResistance; }
			set { this._colorSetupTDSTResistance = value; }
		}
		[Browsable(false)]
		public string TDSTResistanceColorSerialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorSetupTDSTResistance); }
			set { this._colorSetupTDSTResistance = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}

		[XmlIgnore()]
		[Description("Support Color.")]
		[Category("3-Setup TDST Parameters")]
		[Gui.Design.DisplayName("d) Support Color")]
		public Color TDSTSupportColor
		{
			get { return this._colorSetupTDSTSupport; }
			set { this._colorSetupTDSTSupport = value; }
		}
		[Browsable(false)]
		public string TDSTSupportColorSerialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorSetupTDSTSupport); }
			set { this._colorSetupTDSTSupport = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}

		[Description("Width (0-8).")]
		[Category("3-Setup TDST Parameters")]
		[Gui.Design.DisplayName("e) Width")]
		public int Width
		{
			get { return this._intSetupWidth; }
			set { this._intSetupWidth = Math.Min(Math.Max(0, value), 8); }
		}

		[Description("Buy Q1OR.")]
		[Category("3-Setup TDST Parameters")]
		[Gui.Design.DisplayName("f) Buy Q1OR")]
		public bool BuyQ1OR
		{
			get { return this._boolSetupQ1ORBuy; }
			set { this._boolSetupQ1ORBuy = value; }
		}

		[Description("Sell Q1OR.")]
		[Category("3-Setup TDST Parameters")]
		[Gui.Design.DisplayName("g) Sell Q1OR")]
		public bool SellQ1OR
		{
			get { return this._boolSetupQ1ORSell; }
			set { this._boolSetupQ1ORSell = value; }
		}
		#endregion
		
		#region Sequential
        [Description("Number of bars needed (minimum of 5 bars).")]
        [Category("4-Sequential Countdown Parameters")]
        [Gui.Design.DisplayName("a) Bars needed ")]
        public int SequentialBars
        {
            get { return this._intSequentialBars; }
            set { this._intSequentialBars = Math.Max(5, value); }
        }

        [Description("Look back bars (minimum of 2 bars).")]
        [Category("4-Sequential Countdown Parameters")]
        [Gui.Design.DisplayName("b) Look back bars")]
        public int SequentialLookBackBars
        {
            get { return this._intSequentialLookBackBars; }
            set { this._intSequentialLookBackBars = Math.Max(2, value); }
        }


		[Description("Bar 8 versus bar 5 qualifier.")]
		[Category("4-Sequential Countdown Parameters")]
		[Gui.Design.DisplayName("c) Bar 8 versus bar 5 qualifier")]
		public bool SequentialBar8VersusBar5Qualifier
		{
			get { return this._boolSequentialBar8VersusBar5Qualifier; }
			set { this._boolSequentialBar8VersusBar5Qualifier = value; }
		}

		[Description("Bar 13 versus bar 8 qualifier.")]
		[Category("4-Sequential Countdown Parameters")]
		[Gui.Design.DisplayName("d) Bar 13 versus bar 8 qualifier")]
		public bool SequentialBar13VersusBar8Qualifier
		{
			get { return this._boolSequentialBar13VersusBar8Qualifier; }
			set { this._boolSequentialBar13VersusBar8Qualifier = value; }
		}

        [Description("Number of Setup bars needed for a recycle.")]
        [Category("4-Sequential Countdown Parameters")]
        [Gui.Design.DisplayName("e) Recycle count")]
        public int SequentialRecycleBars
        {
            get { return this._intSequentialRecycleBars; }
            set { this._intSequentialRecycleBars = value; }
        }

        [Description("This refers to the 13th count where instead of comparing the close vs the low of 2 bars earlier, you compare the open vs the low of 2 bars earlier.")]
        [Category("4-Sequential Countdown Parameters")]
        [Gui.Design.DisplayName("f) Termination count")]
        public TDTerminationCount2 TerminationCount
        {
            get { return this._TDTerminationCount2; }
            set { this._TDTerminationCount2 = value; }
        }

        [XmlIgnore()]
        [Description("Font color.")]
        [Category("4-Sequential Countdown Parameters")]
        [Gui.Design.DisplayName("g) Font color")]
        public Color SequentialFontColor
        {
            get { return this._colorSequentialFontColor; }
            set { this._colorSequentialFontColor = value; }
        }
        [Browsable(false)]
        public string SequentialFontColorSerialize
        {
            get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorSequentialFontColor); }
            set { this._colorSequentialFontColor = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
        }

        [Description("Font size.")]
        [Category("4-Sequential Countdown Parameters")]
        [Gui.Design.DisplayName("h) Font size")]
        public int SequentialFontSize
        {
            get { return this._intSequentialFontSize; }
            set { this._intSequentialFontSize = Math.Min(Math.Max(5, value), 21); }
        }

		[Description("Background color bar 1.")]
		[Category("4-Sequential Countdown Parameters")]
		[Gui.Design.DisplayName("i) BG color bar 1")]
		public Color SequentialBackgroundColorBar1
		{
			get { return this._colorSequentialBackgroundColorBar1; }
			set { this._colorSequentialBackgroundColorBar1 = value; }
		}
		[Browsable(false)]
		public string SequentialBackgroundColorBar1Serialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorSequentialBackgroundColorBar1); }
			set { this._colorSequentialBackgroundColorBar1 = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}

        [Description("Font color bar 1.")]
        [Category("4-Sequential Countdown Parameters")]
        [Gui.Design.DisplayName("j) Font color bar 1")]
        public Color SequentialFontColorBar1
        {
            get { return this._colorSequentialFontColorBar1; }
            set { this._colorSequentialFontColorBar1 = value; }
        }
        [Browsable(false)]
        public string SequentialFontColorBar1Serialize
        {
            get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorSequentialFontColorBar1); }
            set { this._colorSequentialFontColorBar1 = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
        }

        [Description("Font size bar 1.")]
        [Category("4-Sequential Countdown Parameters")]
        [Gui.Design.DisplayName("k) Font size bar 1")]
        public int SequentialFontSizeBar1
        {
            get { return this._intSequentialFontSizeBar1; }
            set { this._intSequentialFontSizeBar1 = Math.Min(Math.Max(5, value), 21); }
        }

		[Description("Background color bar 13.")]
		[Category("4-Sequential Countdown Parameters")]
		[Gui.Design.DisplayName("l) BG color bar 13")]
		public Color SequentialBackgroundColorBar13
		{
			get { return this._colorSequentialBackgroundColorBar13; }
			set { this._colorSequentialBackgroundColorBar13 = value; }
		}
		[Browsable(false)]
		public string SequentialBackgroundColorBar13Serialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorSequentialBackgroundColorBar13); }
			set { this._colorSequentialBackgroundColorBar13 = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}

        [Description("Font color bar 13.")]
        [Category("4-Sequential Countdown Parameters")]
        [Gui.Design.DisplayName("m) Font color bar 13")]
        public Color SequentialFontColorBar13
        {
            get { return this._colorSequentialFontColorBar13; }
            set { this._colorSequentialFontColorBar13 = value; }
        }
        [Browsable(false)]
        public string SequentialFontColorBar13Serialize
        {
            get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorSequentialFontColorBar13); }
            set { this._colorSequentialFontColorBar13 = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
        }

        [Description("Font size bar 13.")]
        [Category("4-Sequential Countdown Parameters")]
        [Gui.Design.DisplayName("n) Font size bar 13")]
        public int SequentialFontSizeBar13
        {
            get { return this._intSequentialFontSizeBar13; }
            set { this._intSequentialFontSizeBar13 = Math.Min(Math.Max(5, value), 21); }
        }

		[Description("Background color bar Recycle.")]
		[Category("4-Sequential Countdown Parameters")]
		[Gui.Design.DisplayName("o) BG color bar Recycle")]
		public Color SequentialBackgroundColorBarRecycle
		{
			get { return this._colorSequentialBackgroundColorBarRecycle; }
			set { this._colorSequentialBackgroundColorBarRecycle = value; }
		}
		[Browsable(false)]
		public string SequentialBackgroundColorBarRecycleSerialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorSequentialBackgroundColorBarRecycle); }
			set { this._colorSequentialBackgroundColorBarRecycle = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}
		
		[Description("Font color bar Recycle.")]
		[Category("4-Sequential Countdown Parameters")]
		[Gui.Design.DisplayName("p) Font color bar Recycle")]
		public Color SequentialFontColorBarRecycle
		{
			get { return this._colorSequentialFontColorBarRecycle; }
			set { this._colorSequentialFontColorBarRecycle = value; }
		}
		[Browsable(false)]
		public string SequentialFontColorBarRecycleSerialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorSequentialFontColorBarRecycle); }
			set { this._colorSequentialFontColorBarRecycle = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}

		[Description("Font size bar Recycle.")]
		[Category("4-Sequential Countdown Parameters")]
		[Gui.Design.DisplayName("q) Font size bar Recycle")]
		public int SequentialFontSizeBarRecycle
		{
			get { return this._intSequentialFontSizeBarRecycle; }
			set { this._intSequentialFontSizeBarRecycle = Math.Min(Math.Max(5, value), 21); }
		}

        [Description("This option cancel countdowns (sequential and combo) in progress once the minimum required count of a setup in the opposite direction is met.")]
        [Category("4-Sequential Countdown Parameters")]
        [Gui.Design.DisplayName("r) Cancel if reverse setup")]
        public bool SequentialCancelReverseSetup
        {
            get { return this._boolSequentialCancelReverseSetup; }
            set { this._boolSequentialCancelReverseSetup = value; }
        }		

        [Description("This option cancel sequential countdowns in progress if a true low is formed above the TDST Buy Line for a Buy Countdown or a true high is formed below the TDST Sell Line for a Sell Countdown.")]
        [Category("4-Sequential Countdown Parameters")]
        [Gui.Design.DisplayName("s) Cancel TDST")]
        public bool SequentialCancelTDST
        {
            get { return this._boolSequentialCancelTDST; }
            set { this._boolSequentialCancelTDST = value; }
        }		
		#endregion
		
		#region Combo
        [Description("Number of bars needed (minimum of 8 bars).")]
        [Category("5-Combo Countdown Parameters")]
        [Gui.Design.DisplayName("a) Bars needed ")]
        public int ComboBars
        {
            get { return this._intComboBars; }
            set { this._intComboBars = Math.Max(8, value); }
        }

        [Description("Look back bars (minimum of 2 bars).")]
        [Category("5-Combo Countdown Parameters")]
        [Gui.Design.DisplayName("b) Look back bars")]
        public int ComboLookBackBars
        {
            get { return this._intComboLookBackBars; }
            set { this._intComboLookBackBars = Math.Max(2, value); }
        }

        [Description("Number of Setup bars needed for a recycle.")]
        [Category("5-Combo Countdown Parameters")]
        [Gui.Design.DisplayName("c) Recycle count")]
        public int ComboRecycleBars
        {
            get { return this._intComboRecycleBars; }
            set { this._intComboRecycleBars = value; }
        }

        [XmlIgnore()]
        [Description("Font color.")]
        [Category("5-Combo Countdown Parameters")]
        [Gui.Design.DisplayName("d) Font color")]
        public Color ComboFontColor
        {
            get { return this._colorComboFontColor; }
            set { this._colorComboFontColor = value; }
        }
        [Browsable(false)]
        public string ComboFontColorSerialize
        {
            get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorComboFontColor); }
            set { this._colorComboFontColor = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
        }

        [Description("Font size.")]
        [Category("5-Combo Countdown Parameters")]
        [Gui.Design.DisplayName("e) Font size")]
        public int ComboFontSize
        {
            get { return this._intComboFontSize; }
            set { this._intComboFontSize = Math.Min(Math.Max(5, value), 21); }
        }

        [Description("Background color bar 1.")]
        [Category("5-Combo Countdown Parameters")]
        [Gui.Design.DisplayName("f) BG color bar 1")]
        public Color ComboBackgroundColorBar1
        {
            get { return this._colorComboBackgroundColorBar1; }
            set { this._colorComboBackgroundColorBar1 = value; }
        }
        [Browsable(false)]
        public string ComboBackgroundColorBar1Serialize
        {
            get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorComboBackgroundColorBar1); }
            set { this._colorComboBackgroundColorBar1 = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
        }

        [XmlIgnore()]
        [Description("Font color bar 1.")]
        [Category("5-Combo Countdown Parameters")]
        [Gui.Design.DisplayName("g) Font color bar 1")]
        public Color ComboFontColorBar1
        {
            get { return this._colorComboFontColorBar1; }
            set { this._colorComboFontColorBar1 = value; }
        }
        [Browsable(false)]
        public string ComboFontColorBar1Serialize
        {
            get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorComboFontColorBar1); }
            set { this._colorComboFontColorBar1 = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
        }

        [Description("Font size bar 1.")]
        [Category("5-Combo Countdown Parameters")]
        [Gui.Design.DisplayName("h) Font size bar 1")]
        public int ComboFontSizeBar1
        {
            get { return this._intComboFontSizeBar1; }
            set { this._intComboFontSizeBar1 = Math.Min(Math.Max(5, value), 21); }
        }

		[Description("Background color bar 13.")]
		[Category("5-Combo Countdown Parameters")]
		[Gui.Design.DisplayName("i) BG color bar 13")]
		public Color ComboBackgroundColorBar13
		{
			get { return this._colorComboBackgroundColorBar13; }
			set { this._colorComboBackgroundColorBar13 = value; }
		}
		[Browsable(false)]
		public string ComboBackgroundColorBar13Serialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorComboBackgroundColorBar13); }
			set { this._colorComboBackgroundColorBar13 = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}

        [XmlIgnore()]
        [Description("Font color bar 13.")]
        [Category("5-Combo Countdown Parameters")]
        [Gui.Design.DisplayName("j) Font color bar 13")]
        public Color ComboFontColorBar13
        {
            get { return this._colorComboFontColorBar13; }
            set { this._colorComboFontColorBar13 = value; }
        }
        [Browsable(false)]
        public string ComboFontColorBar13Serialize
        {
            get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorComboFontColorBar13); }
            set { this._colorComboFontColorBar13 = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
        }

        [Description("Font size bar 13.")]
        [Category("5-Combo Countdown Parameters")]
        [Gui.Design.DisplayName("k) Font size bar 13")]
        public int ComboFontSizeBar13
        {
            get { return this._intComboFontSizeBar13; }
            set { this._intComboFontSizeBar13 = Math.Min(Math.Max(5, value), 21); }
        }

		[Description("Background color bar Recycle.")]
		[Category("5-Combo Countdown Parameters")]
		[Gui.Design.DisplayName("l) BG color bar Recycle")]
		public Color ComboBackgroundColorBarRecycle
		{
			get { return this._colorComboBackgroundColorBarRecycle; }
			set { this._colorComboBackgroundColorBarRecycle = value; }
		}
		[Browsable(false)]
		public string ComboBackgroundColorBarRecycleSerialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorComboBackgroundColorBarRecycle); }
			set { this._colorComboBackgroundColorBarRecycle = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}

		[XmlIgnore()]
		[Description("Font color bar Recycle.")]
		[Category("5-Combo Countdown Parameters")]
		[Gui.Design.DisplayName("m) Font color bar Recycle")]
		public Color ComboFontColorBarRecycle
		{
			get { return this._colorComboFontColorBarRecycle; }
			set { this._colorComboFontColorBarRecycle = value; }
		}
		[Browsable(false)]
		public string ComboFontColorBarRecycleSerialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(this._colorComboFontColorBarRecycle); }
			set { this._colorComboFontColorBarRecycle = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}

		[Description("Font size bar Recycle.")]
		[Category("5-Combo Countdown Parameters")]
		[Gui.Design.DisplayName("n) Font size bar Recycle")]
		public int ComboFontSizeBarRecycle
		{
			get { return this._intComboFontSizeBarRecycle; }
			set { this._intComboFontSizeBarRecycle = Math.Min(Math.Max(5, value), 21); }
		}
		#endregion
		
		#endregion
	}
}

public enum TDSequentialPlotType2
{
	DrawDots,
	DrawText
}

public enum TDTerminationCount2
{
	Close,
	Open
}

public enum TDType2
{
	Buy,
	Sell,
	Null
}

public class TDSetupHelper2 
{
	public bool Cancelled;
	public bool Completed;
	public bool ComboCountdownCancelled;
	public bool ComboCountdownCompleted;
	public bool ComboCountdownInProgress;
	public bool InProgress;
	public bool PlotSetupCountDownAfterMinBars;
	public bool SearchForPerfectSignal;
	public bool SequentialCountdownCancelled;
	public bool SequentialCountdownCompleted;
    public bool SequentialCountdownInProgress;
    public bool SequentialCountdownIsRecycle;
    public bool SequentialCountdownRecycled;
	
	public double Bar1High; 
	public double Bar6High; 
	public double Bar7High; 
	public double Bar8or9High; 
	public double Bar1Low; 
	public double Bar6Low; 
	public double Bar7Low; 
	public double Bar8or9Low;

	public double LastComboClose;
	public double LastComboHigh;
	public double LastComboLow;

	public double Multiplier;
	public double SequentialBar5Close; 
	public double SequentialBar8Close; 
	public double SequentialBar13YLocation; 
	
	public int StartBar; 
	public double HighestHigh; 
	public double LowestLow; 
	public double TrueHigh; 
	public double TrueLow; 

	public int ComboCount;
	public int Count;
	public int ID;
	public int SequentialBar13;
	public int SequentialBar13YPixelOffsetLow; 
	public int SequentialCount;
	public int SequentialTagsIndex;

	public string[] ComboTags; 
	public string[] SequentialTags;
	public string[] Tags;

	public object CountdownToRecycle;
	public TDType2 Type;

	public TDSetupHelper2(Int32 intID, TDType2 TDType2, bool boolPlotSetupCountDownAfterMinBars)
	{
		Type = TDType2;

		Cancelled = false;
		Completed = false;
		ComboCountdownCancelled = false;
		ComboCountdownCompleted = false;
		ComboCountdownInProgress = false;
		InProgress = true;
		PlotSetupCountDownAfterMinBars = boolPlotSetupCountDownAfterMinBars;
		SearchForPerfectSignal = false;
		SequentialCountdownCancelled = false;
		SequentialCountdownCompleted = false;
        SequentialCountdownInProgress = false;
        SequentialCountdownIsRecycle = false;
        SequentialCountdownRecycled = false;

		Bar1High = 0.0; 
		Bar6High = 0.0; 
		Bar7High = 0.0; 
		Bar8or9High = 0.0;
		Bar1Low = 0.0; 
		Bar6Low = 0.0; 
		Bar7Low = 0.0; 
		Bar8or9Low = 0.0;
		
		if (TDType2 == TDType2.Buy)
		{
			LastComboClose = double.MaxValue;
			LastComboHigh = 0.0;
			LastComboLow = double.MaxValue;
			Multiplier = -1.0;			
		}
		else
		{
			LastComboClose = double.MinValue;
			LastComboHigh = double.MinValue;
			LastComboLow = 0.0;
			Multiplier = 1.0;			
		}

		SequentialBar5Close = 0.0;
		SequentialBar8Close = 0.0;
		SequentialBar13YLocation = 0.0;
		StartBar = 0;
		HighestHigh = double.MinValue;
		LowestLow = double.MaxValue;
		TrueHigh = double.MinValue;
		TrueLow = double.MaxValue;
		
		ComboCount = 0;		
		Count = 0;
		ID = intID;
		SequentialBar13 = 0;
		SequentialBar13YPixelOffsetLow = 0;
		SequentialCount = 0;
		SequentialTagsIndex = 0;
		
		ComboTags = new String[144];
		SequentialTags = new String[144];
		Tags = new String[144];
		
		CountdownToRecycle = null;
	}
}

public struct TDSTHelper2 
{
	public TDType2 Type;
	public bool Completed;
	public string Tag;
	public int StartBar;
	public double StartY;
	public int EndBar;
	public double EndY;
	public Color Color;
	public DashStyle DashStyle;
	public int Width;
	
	public TDSTHelper2(TDType2 TDType2, bool boolCompleted, string strTag, int intStartBar, double doubleStartY, int intEndBar, double doubleEndY, Color colorTDST, DashStyle dsTDST, int intWidth) 
	{
		Type = TDType2;
		Completed = boolCompleted;
		Tag = strTag;
		StartBar = intStartBar;    
		StartY = doubleStartY;    
		EndBar = intEndBar;    
		EndY = doubleEndY;    
		Color = colorTDST;    
		DashStyle = dsTDST;    
		Width = intWidth;    
	}
}

#region NinjaScript generated code. Neither change nor remove.
// This namespace holds all indicators and is required. Do not change it.
namespace NinjaTrader.Indicator
{
    public partial class Indicator : IndicatorBase
    {
        private IGTDSequential2[] cacheIGTDSequential2 = null;

        private static IGTDSequential2 checkIGTDSequential2 = new IGTDSequential2();

        /// <summary>
        /// TD Sequential/Combo Countdown
        /// </summary>
        /// <returns></returns>
        public IGTDSequential2 IGTDSequential2()
        {
            return IGTDSequential2(Input);
        }

        /// <summary>
        /// TD Sequential/Combo Countdown
        /// </summary>
        /// <returns></returns>
        public IGTDSequential2 IGTDSequential2(Data.IDataSeries input)
        {
            if (cacheIGTDSequential2 != null)
                for (int idx = 0; idx < cacheIGTDSequential2.Length; idx++)
                    if (cacheIGTDSequential2[idx].EqualsInput(input))
                        return cacheIGTDSequential2[idx];

            lock (checkIGTDSequential2)
            {
                if (cacheIGTDSequential2 != null)
                    for (int idx = 0; idx < cacheIGTDSequential2.Length; idx++)
                        if (cacheIGTDSequential2[idx].EqualsInput(input))
                            return cacheIGTDSequential2[idx];

                IGTDSequential2 indicator = new IGTDSequential2();
                indicator.BarsRequired = BarsRequired;
                indicator.CalculateOnBarClose = CalculateOnBarClose;
#if NT7
                indicator.ForceMaximumBarsLookBack256 = ForceMaximumBarsLookBack256;
                indicator.MaximumBarsLookBack = MaximumBarsLookBack;
#endif
                indicator.Input = input;
                Indicators.Add(indicator);
                indicator.SetUp();

                IGTDSequential2[] tmp = new IGTDSequential2[cacheIGTDSequential2 == null ? 1 : cacheIGTDSequential2.Length + 1];
                if (cacheIGTDSequential2 != null)
                    cacheIGTDSequential2.CopyTo(tmp, 0);
                tmp[tmp.Length - 1] = indicator;
                cacheIGTDSequential2 = tmp;
                return indicator;
            }
        }
    }
}

// This namespace holds all market analyzer column definitions and is required. Do not change it.
namespace NinjaTrader.MarketAnalyzer
{
    public partial class Column : ColumnBase
    {
        /// <summary>
        /// TD Sequential/Combo Countdown
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.IGTDSequential2 IGTDSequential2()
        {
            return _indicator.IGTDSequential2(Input);
        }

        /// <summary>
        /// TD Sequential/Combo Countdown
        /// </summary>
        /// <returns></returns>
        public Indicator.IGTDSequential2 IGTDSequential2(Data.IDataSeries input)
        {
            return _indicator.IGTDSequential2(input);
        }
    }
}

// This namespace holds all strategies and is required. Do not change it.
namespace NinjaTrader.Strategy
{
    public partial class Strategy : StrategyBase
    {
        /// <summary>
        /// TD Sequential/Combo Countdown
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.IGTDSequential2 IGTDSequential2()
        {
            return _indicator.IGTDSequential2(Input);
        }

        /// <summary>
        /// TD Sequential/Combo Countdown
        /// </summary>
        /// <returns></returns>
        public Indicator.IGTDSequential2 IGTDSequential2(Data.IDataSeries input)
        {
            if (InInitialize && input == null)
                throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'Initialize()' method");

            return _indicator.IGTDSequential2(input);
        }
    }
}
#endregion
