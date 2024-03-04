using Demo;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

internal class Program
{
    private static void Main(string[] args)
    {
        MongoDatabase db = new MongoDatabase();
        MongoDisc dao = new MongoDisc(db);
        string name;

        // Es lo mateix
        //MongoDAO<Disc> dao = new MongoDAO<Disc>(db, "Disc");

        /*List<Disc> discs = dao.Get();
        foreach (Disc d in discs)
        {
            Console.WriteLine(d);
        }*/

        Menu(dao);
    }

    /// <summary>
    /// Menu per escollir entre els diferents metodes
    /// </summary>
    public static void Menu(MongoDisc dao)
    {
        int opcio = 0;
        do
        {
            Console.WriteLine("1. Llistar tots els discos");
            Console.WriteLine("2. Inserir un disc");
            Console.WriteLine("3. Modificar un disc");
            Console.WriteLine("4. Eliminar un disc");
            Console.WriteLine("5. Exportar a CSV");
            Console.WriteLine("6. Serialitzar a XML");
            Console.WriteLine("7. Transferir a MySQL");
            Console.WriteLine("8. Sortir\n");
            Console.Write("Escull una opció: ");
            
            if (!int.TryParse(Console.ReadLine(), out opcio) || opcio < 1 || opcio > 8)
            {
                Console.WriteLine("Opció incorrecta");
            }
            else
            {
                switch (opcio)
                {
                    case 1:
                        GetDiscs(dao);
                        break;
                    case 2:
                        InsertDisc(dao);
                        break;
                    case 3:
                        UpdateDisc(dao);
                        break;
                    case 4:
                        DeleteDisc(dao);
                        break;
                    case 5:
                        ExportToCSV(dao);
                        break;
                    case 6:
                        Serialize(dao);
                        break;
                    case 7:
                        TransferToMySQL(dao);
                        break;
                    case 8:
                        Console.WriteLine("Adeu!");
                        break;
                }
            }
            
        } while (opcio != 8);
    }

    /// <summary>
    /// Llistar tots els discos que hi ha a la base de dades MongoDB. 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static List<Disc> GetDiscs(MongoDisc dao)
    {
        List<Disc> discs = new List<Disc>();

        try
        {
            discs = dao.Get();
            Console.WriteLine("DISCS\n");
            foreach (Disc d in discs)
            {
                Console.WriteLine(d.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        return discs;

    }

    /// <summary>
    /// Inserir un nou registre a la base de dades MongoDB.
    /// </summary>
    /// <param name="disc"></param>
    /// <returns></returns>
    public static bool InsertDisc(MongoDisc dao)
    {
        bool inserted = false;

        try
        {
            Disc disc = new Disc();

            Console.WriteLine("Introdueix el títol del disc: ");
            disc.Title = Console.ReadLine();

            Console.WriteLine("Introdueix el nom de l'artista: ");
            disc.Artist = Console.ReadLine();

            Console.WriteLine("Introdueix el país: ");
            disc.Country = Console.ReadLine();

            Console.WriteLine("Introdueix la companyia: ");
            disc.Company = Console.ReadLine();

            Console.WriteLine("Introdueix el preu: ");
            disc.Price = Console.ReadLine();

            Console.WriteLine("Introdueix l'any: ");
            disc.Year = Console.ReadLine();

            dao.Create(disc);

            Console.WriteLine("Disc afegit correctament!\n");
            inserted = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        return inserted;
    }

    /// <summary>
    /// Eliminar un registre de la base de dades MongoDB. 
    /// </summary>
    /// <param name="disc"></param>
    /// <returns></returns>
    public static bool DeleteDisc(MongoDisc dao)
    {
        bool deleted = false;

        Console.WriteLine("Introdueix l'identificador del disc que vols eliminar: ");
        string id = Console.ReadLine();

        try
        {
            dao.Delete(id);
            deleted = true;
            Console.WriteLine("Disc eliminat correctament!\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine("NO S'HA POGUT ELIMINAR EL DISC\n");
            Console.WriteLine(ex.ToString());
        }

        return deleted;

    }

    /// <summary>
    /// Modificar un registre de la base de dades MongoDB. 
    /// </summary>
    /// <param name="disc"></param>
    /// <returns></returns>
    public static bool UpdateDisc(MongoDisc dao)
    {
        bool updated = false;

        try
        {
            Console.WriteLine("Introdueix l'identificador del disc que vols modificar: ");
            string id = Console.ReadLine();

            Disc disc = dao.GetDisc(id);

            Console.WriteLine("Introdueix el títol del disc: ");
            disc.Title = Console.ReadLine();

            Console.WriteLine("Introdueix el nom de l'artista: ");
            disc.Artist = Console.ReadLine();

            Console.WriteLine("Introdueix el país: ");
            disc.Country = Console.ReadLine();

            Console.WriteLine("Introdueix la companyia: ");
            disc.Company = Console.ReadLine();

            Console.WriteLine("Introdueix el preu: ");
            disc.Price = Console.ReadLine();

            Console.WriteLine("Introdueix l'any: ");
            disc.Year = Console.ReadLine();

            dao.Update(id, disc);
            updated = true;

            Console.WriteLine("Disc modificat correctament!\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine("NO S'HA POGUT MODIFICAR EL DISC\n");
            Console.WriteLine(ex.ToString());
        }

        return updated;
    }

    /// <summary>
    /// Exportar tots els registres en un fitxer CSV.
    /// </summary>
    /// <param name="path"></param>
    public static void ExportToCSV(MongoDisc dao)
    {
        Console.WriteLine("Introdueix el nom del fitxer: ");
        string path = Console.ReadLine();

        List<Disc> items = dao.Get();
        StringBuilder sb = new StringBuilder();

        if (items.Count > 0)
        {
            PropertyInfo[] props = items[0].GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                sb.Append(prop.Name).Append(",");
            }
            sb.Remove(sb.Length - 1, 1).AppendLine();

            foreach (Disc item in items)
            {
                foreach (PropertyInfo prop in props)
                {
                    sb.Append(prop.GetValue(item)).Append(",");
                }
                sb.Remove(sb.Length - 1, 1).AppendLine();
            }

            System.IO.File.WriteAllText(path, sb.ToString());
        }

        Console.WriteLine($"Els discs s'han exportat com a .csv amb el nom: {path} a la ruta :\\MongoDBPract\\MongoDBPract\\bin\\Debug\\net8.0 \n");
    }

    /// <summary>
    /// Serialitzar tots els registres a un fitxer. 
    /// </summary>
    /// <param name="path"></param>
    public static void Serialize(MongoDisc dao)
    {
        Console.WriteLine("Introdueix el nom del fitxer: ");
        string path = Console.ReadLine();

        List<Disc> items = dao.Get();
        if (items.Count > 0)
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(items.GetType());
            System.IO.FileStream file = System.IO.File.Create(path);
            ser.Serialize(file, items);
            file.Close();
        }

        Console.WriteLine($"Els discs s'han serialitzat amb el nom: {path} a la ruta :\\MongoDBPract\\MongoDBPract\\bin\\Debug\\net8.0 \n");

    }

    /// <summary>
    //Transferir tots els registres a una base de dades MySQL 
    /// </summary>
    public static void TransferToMySQL(MongoDisc dao)
    {
        List<Disc> items = dao.Get();
        if (items.Count > 0)
        {
            string connStr = "server=localhost";
            connStr += ";user=root";
            connStr += ";database=cataleg";
            connStr += ";port=3306";
            connStr += ";password=\"\"";

            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            conn.Open();

            string sql = "CREATE TABLE discs (id INT AUTO_INCREMENT PRIMARY KEY, title VARCHAR(255), artist VARCHAR(255), country VARCHAR(255), company VARCHAR(255), price VARCHAR(255), year VARCHAR(255))";

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

            cmd.ExecuteNonQuery();
            string sql2 ="";
            foreach (Disc item in items)
            {
                sql2 += $"INSERT INTO discs (title, artist, country, company, price, year) VALUES ('{item.Title}', '{item.Artist}', '{item.Country}', '{item.Company}', '{item.Price}', '{item.Year}');";
            }

            MySql.Data.MySqlClient.MySqlCommand cmd2 = new MySql.Data.MySqlClient.MySqlCommand(sql2, conn);

            cmd2.ExecuteNonQuery();
            Console.WriteLine("Base de dades establerta!\n");
            conn.Close();
        }
    }
}