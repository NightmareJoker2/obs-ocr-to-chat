using ObsOcrToChat.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TwitchLib.Api;
using TwitchLib.Api.Auth;
using TwitchLib.Api.Core.Enums;
using TwitchLib.Api.Helix.Models.Users.GetUsers;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace ObsOcrToChat
{
    internal class Bot
    {
        TwitchClient client = new TwitchClient();
        TwitchAPI api = new TwitchAPI();
        string clientId = "muk6gd4m3t7cp23e6842gibjlaau66";
        public string userName = null;
        public bool isConnected { get { return client.IsConnected; } }

        public Bot()
        {
            Initialize();
        }

        public async void Authenticate()
        {
            if (!client.IsConnected)
            {
                Authentication auth = new Authentication();
                auth.SendRequestToBrowser(clientId);
                AuthenticationModel authResult = await auth.GetAuthenticationValuesAsync();
                Settings.Default.AccessToken = authResult.Token;

                Initialize();
            }
        }

        public async void Initialize()
        {
            if (!string.IsNullOrEmpty(Settings.Default.AccessToken))
            {
                api.Settings.AccessToken = Settings.Default.AccessToken;
                api.Settings.ClientId = clientId;
                ValidateAccessTokenResponse authenticated = await api.Auth.ValidateAccessTokenAsync(Settings.Default.AccessToken);
                GetUsersResponse user = await api.Helix.Users.GetUsersAsync(null, null, Settings.Default.AccessToken);
                userName = user.Users[0].Login;
                ConnectionCredentials credentials = new ConnectionCredentials(userName, Settings.Default.AccessToken);
                ClientOptions clientOptions = new ClientOptions
                {
                    MessagesAllowedInPeriod = 750,
                    ThrottlingPeriod = TimeSpan.FromSeconds(30)
                };
                WebSocketClient customClient = new WebSocketClient(clientOptions);
                client = new TwitchClient(customClient);
                client.Initialize(credentials, userName);
                Connect();
            }
        }

        public void Connect()
        {
            client.Connect();
            client.JoinChannel(userName);
        }

        public void Send(string message)
        {
            client.SendMessage(client.JoinedChannels.First(), message);
        }
    }

    public class Authentication
    {
        private readonly HttpListener TwitchListener;
        private const string ReturnUrl = "http://localhost:56709";

        public Authentication()
        {
            TwitchListener = new HttpListener();
            TwitchListener.Prefixes.Add(ReturnUrl + "/");
        }

        /// <summary>
        /// Get Twitch Authentication Async
        /// </summary>
        /// <returns></returns>
        public Task<AuthenticationModel> GetAuthenticationValuesAsync()
        {
            return Task.Run(() => GetAuthenticationValues());
        }

        /// <summary>
        /// Start the listener
        /// </summary>
        /// <returns>returns IsListening Value</returns>
        private bool StartListener()
        {
            if (!TwitchListener.IsListening)
            {
                try
                {
                    TwitchListener.Start();
                }
                catch (HttpListenerException ex)
                {
                    throw new Exception("Can't start listener for Twitch authentication" + Environment.NewLine + ex);
                }
            }

            return TwitchListener.IsListening;
        }

        /// <summary>
        /// Stop the listener
        /// </summary>
        private void StopListener()
        {
            TwitchListener.Stop();
        }

        /// <summary>
        /// Get Twitch Auths
        /// </summary>
        /// <returns></returns>
        public AuthenticationModel GetAuthenticationValues()
        {
            StartListener();

            AuthenticationModel Values = null;

            while (TwitchListener.IsListening)
            {
                var context = TwitchListener.GetContext();

                if (context.Request.QueryString.HasKeys())
                {
                    if (context.Request.RawUrl.Contains("access_token"))
                    {
                        Uri myUri = new Uri(context.Request.Url.OriginalString);
                        string scope = HttpUtility.ParseQueryString(myUri.Query).Get("scope");
                        string access_token = HttpUtility.ParseQueryString(myUri.Query).Get(0).Replace("access_token=", "");

                        if (!String.IsNullOrEmpty(scope) && !String.IsNullOrEmpty(access_token))
                        {
                            Values = GetModel(access_token, scope);
                        }
                    }
                }

                byte[] b = Encoding.UTF8.GetBytes(GetResponse());
                context.Response.StatusCode = 200;
                context.Response.KeepAlive = false;
                context.Response.ContentLength64 = b.Length;

                var output = context.Response.OutputStream;
                output.Write(b, 0, b.Length);
                context.Response.Close();

                if (Values != null)
                {
                    StopListener();
                    return Values;
                }
            }

            return null;
        }

        /// <summary>
        /// Creates the Response for TwitchOAuth
        /// </summary>
        /// <returns>Response</returns>
        private string GetResponse()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<html>");
            builder.Append(Environment.NewLine);
            builder.Append("<head>");
            builder.Append(Environment.NewLine);
            builder.Append("<title>Twitch Oauth Return</title>");
            builder.Append(Environment.NewLine);
            builder.Append("<script language=\"JavaScript\">");
            builder.Append(Environment.NewLine);
            builder.Append("if(window.location.hash) {");
            builder.Append(Environment.NewLine);
            builder.Append("window.location.href = window.location.href.replace(\"/#\",\"?=\");");
            builder.Append(Environment.NewLine);
            builder.Append("}");
            builder.Append(Environment.NewLine);
            builder.Append("</script>");
            builder.Append(Environment.NewLine);
            builder.Append("</head>");
            builder.Append(Environment.NewLine);
            builder.Append("<body><p>You can close this tab.</p></body>");
            builder.Append(Environment.NewLine);
            builder.Append("</html>");

            return builder.ToString();
        }

        /// <summary>
        /// Creates the Model to return
        /// </summary>
        /// <param name="token">Twitch Token</param>
        /// <param name="scopes">Twitch Scopes</param>
        /// <returns></returns>
        private AuthenticationModel GetModel(string token, string scopes)
        {
            return new AuthenticationModel
            {
                Token = token,
                Scopes = scopes
            };
        }

        /// <summary>
        /// Starts the Request for the User to authorize for twitch
        /// </summary>
        /// <param name="ClientID"></param>
        public void SendRequestToBrowser(string ClientID)
        {
            Thread.Sleep(500);

            string url = GetUrl(ClientID);
            Uri uri = new Uri(url);

            Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
        }

        /// <summary>
        /// Returns the URL which we call to create a oauth token
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        private static string GetUrl(string ClientID)
        {
            var api = new TwitchAPI();
            return api.Auth.GetAuthorizationCodeUrl(ReturnUrl, new[] { AuthScopes.Chat_Edit, AuthScopes.Chat_Read }, false, null, ClientID).Replace("response_type=code", "response_type=token");
        }
    }
    public class AuthenticationModel
    {
        public string Token { get; set; }
        public string Scopes { get; set; }
    }
}
