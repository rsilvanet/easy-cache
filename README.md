# Easy Cache

Easy Cache is a .NET library that turns it simple to cache data in your apps with an easy usability and less lines of code. By default, it implements 2 simple types of storage:

* **MemoryCacheStorage**: stores the cache in memory and it's acessible only in your application scope and lifecycle.

* **FileCacheStorage**: stores the cache serialized in files, so you can actually share it between applications.

Note that I do not recommend you to use these implementations in your production code since they are not the best storages for effective cache and, beyond that, are not optimized. Instead, I recommend you to do your own implementation of the ICacheStorage using an appropriate storage of your choice or have a look in my Redis implementation (comming soon).

Now, let me show you the code...

In your startup code, configure the Caching and inject in your DI resolver, container or static field:

```cs
public void ConfigureServices(IServiceCollection services)
{
    var storage = new MemoryCacheStorage();
    var caching = new Caching(storage);

    services.AddSingleton(caching);
    
    ...
}
```

To use it, just retrieve the Caching instance like this:

```cs
private readonly Database _db;
private readonly Caching _cache;

public ValuesController(Database db, Caching cache)
{
    _db = db;
    _cache = cache;
}

public ActionResult Get()
{
    var data = _cache.GetValue(
        "key", // The unique key to search or add in the storage in case it don't exists
        () => _db.GetData(), // The function tha returns the source data in case the key isn't found in cache
        TimeSpan.FromSeconds(30) // The expiration of the cache in case it's added by this invocation
      );

    return Ok(data);
}
```

You can also see a sample code [here](https://github.com/rsilvanet/easy-cache/tree/master/samples/EasyCache.API.Sample).

