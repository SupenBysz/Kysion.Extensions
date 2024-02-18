using Newtonsoft.Json;
using System.Xml.Serialization;
using YamlDotNet.Serialization;

namespace Kysion.Extensions.Core.Models.Base
{
    public abstract class BaseModel : PropertyChangedBase
    {
        [XmlIgnore, JsonIgnore, YamlIgnore]
        protected dynamic data = new DynamicModel();

        [XmlIgnore, JsonIgnore, YamlIgnore]
        public Dictionary<string, object?> dicProperty
        {
            get { return (data as DynamicModel)!.dicProperty; }
        }
    }
}
