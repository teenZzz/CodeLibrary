using System.Windows;
using CodeLibrary.ViewModels;

namespace CodeLibrary.Views;

public partial class AddBookWindow : Window
{
    public AddBookWindow(AddBookWindowViewModel vm)
    {
        InitializeComponent();
        
        DataContext = vm;

        vm.CloseRequested += (s, result) =>
        {
            DialogResult = result;
            Close();
        };
    }
}