using PrimeNumber.Client.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrimeNumber.Client.ViewModel;

public class PrimeCalculatorViewModel : INotifyPropertyChanged
{
    private readonly PrimeNetClient _primeNetworkClient;
    private readonly DelegateCommand _nextPrimeCommand;
    private string _inputOutput = "10000000";
    private string _status = "";

    public event PropertyChangedEventHandler? PropertyChanged;

    public PrimeCalculatorViewModel()
    {
        _primeNetworkClient = new PrimeNetClient();

        _nextPrimeCommand = new DelegateCommand(
            ComputeNextPrime,
            () => !IsBusy);
    }

    public string InputOutput
    {
        get => _inputOutput;
        set
        {
            if (!string.Equals(_inputOutput, value))
            {
                _inputOutput = value;
                OnPropertyChanged();
            }
        }
    }

    public ConcurrencyMode ConcurrencyMode
    {
        get => _concurrencyMode;
        set
        {
            if (_concurrencyMode != value)
            {
                _concurrencyMode = value;
                OnPropertyChanged();
            }
        }
    } private ConcurrencyMode _concurrencyMode = ConcurrencyMode.Blocking;

    public string Status
    {
        get => _status;
        set
        {
            if (!string.Equals(_status, value))
            {
                _status = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand NextPrimeCommand => _nextPrimeCommand;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public bool IsBusy
    {
        get
        {
            return _isBusy;
        }
        set
        {
            if (_isBusy != value)
            {
                _isBusy = value;
                OnPropertyChanged();
                _nextPrimeCommand.OnCanExecuteChanged();
            }
        }
    }
    private bool _isBusy;

    private void ComputeNextPrime()
    {
        switch (ConcurrencyMode)
        {
            case ConcurrencyMode.AsyncAndAwait:
                ComputeNextPrimeAsync();
                break;
            case ConcurrencyMode.Blocking:
                throw new NotImplementedException();
                break;
            case ConcurrencyMode.Threading:
                throw new NotImplementedException();
                break;
        }
    }

    private async void ComputeNextPrimeAsync()
    {
        Status = "";
        IsBusy = true;

        try
        {
            int n = int.Parse(InputOutput);

            Status = "Processing...";

            var stopwatch = Stopwatch.StartNew();

            int p = await _primeNetworkClient.NextPrimeAsync(n);

            stopwatch.Stop();

            InputOutput = p.ToString();
            Status = $"Elapsed time:\n{stopwatch.Elapsed}";
        }
        catch (Exception e)
        {
            Status = e.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ComputeNextPrimeBlocking()
    {
        Status = "";
        IsBusy = true;

        try
        {
            int n = int.Parse(InputOutput);

            Status = "Processing...";

            var stopwatch = Stopwatch.StartNew();

            int p = _primeNetworkClient.NextPrime(n);

            stopwatch.Stop();

            InputOutput = p.ToString();
            Status = $"Elapsed time:\n{stopwatch.Elapsed}";
        }
        catch (Exception e)
        {
            Status = e.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

}
