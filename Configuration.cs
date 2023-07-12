namespace WebBlog;
public static class Configuration
{
    public static string JwtKey = "jQ7/OgWOKnMIH8Glc8ak/b+gPLQwE568Js74Dxqi+9c=";
    public static string ApiKeyName = "api_key";
    public static string ApiKey = "curso_api_jkdgauyTrsjsajs==kljd/";
    public static SmtpConfiguration Smtp = new();

    public class SmtpConfiguration
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 25;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
