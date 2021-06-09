using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlrAPI.Models.Database;

namespace PlrAPI.Models.InputCards
{
    public class InputRace
    {
        // Ключ в БД и уникальный ID расы.
        public int Id { get; set; }

        // Название расы.
        public string Name { get; set; }

        // Описание расы.
        public string Desc { get; set; }


        // Преобразовать к объекту Race (объекту БД)
        public Race ToRace()
        {
            Race race = new Race
            {
                Name = this.Name,
                Desc = this.Desc
            };

            return race;
        }

        // Записать значения в объект Race (объект БД)
        public void WriteIn(Race race)
        {
            if (Name != null && !Name.Equals(""))
                race.Name = Name;

            if (Desc != null && !Desc.Equals(""))
                race.Desc = Desc;
        }
    }
}
