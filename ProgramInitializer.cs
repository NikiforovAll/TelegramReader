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

        public ProgramInitializer()
        {
        }
        public async void StartClient()
        {
            var creds = GetTelegramCredentials();
            var client = new TelegramClient(creds.appId, creds.hash);
            await client.ConnectAsync();

            await AuthUser(client);
        }

        private async Task AuthUser(TelegramClient client)
        {
            var userModel = GetUserConfig();
            var hash = await client.SendCodeRequestAsync(userModel.PhoneNumber);
            var code = DefaultPrompt.ShowDialog("Verification code", "Telegram verification");
            var user = await client.MakeAuthAsync("<user_number>", hash, code);
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
            if(Int32.TryParse(ConfigurationManager.AppSettings["appId"], out var appId)){
                return (appId, ConfigurationManager.AppSettings["appHash"]);
            }
            
            return (0, "");
        }


        private static  UserModel GetUserConfig()
        {
            var config = (UserSection)ConfigurationManager.GetSection("user");
            return config?.CreateUserConfig();
        }
            
    }

    class InitializerUtil
    {
    }
}
