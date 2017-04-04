using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MktSrvcAPI;

namespace LOAMS
{
    public class TradeInfo
    {
        public InstrInfo[] Instr { get; set; }
        public uint TS { get; set; }
        public byte PartID { get; set; }
        public float Prc { get; set; }
        public uint Sz { get; set; }
        public byte CondNum { get; set; }
        public byte[] Cond { get; set; }
        public float NBBOBidPrc { get; set; }
        public float NBBOAskPrc { get; set; }
        public uint NBBOBidSz { get; set; }
        public uint NBBOAskSz { get; set; }
        public string BidExch { get; set; }
        public string AskExch { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Open { get; set; }
        public uint TotVol { get; set; }
    }
}
