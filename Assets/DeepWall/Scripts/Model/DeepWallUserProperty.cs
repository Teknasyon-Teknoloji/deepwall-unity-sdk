namespace DeepWallModule
{
    public class DeepWallUserProperty
    {
        public string UUID { get; }

        public string Country;
        public string Language;
        public DeepWallEnvironmentStyle EnvironmentStyle;

        public DeepWallUserProperty(string uuid, string country, string language)
        {
            UUID = uuid;
            Country = country;
            Language = language;
            EnvironmentStyle = DeepWallEnvironmentStyle.LIGHT;
        }

        public DeepWallUserProperty(string uuid, string country, string language,
            DeepWallEnvironmentStyle environmentStyle) : this(uuid, country, language)
        {
            EnvironmentStyle = environmentStyle;
        }
    }
}