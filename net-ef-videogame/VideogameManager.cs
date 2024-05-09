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

        public static void InsertNewSoftwareHouse(string name, string pIva, string city, string country)
        {
            try
            {
                // validation
                if (string.IsNullOrWhiteSpace(name)) throw new Exception("ERROR: name is invalid");
                if (string.IsNullOrWhiteSpace(pIva)) throw new Exception("ERROR: pIva is invalid");
                if (pIva.Length != 11) throw new Exception("ERROR: pIva must have 11 characters");
                if (string.IsNullOrWhiteSpace(city)) throw new Exception("ERROR: city is invalid");
                if (string.IsNullOrWhiteSpace(country)) throw new Exception("ERROR: country is invalid");

                using VideogameContext db = new();
                db.Add(new SoftwareHouse(name, pIva, city, country));
                db.SaveChanges();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static Videogame GetVideogameById(int id)
        {
            // validation
            if (id <= 0) throw new Exception("ERROR: ID must be positive");
            
            using VideogameContext db = new();
            return db.Videogames.Find(id) ?? throw new Exception("ERROR: Videogame not found");
        }

        public static List<Videogame> GetVideogamesByName(string name)
        {
            // validation
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("ERROR: name is invalid");
            
            using VideogameContext db = new();
            return db.Videogames.Where(x => x.Name.Contains(name)).ToList();
        }

        public static void DeleteVideogame(int id)
        {
            // validation
            if (id <= 0) throw new Exception("ERROR: ID must be positive");

            using VideogameContext db = new();
            Videogame videogame = db.Videogames.Find(id) ?? throw new Exception("ERROR: Videogame not found");
            db.Remove(videogame);
            db.SaveChanges();
        }

        public static List<Videogame> GetVideogamesBySoftwareHouseId(int id)
        {
            // validation
            if (id <= 0) throw new Exception("ERROR: ID must be positive");
            if (!IsSoftwareHouse(id)) throw new Exception("ERROR: Software house not found");

            using VideogameContext db = new();
            // return db.Videogames.Where(x => x.SoftwareHouseId == id).ToList();
            return db.SoftwareHouses.Where(x => x.Id == id).SelectMany(x => x.Videogames).ToList();
        }

        private static void Validation(string name, string overview, int softwareHouseId, DateTime releaseDate)
        {
            try
            {
                // validation
                if (typeof(DateTime) != releaseDate.GetType()) throw new Exception("ERROR: value of releaseDate is not a DateTime");
                if (string.IsNullOrWhiteSpace(name)) throw new Exception("ERROR: name is invalid");
                if (string.IsNullOrWhiteSpace(overview)) throw new Exception("ERROR: overview is invalid");
                if (!IsSoftwareHouse(softwareHouseId)) throw new Exception("ERROR: Software house not found");
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
                    throw new Exception("ERROR: SoftwareHouse not found");
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}