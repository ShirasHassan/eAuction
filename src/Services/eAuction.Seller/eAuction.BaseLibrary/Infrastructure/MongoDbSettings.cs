namespace eAuction.BaseLibrary.Infrastructure
{
    /// <summary>
    /// MongoDbSettings
    /// </summary>
    public class MongoDbSettings : IMongoDbSettings
    {
        /// <summary>
        /// ConnectionString
        /// </summary>
        public string ConnectionString { get; set; }
        
        /// <summary>
        /// DatabaseName
        /// </summary>
        public string DatabaseName { get; set; }
    }
    
    /// <summary>
    /// IMongoDbSettings
    /// </summary>
    public interface IMongoDbSettings
    {
        /// <summary>
        /// ConnectionString
        /// </summary>
        public string ConnectionString { get;  }
        
        /// <summary>
        /// DatabaseName
        /// </summary>
        public string DatabaseName { get;  }
    }
}