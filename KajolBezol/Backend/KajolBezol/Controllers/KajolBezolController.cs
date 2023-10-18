using KajolBezol.Hubs;
using KajolBezol.Interfaces;
using KajolBezol.Model;
using KajolBezol.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace KajolBezol.Controllers
{
    [ApiController]
    public class KajolBezolController : ControllerBase
    {

        private readonly ILogger<KajolBezolController> _logger;
        private IKajolBezolService KajolBeZolService { get; set; }
        private readonly IConfiguration _configuration;

        private readonly IHubContext<TradeContext> _hubContext;

        public KajolBezolController(ILogger<KajolBezolController> logger, IKajolBezolService kajolBezolservice,
            IHubContext<TradeContext> hubcontext, IConfiguration configuration)
        {
            _logger = logger;
            this.KajolBeZolService = kajolBezolservice;
            this._configuration = configuration;
            this._hubContext = hubcontext;

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IResult> Login(UserLogin userLogin)
        {

            try
            {

                var user = await KajolBeZolService.GetUser(userLogin);

                if (user == null)
                {
                    return Results.Unauthorized();
                }

                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes
                (_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("UserName", user.UserName),
                        new Claim("WhatsAppNumber",user.WhatsAppNumber.ToString()),
                        new Claim("Email",user.Email),
                        new Claim("UserRole",user.Role),
                        new Claim("DeviceKey",user.DeviceKey)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                var stringToken = tokenHandler.WriteToken(token);

                var userProfile = new UserProfile(this.KajolBeZolService, user, stringToken);

                return Results.Ok(userProfile);
            }
            catch
            {
                return Results.Unauthorized();
            }



        }


        [AllowAnonymous]
        [HttpGet]
        [Route("PrivacyPolicy")]
        public async Task<ContentResult> PrivacyPolicy()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = this.KajolBeZolService.GetPrivacyPolicy()
            };

        }



        [HttpPost]
        [Authorize]
        [Route("Profile")]
        public async Task<IResult> Profile()
        {

            try
            {

                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claims = identity.Claims;


                var user = new User
                {
                    Email = claims.Where(x => x.Type == "Email").First().Value,
                    Role = claims.Where(x => x.Type == "UserRole").First().Value,
                    UserName = claims.Where(x => x.Type == "UserName").First().Value,
                    WhatsAppNumber = int.Parse(claims.Where(x => x.Type == "WhatsAppNumber").First().Value)
                };

                var userProfile = new UserProfile(this.KajolBeZolService, user, accessToken);


                return Results.Ok(userProfile);
            }
            catch
            {
                return Results.Unauthorized();
            }



        }


        [Route("GetDollarPrice")]
        [HttpGet]
        [Authorize]
        public async Task<decimal> GetDollarPrice()
        {
            var blueCurrency = await this.KajolBeZolService.GetBlueNow();
            return blueCurrency;
        }

        [Route("GetSellingPrice")]
        [HttpGet]
        public async Task<decimal> GetSellingPrice()
        {
            return await this.KajolBeZolService.GetSellingPrice();

        }

        [Route("GetBuyingPrice")]
        [HttpGet]
        [Authorize]
        public async Task<decimal> GetBuyingPrice()
        {
            return await this.KajolBeZolService.GetBuyingPrice();
        }

        [Route("GetPendingTradeRequests")]
        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<List<TradeRequest>> GetPendingTradRequests()
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;

            var usernameClaim = claims
                .Where(x => x.Type == "WhatsAppNumber").First().Value;


            return await this.KajolBeZolService.GetPendingTradeRequests(usernameClaim, true);
        }


        [Route("GetUserPendingTradeRequests")]
        [HttpGet]
        [Authorize]
        public async Task<List<TradeRequest>> GetUserPendingTradRequests()
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;

            var usernameClaim = claims
                .Where(x => x.Type == "WhatsAppNumber").First().Value;

            return await this.KajolBeZolService.GetPendingTradeRequests(usernameClaim, false);
        }


        [Route("InsertBuyRequest")]
        [HttpPost]
        [Authorize]
        public async Task<List<TradeRequest>> InsertBuyRequest(int amount)
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;

            var usernameClaim = claims
                .Where(x => x.Type == "WhatsAppNumber").First().Value;

            var deviceKeyClaim = claims
                .Where(x => x.Type == "DeviceKey").First().Value;

            var emailClaim = claims
                .Where(x => x.Type == "Email").First().Value;

            var userNameAsInt = int.Parse(usernameClaim);

            var tradeRequests = this.KajolBeZolService.InsertTradeRequest(userNameAsInt, amount, TradeType.Buy);

            if (tradeRequests != null)
            {
                foreach (var request in tradeRequests)
                {
                    await this.KajolBeZolService.SendMessageToUser(deviceKeyClaim, request.Title, request.Body);
                    await this.KajolBeZolService.SendEmailToUser(emailClaim, request.Title, request.Body);


                }
            }

            return tradeRequests;
        }

        [Route("InsertSellRequest")]
        [HttpPost]
        [Authorize]
        public async Task<List<TradeRequest>> InsertSellRequest(int amount)
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;

            var usernameClaim = claims
                .Where(x => x.Type == "WhatsAppNumber").First().Value;

            var deviceKeyClaim = claims
                .Where(x => x.Type == "DeviceKey").First().Value;

            var emailClaim = claims
               .Where(x => x.Type == "Email").First().Value;

            var userNameAsInt = int.Parse(usernameClaim);

            var returnRequests = this.KajolBeZolService.InsertTradeRequest(userNameAsInt, amount, TradeType.Sell);

            if (returnRequests != null)
            {
                foreach (var request in returnRequests)
                {
                    await this.KajolBeZolService.SendMessageToUser(deviceKeyClaim, request.Title, request.ShortBody);
                    await this.KajolBeZolService.SendEmailToUser(emailClaim, request.Title, request.Body);
                }
            }

            return returnRequests;
        }

        [Route("UserHasNoBids")]
        [HttpGet]
        [Authorize]
        public bool UserHasNoBids()
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;

            var usernameClaim = claims
                .Where(x => x.Type == "WhatsAppNumber").First().Value;

            var userNameAsInt = int.Parse(usernameClaim);

            var retValue = this.KajolBeZolService.UserHasNoBids(userNameAsInt);

            return retValue;
        }

        [Route("IsTradingOpen")]
        [HttpGet]
        [Authorize]
        public bool IsTradingOpen()
        {


            var retValue = this.KajolBeZolService.IsTradingOpen();

            return retValue;
        }


        [Route("MyRequests")]
        [HttpGet]
        [Authorize]
        public async Task<List<TradeRequest>> MyRequests()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;

            var usernameClaim = claims
                .Where(x => x.Type == "WhatsAppNumber").First().Value;

            var userNameAsInt = int.Parse(usernameClaim);

            var retRequests = this.KajolBeZolService.GetRequests(userNameAsInt);

            return retRequests;
        }



        [Route("AppVersion")]
        [HttpGet]
        [AllowAnonymous]
        public int AppVersion()
        {

            var retValue = this.KajolBeZolService.GetAppVersion();
            return retValue;
        }




    }
}