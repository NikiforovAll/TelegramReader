using Serilog;
using System;
using System.Configuration;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using WMPLib;

namespace TelegramReader
{
    public partial class MainForm : Form
    {



        public int TIMER_DELAY { get; private set; } = 1800 * 000;

        internal ProgramInitializer Initializer { get; }
        internal TelegramReaderClient TGClient { get; private set; }
        internal System.Timers.Timer MainTimer { get; private set; }
        internal WindowsMediaPlayer AudioPlayer { get; private set; }

        public MainForm()
        {
            InitializeComponent();
            Initializer = new ProgramInitializer();
            InitializeTimer();
            InitializeAudioPlayer();
            Load += MainForm_Load;

        }

        private void InitializeAudioPlayer()
        {
            AudioPlayer = new WMPLib.WindowsMediaPlayer();
            AudioPlayer.settings.setMode("loop", true);
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

            var rowMessage = await TGClient.GetNewMessage(Initializer.UserModel.ChannelName.Trim());
            if (rowMessage.isNew)
            {
                if (sender != null)
                {
                    AudioPlayer.URL = ConfigurationManager.AppSettings["alarm-track"];
                    AudioPlayer.controls.play();
                }

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
            MainTimer.Start();
            Log.Information("Turned on Timer");
            OnTimedEvent(null, null);
            ToggleTimerActivationButtons();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            MainTimer.Enabled = false;
            Log.Information("Turned off Timer");
            ToggleTimerActivationButtons();

        }

        private void ToggleTimerActivationButtons()
        {
            if (button1.Enabled)
            {
                button1.Enabled = false;
                button2.Enabled = true;
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AudioPlayer.controls.stop();
        }
    }
}
