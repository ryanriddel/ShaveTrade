using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MktSrvcAPI;
using System.IO;
namespace LOAMS
{
    public abstract class Logger<T>
    {
        string _pathname;

        int bufferSize = 100;
        long bufferCounter = 1;
        string currentStr = "";
        StreamWriter writer;

        public Logger(string pathname)
        {
            _pathname = pathname;
            writer = new StreamWriter(pathname);
        }
        ~Logger()
        {
            writer.Close();
        }

        public abstract string ItemToString(T item);

        public void Log(T item)
        {
            string str = ItemToString(item); 
            currentStr += str + Environment.NewLine;

            if (bufferCounter % bufferSize == 0)
                WriteBuffer();

            bufferCounter++;
        }

        void WriteBuffer()
        {
            writer.WriteAsync(currentStr);
            currentStr = "";
        }
    }

    public class QuoteFeedLogger : Logger<QuoteBook>, IMarketDataConsumer<QuoteBook>
    {

        public QuoteFeedLogger(string pathname) : base(pathname)
        {

        }

        public void NewDataHandler(QuoteBook newData)
        {
            Log(newData);
        }

        public override string ItemToString(QuoteBook quote)
        {
            return Utilities.FullQuoteBookToString(quote);
        }
    }
}
