using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlrAPI.Models.Database
{
    public class AdditionalFieldType
    {
        // Ключ в БД и уникальный ID типа дополнительного поля
        [Key]
        public int Id { get; set; }

        // Название типа дополнительного поля
        [Required]
        public string Name { get; set; }

        // Список дополнительных полей данного типа
        public List<CharAdditionalValue> CharAdditionalValues { get; set; }
    }
}
