using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace CefSharpExample
{
    public partial class FormMain : Form
    {
        private string web_address = string.Empty;
        Button btn;
        public ChromiumWebBrowser browser;
        private bool has_args=false;


        public FormMain(string[] args)
        {
            InitializeComponent();
            if (args.Length > 0)
            {
                has_args = true;
                web_address = args[0];
            }
            else
            {
                if(File.Exists("address.txt"))
                    web_address = System.IO.File.ReadLines("address.txt").FirstOrDefault();
            }

            this.Width = 800;
            this.Height = 400;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(-5, 650);

            btn = new Button();
            btn.Text = "Top [Off]";
            btn.Click += Btn_Click;
            this.Controls.Add(btn);

            InitializeChromium();
        }


        private void Btn_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
            btn.Text = this.TopMost ? "Top [On]" : "Top [Off]";
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
        }

        private void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            settings.CefCommandLineArgs.Add("disable-image-loading", "1");
            Cef.Initialize(settings);
            browser = new ChromiumWebBrowser(web_address)
            {
                Dock = DockStyle.Fill,
            }; ;
            this.Controls.Add(browser);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!has_args)
            {
                web_address = browser.Address;
                File.WriteAllText("address.txt", web_address);
            }
            Cef.Shutdown();
        }
    }
}
