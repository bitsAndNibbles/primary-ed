using PrimeNumber.Client.View;
using PrimeNumber.Client.ViewModel;
using System;
using System.Windows;

namespace PrimeNumber.Client;

internal class Program
{
    [STAThread]
    public static void Main(String[] args)
    {
        var app = new Application()
        {
            ShutdownMode = ShutdownMode.OnMainWindowClose
        };

        var viewModelRoot = new PrimeCalculatorViewModel();

        app.MainWindow = new MainWindow()
        {
            DataContext = viewModelRoot
        };

        app.MainWindow.Show();
        app.Run(app.MainWindow);
    }
}

