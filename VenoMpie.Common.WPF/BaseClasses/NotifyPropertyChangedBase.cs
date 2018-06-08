using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.WPF.BaseClasses
{
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string property = null)
        {
            System.Threading.Interlocked.CompareExchange(ref PropertyChanged, null, null)?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        protected void OnPropertyChangedExplicit(string property)
        {
            System.Threading.Interlocked.CompareExchange(ref PropertyChanged, null, null)?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
