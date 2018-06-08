using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.GGn.SearchResults
{
    public class GGnResult<T> where T : class, new()
    {
        public string status { get; set; }
        public T response { get; set; }
    }
}
