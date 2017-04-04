using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MktSrvcAPI;

namespace LOAMS
{
    public interface IMarketDataConsumer<T> //T => QuoteBook or TradeInfo
    {
        void NewDataHandler(T _event);
    }

    public interface IMarketDataProducer
    {
        void AddQuoteConsumer(InstrInfo instrument, IMarketDataConsumer<QuoteBook> consumer);
        void AddTradeConsumer(InstrInfo instrument, IMarketDataConsumer<TradeInfo> consumer);
    }
    public interface IQuoteSubscriber
    {

    }
}
