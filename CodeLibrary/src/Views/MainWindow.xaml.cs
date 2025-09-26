using System.Windows;
using System.Windows.Input;
using CodeLibrary.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CodeLibrary.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void Minimize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized; // свернуть окно
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
    
    private void SearchMouseDown(object sender, MouseButtonEventArgs e)
    {
        SearchBox.Focus();
    }
    
    private void SearchTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(SearchBox.Text) && SearchBox.Text.Length > 0)
        {
            SearchBlock.Visibility = Visibility.Collapsed;
        }
        else
        {
            SearchBlock.Visibility = Visibility.Visible;
        }
    }
}