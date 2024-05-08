using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace net_ef_videogame
{
    public static class VideogameManager
    {
        public static void InsertNewVideogame(string name, string overview, int softwareHouseId, DateTime releaseDate)
        {
            try
            {
                Validation(name, overview, softwareHouseId, releaseDate);
                using VideogameContext db = new();
                db.Add(new Videogame(name, overview, releaseDate, softwareHouseId));
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static Videogame GetVideogameById(int id)
        {
            // validation
            if (id <= 0) throw new Exception("ID must be positive");
            
            using VideogameContext db = new();
            return db.Videogames.Find(id) ?? throw new Exception("Videogame not found");
        }

        public static List<Videogame> GetVideogamesByName(string name)
        {
            // validation
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("name is invalid");
            
            using VideogameContext db = new();
            return db.Videogames.Where(x => x.Name.Contains(name)).ToList();
        }

        public static void DeleteVideogame(int id)
        {
            // validation
            if (id <= 0) throw new Exception("ID must be positive");

            using VideogameContext db = new();
            Videogame videogame = db.Videogames.Find(id) ?? throw new Exception("Videogame not found");
            db.Remove(videogame);
            db.SaveChanges();
        }

        private static void Validation(string name, string overview, int softwareHouseId, DateTime releaseDate)
        {
            try
            {
                // validation
                if (typeof(DateTime) != releaseDate.GetType()) throw new Exception("value of releaseDate is not a DateTime");
                if (string.IsNullOrWhiteSpace(name)) throw new Exception("name is invalid");
                if (string.IsNullOrWhiteSpace(overview)) throw new Exception("overview is invalid");
                if (!IsSoftwareHouse(softwareHouseId)) throw new Exception("Software house not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static bool IsSoftwareHouse(int id)
        {
            try
            {
                using VideogameContext db = new VideogameContext();
                if (db.SoftwareHouses.Any(x => x.Id == id))
                {
                    return true;
                }
                else
                {
                    throw new Exception("SoftwareHouse not found");
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}