using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
public class NumbersObservable : IObservable<int>
{

    private readonly int _amount;

    public NumbersObservable(int amount)
    {
        //initialized with the amount of numbers it will push
        _amount = amount;
    }

    public IDisposable Subscribe (IObserver<int> observer)
    {
        for (int i = 0; i<_amount;i++){
            observer.OnNext(i);
        }
        observer.OnCompleted();
        observer.OnNext(5);
        return Disposable.Empty;
    }
}


public class ConsoleObserver<T> : IObserver<T>
{
    private readonly string _name;

    public ConsoleObserver(string name = "")    
    {
        //for printing a name if provided for every notification
        _name = name;
    }

    public void OnNext(T value)
    {
        Console.WriteLine("{0} - OnNext ({1})",_name,value);
    }

    public void OnError(Exception error)
    {
        Console.WriteLine("{0} - OnError: ", _name);
        Console.WriteLine("\t {0}", error);
    }

    public void OnCompleted()
    {
        Console.WriteLine("{0} - OnCompleted()",_name);
    }
}

public static class Extension


{
    public static IDisposable SubscribeConsole<T>(this IObservable<T> observable, string name="")
    {
        return observable.Subscribe(new ConsoleObserver<T>(name));
    }

    public static IObservable<string> ToObservable(this IChatConnection connection)
    {
        return new ObservableConnection(connection);
    }
}

public interface IChatConnection
{
    event Action<string> Received;
    event Action Closed;
    event Action<Exception> Error;

    void Disconnect();
}

public class ChatClient
{
    public IChatConnection Connect (string user, string password)
    {
        //connects to the chat service
        return default;
    }

    public IObservable<string> ObserveMessages(string user, string password)
    {
        return Connect(user, password).ToObservable();
    }

    public IObservable<string> ObserveMessagesDeferred(string user, string password)
    {
        return Observable.Defer(() =>
        {
            //this observable wont be shared among observers
            //connet method will be different for each observer
            return Connect(user, password).ToObservable();

        });
    }


}

public static class Misc
{


    public static IObservable<int> ObserveNumbers(int amount)
    {
        return Observable.Create<int>(observer =>
        {
            for (int i = 0; i<amount;i++){
            observer.OnNext(i);
        }
        observer.OnCompleted();
        return Disposable.Empty;
        });
    }
}

public class WiFiScanner{

    public  event NetworkFoundEventHandler NetworkFound = delegate {};
    public  event ExNetworkFoundEventHandler ExNetworkFound = delegate {};
    public delegate void NetworkFoundEventHandler (string ssid);
    public delegate void ExNetworkFoundEventHandler (string ssid, int strength);
    public event Action Connected = delegate {};

    public void OnWiFiFound()
    {
        NetworkFound.Invoke("Fortinet");
    }
    public void OnConnected()
    {
        Connected();
    }
    public void OnExWiFiFOund()
    {
        ExNetworkFound.Invoke("HynetFlex", 5);
    }
}

public class MockWPF
{
    public static event RoutedEventHandler Click = delegate {};
    public delegate void RoutedEventHandler (object sender, EventArgs e);
    public WiFiScanner scanner = new();
    // private static MockWPF? myself;
    // public MockWPF()
    // {
    //     // myself = new MockWPF();
    //     // Click = default;
        
    // }

    public MockWPF()
    {
        var clicks = 
            Observable.FromEventPattern<RoutedEventHandler, EventArgs>(
                h => Click += h,
                h => Click -= h
            );
            
        // var clicked = Observable.FromEventPattern(this,nameof(Click));
        clicks.SubscribeConsole("clicks");

        var networks = Observable.FromEvent<WiFiScanner.NetworkFoundEventHandler, string>(
            h => scanner.NetworkFound += h,
            h => scanner.NetworkFound -= h
        );
        networks.SubscribeConsole("Network Found");

        var exNetworks = 
            Observable.FromEvent<WiFiScanner.ExNetworkFoundEventHandler, Tuple<string,int>>(
                rxHandler => (ssid, strength) => rxHandler(Tuple.Create(ssid,strength)),
                h => scanner.ExNetworkFound += h,
                h => scanner.ExNetworkFound -= h
            );
        exNetworks.SubscribeConsole("Ex Network Found");

        IObservable<Unit> connected =
            Observable.FromEvent(
                h => scanner.Connected +=h,
                h => scanner.Connected -= h
            );

        connected.SubscribeConsole("connected");
    }

    public static void Method1(object sender, EventArgs e)
   {

   }
    
    // IObservable<EventPattern<EventArgs>> clicks = 
    //         Observable.FromEventPattern<RoutedEventHandler, EventArgs>(
    //             h => Click += h,
    //             h => Click -= h
    //         );

    
}

public class MagicalPrimeGenerator
{
    private IEnumerable<int> GeneratePrimesNaive(int n)
    {
        IList<int> primes = new List<int>();
        primes.Add(2);
        yield return 2;
        int nextPrime = 3;
        while (primes.Count < n)
        {
            int sqrt = (int)Math.Sqrt(nextPrime);
            bool isPrime = true;
            for (int i = 0; (int)primes[i] <= sqrt; i++)
            {
                if (nextPrime % primes[i] == 0)
                {
                    isPrime = false;
                    break;
                }
            }
            if (isPrime)
            {
                primes.Add(nextPrime);
                yield return nextPrime;
            }
            nextPrime += 2;
        }
        // return primes;
    }

    public IObservable<int> GeneratePrimes(int amount)
    {
        // return Observable.Create<int>()
        // var cts = new CancellationTokenSource();
        return Observable.Create<int>
        ((O,ct) =>
            {
                return Task.Run(() => 
                {
                    foreach (var prime in GeneratePrimesNaive(amount))
                    {
                        ct.ThrowIfCancellationRequested();
                        O.OnNext(prime);    
                    }
                    O.OnCompleted();

                });
                // return new CancellationDisposable(cts);
            }
        );
    }

}

class SearchEngine
{
    private async Task<IEnumerable<string>> SearchAsync(string term, int delayms)
    {
        Console.WriteLine("SearchEngine A/B - "+term);

        await Task.Delay(delayms);//simulate latency
        return new[] {term, "resultA", "resultB"};
    }

    public IObservable<string> search(string searchTerm)
    {
        return Observable.Create<string>(async (O,ct) =>
        {
            var searchA = await SearchAsync("first Search "+searchTerm, 1500);
            foreach(var result in searchA){
                ct.ThrowIfCancellationRequested();
                O.OnNext(result);
            }
            ct.ThrowIfCancellationRequested();
            var searchB = await SearchAsync("second Search "+searchTerm, 900);
            ct.ThrowIfCancellationRequested();
            foreach(var result in searchB){
                O.OnNext(result);
            }
            O.OnCompleted();
        });
    }
}
