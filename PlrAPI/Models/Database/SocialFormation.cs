using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MirageArchiveAPI.Models.Database
{
    /// <summary>
    /// Класс, описывающий общественное формирование:
    /// государство, клан, секту, народ и т.п.
    /// </summary>
    public class SocialFormation
    {
        // Ключ в БД и уникальный ID общественного формирования.
        [Key]
        public int Id;

        // Название общественного формирования.
        [Required]
        public string Name;

        // Описание общественного формирования.
        public string Desc;
    }
}