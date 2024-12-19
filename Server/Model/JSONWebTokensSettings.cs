namespace Server.Model
{
    public class JSONWebTokensSettings
    {
        public JSONWebTokensSettings(string? key, string? issuer, string? audience, string? durationInMinutes)
        {
            if (key is not null)
            {
                Key = key;
            }
            if (issuer is not null)
            {
                Issuer = issuer;
            }
            if (audience is not null)
            {
                Audience = audience;
            }
            if (double.TryParse(durationInMinutes, out double result))
            {
                DurationInMinutes = result;
            }
            else
            {
                DurationInMinutes = 5;
            }
        }
        public JSONWebTokensSettings(string key, string issuer, string audience, double durationInMinutes)
        {
            Key = key;
            Issuer = issuer;
            Audience = audience;
            DurationInMinutes = durationInMinutes;
        }
        public JSONWebTokensSettings()
        { }
        public string Key { get; set; } = "";
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
        public double DurationInMinutes { get; set; }
    }
}
