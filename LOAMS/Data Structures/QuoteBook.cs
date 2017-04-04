using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MktSrvcAPI;

namespace LOAMS
{
    public class QuoteBook
    {
        public InstrInfo[] Instr { get; set; }
        public uint TS { get; set; }
        public byte PartID { get; set; }
        public int Mod { get; set; }

        public byte NumBid { get; set; }
        public byte[] BidExch { get; set; }
        public QuoteInfo[] BidBk { get; set; }

        public byte NumAsk { get; set; }
        public byte[] AskExch { get; set; }
        public QuoteInfo[] AskBk { get; set; }

        public void GetTOB(ref float bidprc, ref uint bidsz, ref float askprc, ref uint asksz)
        {
            int i = 0;
            bool valid = true;
            while (valid &&
                   i < NumBid)
            {
                bidsz += ((i == 0 || BidBk[i].prc == BidBk[i - 1].prc) ? BidBk[i].sz : 0);
                valid = ((i == 0 || BidBk[i].prc == BidBk[i - 1].prc) ? true : false);
                ++i;
            }

            bidprc = (NumBid > 0 ? BidBk[0].prc : 0.0f);

            i = 0;
            valid = true;
            while (valid &&
                   i < NumAsk)
            {
                asksz += ((i == 0 || AskBk[i].prc == AskBk[i - 1].prc) ? AskBk[i].sz : 0);
                valid = ((i == 0 || AskBk[i].prc == AskBk[i - 1].prc) ? true : false);
                ++i;
            }

            askprc = (NumAsk > 0 ? AskBk[0].prc : 0.0f);
        }
    }
}
