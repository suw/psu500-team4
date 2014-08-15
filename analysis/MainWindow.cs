using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nlp_test1
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static void ThreadHP()
        {
            Application.Run(new PriceLoader());
        }
        public static void ThreadHS()
        {
            Application.Run(new Historical());
        }

        public static void ThreadLive()
        {           
            Application.Run(new RunItLive());
        }
       
        private void btnHistSent_Click(object sender, EventArgs e)
        {
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadHS));
            t.Start();
        }

        private void btnHistPrices_Click(object sender, EventArgs e)
        {
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadHP));
            t.Start();
        }

        private void btnLive_Click(object sender, EventArgs e)
        {
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadLive));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

    }
}
