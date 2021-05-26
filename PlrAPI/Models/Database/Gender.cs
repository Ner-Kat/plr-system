using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MirageArchiveAPI.Models.Database
{
    /// <summary>
    /// Класс, описывающий пол существа.
    /// </summary>
    public class Gender
    {
        // Ключ в БД и уникальный ID пола.
        [Key]
        public int Id;

        // Название пола.
        [Required]
        public string Name;
    }
}
