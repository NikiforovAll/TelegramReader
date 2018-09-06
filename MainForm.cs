using Serilog;
using System;
using System.Configuration;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace TelegramReader
{
    public partial class MainForm : Form
    {


        internal ProgramInitializer Initializer { get; }
        internal TelegramReaderClient TGClient { get; private set; }
        internal System.Timers.Timer MainTimer { get; private set; }
        public int TIMER_DELAY { get; private set; } = 1800 * 000;

        public MainForm()
        {
            InitializeComponent();
            Initializer = new ProgramInitializer();
            InitializeTimer();
            Load += MainForm_Load;

        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            if (TGClient == null)
            {
                await Initializer.StartClient();
                TGClient = new TelegramReaderClient(Initializer.Client);
            }
        }

        private void InitializeTimer()
        {
            if (!Int32.TryParse(ConfigurationManager.AppSettings["timerDelay"], out var timerDelay))
            {
                timerDelay = TIMER_DELAY;
            }

            MainTimer = new System.Timers.Timer(timerDelay);

            MainTimer.Elapsed += OnTimedEvent;
        }

        private async void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            Log.Information("Tick!");

            var rowMessage = await TGClient.GetChannel(Initializer.UserModel.ChannelName);
            if (rowMessage.isNew)
            {
                listView1.Invoke((MethodInvoker)delegate
                {
                    listView1.Items.Add(
                        new ListViewItem(new string[] { rowMessage.id.ToString(), rowMessage.message })
                    );
                });
            }

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            MainTimer.Enabled = true;
            Log.Information("Turned on Timer");
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            MainTimer.Enabled = false;
            Log.Information("Turned off Timer");

        }
    }
}
