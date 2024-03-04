using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class MongoDisc : MongoDAO<Disc>
    {
        public MongoDisc(MongoDatabase db) : base(db, "discs")
        {
        }
    }


}
