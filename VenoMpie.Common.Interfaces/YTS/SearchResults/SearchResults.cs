using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.YTS.SearchResults
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class SearchClassAttribute : Attribute
    {
        public string SearchCommand { get; set; }
        public SearchClassAttribute(string searchCommand)
        {
            SearchCommand = searchCommand;
        }
    }
    public class SearchResult<T> where T : class, new()
    {
        public string status { get; set; }
        public string status_message { get; set; }
        public T data { get; set; }

        public SearchResult() { }
    }
}
