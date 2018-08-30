using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelegramReader
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            initializer = new ProgramInitializer();
            initializer.StartClient();
        }

        internal ProgramInitializer initializer { get; }

        private void button1_Click(object sender, EventArgs e)
        {
            var pollingClient = new TelegramReaderClient(initializer.Client);
            pollingClient.GetChannels();
        }
    }
}
