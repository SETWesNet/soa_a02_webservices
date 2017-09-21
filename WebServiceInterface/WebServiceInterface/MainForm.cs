using System;
using System.Diagnostics;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Xml;
using System.IO;
using WebServiceInterface.Library;

namespace WebServiceInterface
{
    public partial class MainForm : Form
    {
        private LibraryManager configLibrary;
        private string configPath;

        public MainForm()
        {
            InitializeComponent();
            configPath = "test.txt";
            configLibrary = new LibraryManager(configPath);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }
    }
}
