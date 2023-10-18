using FirebaseAdmin.Messaging;
using KajolBezol.Interfaces;
using KajolBezol.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Server.IIS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Web;

namespace KajolBezol.Services
{
    public class KajolBezolService : IKajolBezolService
    {

        private string _connStr { get; set; }
        private IConfiguration _config;

        private readonly IHttpClientFactory _httpClientFactory;
        public KajolBezolService(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            this._config = config;
            _connStr = _config["DBConnectionString"].ToString();
            this._httpClientFactory = httpClientFactory;
        }


        public async Task<decimal> GetBlueNow()
        {


            //var httpClient = _httpClientFactory.CreateClient();
            //var response = await httpClient.GetAsync("https://api.bluelytics.com.ar/v2/latest");
            //var payload = await response.Content.ReadAsStringAsync();

            //dynamic prices = JsonConvert.DeserializeObject<ExpandoObject>(payload);
            //dynamic x = prices.blue as ExpandoObject;
            //decimal avg_price = (decimal)x.value_avg;



            return 1.36M;


        }
        public async Task<decimal> GetBuyingPrice()
        {
            //var httpClient = _httpClientFactory.CreateClient();
            //var response = await httpClient.GetAsync("https://api.bluelytics.com.ar/v2/latest");
            //var payload = await response.Content.ReadAsStringAsync();

            //dynamic prices = JsonConvert.DeserializeObject<ExpandoObject>(payload);
            //dynamic x = prices.blue as ExpandoObject;
            //decimal buy_price = (decimal)x.value_avg;

            //var fees = GetFees() / 2;
            //var newPrice = buy_price - (buy_price * fees / 100);
            //var newPriceRounded = decimal.Round(newPrice, 0);

            //return newPriceRounded;

            return 1.36M;



        }

        public async Task<decimal> GetSellingPrice()
        {

            //var httpClient = _httpClientFactory.CreateClient();
            //var response = await httpClient.GetAsync("https://api.bluelytics.com.ar/v2/latest");
            //var payload = await response.Content.ReadAsStringAsync();

            //dynamic prices = JsonConvert.DeserializeObject<ExpandoObject>(payload);
            //dynamic x = prices.blue as ExpandoObject;
            //decimal sell_price = (decimal)x.value_sell;


            //var fees = GetFees() / 2;
            //var newPrice = sell_price + (sell_price * fees / 100);
            //var newPriceRounded = decimal.Round(newPrice, 0);

            //return newPriceRounded;
            return 1.36M;
        }

        public decimal GetFees()
        {

            var sqlParam = new SqlParameter("@percentageFee", SqlDbType.Decimal)
            {
                Direction = ParameterDirection.Output
            };


            using (var conn = new SqlConnection(_connStr))
            using (var command = new SqlCommand("sp_getPercentageFee", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.Parameters.Add(sqlParam);
                command.ExecuteNonQuery();
            }
            return (decimal)sqlParam.Value;
        }

        public async Task<User> GetUser(UserLogin userToAuthenticate)
        {
            var userNameParam = new SqlParameter("@UserName", userToAuthenticate.UserName);
            var passwordParam = new SqlParameter("@Password", userToAuthenticate.PassCode);
            var deviceKeyParam = new SqlParameter("@DeviceKey", userToAuthenticate.DeviceKey);

            using (var conn = new SqlConnection(_connStr))
            using (var command = new SqlCommand("sp_Authenticate", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.Parameters.Add(userNameParam);
                command.Parameters.Add(passwordParam);
                command.Parameters.Add(deviceKeyParam);

                var userReader = command.ExecuteReader();

                if (userReader.HasRows)
                {

                    userReader.Read();
                    var returnUser = new User
                    {
                        Email = userReader["Email"].ToString(),
                        UserName = userReader["UserName"].ToString(),
                        Role = userReader["UserRole"].ToString(),
                        WhatsAppNumber = (int)userReader["WhatsAppNumber"],
                        FullName = userReader["FullName"].ToString(),
                        DeviceKey = userReader["DeviceKey"].ToString()
                    };

                    return returnUser;
                }


            }

            return null;

        }


        public async Task<List<TradeRequest>> GetPendingTradeRequests(string filterUserId, bool viewAllFields = false)
        {

            List<TradeRequest> requests = new List<TradeRequest>();


            using (var conn = new SqlConnection(_connStr))
            using (var command = new SqlCommand("usp_GetPendingTrades", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        var returnedRequest = new TradeRequest()
                        {
                            RequestId = (int)reader["RequestId"],
                            amount = (decimal)reader["amount"],
                            RequestTime = (DateTime)reader["RequestTime"],
                            Cancelled = (bool)reader["Cancelled"],
                            Fulfilled = (bool)reader["Fulfilled"],
                            Commited = (bool)reader["CommittedOperation"],

                        };

                        if (viewAllFields || reader["UserId"].ToString() == filterUserId)
                        {
                            returnedRequest.UserId = (int)reader["UserId"];
                        }

                        switch (reader["TradeType"])
                        {
                            case "Sell":
                                returnedRequest.Type = TradeType.Sell;
                                break;
                            case "Buy":
                                returnedRequest.Type = TradeType.Buy;
                                break;
                            default:
                                break;
                        }

                        requests.Add(returnedRequest);
                    };

                }
            }



            return requests;

        }

        public List<TradeRequest> InsertTradeRequest(int userId, decimal amount, TradeType tradeType)
        {

            if (!IsTradingOpen())
            {
                return null;
            }

            List<TradeRequest> requests = new List<TradeRequest>();

            string spName = tradeType == TradeType.Buy ? "spBuyRequest" : String.Empty;
            spName = tradeType == TradeType.Sell ? "spSellRequest" : spName;

            var userIdSql = new SqlParameter("@UserName", userId);
            var amountSql = new SqlParameter("@Amount", amount);

            using (var conn = new SqlConnection(_connStr))
            using (var command = new SqlCommand(spName, conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.Parameters.Add(userIdSql);
                command.Parameters.Add(amountSql);
                var reader = command.ExecuteReader();


                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        var returnedRequest = new TradeRequest()
                        {
                            UserId = (int)reader["UserId"],
                            RequestId = (int)reader["RequestId"],
                            amount = (decimal)reader["amount"],
                            RequestTime = (DateTime)reader["RequestTime"],
                            Cancelled = (bool)reader["Cancelled"],
                            Fulfilled = (bool)reader["Fulfilled"],
                            Commited = (bool)reader["CommittedOperation"],
                            TransactionId = reader["TransactionId"] as int?
                        };

                        switch (reader["TradeType"])
                        {
                            case "Sell":
                                returnedRequest.Type = TradeType.Sell;
                                break;
                            case "Buy":
                                returnedRequest.Type = TradeType.Buy;
                                break;
                            default:
                                break;
                        }

                        requests.Add(returnedRequest);

                    }


                    return requests;

                }


            }

            return null;

        }

        public bool UserHasNoBids(int userId)
        {
            var spName = "sp_UserhasNoBids";

            var userIdSql = new SqlParameter("@userId", userId);

            using (var conn = new SqlConnection(_connStr))
            using (var command = new SqlCommand(spName, conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.Parameters.Add(userIdSql);
                command.Parameters.Add("@retValue", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.ReturnValue;

                command.ExecuteNonQuery();

                var retVal = (int)command.Parameters["@retValue"].Value;

                if (retVal == 1)
                {
                    return true;
                }


            }

            return false;



        }

        public bool IsTradingOpen()
        {
            var spName = "sp_IsOpen";


            using (var conn = new SqlConnection(_connStr))
            using (var command = new SqlCommand(spName, conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.Parameters.Add("@retValue", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.ReturnValue;

                command.ExecuteNonQuery();

                var retVal = (int)command.Parameters["@retValue"].Value;

                if (retVal == 1)
                {
                    return true;
                }


            }

            return false;


        }

        public List<TradeRequest> GetRequests(int userNameAsInt)
        {

            List<TradeRequest> returnedRequests = new List<TradeRequest>();

            var userNameParam = new SqlParameter("@UserName", userNameAsInt);

            using (var conn = new SqlConnection(_connStr))
            using (var command = new SqlCommand("sp_MyRequests", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.Parameters.Add(userNameParam);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        var returnedRequest = new TradeRequest()
                        {
                            UserId = (int)reader["UserId"],
                            RequestId = (int)reader["RequestId"],
                            amount = (decimal)reader["amount"],
                            RequestTime = (DateTime)reader["RequestTime"],
                            Cancelled = (bool)reader["Cancelled"],
                            Fulfilled = (bool)reader["Fulfilled"],
                            Commited = (bool)reader["CommittedOperation"],
                            TransactionId = reader["TransactionId"] as int?
                        };

                        switch (reader["TradeType"])
                        {
                            case "Sell":
                                returnedRequest.Type = TradeType.Sell;
                                break;
                            case "Buy":
                                returnedRequest.Type = TradeType.Buy;
                                break;
                            default:
                                break;
                        }

                        returnedRequests.Add(returnedRequest);

                    }

                };

                return returnedRequests;
            }




        }

        public string GetUsePolicy()
        {
            var spName = "spUsePolicy";
            var policy = string.Empty;

            using (var conn = new SqlConnection(_connStr))
            using (var command = new SqlCommand(spName, conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                var value = command.ExecuteScalar();
                return value.ToString();

            }

            return policy;

        }

        public string GetWhatsAppContact()
        {
            var spName = "spWhatsAppcontact";
            var whatsAppURL = string.Empty;

            using (var conn = new SqlConnection(_connStr))
            using (var command = new SqlCommand(spName, conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                var value = command.ExecuteScalar();
                return value.ToString();

            }

            return whatsAppURL;

        }


        public int GetAppVersion()
        {
            var spName = "sp_AppVersion";
            string valueStr = string.Empty;

            using (var conn = new SqlConnection(_connStr))
            using (var command = new SqlCommand(spName, conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                valueStr = (string)command.ExecuteScalar();

            }
            return int.Parse(valueStr);

        }


        public async Task SendMessageToUser(string token, string title, string body)
        {

            var message = new Message()
            {
                Token = token,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body,

                }
            };

            await FirebaseMessaging.DefaultInstance.SendAsync(message);

        }

        public async Task SendEmailToUser(string emailAddr, string title, string body)
        {
            //var email = new Email()
            //{
            //    To = emailAddr,
            //    Title = title,
            //    Body = body
            //};

            //var json = JsonConvert.SerializeObject(email);
            //var data = new StringContent(json, Encoding.UTF8, "application/json");


            //var httpClient = _httpClientFactory.CreateClient();
            //var response = await httpClient.PostAsync($"{_config["EmailTriggerUrl"].ToString()}", data);

        }

        public string GetPrivacyPolicy()
        {
            var spName = "spPrivacyPolicy";
            var policy = string.Empty;

            using (var conn = new SqlConnection(_connStr))
            using (var command = new SqlCommand(spName, conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                var value = command.ExecuteScalar();
                return HttpUtility.HtmlDecode(value.ToString());
            }

            return policy;
        }
    }
}
