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
    }
}
