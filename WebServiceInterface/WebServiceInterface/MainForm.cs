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
            configPath = "config.json";
            configLibrary = new LibraryManager(configPath);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            drpdwnWebServices.DisplayMember = nameof(WebService.Name);
            drpdwnWebServices.ValueMember = nameof(WebService.Url);
            drpdwnWebServices.DataSource = configLibrary.Services;
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            SOAPWebService webService = new SOAPWebService(@"http://www.webservicex.net/airport.asmx");
            richtxtReturnValue.Text = await webService.CallMethodAsync(null);
        }
    }
}
