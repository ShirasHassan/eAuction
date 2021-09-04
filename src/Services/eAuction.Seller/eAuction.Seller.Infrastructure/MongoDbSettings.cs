namespace eAuction.Seller.Infrastructure
{
    /// <summary>
    /// MongoDbSettings
    /// </summary>
    public class MongoDbSettings : IMongoDbSettings
    {
        /// <summary>
        /// ReadConnectionString
        /// </summary>
        public string ReadConnectionString { get; set; }
        
        /// <summary>
        /// ReadConnectionString
        /// </summary>
        public string WriteConnectionString { get; set; }
        
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
        /// ReadConnectionString
        /// </summary>
        public string ReadConnectionString { get;  }
        
        /// <summary>
        /// ReadConnectionString
        /// </summary>
        public string WriteConnectionString { get;  }
        
        /// <summary>
        /// DatabaseName
        /// </summary>
        public string DatabaseName { get;  }
    }
}