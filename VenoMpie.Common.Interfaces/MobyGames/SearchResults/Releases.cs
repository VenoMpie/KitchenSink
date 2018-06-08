
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.MobyGames.SearchResults
{
    public class Release
    {
        public List<Company> companies { get; set; }
        public List<string> countries { get; set; }
        public string description { get; set; }
        public List<ProductCode> product_codes { get; set; }
        public string release_date { get; set; }
    }
}
