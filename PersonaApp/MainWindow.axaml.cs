using Avalonia.Controls;
using PersonaApp.ViewModels;

namespace PersonaApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new PersonaViewModel();
    }
}