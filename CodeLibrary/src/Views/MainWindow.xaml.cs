using System.Windows;
using System.Windows.Input;

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
        // Закрыть окно (если это MainWindow и ShutdownMode=OnMainWindowClose — приложение завершится)
        this.Close();

        // Если хочешь закрывать приложение всегда, даже при открытых окнах:
        // Application.Current.Shutdown();
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