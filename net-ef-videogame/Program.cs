using ConsoleTables;

namespace net_ef_videogame
{
    public class Program
    {
        public const string stringaDiConnessione = "Data Source=localhost;Initial Catalog=videogames;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        static void Main(string[] args)
        {
            using (VideogameContext db = new())
            {
                if (!db.SoftwareHouses.Any())
                {
                    SoftwareHouse nintendo = new SoftwareHouse("Nintendo", "11111111111", "Kyoto", "Japan");
                    SoftwareHouse rockstar = new SoftwareHouse("Rockstar Games", "22222222222", "New York City", "USA");
                    SoftwareHouse valve = new SoftwareHouse("Valve Corporation", "33333333333", "Bellevue", "USA");
                    SoftwareHouse electronicArts = new SoftwareHouse("Electronic Arts", "44444444444", "Redwood City", "USA");
                    SoftwareHouse ubisoft = new SoftwareHouse("Ubisoft", "55555555555", "Montreal", "Canada");
                    SoftwareHouse Konami = new SoftwareHouse("Konami", "66666666666", "Kyoto", "Japan");
                    db.SoftwareHouses.Add(nintendo);
                    db.SoftwareHouses.Add(rockstar);
                    db.SoftwareHouses.Add(valve);
                    db.SoftwareHouses.Add(electronicArts);
                    db.SoftwareHouses.Add(ubisoft);
                    db.SoftwareHouses.Add(Konami);
                    db.SaveChanges();
                }
            }

            ConsoleTable tableVideogames = new(
                                "Name", "Overview", "Software House Id", "Release Date", "Created At", "Updated At"
                                );
            Console.WriteLine("Welcome to Videogame Manager! v1.0.0");
            while (true)
            {
                try
                {
                    Console.WriteLine("\nEnter a command:");
                    Console.WriteLine("1. Insert a new videogame");
                    Console.WriteLine("2. Search for a videogame by ID");
                    Console.WriteLine("3. Search for videogames by name");
                    Console.WriteLine("4. Delete a videogame");
                    Console.WriteLine("5. Insert a new software house");
                    Console.WriteLine("6. Print all videogames of a software house");
                    Console.WriteLine("7. Exit");

                    int command = int.Parse(Console.ReadLine());
                    if (command > 7 || command < 1) throw new Exception("ERROR: invalid command");
                    switch (command)
                    {
                        // Insert new videogame
                        case 1:
                            string name;
                            string overview;
                            int softwareHouseId = -1;
                            DateTime releaseDate;

                            // name
                            while (true)
                            {
                                try
                                {
                                    Console.Write("Enter the name of the videogame: ");
                                    name = Console.ReadLine();
                                    if (string.IsNullOrWhiteSpace(name)) throw new Exception("ERROR: name is invalid");
                                    break;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }

                            // overview
                            while (true)
                            {
                                try
                                {
                                    Console.Write("Enter the overview of the videogame: ");
                                    overview = Console.ReadLine();
                                    if (string.IsNullOrWhiteSpace(overview)) throw new Exception("ERROR: overview is invalid");
                                    break;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }

                            // softwareHouseId
                            while (true)
                            {
                                try
                                {
                                    Console.WriteLine("Select a software house from the list below: ");
                                    ConsoleTable tableHouses = new("ID", "Name", "Country");
                                    using VideogameContext db = new();
                                    if (!db.SoftwareHouses.Any()) throw new Exception("ERROR: Software Houses not found");
                                    List<SoftwareHouse> houses = db.SoftwareHouses.ToList();
                                    foreach (SoftwareHouse house in houses)
                                        tableHouses.AddRow(house.Id, house.Name, house.Country);
                                    Console.WriteLine(tableHouses);
                                    Console.Write("ID: ");
                                    softwareHouseId = int.Parse(Console.ReadLine());
                                    if (!VideogameManager.IsSoftwareHouse(softwareHouseId)) throw new Exception("ERROR: Software House not found");
                                    break;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }

                            // release date
                            while (true)
                            {
                                try
                                {
                                    Console.Write("Enter the release date of the videogame: ");
                                    DateTime input = DateTime.Parse(Console.ReadLine());
                                    if (input < DateTime.Parse("1/1/1753") || input > DateTime.MaxValue) throw new Exception("ERROR: release date must be between 1/1/1753 and 31/12/9999");
                                    releaseDate = input;
                                    break;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }

                            VideogameManager.InsertNewVideogame(name, overview, softwareHouseId, releaseDate);
                            tableVideogames.AddRow(
                                name.Length > 25 ? name.Substring(0, 25) + "..." : name,
                                overview.Length > 10 ? overview.Substring(0, 10) + "..." : overview,
                                softwareHouseId,
                                releaseDate.ToString("dd/MM/yyyy"),
                                DateTime.Now,
                                DateTime.Now
                                );
                            Console.WriteLine("Your videogames: ");
                            Console.WriteLine(tableVideogames);
                            break;

                        // search for a videogame by ID
                        case 2:
                            while (true)
                            {
                                try
                                {
                                    Console.Write("Enter the ID of the videogame: ");
                                    int id = int.Parse(Console.ReadLine());
                                    Videogame videogame = VideogameManager.GetVideogameById(id);
                                    ConsoleTable videogameId = new("Name", "Overview", "Software House ID", "Release Date", "Created At", "Updated At");
                                    videogameId.AddRow(
                                        videogame.Name.Length > 25 ? videogame.Name.Substring(0, 25) + "..." : videogame.Name,
                                        videogame.Overview?.Length > 10 ? videogame.Overview.Substring(0, 10) + "..." : videogame.Overview,
                                        videogame.SoftwareHouseId,
                                        videogame.ReleaseDate.ToString("dd/MM/yyyy"),
                                        videogame.CreatedAt,
                                        videogame.UpdatedAt
                                        );
                                    Console.WriteLine($"Result for videogame ID: {id}");
                                    Console.WriteLine(videogameId);
                                    break;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                            break;

                        // search for videogames by name
                        case 3:
                            while (true)
                            {
                                try
                                {
                                    Console.Write("Enter the name of the videogame: ");
                                    string input = Console.ReadLine();
                                    List<Videogame> videogames = VideogameManager.GetVideogamesByName(input);
                                    if (videogames.Count == 0) throw new Exception("ERROR: Videogames not found");
                                    ConsoleTable videogameName = new("ID", "Name", "Overview", "Software House ID", "Release Date", "Created At", "Updated At");
                                    foreach (Videogame videogame in videogames)
                                        videogameName.AddRow(
                                            videogame.Id,
                                            videogame.Name.Length > 25 ? videogame.Name.Substring(0, 25) + "..." : videogame.Name,
                                            videogame.Overview?.Length > 10 ? videogame.Overview.Substring(0, 10) + "..." : videogame.Overview,
                                            videogame.SoftwareHouseId,
                                            videogame.ReleaseDate.ToString("dd/MM/yyyy"),
                                            videogame.CreatedAt,
                                            videogame.UpdatedAt
                                            );
                                    Console.WriteLine($"Result for videogame name: {input}");
                                    Console.WriteLine(videogameName);
                                    break;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                    if (0 != 1)
                                    {

                                    }
                                }
                            }
                            break;

                        // delete a videogame
                        case 4:
                            while (true)
                            {
                                try
                                {
                                    Console.Write("Enter the ID of the videogame: ");
                                    int id = int.Parse(Console.ReadLine());
                                    Console.Write($"Are you sure you want to delete <{VideogameManager.GetVideogameById(id).Name}>? [y/n] ");
                                    if (Console.ReadLine().ToLower() == "y") VideogameManager.DeleteVideogame(id);
                                    break;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                            break;

                        // insert a new software house
                        case 5:
                            string softwareHouseName;
                            string softwareHousePIva;
                            string softwareHouseCity;
                            string softwareHouseCountry;

                            // name
                            while (true)
                            {
                                try
                                {
                                    Console.Write("Enter the name of the software house: ");
                                    softwareHouseName = Console.ReadLine();
                                    if (string.IsNullOrWhiteSpace(softwareHouseName)) throw new Exception("ERROR: name is invalid");
                                    break;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }

                            // piva
                            while (true)
                            {
                                try
                                {
                                    Console.Write("Enter the PIVA of the software house: ");
                                    softwareHousePIva = Console.ReadLine();
                                    if (string.IsNullOrWhiteSpace(softwareHousePIva)) throw new Exception("ERROR: PIVA is invalid");
                                    if (softwareHousePIva.Length != 11) throw new Exception("ERROR: PIVA must have 11 characters");
                                    break;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }

                            // city
                            while (true)
                            {
                                try
                                {
                                    Console.Write("Enter the city of the software house: ");
                                    softwareHouseCity = Console.ReadLine();
                                    if (string.IsNullOrWhiteSpace(softwareHouseCity)) throw new Exception("ERROR: city is invalid");
                                    break;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }

                            // country
                            while (true)
                            {
                                try
                                {
                                    Console.Write("Enter the country of the software house: ");
                                    softwareHouseCountry = Console.ReadLine();
                                    if (string.IsNullOrWhiteSpace(softwareHouseCountry)) throw new Exception("ERROR: country is invalid");
                                    break;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }

                            VideogameManager.InsertNewSoftwareHouse(softwareHouseName, softwareHousePIva, softwareHouseCity, softwareHouseCountry);
                            ConsoleTable consoleTable = new("Name", "PIVA", "City", "Country");
                            consoleTable.AddRow(softwareHouseName, softwareHousePIva, softwareHouseCity, softwareHouseCountry);
                            Console.WriteLine(consoleTable);
                            break;

                        // print all games of a software house
                        case 6:
                            // softwareHouseId
                            while (true)
                            {
                                try
                                {
                                    Console.WriteLine("Select a software house from the list below: ");
                                    ConsoleTable tableHouses = new("ID", "Name", "Country");
                                    using VideogameContext db = new();
                                    if (!db.SoftwareHouses.Any()) throw new Exception("ERROR: Software Houses not found");
                                    List<SoftwareHouse> houses = db.SoftwareHouses.ToList();
                                    foreach (SoftwareHouse house in houses)
                                        tableHouses.AddRow(house.Id, house.Name, house.Country);
                                    Console.WriteLine(tableHouses);
                                    Console.Write("ID: ");
                                    int id = int.Parse(Console.ReadLine());
                                    if (!VideogameManager.IsSoftwareHouse(id)) throw new Exception("ERROR: Software House not found");
                                    List<Videogame> videogames = VideogameManager.GetVideogamesBySoftwareHouseId(id);
                                    ConsoleTable tableVideogames2 = new("ID", "Name", "Overview", "Release Date", "Created At", "Updated At");
                                    foreach (Videogame videogame in videogames)
                                        tableVideogames2.AddRow(videogame.Id, videogame.Name, videogame.Overview, videogame.ReleaseDate, videogame.CreatedAt, videogame.UpdatedAt);
                                    Console.WriteLine($"Games of <{db.SoftwareHouses.Find(id).Name}>:");
                                    Console.WriteLine(tableVideogames2);
                                    break;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                            break;

                        // exit
                        case 7:
                            Environment.Exit(0);
                            break;

                        default:
                            break;

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}