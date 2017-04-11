using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MktSrvcAPI;

namespace LOAMS
{
    

    public static class Hub
    {
        static MarketDataFeed _marketDataFeed;

        

        public static void Initialize()
        {
            _marketDataFeed = new MarketDataFeed();

            
        }
        
        public static void runTest()
        {


            InstrInfo testInstrument;
            int maturity = 20170413;
            testInstrument = new InstrInfo();
            testInstrument.maturity = maturity;
            testInstrument.strike = 144;
            testInstrument.type = InstrInfo.EType.OPTION;
            testInstrument.callput = InstrInfo.ECallPut.CALL;
            testInstrument.sym = "AAPL";
            _marketDataFeed.SubscribeToQuoteFeed(testInstrument);


            Thread newThread = new Thread(() =>
            {
                long counter = 0;
                while (true)
                {
                    QuoteBook q = new QuoteBook();

                    q.BidBk = new QuoteInfo[8];
                    q.AskBk = new QuoteInfo[8];
                    q.BidExch = new byte[8];
                    q.AskExch = new byte[8];

                    q.Instr = new InstrInfo[1];
                    q.Instr[0] = testInstrument;

                    q.InstrumentName = Utilities.InstrToStr(testInstrument);
                    q.TEST_TIMESTAMP_TICKS = DateTime.Now.Ticks;

                    Hub._marketDataFeed.AddQuote(q);
                    counter++;

                    if (counter % 1000000 == 0)
                        Console.WriteLine("MIL: " + (int)counter / 1000000);
                }
            });
            newThread.Start();


        }





    }
}
