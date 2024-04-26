using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UF3_test.model
{
    public class Date
    {
        [JsonProperty("$date")]
        public long UnixMilliseconds { get; set; }

        public DateTime ToDateTime()
        {
            // Convertir de milisegundos Unix a DateTime
            return DateTimeOffset.FromUnixTimeMilliseconds(UnixMilliseconds).UtcDateTime;
        }
    }
}
