using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MirageArchiveAPI.Models.Database
{
    /// <summary>
    /// Класс, описывающий локацию:
    /// мир, планету, страну, город, область и т.д.
    /// </summary>
    public class Location
    {
        // Ключ в БД и уникальный ID локации.
        [Key]
        public int Id;

        // Название локации.
        [Required]
        public string Name;

        // Описание локации.
        public string Desc;

        // "Родительская" локация по отношению к данной.
        public Location PartOfLoc;

        // Локации, которые являются частями данной.
        public Location[] ContainsLocs;
    }
}
