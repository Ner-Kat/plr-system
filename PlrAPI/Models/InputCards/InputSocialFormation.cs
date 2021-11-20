using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlrAPI.Models.Database;

namespace PlrAPI.Models.InputCards
{
    public class InputSocialFormation
    {
        // Ключ в БД и уникальный ID общественного формирования.
        public int Id { get; set; }

        // Название общественного формирования.
        public string Name { get; set; }

        // Описание общественного формирования.
        public string Desc { get; set; }


        // Преобразовать к объекту Race (объекту БД)
        public SocialFormation ToSocialFormation()
        {
            SocialFormation socialFormation = new SocialFormation
            {
                Name = this.Name,
                Desc = this.Desc
            };

            return socialFormation;
        }

        // Записать значения в объект Race (объект БД)
        public void WriteIn(SocialFormation socialFormation)
        {
            if (Name != null && !Name.Equals(""))
                socialFormation.Name = Name;

            if (Desc != null)
                socialFormation.Desc = Desc;
        }
    }
}
