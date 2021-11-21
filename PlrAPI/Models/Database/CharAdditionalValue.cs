using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlrAPI.Models.Database
{
    public class CharAdditionalValue
    {
        // Ключ в БД и уникальный ID записи со значением дополнительного поля
        [Required]
        public int Id { get; set; }

        // Тип дополнительного поля
        [Required]
        public int? AdditionalFieldTypeId { get; set; }
        public AdditionalFieldType AdditionalFieldType { get; set; }

        // Пользователь, с которому относится дополнительное поле
        [Required]
        public int? CharacterId { get; set; }
        public Character Character { get; set; }

        // Значение дополнительного поля
        [Required]
        public string Value { get; set; }
    }
}
