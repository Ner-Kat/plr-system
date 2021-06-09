using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using PlrAPI.Models.Database;

namespace PlrAPI.Models.InputCards
{
    public class InputCharacter
    {
        // Ключ в БД и уникальный ID персонажа.
        public int Id { get; set; }

        // Имя персонажа.
        public string Name { get; set; }

        // Альтернативные имена персонажа (в т.ч. прозвища и клички).
        public string[] AltNames { get; set; }

        // Дата рождения персонажа.
        public string DateBirth { get; set; }

        // Дата смерти персонажа.
        public string DateDeath { get; set; }

        // Пол персонажа.
        public int? GenderId { get; set; }

        // Место (локация) рождения персонажа.
        public int? LocBirthId { get; set; }

        // Место (локация) смерти персонажа.
        public int? LocDeathId { get; set; }

        // Раса персонажа.
        public int? RaceId { get; set; }

        // Все варианты национальной или общественной идентификация персонажа.
        public int[] SocFormsId { get; set; }

        // Рост персонажа
        public int? Growth { get; set; }

        // ID биологического отца персонажа.
        public int? BioFatherId { get; set; }

        // ID биологической матери персонажа.
        public int? BioMotherId { get; set; }

        // Список ID детей.
        public int[] ChildrenId { get; set; }

        // Титулы, звания и ранги персонажа.
        public string[] Titles { get; set; }

        // Цвет волос персонажа.
        public int? ColorHair { get; set; }

        // Цвет глаз персонажа.
        public int? ColorEyes { get; set; }

        // Описание и биография персонажа.
        public string Desc { get; set; }

        // Список ID других личностей персонажа.
        public int[] AltCharsId { get; set; }

        // Дополнительная информация о персонаже.
        public string[,] Additions { get; set; }


        // Преобразовать к объекту Character (объекту БД)
        public Character ToCharacter(Func<List<SocialFormation>> socFormsfill)
        {
            Character character = new Character
            {
                Name = this.Name,
                AltNames = new List<string>(this.AltNames),
                DateBirth = GetDateTime(this.DateBirth),
                DateDeath = GetDateTime(this.DateDeath),
                GenderId = this.GenderId,
                LocBirthId = this.LocBirthId,
                LocDeathId = this.LocDeathId,
                RaceId = this.RaceId,
                SocFormsId = new List<int>(this.SocFormsId),
                Growth = this.Growth,
                BioFatherId = this.BioFatherId,
                BioMotherId = this.BioMotherId,
                ChildrenId = new List<int>(this.ChildrenId),
                Titles = new List<string>(this.Titles),
                ColorHair = this.ColorHair,
                ColorEyes = this.ColorEyes,
                Desc = this.Desc,
                AltCharsId = new List<int>(this.AltCharsId),
                Additions = this.Additions,
                SocForms = socFormsfill()
            };

            return character;
        }

        // Парсинг даты из строки одного из определённых форматов
        private static DateTime? GetDateTime(string dateTimeStr)
        {
            if (dateTimeStr == null || dateTimeStr.Equals(""))
                return null;

            string[] templates = new string[] { "HH:mm dd-MM-yyyyy", "dd-MM-yyyyy", "MM-yyyyy", "yyyyy" };
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime? dt = null;

            foreach (string template in templates)
            {
                try
                {
                    dt = DateTime.ParseExact(dateTimeStr, template, provider);
                    break;
                }
                catch
                {
                    continue;
                }
            }

            return dt;
        }

        // Записать значения в объект Character (объект БД)
        public void WriteIn(Character character, Func<List<SocialFormation>> socFormsfill)
        {
            if (Name != null && !Name.Equals(""))
                character.Name = Name;

            if (AltNames != null)
                character.AltNames = new List<string>(AltNames);

            if (DateBirth != null && !DateBirth.Equals(""))
                character.DateBirth = GetDateTime(DateBirth);

            if (DateDeath != null && !DateDeath.Equals(""))
                character.DateDeath = GetDateTime(DateDeath);

            if (GenderId.HasValue)
                character.GenderId = GenderId;

            if (LocBirthId.HasValue)
                character.LocBirthId = LocBirthId;

            if (LocDeathId.HasValue)
                character.LocDeathId = LocDeathId;

            if (RaceId.HasValue)
                character.RaceId = RaceId;

            if (SocFormsId != null)
                character.SocFormsId = new List<int>(SocFormsId);

            if (Growth.HasValue)
                character.Growth = Growth;

            if (BioFatherId.HasValue)
                character.BioFatherId = BioFatherId;

            if (BioMotherId.HasValue)
                character.BioMotherId = BioMotherId;

            if (ChildrenId != null)
                character.ChildrenId = new List<int>(ChildrenId);

            if (Titles != null)
                character.Titles = new List<string>(Titles);

            if (ColorHair.HasValue)
                character.ColorHair = ColorHair;

            if (ColorEyes.HasValue)
                character.ColorEyes = ColorEyes;

            if (Desc != null && !Name.Equals(""))
                character.Desc = Desc;

            if (AltCharsId != null)
                character.AltCharsId = new List<int>(AltCharsId);

            if (Additions != null)
                character.Additions = Additions;

            character.SocForms = socFormsfill();
        }
    }
}
