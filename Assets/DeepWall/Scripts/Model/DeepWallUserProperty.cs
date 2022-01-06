namespace DeepWallModule
{
    public class DeepWallUserProperty
    {
        public string UUID { get; }

        public string Country;
        public string Language;
        public string PhoneNumber;
        public string Email;
        public string FirstName;
        public string LastName;
        public DeepWallEnvironmentStyle EnvironmentStyle;

        public DeepWallUserProperty(string uuid, string country, string language, string phoneNumber, string email, string firstName, string lastName)
        {
            UUID = uuid;
            Country = country;
            Language = language;
            PhoneNumber = phoneNumber;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            EnvironmentStyle = DeepWallEnvironmentStyle.LIGHT;
        }

        public DeepWallUserProperty(string uuid, string country, string language, string phoneNumber, string email, string firstName, string lastName,
            DeepWallEnvironmentStyle environmentStyle) : this(uuid, country, language, phoneNumber, email, firstName, lastName)
        {
            EnvironmentStyle = environmentStyle;
        }
    }
}