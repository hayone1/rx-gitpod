using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
// var numbers = new NumbersObservable(5);
// var subscription = numbers.SubscribeConsole("numbers");

// ChatClient chatClient = new();
// var connection = chatClient.Connect("guest", "guest");

// IObservable<string> observableConnection = new ObservableConnection(connection);

// var subscription = observableConnection.SubscribeConsole("receiver");
// chatClient.Connect("guest", "pa$$w0rd")
//             .ToObservable()
//             .SubscribeConsole("receiver");

// var deferredMessages = chatClient.ObserveMessagesDeferred("user", "password");
// var sub1 = deferredMessages.SubscribeConsole("ok");
// var sub2 = deferredMessages.SubscribeConsole("yo");

MockWPF EventPatternTest = new();
EventPatternTest.scanner.OnWiFiFound();

EventPatternTest.scanner.OnExWiFiFOund();
EventPatternTest.scanner.OnConnected();


var _toObservable = new EnumToObservable();
//_toObservable.myobservable.SubscribeConsole("get the names");
// _toObservable.NumbersAndThrow().ToObservable().SubscribeConsole("will_Throw_Error");
//_toObservable.names.Subscribe(new ConsoleObserver<string>("Subscribe Directly"));
// _toObservable.ConcatMessages();
//_toObservable.ObservableToEnumerable();
//_toObservable.ObservableToList();
// _toObservable.ObservableToDict();
// _toObservable.ObservableToLookup();
// _toObservable.GenerateNUmbers();

// MagicalPrimeGenerator generator = new();
// foreach(var prime in generator.GeneratePrimesNaive(10)){
//     // Console.WriteLine("New number ");
//     Console.Write("{0},",prime);
// }
// generator.GeneratePrimes(5)
//         .Timestamp()
//         .SubscribeConsole("Prime Generator");
// Console.WriteLine("Generation done");

// SearchEngine engine = new();
// engine.search("Observable Search").SubscribeConsole("Start Searching");

Subject<int> sbj = new();

sbj.SubscribeConsole("First ubject sub");

sbj.SubscribeConsole("Second subject sub");

sbj.OnNext(1); sbj.OnNext(2); sbj.OnCompleted();                        
Console.ReadLine();

