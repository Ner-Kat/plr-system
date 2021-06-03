using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PlrAPI.Models.Database
{
    /// <summary>
    /// Класс, описывающий общественное формирование:
    /// государство, клан, секту, народ и т.п.
    /// </summary>
    public class SocialFormation
    {
        // Ключ в БД и уникальный ID общественного формирования.
        [Key]
        public int Id { get; set; }

        // Название общественного формирования.
        [Required]
        public string Name { get; set; }

        // Описание общественного формирования.
        public string Desc { get; set; }


        // Навигационное свойство: все персонажи, относящиеся к данному социальному формированию.
        public List<Character> Characters { get; set; }
    }
}
