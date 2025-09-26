namespace Web.API.Requests
{
    public sealed class AuthenticateRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
