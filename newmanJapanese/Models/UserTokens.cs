namespace JLearning.Models
{
    public class UserTokens
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public TimeSpan Validaty { get; set; }
        public string RefreshToken { get; set; }
        public string Id { get; set; }
        public DateTime ExpiredTime { get; set; }

    }
}
