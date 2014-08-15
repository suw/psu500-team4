using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using MySql;
using MySql.Data.MySqlClient;

namespace nlp_test1
{
    public partial class PriceLoader : Form
    {
        public static string connectMySQL = "Server=host320.hostmonster.com;Database=fivninni_sweng500;Uid=fivninni_apimax;Pwd=make1tw0rk;Port=3306";

        public static DataTable priceDT = new DataTable();
        public static int insertedToDB = 0;

        public static string _symbol;

        public static void addCols()
        {
            if (priceDT.Columns.Count > 1)
                return;

            priceDT.Columns.Add("Date");
            priceDT.Columns.Add("Open");
            priceDT.Columns.Add("High");
            priceDT.Columns.Add("Low");
            priceDT.Columns.Add("Close");
            priceDT.Columns.Add("Volume");
            priceDT.Columns.Add("AdjClose");
        }

        public PriceLoader()
        {
            InitializeComponent();
        }

        public void updateGrid()
        {
            int from_day = dateTimePicker1.Value.Day;
            int from_mo;
            if (dateTimePicker1.Value.Month == 1)
            {
                from_mo = 0;
            }
            else
                from_mo = (dateTimePicker1.Value.AddMonths(-1)).Month;

            int from_yr = dateTimePicker1.Value.Year;
            int to_day = dateTimePicker2.Value.Day;
            int to_mo;
            if (dateTimePicker2.Value.Month == 1)
                to_mo = 0;
            else
                to_mo = (dateTimePicker2.Value.AddMonths(-1)).Month;
            int to_yr = dateTimePicker2.Value.Year;

            string url = @"http://ichart.finance.yahoo.com/table.csv?s="+_symbol+"%20&a="+from_mo+"&b="+from_day+"&c="+from_yr+"&g="+to_mo+"&e="+to_day+"&f="+to_yr;


            using (StreamWriter writer = new StreamWriter("urls.txt", true))
            {
                writer.WriteLine(DateTime.Now.ToString("dd/MM/yy | hh:mm:ss :: ") + url );
            }
                        
            string prices = Extract(url);
                                                                                       
            try
            {
                using (StringReader reader = new StringReader(prices))
                {
                    int lines = 0;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (lines++ > 0)
                        {
                            string[] fieldData = line.Split(',');      
                            for (int i = 0; i < fieldData.Length; i++)
                            {
                                if (fieldData[i] == "")                                
                                    fieldData[i] = null;                                
                            }
                            priceDT.Rows.Add(fieldData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            dgvPrices.DataSource = priceDT;
            dgvPrices.Update();
            string symb = "xxxxx";
            backgroundWorker1.RunWorkerAsync(symb);
        }

        public void writePricesToDB(string symbol)
        {
            insertedToDB = 1;
            lock (priceDT)
            {                
                using (MySqlConnection cn1 = new MySqlConnection(connectMySQL))
                {
                    MySqlCommand cmd;

                    foreach (DataRow row in priceDT.Rows)        
                    {
                        if(cn1.State != ConnectionState.Open)
                            cn1.Open();

                        DateTime date = Convert.ToDateTime(row[0].ToString());

                        string qry = "SELECT COUNT(close) FROM dailyPrices WHERE date = '" + date.ToString("yyyy-MM-dd") + "' AND symbol = '" + _symbol + "'";
                        cmd = new MySqlCommand(qry, cn1);
                        int outCount = Convert.ToInt32(cmd.ExecuteScalar());

                        _StatusLbl("inserting: " + insertedToDB++ + "/" + priceDT.Rows.Count);

                        if (outCount > 0)
                            continue;

                        decimal open = Convert.ToDecimal(row[1].ToString());
                        decimal high = Convert.ToDecimal(row[2].ToString());
                        decimal low = Convert.ToDecimal(row[3].ToString());
                        decimal close = Convert.ToDecimal(row[4].ToString());
                        int volume = Convert.ToInt32(row[5].ToString());
                        decimal adjClose = Convert.ToDecimal(row[6].ToString());
                        string query = @"Insert INTO dailyPrices (symbol, date, open, high, low, close, volume, adjClose, source)  values ('" + _symbol + "','" + date.ToString("yyyy-MM-dd") + "','" + open + "','" + high + "','" + low + "','" + close + "','" + volume + "','" + adjClose + "','yahoo')";
                        cmd = new MySqlCommand(query, cn1);
                        cmd.ExecuteNonQuery();
                        cn1.Close();                        
                    }
                }
            }
            //return inserted;
        }


        public void _StatusLbl(string lblText)
        {
            MethodInvoker inv = delegate
            {
                this.lblStatus.Text = lblText;            
            };
            this.Invoke(inv);
        }

        public static string Extract(string yahooHttpRequestString)
        {
            System.Net.WebClient webClient = new WebClient();
            webClient.Proxy = HttpWebRequest.GetSystemWebProxy();
            webClient.Proxy.Credentials = CredentialCache.DefaultCredentials;
            Stream strm = webClient.OpenRead(yahooHttpRequestString);
            StreamReader sr = new StreamReader(strm);
            string result = sr.ReadToEnd();
            strm.Close();
            return result;
        } 

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (txtSymbol.Text == "")
            {
                MessageBox.Show("please enter symbol...");
                return;
            }

            lblStatus.Text = "";
            dgvPrices.DataSource = null;
            dgvPrices.Refresh();

            if (priceDT != null)
                priceDT.Clear();

            addCols();
            _symbol = txtSymbol.Text;
            updateGrid();
        } 
        
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string symb = (string)e.Argument;
            writePricesToDB(symb);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Done! Inserted " + insertedToDB + " to DB");
        }

    }
}
