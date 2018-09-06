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

        private Dictionary<int, string> Messages { get; set; } = new Dictionary<int, string>();
        private readonly TelegramClient client;

        public TelegramReaderClient(TelegramClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<(int id, string message, bool isNew)> GetChannel(string channelName)
        {
            var dialogs = (TLDialogsSlice)await client.GetUserDialogsAsync();
            var channel = dialogs.Chats
                .Where(c => c.GetType() == typeof(TLChannel))
                .Cast<TLChannel>()
                .FirstOrDefault(c => c.Title == channelName);

            var messages = (TLChannelMessages)await client
                .GetHistoryAsync(
                    new TLInputPeerChannel()
                    {
                        ChannelId = channel.Id,
                        AccessHash = channel.AccessHash ?? 0
                    }, 0, -1, 2);

            var hist = messages.Messages
                .Cast<TLMessage>()
                .OrderByDescending(m => m.Date)
                .FirstOrDefault();
            var result = (0, "", false);
            if(hist != null && !Messages.ContainsKey(hist.Id))
            {
                result = (hist.Id, hist.Message, true);
                Messages.Add(hist.Id, hist.Message);
            }
            Log.Information($"Current message: {result}");
            return result;
        }
    }
}
