using System;
using System.Collections.Generic;

namespace PlrAPI.Systems
{
    public class PlrDate
    {
        private struct Date
        {
            public bool After = true;

            public uint Year { get; set; } = 0;

            private uint month = 0;
            public uint Month
            {
                get => month;
                set
                {
                    if (value <= 12)
                        month = value;
                }
            }

            private uint day = 0;
            public uint Day
            {
                get => day;
                set
                {
                    if (value <= 31)
                        day = value;
                }
            }

            private uint hour = 0;
            public uint Hour
            {
                get => hour;
                set
                {
                    if (value <= 24)
                        hour = value;
                }
            }

            private uint minute = 0;
            public uint Minute
            {
                get => minute;
                set
                {
                    if (value <= 60)
                        minute = value;
                }
            }

            private uint second = 0;
            public uint Second
            {
                get => second;
                set
                {
                    if (value <= 60)
                        second = value;
                }
            }
        }

        private Date _date = new();

        private static List<string> formatsCodes = new List<string>()
        {
            "t", "nt"
        };

        private delegate void FillAction(ref Date d, uint value);

        private static List<FillAction> fillActions = new()
        {
            (ref Date d, uint n) => d.Year = n,
            (ref Date d, uint n) => d.Month = n,
            (ref Date d, uint n) => d.Day = n,
            (ref Date d, uint n) => d.Hour = n,
            (ref Date d, uint n) => d.Minute = n,
            (ref Date d, uint n) => d.Second = n,
        };

        public PlrDate(string date)
        {
            _date = ParseToDate(date);
        }

        private PlrDate(Date date)
        {
            _date = date;
        }

        public string ToString(string format = "nt")
        {
            if (!formatsCodes.Contains(format))
                throw new System.Exception(message: "Неверный формат");

            string res = "";

            if (format == "t" || format == "nt")
            {
                res = $"{_date.Year}-{Gl(_date.Month, 2)}-{Gl(_date.Day, 2)}";
                res = _date.After ? res : "-" + res;
            }

            if (format == "t")
            {
                res += $"-{Gl(_date.Hour, 2)}-{Gl(_date.Minute, 2)}-{Gl(_date.Second, 2)}";
            }

            return res;
        }

        private static string GuaranteeLength(uint value, int length)
        {
            string res = value.ToString();
            if (res.Length < length)
                res = res.PadLeft(length, '0');
            return res;
        }

        private static string Gl(uint value, int length)
        {
            return GuaranteeLength(value, length);
        }

        public static PlrDate ParseDate(string d)
        {
            return new PlrDate(ParseToDate(d));
        }

        private static Date ParseToDate(string d)
        {
            var date = new Date();
            if (d is null)
                return date;

            if (d.StartsWith('-'))
            {
                d = d[1..];
                date.After = false;
            }

            string[] elems = d.Split('-');
            for (int i = 0; i < elems.Length; i++)
            {
                try
                {
                    int value = Convert.ToInt32(elems[i].TrimStart('0'));
                    fillActions[i](ref date, (uint)value);
                }
                catch
                {
                    throw new FormatException(message: $"Некорректное значение: {elems[i]}");
                }
            }

            return date;
        }
    }
}