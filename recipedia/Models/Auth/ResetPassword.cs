namespace recipedia.Models.Auth
{
    public class ResetPassword
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
