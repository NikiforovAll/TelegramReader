using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using System.Configuration;
using TLSharp.Core;

namespace TelegramReader
{
    class ProgramInitializer
    {

        public TelegramClient Client { get; private set; }
        public UserModel UserModel { get; private set; }
        public ProgramInitializer()
        {
            SetUpLogger();
        }
        public async Task StartClient()
        {
            var creds = GetTelegramCredentials();
            UserModel = GetUserConfig();
            var client = new TelegramClient(creds.appId, creds.hash);
            await client.ConnectAsync();
            if (!client.IsConnected)
            {
                await AuthUser(client);
            }
            Client = client;
        }

        private async Task AuthUser(TelegramClient client)
        {
            var hash = await client.SendCodeRequestAsync(UserModel.PhoneNumber);
            var code = DefaultPrompt.ShowDialog("Verification code", "Telegram verification");
            var user = await client.MakeAuthAsync(UserModel.PhoneNumber, hash, code);
        }

        public void SetUpLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        private static (int appId, string hash) GetTelegramCredentials()
        {
            if (Int32.TryParse(ConfigurationManager.AppSettings["appId"], out var appId))
            {
                return (appId, ConfigurationManager.AppSettings["appHash"]);
            }

            return (0, "");
        }

        private static UserModel GetUserConfig()
        {
            var config = (UserSection)ConfigurationManager.GetSection("user");
            var result = config?.CreateUserConfig();
            Log.Information($"User:\n {result}");
            return result;
        }
    }

    class InitializerUtil
    {
    }
}
