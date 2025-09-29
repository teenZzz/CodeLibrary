using System.Windows.Input;

namespace CodeLibrary.Common;

public class RelayCommand : ICommand
{
    private readonly Func<Task> _execute;
    private readonly Func<bool> _canExecute;
    private bool _isExecuting;

    public RelayCommand(Func<Task> execute, Func<bool> canExecute)
    {
        _execute = execute; _canExecute = canExecute;
    }

    public bool CanExecute(object? _) => !_isExecuting && _canExecute();

    public async void Execute(object? _)
    {
        _isExecuting = true; RaiseCanExecuteChanged();
        try { await _execute(); }
        finally { _isExecuting = false; RaiseCanExecuteChanged(); }
    }

    public event EventHandler? CanExecuteChanged;
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

// С параметром
public class RelayCommand<T> : ICommand
{
    private readonly Func<T?, Task> _execute;
    private readonly Predicate<T?>? _canExecute;
    private bool _isExecuting;

    public RelayCommand(Func<T?, Task> execute, Predicate<T?>? canExecute = null)
    {
        _execute = execute; _canExecute = canExecute;
    }

    public bool CanExecute(object? p) => !_isExecuting && (_canExecute?.Invoke((T?)p) ?? true);

    public async void Execute(object? p)
    {
        _isExecuting = true; RaiseCanExecuteChanged();
        try { await _execute((T?)p); }
        finally { _isExecuting = false; RaiseCanExecuteChanged(); }
    }

    public event EventHandler? CanExecuteChanged;
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}