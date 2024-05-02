using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BombGameWPF.ViewModel
{
    public class DelegateCommand : ICommand
    {
        //a tevekenyseget vegrehajto lambda-kifejezes
        private readonly Action<Object?> _execute;
        //a tevekenyseg feltetelet ellenorzo lambda-kifejezes
        private readonly Func<Object?, Boolean>? _canExecute;  

        //parancs letrehozasa
        public DelegateCommand(Action<Object?> execute) : this(null, execute) { }
        //parancs letrehozasa
        public DelegateCommand(Func<Object?, Boolean>? canExecute, Action<Object?> execute)
        {
            if(execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = execute;
            _canExecute = canExecute;
        }
        //vegrehajthatosag valtozasanak esemenye
        public event EventHandler? CanExecuteChanged;
        //vegrehajthatosag ellenorzese
        public Boolean CanExecute(Object? parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }
        //tevekenyseg vegrehajtasa
        public void Execute(Object? parameter)
        {
            if (!CanExecute(parameter))
            {
                throw new InvalidOperationException("Command execution is disabled.");
            }
            _execute(parameter);
        }
        //vegrehajthatosad valtozasanak esemenykivaltasa
        public void RaiseCanExecuteChanged()
        {
            if(CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

    }
}
