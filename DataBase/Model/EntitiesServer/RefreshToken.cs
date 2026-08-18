namespace DataBase.Model.EntitiesServer
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; } = "";
        public string Value { get; set; } = "";
        public long ExpireDate { get; set; }
        public RefreshToken()
        {
        }
        public RefreshToken(RefreshToken copy)
        {
            UserId = copy.UserId;
            Value = copy.Value;
            ExpireDate = copy.ExpireDate;
        }
    }
}
