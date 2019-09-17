namespace Congress.Data.Data
{
    public class Connection
    {
        public string mysqlCongress { get; set; }
        public string minioHost { get; set; }
        public string minioAccessKey { get; set; }
        public string minioSecretKey { get; set; }

        public string apiUrl { get; set; }

        public Connection()
        {
            mysqlCongress = "Server=192.168.2.219;Database=congress;Uid=root;Pwd=03102593;";
            minioHost = "192.168.2.219:9000";
            minioSecretKey = "03102593";
            minioAccessKey = "admin";
            apiUrl = "http://localhost:5000";
        }
    }
}
