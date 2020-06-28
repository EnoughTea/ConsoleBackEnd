using Newtonsoft.Json.Converters;

namespace Compro.Demo
{
    public class IsoDateConverter : IsoDateTimeConverter
    {
        public IsoDateConverter()
        {
            DateTimeFormat = "yyyy'-'MM'-'dd";
        }
    }
}