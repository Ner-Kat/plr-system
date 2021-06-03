using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PlrAPI.Models.Database
{
    /// <summary>
    /// Класс, описывающий персонажа.
    /// </summary>
    public class Character
    {
        // Ключ в БД и уникальный ID персонажа.
        [Key]
        public int Id { get; set; }

        // Имя персонажа.
        [Required]
        public string Name { get; set; }

        // Альтернативные имена персонажа (в т.ч. прозвища и клички).
        public string[] AltNames { get; set; }

        // Дата рождения персонажа.
        public DateTime DateBirth { get; set; }

        // Дата смерти персонажа.
        public DateTime DateDeath { get; set; }

        // Пол персонажа.
        [Required]
        public Gender Gender { get; set; }

        // Место (локация) рождения персонажа.
        public Location LocBirth { get; set; }

        // Место (локация) смерти персонажа.
        public Location LocDeath { get; set; }

        // Раса персонажа.
        [Required]
        public Race Race { get; set; }

        // Все варианты национальной или общественной идентификация персонажа.
        public SocialFormation[] SocForm { get; set; }

        // Рост персонажа
        public int? Growth { get; set; }

        // ID биологического отца персонажа.
        public int? BioFatherId { get; set; }

        // ID биологической матери персонажа.
        public int? BioMotherId { get; set; }

        // Титулы, звания и ранги персонажа.
        public string[] Titles { get; set; }

        // Цвет волос персонажа.
        public int ColorHair { get; set; }

        // Цвет глаз персонажа.
        public int ColorEyes { get; set; }

        // Описание и биография персонажа.
        public string Desc { get; set; }

        // Список ID других личностей персонажа.
        public int[] AltCharsId { get; set; }

        // Дополнительная информация о персонаже.
        public string[,] Additions { get; set; }
    }
}
