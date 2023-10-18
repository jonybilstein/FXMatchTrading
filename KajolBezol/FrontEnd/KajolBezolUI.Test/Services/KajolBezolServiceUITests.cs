using KajolBezol.UI.Model;
using KajolBezol.UI.Services;
using KajolBezol.UI.Settings;
using Microsoft.Extensions.Configuration;
using Moq;

namespace KajolBezolUI.Test.Services
{
    [TestClass]
    public class KajolBezolServiceUITests
    {
        [TestMethod]
        public async Task KajolBezolServiceUI_Populates_UserProfile_OK()
        {

            IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.test.json")
                     .Build();



            var service = new KajolBezolServiceUI(config);
            var user = new UserLogin
            {
                UserName = "222",
                PassCode = "1234",
            };

            var profile = await service.PopulateProfile(user);

            Assert.IsTrue(profile.User.Email == "USER1@KKJJ.COM");
        }
    }
}