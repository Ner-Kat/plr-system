using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MirageArchiveAPI.Models.Database
{
    /// <summary>
    /// Класс, описывающий расу.
    /// </summary>
    public class Race
    {
        // Ключ в БД и уникальный ID расы.
        [Key]
        public int Id;

        // Название расы.
        [Required]
        public string Name;

        // Описание расы.
        public string Desc;
    }
}
