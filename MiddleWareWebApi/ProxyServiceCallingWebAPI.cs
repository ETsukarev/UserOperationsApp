using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MiddleWareWebApi.Models;
using MiddleWareWebApi.Proxy;
using Newtonsoft.Json;

namespace MiddleWareWebApi
{
    public interface IProxyServiceCallingWebApi : IHostedService
    {
        Task<ServerSidePage> GetAllUsers(serverSideParams serverSidePrms);

        Task<ServerSidePage> GetAllUsersWithoutAdmins(serverSideParams serverSid);

        Task<User> CheckUserAndPasswd(string user, string password);

        HttpStatusCode DeleteUser(int userId);

        Task<User> GetUser(int userId);

        HttpStatusCode SaveUser(User user);

        HttpStatusCode NewUser(User user);

        string ExistLogin(string login, int userId);
    }

    public class ProxyServiceCallingWebApi : IProxyServiceCallingWebApi, IDisposable
    {
        private readonly HttpClient _client;

        private const string ApiUsersRoot = "api/Users";

        private const string ApiUsersGetAllUsers = "api/Users/AllUsers?";

        private const string ApiUsersWithoutAdmins = "api/Users/WithOutAdmins?";

        private const string ApiUsersExistThatLogin = "/ExistThatLogin?loginCheck=";

        private const string ServerSideParamDraw = "draw=";

        private const string ServerSideParamStart = "&start=";

        private const string ServerSideParamLength = "&length=";

        private const string ServerSideParamSearchValue = "&searchValue=";

        private const string ServerSideParamSearchRegex = "&searchRegex=";

        private const string ServerSideParamOrderColumns = "&orderColumns=";

        private const string ServerSideParamOrderDirs = "&orderDirs=";

        private const string ServerSideParamColumnsDatas = "&columnsDatas=";

        private const string ServerSideParamColumnsSearchable = "&columnsSearchable=";

        private const string ServerSideParamColumnsOrderable = "&columnsOrderable=";

        private const string ServerSideParamСolumnsSearchValue = "&columnsSearchValue=";

        private const string ServerSideParamColumnsSearchRegex = "&columnsSearchRegex=";

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
            var appConfig1 = appConfig;
            _client = new HttpClient {BaseAddress = new Uri(appConfig1.Value.Uri)};
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(appConfig1.Value.MediaType));
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

        #region Implemented_IProxyServiceCallingWebApi

        public string ExistLogin(string login, int userId)
        {
            var result = ExistThatLogin(login, userId);


            if (result.Error == null && !result.ResultCheckLogin)
            {
                return string.Empty;
            }

            if (result.ResultCheckLogin)
            {
                return "Пользователь с таким логином уже cуществует. Выберите другое имя !";
            }

            if (result.Error != null)
            {
                return result.Error.Message;
            }

            return string.Empty;
        }

        public async Task<ServerSidePage> GetAllUsers(serverSideParams serverSidePrms)
        {
            try
            {
                HttpResponseMessage response = _client.GetAsync(PackServerSideInParams(ApiUsersGetAllUsers, serverSidePrms)).Result;

                if (response.IsSuccessStatusCode)
                {
                    var stringResult = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ServerSidePage>(stringResult);
                }

                return new ServerSidePage() { error = $"Error status: {response.StatusCode.ToString()}" };
            }
            catch (Exception e)
            {
                return new ServerSidePage(){error = e.Message};
            }
        }

        public async Task<ServerSidePage> GetAllUsersWithoutAdmins(serverSideParams serverSidePrms)
        {
            try
            {
                HttpResponseMessage response = _client.GetAsync(PackServerSideInParams(ApiUsersWithoutAdmins, serverSidePrms)).Result;

                if (response.IsSuccessStatusCode)
                {
                    var stringResult = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ServerSidePage>(stringResult);
                }

                return new ServerSidePage() { error = $"Error status: {response.StatusCode.ToString()}" };
            }
            catch (Exception e)
            {
                return new ServerSidePage() { error = e.Message };
            }
        }

        public async Task<User> CheckUserAndPasswd(string user, string password)
        {
            var builderQuery = new StringBuilder()
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
            HttpResponseMessage response = _client.PutAsync(PackUserInParams(SaveUserBegin, user), null).Result;
            return response.StatusCode;
        }

        public HttpStatusCode NewUser(User user)
        {
            HttpResponseMessage response = _client.PostAsync(PackUserInParams(NewUserBegin, user), null).Result;
            return response.StatusCode;
        }

        #endregion

        public void Dispose()
        {
        }

        private static string PackUserInParams(string beginParams, User user)
        {
            var builderQuery = new StringBuilder()
                .Append(beginParams)
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

            return builderQuery.ToString();
        }

        private static string PackServerSideInParams(string beginParams, serverSideParams serverSidePrms)
        {
            var strBuilderParams = new StringBuilder()
                .Append(beginParams)
                .Append(ServerSideParamDraw)
                .Append(serverSidePrms.draw)
                .Append(ServerSideParamStart)
                .Append(serverSidePrms.start)
                .Append(ServerSideParamLength)
                .Append(serverSidePrms.length)
                .Append(ServerSideParamColumnsDatas)
                .Append(serverSidePrms.columnsDatas)
                .Append(ServerSideParamColumnsOrderable)
                .Append(serverSidePrms.columnsOrderable)
                .Append(ServerSideParamColumnsSearchable)
                .Append(serverSidePrms.columnsSearchable)
                .Append(ServerSideParamColumnsSearchRegex)
                .Append(serverSidePrms.columnsSearchRegex)
                .Append(ServerSideParamSearchValue)
                .Append(serverSidePrms.searchValue)
                .Append(ServerSideParamOrderColumns)
                .Append(serverSidePrms.orderColumns)
                .Append(ServerSideParamOrderDirs)
                .Append(serverSidePrms.orderDirs)
                .Append(ServerSideParamСolumnsSearchValue)
                .Append(serverSidePrms.columnsSearchValue)
                .Append(ServerSideParamSearchRegex)
                .Append(serverSidePrms.searchRegex);

            return strBuilderParams.ToString();
        }

        private CheckLoginResult ExistThatLogin(string login, int userId)
        {
            try
            {
                HttpResponseMessage response = _client.GetAsync($"{ApiUsersRoot}{ApiUsersExistThatLogin}{login}&userId={userId}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var stringResult = response.Content.ReadAsStringAsync().Result;
                    var res = JsonConvert.DeserializeObject<bool>(stringResult);
                    return new CheckLoginResult() { ResultCheckLogin = res, Error = null };
                }
                return new CheckLoginResult() { ResultCheckLogin = false, Error = new Exception($"Error status: {response.StatusCode.ToString()}") };
            }
            catch (Exception ex)
            {
                return new CheckLoginResult() { ResultCheckLogin = false, Error = new Exception($"Error status: {ex}") };
            }
        }
    }
}
