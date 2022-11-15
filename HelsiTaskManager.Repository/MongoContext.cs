using HelsiTaskManager.Repository;

public class MongoContext : IMongoContext
{
    private IMongoDatabase _database { get; set; }
    private readonly List<Func<Task>> _commands;

    public IMongoDatabase Database => _database;
    public MongoContext(IConfiguration configuration, MongoClient mongoClient)
    {
        _commands = new List<Func<Task>>();
        RegisterConventions();
        _database = mongoClient.GetDatabase(configuration.GetSection("MongoSettings").GetSection("DatabaseName").Value);
    }

    /// <summary>
    /// mongo convenstions config registration
    /// </summary>
    private void RegisterConventions()
    {
        var pack = new ConventionPack
        {
            new IgnoreExtraElementsConvention(true),
            new IgnoreIfDefaultConvention(true)
        };
        ConventionRegistry.Register("Helsi Task Manager Conventions", pack, t => true);
    }

    /// <summary>
    /// run commands as a trunsaction
    /// </summary>
    /// <returns></returns>
    public async Task<int> SaveChanges()
    {
        var qtd = _commands.Count;
        foreach (var command in _commands)
        {
            await command();
        }

        _commands.Clear();
        return qtd;
    }

    /// <summary>
    /// get collection by name
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public IMongoCollection<T> GetCollection<T>(string name) => _database.GetCollection<T>(name);

    //TODO: check how MongoClient manage connections
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// add command to command list
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public Task AddCommand(Func<Task> func)
    {
        _commands.Add(func);
        return Task.CompletedTask;
    }
}