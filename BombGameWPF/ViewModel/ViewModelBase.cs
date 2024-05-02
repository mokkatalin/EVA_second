using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BombGameWPF.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected ViewModelBase() { } //ososztaly peldanyositasa

        //tulajdonsag valtozasanak az esemenye
        public event PropertyChangedEventHandler? PropertyChanged; 

        //tulajdonsag valtozasa ellenorzessel
        protected virtual void OnPropertChanged([CallerMemberName] String? propertyName = null)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
