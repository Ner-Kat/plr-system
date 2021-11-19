using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace PlrAPI.Systems
{
    public interface IPlrJsonOptions
    {
        public JsonSerializerOptions GetJsonOptions();
    }
}
