using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PlrAPI.Models.Database
{
    /// <summary>
    /// Класс, описывающий расу.
    /// </summary>
    public class Race
    {
        // Ключ в БД и уникальный ID расы.
        [Key]
        public int Id { get; set; }

        // Название расы.
        [Required]
        public string Name { get; set; }

        // Описание расы.
        public string Desc { get; set; }


        // Навигационное свойство: список персонажей данной расы.
        public List<Character> Characters { get; set; }
    }
}
