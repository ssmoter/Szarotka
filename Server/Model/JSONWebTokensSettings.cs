namespace Server.Model
{
    public class JSONWebTokensSettings
    {
        public JSONWebTokensSettings(string? key,
                                     string? issuer,
                                     string? audience,
                                     string? durationInAccessToken,
                                     string? durationInRefreshTokenLong,
                                     string? durationInRefreshTokenShort)
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
            if (double.TryParse(durationInAccessToken, out double minutes))
            {
                DurationInAccessToken = minutes;
            }
            else
            {
                DurationInAccessToken = 30;
            }
            if (double.TryParse(durationInRefreshTokenLong, out double days))
            {
                DurationInRefreshTokenLong = days;
            }
            else
            {
                DurationInRefreshTokenLong = 90;
            }

            if (double.TryParse(durationInRefreshTokenShort, out double shortDuration))
            {
                DurationInRefreshTokenShort = shortDuration;
            }
            else
            {
                DurationInRefreshTokenShort = 1;
            }
        }
        public JSONWebTokensSettings(string key,
                                     string issuer,
                                     string audience,
                                     double durationInAccessToken,
                                     double durationInRefreshTokenLong,
                                     double durationInRefreshTokenShort)
        {
            Key = key;
            Issuer = issuer;
            Audience = audience;
            DurationInAccessToken = durationInAccessToken;
            DurationInRefreshTokenLong = durationInRefreshTokenLong;
            DurationInRefreshTokenShort = durationInRefreshTokenShort;
        }
        public JSONWebTokensSettings()
        { }
        public string Key { get; set; } = "";
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
        public double DurationInAccessToken { get; set; }
        public double DurationInRefreshTokenLong { get; set; }
        public double DurationInRefreshTokenShort { get; set; }
    }
}
