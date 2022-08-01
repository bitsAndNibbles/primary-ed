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
        // This is the main entry point of the application.
        //
        // For now, it's also serving as the Presenter, where
        // we place code that connects up view-models to views
        // and which causes views to open or close.

        var viewModelRoot = new PrimeCalculatorViewModel();

        var app = new Application()
        {
            ShutdownMode = ShutdownMode.OnMainWindowClose,
            MainWindow = new MainWindow()
            {
                DataContext = viewModelRoot
            }
        };

        // Start the application's Dispatcher event processor
        // and then show its main window.
        // 
        // The Run method blocks until the application exits.
        app.Run(app.MainWindow);
    }
}

