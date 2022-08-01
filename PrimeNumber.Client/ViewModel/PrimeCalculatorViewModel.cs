using PrimeNumber.Client.Model;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Threading;

namespace PrimeNumber.Client.ViewModel;

public class PrimeCalculatorViewModel : ViewModelBase
{
    public PrimeCalculatorViewModel()
    {
        InputOutputInternal = "100,000,000,000,000";
    }

    #region properties

    /// <summary>
    /// Gets or sets the input/output string in a way that doesn't
    /// also clear the value of <see cref="Status"/>.
    /// </summary>
    private string InputOutputInternal
    {
        get => _inputOutput;
        set
        {
            if (!string.Equals(_inputOutput, value))
            {
                _inputOutput = value;

                if (long.TryParse(_inputOutput, out long longVal))
                {
                    // pretty print the long value to make it
                    // easier for humans to decipher
                    _inputOutput = longVal.ToString("N0");
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(InputOutput));
            }
        }
    }
    private string _inputOutput = string.Empty;

    public string InputOutput
    {
        get => InputOutputInternal;
        set
        {
            if (!string.Equals(_inputOutput, value))
            {
                InputOutputInternal = value;

                // clear our last status when the user makes
                // any change to the input string, since that
                // status no longer relates to their input.
                Status = string.Empty;
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
    }
    private ConcurrencyMode _concurrencyMode = ConcurrencyMode.Blocking;

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
    private string _status = "";

    public DelegateCommand NextPrimeCommand
    {
        get
        {
            if (_nextPrimeCommand == null)
            {
                _nextPrimeCommand = new DelegateCommand(
                    ComputeNextPrime,
                    () => !IsBusy);
            }
            return _nextPrimeCommand;
        }
    }
    private DelegateCommand? _nextPrimeCommand;

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
                NextPrimeCommand.OnCanExecuteChanged();
            }
        }
    }
    private bool _isBusy;

    #endregion properties

    private void ComputeNextPrime()
    {
        if (ConcurrencyMode == ConcurrencyMode.Threading)
        {
            // managing a threaded approach requires us to break up
            // our UI work into multiple methods, and we must use
            // some sort of signaling between threads
            ComputeNextPrimeThreaded();
        }
        else
        {
            // blocking and async/await approaches LOOK very similar
            // but will behave very differently at runtime, due to
            // compiler rewriting of async/await calls.
            ComputeNextPrimeNonThreaded();
        }
    }


    #region threaded approach

    private Stopwatch? threadedApproachTimer;

    private void ComputeNextPrimeThreaded()
    {
        // we can reuse the blocking client, but we'll call it
        // from a separate thread so that the UI doesn't become
        // unresponsive.
        
        IsBusy = true;

        try
        {
            long n = long.Parse(InputOutputInternal, NumberStyles.Number);

            Status = "Processing...";
            threadedApproachTimer = Stopwatch.StartNew();

            var client = new PrimeNetClientBlocking();
            var uiDispatcher = Dispatcher.CurrentDispatcher;

            Thread t = new Thread(() =>
            {
                // the work in THIS delegate's scope is performed in a
                // separate thread -- we must not cause any UI method
                // calls from that other thread, or we'll have exceptions
                // or unexpected UI behavior.

                #region work performed in separate thread

                long prime;
                try
                {
                    prime = client.NextPrime(n);

                    // return the asynchronously computed result back into
                    // the UI thread:
                    DispatchSuccessResponse(uiDispatcher, prime);
                }
                catch (Exception e)
                {
                    DispatchErrorResponse(uiDispatcher, e.Message);
                    return;
                }

                #endregion work performed in separate thread
            })
            {
                IsBackground = true,
                Name = "prime network client"
            };

            // ask the runtime to start the new thread
            t.Start();

            // we've reached the end of the method running in the UI
            // thread, so we're now returning control to the UI
            // dispatcher. until we hear back via
            // DispatchPrimeNumberResponse(), we must leave IsBusy
            // set to true.
        }
        catch (Exception e)
        {
            Status = $"Error:\n{e.Message}";

            // if an exception happens, be sure to reset busy status
            // so that the user is informed and may try again.
            IsBusy = false;
        }
    }

    private void DispatchSuccessResponse(Dispatcher dispatcher, long prime)
    {
        // this method may be invoked from any thread.
        //
        // view model properties must be set from within the
        // Dispatcher (UI) thread because UI controls may be
        // bound to them.

        dispatcher.BeginInvoke(() =>
        {
            threadedApproachTimer?.Stop();

            InputOutputInternal = prime.ToString();
            IsBusy = false;
            Status = $"Elapsed time:\n{threadedApproachTimer?.Elapsed}";
        });
    }

    private void DispatchErrorResponse(Dispatcher dispatcher, string message)
    {
        // this method may be invoked from any thread.
        //
        // view model properties must be set from within the
        // Dispatcher (UI) thread because UI controls may be
        // bound to them.
        dispatcher.BeginInvoke(() =>
        {
            IsBusy = false;
            Status = $"Error:\n{message}";
        });
    }

    #endregion threaded approach


    #region non-threaded approach

    private async void ComputeNextPrimeNonThreaded()
    {
        long prime;

        IsBusy = true;

        try
        {
            long n = long.Parse(InputOutputInternal, NumberStyles.Number);

            Status = "Processing...";
            var stopwatch = Stopwatch.StartNew();

            if (ConcurrencyMode == ConcurrencyMode.AsyncAndAwait)
            {
                prime = await new PrimeNetClientAsync().NextPrimeAsync(n);
            }
            else // Blocking
            {
                prime = new PrimeNetClientBlocking().NextPrime(n);
            }

            stopwatch.Stop();

            InputOutputInternal = prime.ToString();
            Status = $"Elapsed time:\n{stopwatch.Elapsed}";
        }
        catch (Exception e)
        {
            Status = $"Error:\n{e.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion non-threaded approach

}
