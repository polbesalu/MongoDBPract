using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class MongoDAO<T> where T: IEntityId
    {

        private MongoDatabase db;
        private String name;

        /// <summary>
        /// Constructor d'un objecte DAO
        /// </summary>
        /// <param name="db">Connexió a la base de dades</param>
        /// <param name="name">Nom de la col·lecció</param>
        public MongoDAO(MongoDatabase db, String name) { 
            this.db = db; 
            this.name = name;
        }
        
        /// <summary>
        /// Obtenir una llista dels elements d'una col·lecció
        /// </summary>
        /// <returns>Llista dels elements de la col·lecció. En cas d'error retorna una llista buida. </returns>
        public List<T> Get()
        {
            List<T> items = new List<T>();

            try
            {
                var collection = db.Conn.GetCollection<T>(name);
                items = collection.Find(Builders<T>.Filter.Empty).ToListAsync().Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return items;
        }

        /// <summary>
        /// Obtenir un element de la col·lecció a través de l'identificador _id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna l'element de la col·lecció. En cas de no trobar-lo retorna Defaulta<T></returns>
        public T? Get(string id)
        {
            T? entity = default;

            try
            {
                var collection = db.Conn.GetCollection<T>(name);
                var filtre = Builders<T>.Filter.Eq("_id", id);
                entity = collection.Find(filtre).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return entity;
        }

        /// <summary>
        /// Afegeix un element a la col·lecció.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Retorna si l'element ha estat afegit a la base de dades.</returns>
        public bool Create(T entity)
        {

            bool created = false;

            try
            {
                var collection = db.Conn.GetCollection<T>(name);

                int nextId = MaxId() + 1;
                entity.Id = nextId.ToString();
                collection.InsertOne(entity);

                created = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return created; 
        }

        /// <summary>
        /// Actualitzar un element de la col·lecció.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(string id, T entity)
        {

            bool updated = false;

            try
            {
                var collection = db.Conn.GetCollection<T>(name);
                var filter = Builders<T>.Filter.Eq("_id", id);
                var updateResult = collection.ReplaceOne(filter, entity);
                updated = updateResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return updated;
        }

        /// <summary>
        /// Eliminar una element de la col·lecció
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(string id)
        {

            bool deleted = false;   
            try
            {
                var collection = db.Conn.GetCollection<T>(name);
                var filter = Builders<T>.Filter.Eq("_id", id);
                var deleteResult = collection.DeleteOne(filter);
                deleted = deleteResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return deleted;
        }

        /// <summary>
        /// Obtenir el nombre d'elements d'una col·lecció
        /// </summary>
        /// <returns></returns>
        public long Count()
        {
            long  count = 0;  

            try
            {
                var collection = db.Conn.GetCollection<T>(name);
                count = collection.CountDocuments(Builders<T>.Filter.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return count;
        }

        /// <summary>
        /// Obtenir la màxima Id d'una col·lecció
        /// </summary>
        /// <returns></returns>
        private int MaxId()
        {
            var collection = db.Conn.GetCollection<T>(name);

            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", 1 },
                    { "maxId", new BsonDocument("$max", new BsonDocument("$toInt", "$_id")) }
                })
            };

            var result = collection.Aggregate<BsonDocument>(pipeline).FirstOrDefault();

            int maxId = result != null ? result["maxId"].AsInt32 : 0;

            return maxId;
        }

        public Disc GetDisc(string id)
        {
            Disc disc = new Disc();
            try
            {
                var collection = db.Conn.GetCollection<Disc>(name);
                var filtre = Builders<Disc>.Filter.Eq("_id", id);
                disc = collection.Find(filtre).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return disc;
        }
    }
}
