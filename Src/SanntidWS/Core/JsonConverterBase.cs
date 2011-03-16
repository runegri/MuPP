using Newtonsoft.Json.Linq;

namespace AtB
{
    internal class JsonConverterBase
    {
        protected static string GetTokenValue(JToken token)
        {
            return token.ToString().Trim('"');
        }
    }
}