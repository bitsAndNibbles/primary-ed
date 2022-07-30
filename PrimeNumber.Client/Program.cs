using PrimeNumber.Client.View;
using PrimeNumber.Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PrimeNumber.Client;

internal class Program
{
    [STAThread]
    public static void Main(String[] args)
    {
        var app = new Application();
        app.ShutdownMode = ShutdownMode.OnMainWindowClose;

        var viewModelRoot = new PrimeCalculatorViewModel();
        var mainWindow = new MainWindow();

        mainWindow.DataContext = viewModelRoot;

        app.MainWindow.Show();
        app.Run(app.MainWindow);
    }
}

