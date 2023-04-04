using System;
using System.Windows.Input;

namespace HospitalWPF
{
    abstract class Command : ICommand
    {
        public virtual event EventHandler? CanExecuteChanged;

        public virtual bool CanExecute(object? parameter) => true;

        public abstract void Execute(object? parameter);
    }

    class AddButton : Command
    {
        ViewModel _view;
        public AddButton(ViewModel view)
        {
            _view = view;
        }
        public override void Execute(object? parameter)
        {
            _view.AddToDatabase();
        }
    }

    class RemoveButton : Command
    {
        ViewModel _view;
        public RemoveButton(ViewModel view)
        {
            _view = view;
        }
        public override void Execute(object? parameter)
        {
            _view.Remove();
        }
    }

    class ClearButton : Command
    {
        ViewModel _view;
        public ClearButton(ViewModel view)
        {
            _view = view;
        }
        public override void Execute(object? parameter)
        {
            _view.Clear();
        }
    }
}
