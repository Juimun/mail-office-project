using System.Windows.Input;

namespace MailOffice.Infrastructure;

public class RelayCommand(Action<object> execute, Predicate<object> canExecute) : ICommand {

    // Делегат - полезное действие команды
    readonly Action<object> _execute = execute ?? throw new ArgumentNullException("execute");

    // Делегат - проверка возможности выполнения команды
    public RelayCommand(Action<object> execute)
        : this(execute, null!)
    {
    }

    public bool CanExecute(object? parameter) => canExecute?.Invoke(parameter) ?? true;

    public event EventHandler? CanExecuteChanged {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    // public void Execute(object parameter) => _execute.Invoke(parameter);
    public void Execute(object? parameter) => _execute(parameter);

} //RelayCommand

