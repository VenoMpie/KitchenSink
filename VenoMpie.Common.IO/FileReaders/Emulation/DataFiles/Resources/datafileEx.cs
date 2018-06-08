using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace VenoMpie.Common.IO.FileReaders.Emulation.DataFiles.Resources
{
    public partial class datafile { }

    public partial class header { }

    public partial class clrmamepro { }

    public partial class romcenter { }

    public partial class game { }

    public partial class GameEqualityComparer : IEqualityComparer<game>
    {
        public bool Equals(game x, game y)
        {
            if (x.name == null || y.name == null) return false;
            return (x.name == y.name);
        }

        public int GetHashCode(game obj)
        {
            int hashCode = obj.name.GetHashCode() ^ obj.description.GetHashCode();
            return hashCode.GetHashCode();
        }
    }

    public partial class release { }

    public partial class biosset { }

    public partial class rom { }

    public partial class file { }

    public partial class disk { }

    public partial class sample { }

    public partial class archive { }
}