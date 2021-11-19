using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace PlrAPI.Systems.JsonOptions
{
    public class PascalCaseJsonOptions : IPlrJsonOptions
    {
        public JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions()
            {
                PropertyNamingPolicy = null,
            };
        }
    }
}
