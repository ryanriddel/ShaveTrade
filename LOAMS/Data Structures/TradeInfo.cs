using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MktSrvcAPI;

namespace LOAMS
{
    public class TradeInfo : IBufferItem<TradeInfo>
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

        public long TEST_TIMESTAMP_TICKS = 0;
        public string InstrumentName = "";

        public void Update(TradeInfo newInfo)
        {
            this.Instr = newInfo.Instr;
            this.TS = newInfo.TS;
            this.PartID = newInfo.PartID;
            this.Prc = newInfo.Prc;
            this.Sz = newInfo.Sz;
            this.CondNum = newInfo.CondNum;
            this.Cond = newInfo.Cond;
            this.NBBOBidPrc = newInfo.NBBOBidPrc;
            this.NBBOAskPrc = newInfo.NBBOAskPrc;
            this.NBBOBidSz = newInfo.NBBOBidSz;
            this.NBBOAskSz = newInfo.NBBOAskSz;
            this.BidExch = newInfo.BidExch;
            this.AskExch = newInfo.AskExch;
            this.High = newInfo.High;
            this.Low = newInfo.Low;
            this.Open = newInfo.Open;
            this.TotVol = newInfo.TotVol;
            this.TEST_TIMESTAMP_TICKS = newInfo.TEST_TIMESTAMP_TICKS;
            this.InstrumentName = newInfo.InstrumentName;
        }
    }
}
