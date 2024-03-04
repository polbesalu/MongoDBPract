using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class MongoDatabase
    {
        private string strConn = "mongodb://localhost:27017";
        private string database = "cataleg";
        private IMongoClient? client = null;
        private IMongoDatabase? conn = null;

        public IMongoDatabase Conn
        {
            get
            {
                if (conn == null) Connectar();
                return conn;
            }
        }

            public MongoDatabase() { }

        public MongoDatabase(String strConn, String database) { 
            this.strConn = strConn; 
            this.database = database;   

        }

        public void Connectar()
        {
            client = new MongoClient(strConn);
            conn = client.GetDatabase(this.database);
        }

    }
}


