using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Messages;
using TLSharp.Core;
using Serilog;

namespace TelegramReader
{
    public class TelegramReaderClient
    {
        private readonly TelegramClient client;

        public TelegramReaderClient(TelegramClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task GetChannels()
        {
            var dialogs = (TLDialogsSlice)await client.GetUserDialogsAsync();
            var chat = dialogs.Chats
                .Where(c => c.GetType() == typeof(TLChannel))
                .Cast<TLChannel>()
                .FirstOrDefault(c => c.Title == "Saved Messages");
            Log.Information($"chat Id {chat?.Id}");
        }
    }
}
