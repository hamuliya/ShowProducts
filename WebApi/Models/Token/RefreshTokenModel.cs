namespace WebAPI.Models.Token
{
    public class RefreshTokenModel
    {
        public string? Token { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expiry { get; set; }


    }
}
