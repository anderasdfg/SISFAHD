namespace SISFAHD
{
   
        public interface ISisfahdDatabaseSettings
        {
            string ConnectionString { get; set; }
            string DatabaseName { get; set; }
            string StorageConnectionString { get; set; }

        }
        public class SisfahdDatabaseSettings : ISisfahdDatabaseSettings
        {
            public string ConnectionString { get; set; }
            public string DatabaseName { get; set; }
            public string StorageConnectionString { get; set; }
        }
   
}
