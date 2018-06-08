using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.GGn.SearchResults.SubResults
{
    public class UserProfile_Personal
    {
        public string @class { get; set; }
        public bool facilitator { get; set; }
        public int hnrs { get; set; }
        public int paranoia { get; set; }
        public string paranoiaText { get; set; }
        public bool donor { get; set; }
        public bool warned { get; set; }
        public bool enabled { get; set; }
        public string publicKey { get; set; }
        public bool parked { get; set; }
        public string ip { get; set; }
        public string passkey { get; set; }
        public string donated { get; set; }
        public string invites { get; set; }
    }
}
