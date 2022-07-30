using System;
using System.Windows.Input;

namespace PrimeNumber.Client.ViewModel;

public class DelegateCommand : ICommand
{
    private Action execute;
    private Func<bool>? canExecute;

    public DelegateCommand(Action execute, Func<bool>? canExecute)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public DelegateCommand(Action execute) : this(execute, null)
    {
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        if (canExecute != null)
        {
            return canExecute();
        }
        return true;
    }

    public void Execute(object? parameter)
    {
        execute();
    }

    internal void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
