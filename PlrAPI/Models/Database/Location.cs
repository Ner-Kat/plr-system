using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PlrAPI.Models.Database
{
    /// <summary>
    /// Класс, описывающий локацию:
    /// мир, планету, страну, город, область и т.д.
    /// </summary>
    public class Location
    {
        // Ключ в БД и уникальный ID локации.
        [Key]
        public int Id { get; set; }

        // Название локации.
        [Required]
        public string Name { get; set; }

        // Описание локации.
        public string Desc { get; set; }

        // "Родительская" локация по отношению к данной.
        public int? ParentLocationId { get; set; }
        public virtual Location Parent { get; set; }

        // Локации, которые являются частями данной.
        public virtual ICollection<Location> Children { get; set; }

        /*
        // "Родительская" локация по отношению к данной.
        public Location PartOfLoc { get; set; }

        // Локации, которые являются частями данной.
        public Location[] ContainsLocs { get; set; }
        */
    }
}
