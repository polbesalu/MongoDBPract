using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class Disc:IEntityId
    {
        [Key]
        public string Id { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("artist")]
        public string Artist { get; set; }
        [BsonElement("country")]
        public string Country { get; set; }
        [BsonElement("company")]
        public string Company { get; set; }
        [BsonElement("price")]
        public string Price { get; set; }
        [BsonElement("year")]
        public string Year { get; set; }

        public override string ToString()
        {
            return $"{Title} - {Artist} - {Country} - {Company} - {Price} - {Year} \n";
        }
    }
}
