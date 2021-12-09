using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using PlrAPI.Models.Database;
using PlrAPI.Systems;

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
        public int[] SocFormsIds { get; set; }

        // Рост персонажа
        public int? Growth { get; set; }

        // ID биологического отца персонажа.
        public int? BioFatherId { get; set; }

        // ID биологической матери персонажа.
        public int? BioMotherId { get; set; }

        // Список ID детей.
        public int[] ChildrenIds { get; set; }

        // Титулы, звания и ранги персонажа.
        public string[] Titles { get; set; }

        // Цвет волос персонажа.
        // public int? ColorHair { get; set; }
        public string ColorHair { get; set; }

        // Цвет глаз персонажа.
        // public int? ColorEyes { get; set; }
        public string ColorEyes { get; set; }

        // Описание и биография персонажа.
        public string Desc { get; set; }

        // Список ID других личностей персонажа.
        public int[] AltCharsIds { get; set; }

        // Дополнительная информация о персонаже.
        public Dictionary<string, string> Additions { get; set; }


        // Преобразовать к объекту Character (объекту БД)
        public Character ToCharacter(Func<List<SocialFormation>> socFormsfill, Func<List<CharAdditionalValue>> additionalFieldsFill)
        {
            Character character = new Character
            {
                Name = this.Name,
                AltNames = this.AltNames is not null ? new List<string>(this.AltNames) : new List<string>(),
                DateBirth = GetPlrDate(this.DateBirth),
                DateDeath = GetPlrDate(this.DateDeath),
                GenderId = this.GenderId,
                LocBirthId = this.LocBirthId,
                LocDeathId = this.LocDeathId,
                RaceId = this.RaceId,
                SocFormsId = this.SocFormsIds is not null ? new List<int>(this.SocFormsIds) : new List<int>(),
                Growth = this.Growth,
                BioFatherId = this.BioFatherId,
                BioMotherId = this.BioMotherId,
                ChildrenId = this.ChildrenIds is not null ? new List<int>(this.ChildrenIds) : new List<int>(),
                Titles = this.Titles is not null ? new List<string>(this.Titles) : new List<string>(),
                ColorHair = this.ColorHair,
                ColorEyes = this.ColorEyes,
                Desc = this.Desc,
                AltCharsId = this.AltCharsIds is not null ? new List<int>(this.AltCharsIds) : new List<int>(),
                SocForms = socFormsfill(),
                CharAdditionalValues = additionalFieldsFill()
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

        // Парсинг даты формата PlrDate из строки
        private static string GetPlrDate(string dateStr)
        {
            PlrDate plrDate = null;

            try
            {
                plrDate = new PlrDate(dateStr);
                return plrDate.ToString();
            }
            catch
            {
                return null;
            }
        }

        // Записать значения в объект Character (объект БД)
        public void WriteIn(Character character, Func<List<SocialFormation>> socFormsfill, Func<List<CharAdditionalValue>> additionalFieldsFill)
        {
            if (Name != null && !Name.Equals(""))
                character.Name = Name;

            if (AltNames != null)
                character.AltNames = new List<string>(AltNames);

            if (DateBirth != null && !DateBirth.Equals(""))
                character.DateBirth = GetPlrDate(DateBirth);

            if (DateDeath != null && !DateDeath.Equals(""))
                character.DateDeath = GetPlrDate(DateDeath);

            if (GenderId.HasValue)
            {
                if (GenderId.Value == -1)
                    character.GenderId = null;
                else
                    character.GenderId = GenderId;
            }

            if (LocBirthId.HasValue)
            {
                if (LocBirthId.Value == -1)
                    character.LocBirthId = null;
                else
                    character.LocBirthId = LocBirthId;
            }

            if (LocDeathId.HasValue)
            {
                if (LocDeathId.Value == -1)
                    character.LocDeathId = null;
                else
                    character.LocDeathId = LocDeathId;
            }

            if (RaceId.HasValue)
            {
                if (RaceId.Value == -1)
                    character.RaceId = null;
                else
                    character.RaceId = RaceId;
            }

            if (SocFormsIds != null)
                character.SocFormsId = new List<int>(SocFormsIds);

            if (Growth.HasValue)
                character.Growth = Growth;

            if (BioFatherId.HasValue)
            {
                if (BioFatherId.Value == -1)
                    character.BioFatherId = null;
                else
                    character.BioFatherId = BioFatherId;
            }

            if (BioMotherId.HasValue)
            {
                if (BioMotherId.Value == -1)
                    character.BioMotherId = null;
                else
                    character.BioMotherId = BioMotherId;
            }

            if (ChildrenIds != null)
                character.ChildrenId = new List<int>(ChildrenIds);

            if (Titles != null)
                character.Titles = new List<string>(Titles);

            if (ColorHair is not null)
                character.ColorHair = ColorHair;

            if (ColorEyes is not null)
                character.ColorEyes = ColorEyes;

            if (Desc != null)
                character.Desc = Desc;

            if (AltCharsIds != null)
                character.AltCharsId = new List<int>(AltCharsIds);

            if (character.SocForms is not null)
            {
                character.SocForms.Clear();
                character.SocForms = socFormsfill();
            }

            if (character.CharAdditionalValues is not null)
            {
                character.CharAdditionalValues.Clear();
                character.CharAdditionalValues = additionalFieldsFill();
            }
        }
    }
}
