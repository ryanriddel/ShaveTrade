using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MktSrvcAPI;

namespace LOAMS
{
    public static class Utilities
    {
        public static string InstrToStr(InstrInfo instr)
        {
            switch (instr.type)
            {
                case InstrInfo.EType.OPTION:
                    return $"{instr.sym} {instr.strike} {instr.callput} {instr.maturity.ToString("0000-00-00")}";
                case InstrInfo.EType.EQUITY:
                    return instr.sym;
            }
            return "";
        }

        public static string ExchangeCodeToString(char exch)
        {
            return EqExchUtl.name[exch];
        }

        public static string FullQuoteBookToString(QuoteBook quote)
        { 
            string returnString = "";

            if (quote.AskBk == null || quote.BidBk == null)
                return "";

            returnString += quote.TS + "  ";

            for (int i = 0; i < quote.BidBk.Length; i++)
                returnString += "|" + quote.BidBk[i].prc + "|";
            returnString += "===";
            for (int i = 0; i < quote.AskBk.Length; i++)
                returnString += "|" + quote.AskBk[i].prc + "|";
            returnString += "<===>";
            for (int i = 0; i < quote.BidBk.Length; i++)
                returnString += "|" + quote.BidBk[i].sz + "|";
            returnString += "===";
            for (int i = 0; i < quote.AskBk.Length; i++)
                returnString += "|" + quote.AskBk[i].sz + "|";
            returnString += "   ";

            for (int i = 0; i < quote.BidExch.Length; i++)
                returnString += "|" + quote.BidExch[i] +"|";
            for (int i = 0; i < quote.AskExch.Length; i++)
                returnString += "|" + quote.AskExch[i] + "|";
            
            returnString += "    )";
            returnString += quote.NumBid + ", " + quote.NumAsk + ", " + quote.PartID + ", " + quote.Mod;

            return returnString;
        }

        public static uint convertTimestampToMilliseconds(uint timestamp)
        {
            int hours = (int)timestamp / 10000000;
            int minutes = (int)timestamp / 100000 - hours * 100;
            int seconds = (int)timestamp / 1000 - hours * 10000 - minutes * 100;
            return (uint)((timestamp % 1000) + seconds * 1000 + minutes * 60 * 1000 + hours * 60 * 60 * 1000);
        }

        public static uint GetMillisecondsOfToday()
        {
            var currentTime = DateTime.Now;
            var startOfDay = new DateTime(DateTime.Now.Year,
                DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            return (uint)(currentTime - startOfDay).TotalMilliseconds;
        }

        public static string GetDateStamp()
        {
            var t = DateTime.Now;
            return t.Month.ToString() + t.Day.ToString() + "_" + t.Hour.ToString() + t.Minute.ToString();
        }

        public static InstrInfo[] CreateInstrInfo(string under, string exp, double strike, string callput)
        {
            InstrInfo[] _instr;
            if (!string.IsNullOrEmpty(under) && (exp != null) &&
                (strike > 0) && !string.IsNullOrEmpty(callput))
            {
                _instr = new InstrInfo[1];
                _instr[0] = new InstrInfo
                {
                    sym = under,
                    maturity = convertExpToInt(exp),
                    strike = (float)strike,
                    type = InstrInfo.EType.OPTION
                };
                _instr[0].callput = callput == "Call"
                    ? _instr[0].callput = InstrInfo.ECallPut.CALL
                    : _instr[0].callput = InstrInfo.ECallPut.PUT;
                return _instr;
            }
            return null;
        }

        private static int convertExpToInt(string exp)
        {
            var expArr = exp.Split('-');

            return Convert.ToInt32($"{expArr[0]}{expArr[1]}{expArr[2]}");
        }
    }
}
