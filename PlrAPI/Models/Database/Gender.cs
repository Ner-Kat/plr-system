using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PlrAPI.Models.Database
{
    /// <summary>
    /// Класс, описывающий пол существа.
    /// </summary>
    public class Gender
    {
        // Ключ в БД и уникальный ID пола.
        [Key]
        public int Id { get; set; }

        // Название пола.
        [Required]
        public string Name { get; set; }
    }
}
