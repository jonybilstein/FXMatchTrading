namespace KajolBezol.UI.Model
{
    public class User
    {
        public int WhatsAppNumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string DeviceKey { get; set; }


    }

    public class UserLogin
    {
        public string UserName { get; set; }
        public string PassCode { get; set; }
        public string DeviceKey { get; set; }
    }
}