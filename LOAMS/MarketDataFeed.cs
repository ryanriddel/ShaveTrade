using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraPivotGrid;
using System.Threading;
using System.IO;
using MktSrvcAPI;
using System.Net.Sockets;
using System.Collections.Concurrent;

using System.Timers;

namespace LOAMS
{


    public class MarketDataFeed : IMarketDataProducer
    {
        QuoteFeed _quoteFeed = new QuoteFeed();
        TradeFeed _tradeFeed = new TradeFeed();
        FeedHandler _feedHandler = new FeedHandler();
        private bool _isConnected = false;
        private bool _isEnabled = false;


        public MarketDataFeed()
        {
            ConnectToFeed();
        }


        public void ConnectToFeed()
        {
            try
            {
                _feedHandler.InitializeClients();
                _feedHandler.ConnectToData();
                _isConnected = true;
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to initialize market data feed connection.  " + ex.Message);
            }
        }
        public void DisconnectFromFeed()
        {
            try
            {
                _feedHandler.DisconnectLevel2Data();
                _isConnected = false;
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to disconnect from market data feed connection.  " + ex.Message);
            }
        }

        public void Enable()
        {
            if (_isConnected)
            {
                _quoteFeed.FinalizeAllBuffersEvents();
                _isEnabled = true;
            }
            else
                throw new Exception("Please connect to feed before enabling market data collection.");
        }

        public void AddQuote(QuoteBook quote)
        {
            _quoteFeed.AddQuote(quote.InstrumentName, quote);
        }

        public void AddTrade(TradeInfo trade)
        {
            _tradeFeed.AddTrade(trade.InstrumentName, trade);
        }
        
        public void AddQuoteConsumer(InstrInfo instrument, IMarketDataConsumer<QuoteBook> consumer)
        {
            _quoteFeed.AddQuoteConsumer(Utilities.InstrToStr(instrument), consumer);
        }
        public void AddTradeConsumer(InstrInfo instrument, IMarketDataConsumer<TradeInfo> consumer)
        {
            _tradeFeed.AddTradeConsumer(Utilities.InstrToStr(instrument), consumer);
        }

        public void SubscribeToQuoteFeed(InstrInfo instrument)
        {
            _quoteFeed.AddQuoteBuffer(Utilities.InstrToStr(instrument));

            if(_isConnected)
                _feedHandler.subscribeToSymbolQuoteFeed(instrument, DepthOfBkHndlr);
        }

        public void SubscribeToTradeFeed(InstrInfo instrument)
        {
            _tradeFeed.AddTradeBuffer(Utilities.InstrToStr(instrument));

            if (_isConnected)
                _feedHandler.subscribeToSymbolTradeFeed(instrument, LastTrdHndlr);
        }


        private void DepthOfBkHndlr(InstrInfo[] instr, uint ts, byte partid, int mod, byte numbid, byte numask, byte[] bidexch, byte[] askexch, QuoteInfo[] bidbk, QuoteInfo[] askbk)
        {
            if (instr.Length == 0 || ts ==0)
                return;

            QuoteBook quote = new QuoteBook()
            {
                TS = ts,
                PartID = partid,
                Mod = mod,
                NumBid = numbid,
                NumAsk = numask,
                BidExch = bidexch,
                AskExch = askexch,
                BidBk = bidbk,
                AskBk = askbk
            };

            if(_isEnabled )
                _quoteFeed.AddQuote(instr[0], quote);
        }

        private void LastTrdHndlr(InstrInfo[] instr, uint ts, byte partid, float prc, uint sz, byte condnum, byte[] cond, float nbbobidprc, float nbboaskprc, uint nbbobidsz, uint nbboasksz, string bidexch, string askexch, float high, float low, float open, uint totvol)
        {
            if (instr.Length == 0 || ts == 0)
                return;
            
            TradeInfo ti = new TradeInfo()
            {
                Instr = instr,
                TS = ts,
                Prc = prc,
                Sz = sz,
                NBBOBidPrc = nbbobidprc,
                NBBOBidSz = nbbobidsz,
                NBBOAskPrc = nbboaskprc,
                NBBOAskSz = nbboasksz,
                CondNum = condnum
            };

            if(_isEnabled)
            {
                _tradeFeed.AddTrade(instr[0], ti);
            }
        }
    }

    public class FeedHandler 
    {
        #region DATA_FEED_MEMBER_VARIABLES

        private Parsers parsers = new Parsers();
        
        private DepthOfBkClient optDobkClient = null;
        private DepthOfBkClient sprdDobkClient = null;
        private DepthOfBkClient eqDobkClient = null;

        private TradeClient optLastTrdClient = null;
        private TradeClient eqLastTrdClient = null;
        private TradeClient sprdLastTrdClient = null;

        private static string host = "172.20.168.71";

        private static int EQ_QUOTE_PORT = 12000;
        private static int OPT_QUOTE_PORT = 13000;
        private static int OPT_TRADE_PORT = 14000;
        private static int EQ_TRADE_PORT = 15000;

        private static int quotes = 0;
        private static int trades = 0;

        private static int YEAR;
        private static int MONTH;
        private static int DAY;
        #endregion

        #region DATA_FEED_MEMBER_FUNCTIONS
        public void InitializeClients()
        {
            InitializeDOBKClients();
            InitializeTradeClients();
        }
        
        public void ConnectToData()
        {
            ConnectLevel2Data();
        }
        public FeedHandler()
        {

        }

        public void subscribeToSymbolQuoteFeed(InstrInfo instr, DepthOfBkHndlr handler)
        {
            InstrInfo[] _instr = new InstrInfo[1];
            _instr[0] = instr;
            try
            {
                if (_instr[0].type == InstrInfo.EType.EQUITY)
                {
                    eqDobkClient.Subscribe(_instr, handler);

                }
                else if (_instr[0].type == InstrInfo.EType.OPTION)
                {
                    optDobkClient.Subscribe(_instr, handler);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("SUBSCRIBE ERROR: " + ex.Message);
            }

        }



        public void subscribeToSymbolTradeFeed(InstrInfo instr, LastTrdHndlr handler)
        {
            InstrInfo[] _instr = new InstrInfo[1];
            _instr[0] = instr;

            if (_instr[0].type == InstrInfo.EType.EQUITY)
            {
                eqLastTrdClient.Subscribe(_instr, handler);

            }
            else if (_instr[0].type == InstrInfo.EType.OPTION)
            {
                optLastTrdClient.Subscribe(_instr, handler);

            }

        }
        
       
        #region DATA_FEED_CONNECTION_TERMINATORS
        public void DisconnectLevel2Data()
        {
            //optDobkClient.Disconnect();
            if (eqLastTrdClient.IsConnected())
            {
                eqLastTrdClient.Disconnect();
            }
            if (eqDobkClient.IsConnected())
            {
                eqDobkClient.Disconnect();
            }
            if (optLastTrdClient.IsConnected())
            {
                optLastTrdClient.Disconnect();
            }
            if (optDobkClient.IsConnected())
            {
                optDobkClient.Disconnect();
            }
        }
        #endregion
        #region DATA_FEED_CONNECTION_INITIALIZERS
        private void InitializeTradeClients()
        {
            optLastTrdClient = new TradeClient();

            optLastTrdClient.RegisterSessHndlrs(optLastTrdClient_HandleErr, optLastTrdClient_HandleConnectionFailed,
                                        optLastTrdClient_HandleConnected, optLastTrdClient_HandleDisconnected);

            eqLastTrdClient = new TradeClient();
            eqLastTrdClient.RegisterSessHndlrs(eqLastTrdClient_HandleErr, eqLastTrdClient_HandleConnectionFailed,
                                        eqLastTrdClient_HandleConnected, eqLastTrdClient_HandleDisconnected);

            sprdLastTrdClient = new TradeClient();
            sprdLastTrdClient.RegisterSessHndlrs(sprdLastTrdClient_HandleErr, sprdLastTrdClient_HandleConnectionFailed,
                                        sprdLastTrdClient_HandleConnected, sprdLastTrdClient_HandleDisconnected);
        }

        private void InitializeDOBKClients()
        {
            optDobkClient = new DepthOfBkClient();
            optDobkClient.RegisterSessHndlrs(optDobkClient_HandleErr, optDobkClient_HandleConnectionFailed,
                                        optDobkClient_HandleConnected, optDobkClient_HandleDisconnected);


            eqDobkClient = new DepthOfBkClient();
            eqDobkClient.RegisterSessHndlrs(eqDobkClient_HandleErr, eqDobkClient_HandleConnectionFailed,
                                        eqDobkClient_HandleConnected, eqDobkClient_HandleDisconnected);
        }

        private void ConnectLevel2Data()
        {

            optLastTrdClient.Connect(host, OPT_TRADE_PORT);
            eqLastTrdClient.Connect(host, EQ_TRADE_PORT);
            optDobkClient.Connect(host, OPT_QUOTE_PORT);
            eqDobkClient.Connect(host, EQ_QUOTE_PORT);
        }
        #endregion
        #region DATA_FEED_CONNECTION_HANDLERS
        private void eqDobkClient_HandleErr(string errstr) { }
        private void eqDobkClient_HandleConnectionFailed(Socket s) { }
        private void eqDobkClient_HandleConnected(Socket s)
        {
            //InstrInfo instr = new InstrInfo();
            //instr.sym = "SPY";
            //instr.type = InstrInfo.EType.EQUITY;

            //eqDobkClient.Subscribe(instr, this.DepthOfBkHndlr);

            //logMngr.LogMsg("EqDobkClient connection established[" + eqDobkClient.Host + ":" + eqDobkClient.Port.ToString() + "]", LogMngr.LogType.INFO);
        }

        private void eqDobkClient_HandleDisconnected(Socket s) { }

        private void optLastTrdClient_HandleErr(string errstr)
        {
            //logMngr.LogMsg("OptLastTradeClient Error: " + errstr, LogMngr.LogType.INFO);
        }

        private void optLastTrdClient_HandleConnectionFailed(Socket s) { }
        private void optLastTrdClient_HandleConnected(Socket s)
        {


            //logMngr.LogMsg("OptLstTrdClient connection established[" + optLastTrdClient.Host + ":" + optLastTrdClient.Port.ToString() + "]", LogMngr.LogType.INFO);
        }

        private void optLastTrdClient_HandleDisconnected(Socket s)
        {

        }

        private void eqLastTrdClient_HandleErr(string errstr)
        {
            //logMngr.LogMsg("eqLastTradeClient Error: " + errstr, LogMngr.LogType.INFO);
            Console.WriteLine("Last Trade Client Handling Error");
        }

        private void eqLastTrdClient_HandleConnectionFailed(Socket s)
        {
            Console.WriteLine("Last Trade Client Connection Failed");
        }
        private void eqLastTrdClient_HandleConnected(Socket s)
        {
            //InstrInfo instr = new InstrInfo();
            //instr.sym = "SPY";
            //instr.type = InstrInfo.EType.EQUITY;

            //eqLastTrdClient.Subscribe(instr, this.LastTrdHndlr);

            //logMngr.LogMsg("EqLstTrdClient connection established[" + eqLastTrdClient.Host + ":" + eqLastTrdClient.Port.ToString() + "]", LogMngr.LogType.INFO);
        }

        private void eqLastTrdClient_HandleDisconnected(Socket s)
        {

        }

        private void sprdLastTrdClient_HandleErr(string errstr)
        {
            //logMngr.LogMsg("sprdLastTradeClient Error: " + errstr, LogMngr.LogType.INFO);
        }

        private void sprdLastTrdClient_HandleConnectionFailed(Socket s) { }
        private void sprdLastTrdClient_HandleConnected(Socket s)
        {


            //logMngr.LogMsg("SprdLstTrdClient connection established[" + sprdLastTrdClient.Host + ":" + sprdLastTrdClient.Port.ToString() + "]", LogMngr.LogType.INFO);
        }

        private void sprdLastTrdClient_HandleDisconnected(Socket s) { }

        private void sDBClient_HandleErr(string errstr)
        {
            //logMngr.LogMsg("SecureDBClient Error: " + errstr, LogMngr.LogType.INFO);
        }

        private void sDBClient_HandleConnectionFailed(Socket s) { }
        private void sDBClient_HandleConnected(Socket s)
        {


            //logMngr.LogMsg("SecureDBClient connection established[" + sDBClient.Host + ":" + sDBClient.Port.ToString() + "]", LogMngr.LogType.INFO);
        }

        private void sDBClient_HandleDisconnected(Socket s) { }



        private void optDobkClient_HandleErr(string errstr)
        {
            //logMngr.LogMsg("OptDobkClient Error: " + errstr, LogMngr.LogType.INFO);
        }

        private void optDobkClient_HandleConnectionFailed(Socket s)
        {
            //logMngr.LogMsg("Check connection params", LogMngr.LogType.INFO);    //  Host, Port failed
        }

        private void optDobkClient_HandleConnected(Socket s)
        {

            //logMngr.LogMsg("OptDobkClient connection established[" + optDobkClient.Host + ":" + optDobkClient.Port.ToString() + "]", LogMngr.LogType.INFO);
        }

        private void optDobkClient_HandleDisconnected(Socket s)
        {


            if (!optDobkClient.IsConnected())
            {
                //logMngr.LogMsg("Options depth of book connection[" + optDobkClient.Host + ":" + optDobkClient.Port.ToString() + "] lost", LogMngr.LogType.CRITICAL);
            }
        }
        #endregion

        #endregion
    }


    public class QuoteFeedBuffer : FastBuffer<QuoteBook>
    {
        string _instrumentName;

        public QuoteFeedBuffer(string instrumentName, int bufferSize = 1024) : base(bufferSize)
        {
            _instrumentName = instrumentName;
        }

        public void AddQuote(QuoteBook quote)
        {
            this.Add(quote);
        }

    }

    public class TradeFeedBuffer : FastBuffer<TradeInfo>
    {
        string _instrumentName;

        public TradeFeedBuffer(string instrument, int bufferSize = 1024) : base(bufferSize)
        {
            _instrumentName = instrument;
        }

        public void AddTrade(TradeInfo trade)
        {
            this.Add(trade);
        }
    }

    public class QuoteFeed
    {
        public Dictionary<string, QuoteFeedBuffer> _quoteFeedBuffers = new Dictionary<string, QuoteFeedBuffer>();

        public QuoteFeed()
        {

        }
        public bool AddQuoteBuffer(string instrumentName, int bufferSize = 1024)
        {
            if (!_quoteFeedBuffers.ContainsKey(instrumentName))
            {
                _quoteFeedBuffers[instrumentName] = new QuoteFeedBuffer(instrumentName);
                return true;
            }
            else
                return false;
        }

        public bool AddQuoteConsumer(string instrumentName, IMarketDataConsumer<QuoteBook> consumer)
        {
            if (!_quoteFeedBuffers.ContainsKey(instrumentName))
            {
                _quoteFeedBuffers[instrumentName].ConsumerSubscribe(consumer);
                return true;
            }
            else
                return false;
        }

        public void AddQuote(string instrumentName, QuoteBook quote)
        {
            if (_quoteFeedBuffers.ContainsKey(instrumentName))
                _quoteFeedBuffers[instrumentName].Add(quote);
            else
                throw new Exception("Buffer does not exist.");
        }

        public void AddQuote(InstrInfo instrument, QuoteBook quote)
        {
            if (_quoteFeedBuffers.ContainsKey(Utilities.InstrToStr(instrument)))
                _quoteFeedBuffers[Utilities.InstrToStr(instrument)].Add(quote);
            else
                throw new Exception("Buffer does not exist.");
        }

        public void FinalizeBufferEvents(string instrumentName)
        {
            if (_quoteFeedBuffers.ContainsKey(instrumentName))
                _quoteFeedBuffers[instrumentName].Begin();
            else
                throw new Exception("Buffer does not exist.");
        }

        public void FinalizeAllBuffersEvents()
        {
            foreach (KeyValuePair<string, QuoteFeedBuffer> item in _quoteFeedBuffers)
            {
                item.Value.Begin();
            }
        }
    }

    public class TradeFeed
    {
        Dictionary<string, TradeFeedBuffer> _tradeFeedBuffers = new Dictionary<string, TradeFeedBuffer>();

        public TradeFeed()
        {

        }
        public bool AddTradeBuffer(string instrumentName, int bufferSize = 1024)
        {
            if (!_tradeFeedBuffers.ContainsKey(instrumentName))
            {
                _tradeFeedBuffers[instrumentName] = new TradeFeedBuffer(instrumentName);
                return true;
            }
            else
                return false;
        }

        public bool AddTradeConsumer(string instrumentName, IMarketDataConsumer<TradeInfo> consumer)
        {
            if (_tradeFeedBuffers.ContainsKey(instrumentName))
            {
                _tradeFeedBuffers[instrumentName].ConsumerSubscribe(consumer);
                return true;
            }
            else
                return false;
        }

        public void AddTrade(string instrumentName, TradeInfo trade)
        {
            if (_tradeFeedBuffers.ContainsKey(instrumentName))
                _tradeFeedBuffers[instrumentName].Add(trade);
            else
                throw new Exception("Buffer does not exist.");
        }

        public void AddTrade(InstrInfo instrument, TradeInfo trade)
        {
            if (_tradeFeedBuffers.ContainsKey(Utilities.InstrToStr(instrument)))
                _tradeFeedBuffers[Utilities.InstrToStr(instrument)].Add(trade);
            else
                throw new Exception("Buffer does not exist.");
        }

        public void FinalizeBufferEvents(string instrumentName)
        {
            if (_tradeFeedBuffers.ContainsKey(instrumentName))
                _tradeFeedBuffers[instrumentName].Begin();
            else
                throw new Exception("Buffer does not exist.");
        }

        public void FinalizeAllBuffersEvents()
        {
            foreach (KeyValuePair<string, TradeFeedBuffer> item in _tradeFeedBuffers)
            {
                item.Value.Begin();
            }
        }
    }
}
