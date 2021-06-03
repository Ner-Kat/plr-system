using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int? ParentLocId { get; set; }
        public virtual Location ParentLoc { get; set; }

        // Локации, которые являются частями данной.
        public virtual ICollection<Location> Children { get; set; }


        // Навигацонное свойство: список родившихся здесь персонажей.
        [InverseProperty("LocBirth")]
        public List<Character> CharactersBirthed { get; set; }

        // Навигацонное свойство: список умерших здесь персонажей.
        [InverseProperty("LocDeath")]
        public List<Character> CharactersDead { get; set; }
    }
}
