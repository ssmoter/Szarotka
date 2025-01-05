namespace Server.Model
{
    public class JSONWebTokensSettings
    {
        public JSONWebTokensSettings(string? key, string? issuer, string? audience, string? durationInMinutes, string? durationInDays)
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
            if (double.TryParse(durationInMinutes, out double minutes))
            {
                DurationInMinutes = minutes;
            }
            else
            {
                DurationInMinutes = 5;
            }
            if (double.TryParse(durationInDays, out double days))
            {
                DurationInDays = days;
            }
            else
            {
                DurationInDays = 5;
            }
        }
        public JSONWebTokensSettings(string key, string issuer, string audience, double durationInMinutes, double durationInDays)
        {
            Key = key;
            Issuer = issuer;
            Audience = audience;
            DurationInMinutes = durationInMinutes;
            DurationInDays = durationInDays;
        }
        public JSONWebTokensSettings()
        { }
        public string Key { get; set; } = "";
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
        public double DurationInMinutes { get; set; }
        public double DurationInDays { get; set; }
    }
}
