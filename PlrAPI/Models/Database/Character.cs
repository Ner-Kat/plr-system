using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MirageArchiveAPI.Models.Database
{
    /// <summary>
    /// Класс, описывающий персонажа.
    /// </summary>
    public class Character
    {
        // Ключ в БД и уникальный ID персонажа.
        [Key]
        public int Id;

        // Имя персонажа.
        [Required]
        public string Name;
        
        // Альтернативные имена персонажа (в т.ч. прозвища и клички).
        public string[] AltNames;
        
        // Дата рождения персонажа.
        public DateTime DateBirth;
        
        // Дата смерти персонажа.
        public DateTime DateDeath;

        // Пол персонажа.
        [Required]
        public Gender Gender;
        
        // Место (локация) рождения персонажа.
        public Location LocBirth;

        // Место (локация) смерти персонажа.
        public Location LocDeath;

        // Раса персонажа.
        [Required]
        public Race Race;

        // Все варианты национальной или общественной идентификация персонажа.
        public SocialFormation[] SocForm;

        // Рост персонажа
        public int growth;

        // Биологический отец персонажа.
        public Character BioFather;

        // Биологическая мать персонажа.
        public Character BioMother;

        // Титулы, звания и ранги персонажа.
        public string[] Titles;

        // Цвет волос персонажа.
        public int ColorHair;

        // Цвет глаз персонажа.
        public int ColorEyes;

        // Описание и биография персонажа.
        public string Desc;

        // Другие личности персонажа.
        public Character[] AltChars;

        // Дополнительная информация о персонаже.
        public string[][] Additions;
    }
}
