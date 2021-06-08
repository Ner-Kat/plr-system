using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlrAPI.Models.Database;

namespace PlrAPI.Models.InputCards
{
    public class InputLocation
    {
        // Ключ в БД и уникальный ID локации.
        public int Id { get; set; }

        // Название локации.
        public string Name { get; set; }

        // Описание локации.
        public string Desc { get; set; }

        // "Родительская" локация по отношению к данной.
        public int? ParentLocId { get; set; }


        // Преобразовать к объекту Location (объекту БД)
        public Location ToLocation()
        {
            Location location = new Location
            {
                Name = this.Name,
                Desc = this.Desc,
                ParentLocId = this.ParentLocId
            };

            return location;
        }

        // Записать значения в объект Location (объект БД)
        public void WriteIn(Location location)
        {
            if (Name != null && !Name.Equals(""))
                location.Name = Name;

            if (Desc != null && !Desc.Equals(""))
                location.Desc = Desc;

            if (ParentLocId.HasValue)
                location.ParentLocId = ParentLocId;
        }
    }
}
