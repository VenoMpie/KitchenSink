using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.Layer13.SearchResults
{
    public class GetFileSize : Layer13ResultBase
    {
        public string files { get; set; }
        public string size { get; set; }
    }
}
