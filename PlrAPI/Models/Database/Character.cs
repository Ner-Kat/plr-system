using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public List<string> AltNames { get; set; }

        // Дата рождения персонажа.
        // public DateTime? DateBirth { get; set; }
        public string DateBirth { get; set; }

        // Дата смерти персонажа.
        // public DateTime? DateDeath { get; set; }
        public string DateDeath { get; set; }

        // Пол персонажа.
        [Required]
        public int? GenderId { get; set; }
        public Gender Gender { get; set; }

        // Место (локация) рождения персонажа.
        public int? LocBirthId { get; set; }
        public Location LocBirth { get; set; }

        // Место (локация) смерти персонажа.
        public int? LocDeathId { get; set; }
        public Location LocDeath { get; set; }

        // Раса персонажа.
        [Required]
        public int? RaceId { get; set; }
        public Race Race { get; set; }

        // Все варианты национальной или общественной идентификация персонажа.
        public List<SocialFormation> SocForms { get; set; }

        [NotMapped]
        public List<int> SocFormsId { get; set; }

        // Рост персонажа
        public int? Growth { get; set; }

        // ID биологического отца персонажа.
        public int? BioFatherId { get; set; }

        // ID биологической матери персонажа.
        public int? BioMotherId { get; set; }

        // Список ID детей.
        public List<int> ChildrenId { get; set; }

        // Титулы, звания и ранги персонажа.
        public List<string> Titles { get; set; }

        // Цвет волос персонажа.
        // public int? ColorHair { get; set; }
        public string ColorHair { get; set; }

        // Цвет глаз персонажа.
        // public int? ColorEyes { get; set; }
        public string ColorEyes { get; set; }

        // Описание и биография персонажа.
        public string Desc { get; set; }

        // Список ID других личностей персонажа.
        public List<int> AltCharsId { get; set; }

        // Дополнительная информация о персонаже.
        public List<CharAdditionalValue> CharAdditionalValues { get; set; }
    }
}
