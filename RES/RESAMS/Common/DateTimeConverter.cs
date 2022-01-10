using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DateTimeConverter
    {
        public DateTime ConvertFromUnix(long ticks)
        {
            DateTime ret = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            ret = ret.AddSeconds(ticks);

            return ret;
        }

        public long ConvertToUnix(DateTime dateTime)
        {
            var dateTimeOffset = new DateTimeOffset(dateTime);
            var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
            return unixDateTime;
        }
    }
}
