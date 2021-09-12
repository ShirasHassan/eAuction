
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eAuction.BaseLibrary.Domain;
using MongoDB.Bson;
using MongoDB.Driver;

namespace eAuction.BaseLibrary.Infrastructure
{
    public class MongoContext : IMongoContext
    {
        private IMongoDatabase Database { get; set; }
        public IClientSessionHandle Session { get; set; }
        public MongoClient MongoClient { get; set; }
        private readonly List<Func<Task>> _commands;
        private readonly IMongoDbSettings _configuration;

        public MongoContext(IMongoDbSettings configuration)
        {
            _configuration = configuration;
            // Every command will be stored and it'll be processed at SaveChanges
            _commands = new List<Func<Task>>();
        }


        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ConfigureMongo();

            using (Session = await MongoClient.StartSessionAsync())
            {
                Session.StartTransaction();

                var commandTasks = _commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                await Session.CommitTransactionAsync();
            }

            return _commands.Count;
        }

        private void ConfigureMongo()
        {
            if (MongoClient != null)
            {
                return;
            }

            // Configure mongo (You can inject the config, just to simplify)
            MongoClient = new MongoClient(_configuration.ConnectionString);

            Database = MongoClient.GetDatabase(_configuration.DatabaseName);
        }

        public  IMongoCollection<T> GetCollection<T>(string name)
        {
            ConfigureMongo();
            lock (Database)
            {
                if (!CollectionExists(name))
                {
                    Database.CreateCollection(name);
                }
            }
            return Database.GetCollection<T>(name);
        }


    public bool CollectionExists(string collectionName)
    {
        var filter = new BsonDocument("name", collectionName);
        var options = new ListCollectionNamesOptions { Filter = filter };

        return Database.ListCollectionNames(options).Any();
    }

    public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

      
    }
}