using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TelegramReader
{
    class UserSection: ConfigurationSection
    {
        [ConfigurationProperty(nameof(PhoneNumber))]
        public UserElement PhoneNumber
        {
            get { return this[nameof(PhoneNumber)] as UserElement; }
            set { this[nameof(PhoneNumber)] = value; }
        }

        [ConfigurationProperty(nameof(ChannelName))]
        public UserElement ChannelName
        {
            get { return this[nameof(ChannelName)] as UserElement; }
            set { this[nameof(ChannelName)] = value; }
        }

        public UserModel CreateUserConfig()
        {
            return new UserModel()
            {
                PhoneNumber = this.PhoneNumber.InnerText
            };
        }
    }
}
