namespace HoltronBot.Twitch.Models.EventSubAPIMessages
{
    public class AuthCodeResponse
    {
        public string Code { get; set; }
        public string Scope { get; set; }
        public string State { get; set; }
        public string Error { get; set; }
        public string ErrorDescription { get; set; }
    }
}