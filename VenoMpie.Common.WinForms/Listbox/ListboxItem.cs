using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.WinForms.Listbox
{
    public class ListBoxItem
    {
        public string DisplayMember { get; set; }
        public object ValueMember { get; set; }

        public ListBoxItem(string displayMember)
        {
            DisplayMember = displayMember;
            ValueMember = Guid.NewGuid();
        }
        public ListBoxItem(string displayMember, object valueMember)
        {
            DisplayMember = displayMember;
            ValueMember = valueMember;
        }
        public override string ToString()
        {
            return this.DisplayMember;
        }
    }
}
