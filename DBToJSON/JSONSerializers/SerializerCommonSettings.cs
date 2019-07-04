using Newtonsoft.Json.Serialization;

namespace DBToJSON.JsonSerializers
{
    static class SerializerCommonSettings
    {
        private static NamingStrategy _defaultNamingStategy;
        internal static NamingStrategy DefaultNamingStategy
        {
            get => _defaultNamingStategy ?? (_defaultNamingStategy = new CamelCaseNamingStrategy());
        }
    }
}
