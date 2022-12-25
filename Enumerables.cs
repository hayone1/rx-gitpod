using System.Linq;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;

public class EnumToObservable
{
    public IEnumerable<string> names = new []{"Shinra", "Tensi", "Kakashi"};
    public IEnumerable<string> namesDup = new []{"Shinra", "Tensi", "Kakashi", "Takashi" };
    public IObservable<string> myobservable => names.ToObservable();
    public IEnumerable<string> loadedMessages = new []{"Some","existing","messages"};

    public IEnumerable<int> NumbersAndThrow()
    {
        yield return 1;
        yield return 2;
        throw new ApplicationException("Oops, something bad happened");
        yield return 4;
    }

    public void ConcatMessages()
    {
        loadedMessages.ToObservable()
                        .Concat(myobservable)
                        .SubscribeConsole("merged messages");

        myobservable.StartWith(loadedMessages)
                        .SubscribeConsole("merged messages directly");
    }

    public void ObservableToEnumerable()
    {
        var thisObservable = Observable.Create<string>(O => 
        {
            O.OnNext("First toEnum Next");
            O.OnNext("Second toEnum Next");
            O.OnCompleted();
            return Disposable.Empty;
        });
        var thisEnumerable = thisObservable.ToEnumerable();
        foreach (var item in thisEnumerable)
        {
            Console.WriteLine(item);
        }
    }

    public void ObservableToList()
    {
        var thisObservable = Observable.Create<string>(O => 
        {
            O.OnNext("First toEnum Next");
            O.OnNext("Second toEnum Next");
            O.OnCompleted();
            return Disposable.Empty;
        });
        IObservable<IList<string>> myList = thisObservable.ToList();
        // foreach (var item in thisEnumerable)
        // {
        //     Console.WriteLine(item);
        // }
        myList.Select(lst => string.Join(",",lst))
                .SubscribeConsole("list is Ready");
    }

    public void ObservableToDict()
    {
        var dictionaryObservable = names.ToObservable()
                                        .ToDictionary(element => element.Length);
        dictionaryObservable.Select(d => string.Join(",", d))
                            .SubscribeConsole("ObsToDict");
    }

    public void ObservableToLookup()
    {
        var lookupObservable = namesDup.ToObservable()
                                        .ToLookup(element => element.Length);
        lookupObservable.Select(_lookup =>
        {
            var groups = new StringBuilder();
            foreach (var grp in _lookup)
                groups.AppendFormat("[Key:{0} => {1}]", grp.Key, grp.Count());
            return groups.ToString();
        })
        .SubscribeConsole("ObsToLookup");
    }

    public void GenerateNUmbers()
    {
        IObservable<int> observable = Observable.Generate
            (
                0,
                i => i < 10,
                i => i + 1,
                i => i * 2
            );
        observable.SubscribeConsole("Generate Numbers");
    }
}
