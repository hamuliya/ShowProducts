namespace WebAPI.Models.Token
{
    public class ClientTokenModel
    {
        public string? Token { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expiry { get; set; }


    }
}
