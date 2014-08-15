using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using HtmlAgilityPack;
using nlp_test1.Properties;
using System.Configuration;
using MySql;
using MySql.Data.MySqlClient;

namespace nlp_test1
{
    public partial class Historical : Form
    {
        public static string startURL = "http://www.bloomberg.com/archive/news/"; //http://www.bloomberg.com/archive/news/2014-06-25/

        public static bool inTesting = false;
        public static bool printAPIresultXML = false;
        public static bool runHistorical;
        public static bool runningHistorical = false;
        public static bool setupDone;
        
        private BackgroundWorker _worker;
        
        int daysToRun;

        private static System.Timers.Timer aTimer;
        
        public static Dictionary<string, linkDets> dicLinks = new Dictionary<string, linkDets>();

        public class linkDets
        {
            public string source;
            public string title;
            public string url;
            public string date;
            public bool done;
        }

        public static string[] symArray = new string[Settings.Default.Symbols.Count];
        public static string symbs;
        
        public Historical()
        {
            InitializeComponent();
            setupForm();
            GetSymbols();

            this.dataGridView1.DataError +=
                new DataGridViewDataErrorEventHandler(DataGridView1_DataError);
        }

        private void setupForm()
        {
            dataGridView1.Columns.Add("date","date");
            dataGridView1.Columns.Add("url","url");
            dataGridView1.Columns.Add("status","status");
        }

        public static void GetSymbols()
        {
            Settings.Default.Symbols.CopyTo(symArray, 0);
            string symbs = string.Join("','", symArray);
        }

        private void InitWorker(bool live)
        {
            if (_worker != null)
            {
                _worker.Dispose();
            }

            _worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            if (!live)
                _worker.DoWork += DoWork;
            else
                _worker.DoWork += DoWorkLive;

            _worker.RunWorkerCompleted += RunWorkerCompleted;
            _worker.ProgressChanged += ProgressChanged;
            _worker.RunWorkerAsync();
        }
        void DoWork(object sender, DoWorkEventArgs e)
        {
            if (_worker.CancellationPending)
            {
                e.Cancel = true;
            }
            else
            {
                StartHistorical();                
            }
        }
                
        public void Callback(object source, ElapsedEventArgs e) //(object state)
        {
            if (runningHistorical)
                return;

            runningHistorical = true;

            using (StreamWriter writer = new StreamWriter("errors-exceptions-Live_S-Log.txt", true))
            {
                writer.WriteLine(DateTime.Now.ToString("MM/dd/yy | hh:mm:ss :: ") + " *** STARTING TIMER***..." );
            }

            _DayLbl(DateTime.Now.ToString("[HH:mm:ss]"));
            StartLive();
            runningHistorical = false;
        }

        public void Historical_Load(object sender, EventArgs e)
        { }
        

        void DoWorkLive(object sender, DoWorkEventArgs e)
        {
            if (_worker.CancellationPending)
            {
                e.Cancel = true;
            }
            else
            {                
                aTimer = new System.Timers.Timer(60000);
                aTimer.Elapsed += new ElapsedEventHandler(Callback);
                aTimer.Enabled = true;
            }
        }
        void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // Display some message to the user that task has been
                // cancelled
            }
            else if (e.Error != null)
            {
                // Do something with the error
            }
        }

        void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblPercent.Text = string.Format("R: {0}| P: {1}", e.UserState, e.ProgressPercentage);
        }

        
        public static void StartHistorical_ps(DateTime startDate, DateTime endDate)
        {
            int dToRun = Convert.ToInt16((endDate - startDate).TotalDays);

            for (int d = 0; d <= dToRun; d++)
            {
                string dayDate = startDate.AddDays(d).ToString("yyyy-MM-dd/");
                string dayUrl = startURL + dayDate;

                GetLinks_ps(dayUrl, startDate, endDate);                
            }

            SentimentToDB.LoadProcessedLinks(endDate);

            ProcessLinks_ps();
        }

        public void StartHistorical()
        {
            daysToRun = Convert.ToInt16((dtpEndDate.Value - dtpStartDate.Value).TotalDays);

            for (int d = 0; d <= daysToRun; d++)
            {
                string dayDate = dtpStartDate.Value.AddDays(d).ToString("yyyy-MM-dd/");
                string dayUrl = startURL + dayDate;
                
                GetLinks(dayUrl);
                _DayLbl("day " + d + " / " + daysToRun);

            }

            FillTopGrid();

            ProcessLinks();
            Application.DoEvents();
            
            FillBottomGrid();
        }
        public void StartLive()
        {            
            string dayDate = DateTime.Now.ToString("yyyy-MM-dd/");
            string dayUrl = startURL + dayDate;            
            GetLinks(dayUrl);
            _DayLbl(dayDate);
                      
            FillTopGrid();

            Thread t2 = new Thread(GetLivePrices);
            t2.Start();

            ProcessLinks();

            FillBottomGrid();
        }


        public static void GetLivePrices()
        {
            GetSymbols();
            
            string u1 = @"http://download.finance.yahoo.com/d/quotes.csv?s=";
            string u2 = @"&f=sl1d1t1c1hgvbap2";

            foreach (string sym in symArray)
            {
                string symUrl = u1 + sym + u2;
                
                System.Net.WebClient webClient = new WebClient();
                webClient.Proxy = HttpWebRequest.GetSystemWebProxy();
                webClient.Proxy.Credentials = CredentialCache.DefaultCredentials;
                
                Stream strm = null;
                int tries = 0;
                while (tries < 5 && strm == null)
                {
                    try
                    {                     
                        strm = webClient.OpenRead(symUrl);
                    }
                    catch
                    {
                        tries++;
                    }
                }
                
                if (tries >= 5)
                {
                    MessageBox.Show("Cannot connect to: " + symUrl);
                }
                
                StreamReader sr = new StreamReader(strm);
                string result = sr.ReadToEnd();
                strm.Close();
                
                string[] prxArray = result.Split(',');
                
                try
                {
                    WriteLivePrxToDB(prxArray[0], prxArray[2], prxArray[1]);
                }
                catch
                {
                    ///// array not long enough??? check problem//??
                }
            }
            //return result;
        }

        public static void WriteLivePrxToDB(string symbol, string date, string price)
        {
            using (MySqlConnection cn1 = new MySqlConnection(PriceLoader.connectMySQL))
            {
                MySqlCommand cmd;
                                
                if (cn1.State != ConnectionState.Open)
                    cn1.Open();

                DateTime _date = Convert.ToDateTime(date.Replace(@"\", "").Replace("\"", ""));
                string _symbol = symbol.Replace("\"", "");
                double _price = Convert.ToDouble(price);
                
                string qry = "SELECT COUNT(symbol) FROM dailyPrices WHERE date = '" + _date.ToString("yyyy-MM-dd") + "' AND symbol = '" + _symbol + "'";
                cmd = new MySqlCommand(qry, cn1);
                int outCount = Convert.ToInt32(cmd.ExecuteScalar());


                if (outCount == 0)
                {
                    //insert
                    qry = @"INSERT INTO dailyPrices (symbol, date, adjclose) VALUES ('"+_symbol+"','"+_date.ToString("yyyy-MM-dd") + "',"+_price+");";
                    cmd = new MySqlCommand(qry, cn1);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    //modify
                    qry = @"UPDATE dailyPrices SET adjclose = " + _price + "  WHERE date = '" + _date.ToString("yyyy-MM-dd") + "' AND symbol = '" + _symbol + "'";
                    cmd = new MySqlCommand(qry, cn1);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        private void _DayLbl(string lblText)
        {
            MethodInvoker inv = delegate
            {
                this.lblBottom1L.Text = lblText;              
            };

         this.Invoke(inv);
        }

        public void _LinkLbl(string lblText)
        {
            MethodInvoker inv = delegate
            {
                this.lblBottom1R.Text = lblText;              
            };

            this.Invoke(inv);
        }

        public void _DateLbl(string lblDate)
        {
            MethodInvoker inv = delegate
            {
                this.lblDate.Text = lblDate;            
            };

            this.Invoke(inv);
        }

        public void _addTopGridLbl(string date, string url, string status)
        {
            MethodInvoker inv = delegate
            {                
                this.dataGridView1.Rows.Add(date, url, status);
                this.dataGridView1.AutoResizeColumns();
            };

            this.Invoke(inv);
        }
        
        public void _modTopGridLbl(string url, string status)
        {
            MethodInvoker inv = delegate
            {                
                foreach (DataGridViewRow item in dataGridView1.Rows )
                {
                    if (item.Cells["url"].Value == url)
                        item.Cells["status"].Value = status;
                }
            };
            this.Invoke(inv);
        }
        
        public void GetLinks(string dateUrl)
        {
            try
            {

                HtmlWeb web = new HtmlWeb();
                HtmlAgilityPack.HtmlDocument doc = web.Load(dateUrl);

                string linx = "";
                List<string> linkList = new List<string>();

                var div = doc.DocumentNode.SelectSingleNode("//*[contains(@class,'stories')]");  //"//div[@class='stories']"
                HtmlNodeCollection storyLinks;
                if (div != null)
                {
                    var links = div.Descendants("a")
                                   .Select(a => a.Attributes["href"].Value)  //.InnerText)
                                   .ToList();

                    linkList = div.Descendants("a")
                                   .Select(a => a.Attributes["href"].Value)  //.InnerText)
                                   .ToList();

                    linx = string.Join(" ,  ", links.ToArray());
                }
                

                //FILL DICT
                foreach (string l in linkList)
                {
                    if (l.Substring(0, 6) != "/news/")
                        continue;

                    if (Convert.ToDateTime(l.Substring(6, 10)) < dtpStartDate.Value ||
                        Convert.ToDateTime(l.Substring(6, 10)) > dtpEndDate.Value)
                        continue;

                    if (!dicLinks.ContainsKey(l))
                    {
                        linkDets ld = new linkDets();
                        ld.source = "Bloomberg";
                        ld.date = l.Substring(6, 10);
                        ld.title = l.Substring(17, l.Length - 17); 
                        ld.url = "http://www.bloomberg.com" + l;
                        ld.done = false;
                        dicLinks.Add(l, ld);
                    }
                }
            }
            catch (Exception e)
            {
                using (StreamWriter writer = new StreamWriter("errors-exceptions-Log.txt", true))
                {
                    writer.WriteLine(DateTime.Now.ToString("MM/dd/yy | hh:mm:ss :: ") + "Historical.GetLinks() : ");
                }
            }
        }

        public static void GetLinks_ps(string dateUrl, DateTime startDate, DateTime endDate)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(dateUrl);

            string linx = "";
            List<string> linkList = new List<string>();

            var div = doc.DocumentNode.SelectSingleNode("//*[contains(@class,'stories')]");  
            HtmlNodeCollection storyLinks;
            if (div != null)
            {
                var links = div.Descendants("a")
                               .Select(a => a.Attributes["href"].Value) 
                               .ToList();

                linkList = div.Descendants("a")
                               .Select(a => a.Attributes["href"].Value) 
                               .ToList();


                linx = string.Join(" ,  ", links.ToArray());
            }

            foreach (string l in linkList)
            {
                if (l.Substring(0, 6) != "/news/")
                    continue;

                if (Convert.ToDateTime(l.Substring(6, 10)) < startDate ||
                    Convert.ToDateTime(l.Substring(6, 10)) > endDate)
                    continue;

                if (!dicLinks.ContainsKey(l))
                {
                    linkDets ld = new linkDets();
                    ld.source = "Bloomberg";
                    ld.date = l.Substring(6, 10);
                    ld.title = l.Substring(17, l.Length - 17);  
                    ld.url = "http://www.bloomberg.com" + l;
                    dicLinks.Add(ld.url, ld);
                }
            }
        }

        public void FillTopGrid()
        {
        }

        public void FillBottomGrid()
        {
          
        }

        public void ProcessLinks()
        {
            int ct = 1;
            foreach (var k in dicLinks.Values)
            {
                if (inTesting && ct > 6)
                    break;

                if (k.done)
                    continue;

                _LinkLbl("link " + ct++ + " / " + dicLinks.Count);
                _DateLbl(k.date);
                _addTopGridLbl(k.date, k.url, "starting");

                SentimentToDB.ProcessUrl(k.url, Convert.ToDateTime(k.date), k.source, k.title);

                k.done = true;               
            }
            
        }

        public static void ProcessLinks_ps() 
        {
            int ct = 1;
            foreach (var k in dicLinks.Values)
            {
                if (inTesting && ct > 6)
                    break;

                SentimentToDB.ProcessUrl(k.url, Convert.ToDateTime(k.date), k.source, k.title);
            }

            RunItLive.doneLHS = true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
           
            if (runHistorical) 
            {
                runHistorical = false;
                btnStart.Text = "continue";
            }
            else  
            {
                runHistorical = true;
                btnStart.Text = "pause";
                InitWorker(false);
            }
        }

        private void btnLive_Click(object sender, EventArgs e)
        {
            btnLive.BackColor = Color.Green;
            InitWorker(true);
        }


        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            MessageBox.Show("pop!");
        }
    }
}
