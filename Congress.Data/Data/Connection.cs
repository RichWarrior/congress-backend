using System;
using System.Collections.Generic;
using System.Text;

namespace Congress.Data.Data
{
    public class Connection
    {
        public string mysqlCongress { get; set; }

        public Connection()
        {
            mysqlCongress = "Server=192.168.2.219;Database=congress;Uid=root;Pwd=03102593;";
        }
    }
}
