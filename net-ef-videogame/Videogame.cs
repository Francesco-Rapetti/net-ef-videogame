using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net_ef_videogame
{
    [Table("videogames")]
    [Index(nameof(Name), IsUnique = true)]
    public class Videogame
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public string? Overview { get; set; }
        [Column("release_date")]
        public DateTime ReleaseDate { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [Column("software_house_id")]
        public int SoftwareHouseId { get; set; }
        public SoftwareHouse SoftwareHouse { get; set; }

        public Videogame(string name, string overview, DateTime releaseDate, int softwareHouseId)
        {
            this.Name = name;
            this.Overview = overview;
            this.ReleaseDate = releaseDate;
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
            this.SoftwareHouseId = softwareHouseId;
        }
    }
}