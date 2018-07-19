using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using UserWebApi.Models;

namespace UserMVCApp
{
    public interface IProxyServiceCallingWebApi : IHostedService
    {
        Task<List<User>> GetAllUsers();

        Task<List<User>> GetAllUsersWithoutAdmins();

        Task<User> CheckUserAndPasswd(string user, string password);

        HttpStatusCode DeleteUser(int userId);

        Task<User> GetUser(int userId);

        HttpStatusCode SaveUser(User user);

        HttpStatusCode NewUser(User user);
    }

    public class ProxyServiceCallingWebApi : IProxyServiceCallingWebApi, IDisposable
    {
        private readonly IOptions<UriWebAPI> _appConfig;

        private readonly HttpClient _client;

        private const string ApiUsersRoot = "api/Users";

        private const string ApiUsersWithoutAdmins = "api/Users/WithOutAdmins";

        private const string ApiCheckUserLogin = "/CheckUser?login=";

        private const string ApiCheckUserPassword = "&password=";

        private const string ApiDeleteUser = "api/Users/Delete?id=";

        private const string GeyUserById = "api/Users/GetUser?id=";

        private const string SaveUserBegin = "api/Users/SaveUser?id=";

        private const string SaveUserLogin = "&login=";

        private const string SaveUserPassword = "&password=";

        private const string SaveUserFirstName = "&FirstName=";

        private const string SaveUserMiddleName = "&MiddleName=";

        private const string SaveUserLastName = "&LastName=";

        private const string SaveUserTelephone = "&Telephone=";

        private const string SaveUserIsAdmin = "&IsAdmin=";

        private const string NewUserBegin = "api/Users/NewUser?id=";

        public ProxyServiceCallingWebApi(IOptions<UriWebAPI> appConfig)
        {
            _appConfig = appConfig;
            _client = new HttpClient {BaseAddress = new Uri(_appConfig.Value.Uri)};
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(_appConfig.Value.MediaType));
        }

        #region Implemented_IHostedService

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #endregion

        public async Task<List<User>> GetAllUsers()
        {
            HttpResponseMessage response = await _client.GetAsync(ApiUsersRoot);

            if (response.IsSuccessStatusCode)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<User>>(stringResult);
            }
            return new List<User>();
        }

        public async Task<List<User>> GetAllUsersWithoutAdmins()
        {
            HttpResponseMessage response = await _client.GetAsync(ApiUsersWithoutAdmins);

            if (response.IsSuccessStatusCode)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<User>>(stringResult);
            }
            return new List<User>();
        }

        public async Task<User> CheckUserAndPasswd(string user, string password)
        {
            StringBuilder builderQuery = new StringBuilder()
                .Append(ApiUsersRoot)
                .Append(ApiCheckUserLogin)
                .Append(user)
                .Append(ApiCheckUserPassword)
                .Append(password);

            HttpResponseMessage response = await _client.GetAsync(builderQuery.ToString());

            if (response.IsSuccessStatusCode)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(stringResult);
            }
            return new User();
        }

        public HttpStatusCode DeleteUser(int userId)
        {
            HttpResponseMessage response =  _client.DeleteAsync($"{ApiDeleteUser}{userId}").Result;
            return response.StatusCode;
        }

        public async Task<User> GetUser(int userId)
        {
            HttpResponseMessage response = _client.GetAsync($"{GeyUserById}{userId}").Result;
            if (response.IsSuccessStatusCode)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(stringResult);
            }
            return new User();
        }

        public HttpStatusCode SaveUser(User user)
        {
            StringBuilder builderQuery = new StringBuilder()
                .Append(SaveUserBegin)
                .Append(user.Id)
                .Append(SaveUserLogin)
                .Append(user.Login)
                .Append(SaveUserPassword)
                .Append(user.Password)
                .Append(SaveUserFirstName)
                .Append(user.FirstName)
                .Append(SaveUserMiddleName)
                .Append(user.MiddleName)
                .Append(SaveUserLastName)
                .Append(user.LastName)
                .Append(SaveUserTelephone)
                .Append(user.Telephone)
                .Append(SaveUserIsAdmin)
                .Append(user.IsAdmin);


            HttpResponseMessage response = _client.PutAsync(builderQuery.ToString(), null).Result;
            return response.StatusCode;
        }

        public HttpStatusCode NewUser(User user)
        {
            StringBuilder builderQuery = new StringBuilder()
                .Append(NewUserBegin)
                .Append(user.Id)
                .Append(SaveUserLogin)
                .Append(user.Login)
                .Append(SaveUserPassword)
                .Append(user.Password)
                .Append(SaveUserFirstName)
                .Append(user.FirstName)
                .Append(SaveUserMiddleName)
                .Append(user.MiddleName)
                .Append(SaveUserLastName)
                .Append(user.LastName)
                .Append(SaveUserTelephone)
                .Append(user.Telephone)
                .Append(SaveUserIsAdmin)
                .Append(user.IsAdmin);

            HttpResponseMessage response = _client.PostAsync(builderQuery.ToString(), null).Result;
            return response.StatusCode;
        }

        public void Dispose()
        {
        }
    }
}
