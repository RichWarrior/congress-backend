namespace Congress.Data.Data
{
    public class Connection
    {
        public string mysqlCongress { get; set; }
        public string minioHost { get; set; }
        public string minioAccessKey { get; set; }
        public string minioSecretKey { get; set; }
        public string remoteMinioHost { get; set; }

        public string apiUrl { get; set; }
        public string weburl { get; set; }

        public Connection()
        {
            //mysqlCongress = "Server=192.168.2.219;Database=congress;Uid=root;Pwd=03102593;";
            //minioHost = "192.168.2.219:9000";
            //remoteMinioHost = "212.154.81.35:9000";
            //minioSecretKey = "03102593";
            //minioAccessKey = "admin";
            //apiUrl = "http://localhost:5000";
            //weburl = "http://localhost:8080";
            mysqlCongress = "Server=165.22.81.76;Database=congress;Uid=root;Pwd=03102593;";
            minioHost = "165.22.81.76:9000";
            remoteMinioHost = "165.22.81.76:9000";
            minioSecretKey = "123456789";
            minioAccessKey = "root";
            apiUrl = "http://api.congretic.com";
            weburl = "http://congretic.com";
        }
    }
}
