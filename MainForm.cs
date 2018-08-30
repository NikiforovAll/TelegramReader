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
            var initializer = new ProgramInitializer();
            initializer.SetUpLogger();
            initializer.StartClient();
        }
    }
}
