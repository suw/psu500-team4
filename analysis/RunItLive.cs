using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using nlp_test1.Properties;
using System.Configuration;


namespace nlp_test1
{
    public partial class RunItLive : Form
    {
        public static string connectMySQL = "Server=host320.hostmonster.com;Database=fivninni_sweng500;Uid=fivninni_apimax;Pwd=make1tw0rk;Port=3306";

        public static bool doneLHS = false;

        public static string[] symArray = new string[Settings.Default.Symbols.Count];
        public static string symbs;

        public static int roundPrx = 2;
        public static int roundData = 3;

        public static bool firstRun = true;
        public static bool _formClosed = true;

        private static System.Timers.Timer rTimer;

        public RunItLive()
        {
            InitializeComponent();
            GetSymbols();
            _buildGridTbl();
            _formClosed = false; ;
        }

        public static void GetSymbols()
        {
            Settings.Default.Symbols.CopyTo(symArray, 0);
            string symbs = string.Join("','", symArray);
        }
                
        private void RunItLive_Load(object sender, EventArgs e)
        {    
            UpdateFirst();
            rTimer = new System.Timers.Timer(20000);
            rTimer.Elapsed += new ElapsedEventHandler(UpdateRTime);
            rTimer.Enabled = true;
        }

        public void RunItLive_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        public void UpdateFirst()
        {
            int Cdays = Properties.Settings.Default.CorrelationDays;
            int Clookback = Properties.Settings.Default.CorrelationLookBack;
            DateTime ClookbackDate = DateTime.Now.AddDays(Clookback * -3);

            toolStripStatusLabel1.Text = "start loading prices...";
            Thread LHP = new Thread(() => LoadHistPrices(ClookbackDate));
            LHP.Start();

            toolStripStatusLabel1.Text = "start loading daily calc table...";
            Thread LHS = new Thread(() => LoadDailyCalcs()); // ClookbackDate));
            LHS.Start();
        }

        public void UpdateRTime(object source, ElapsedEventArgs e)
        {
            _toolStripLabel1("start loading daily calc table...");
            Thread LHS = new Thread(() => LoadDailyCalcs()); // ClookbackDate));
            LHS.Start();

            Thread GLP = new Thread(Historical.GetLivePrices);
            GLP.Start();
        }
                
        public void LoadDailyCalcs()
        {
            lock (MyDataSet.tblDailyCalcs)
            {
                MyDataSet.tblDailyCalcs.Clear();

                using (MySqlConnection cn1 = new MySqlConnection(connectMySQL))
                {
                    MySqlCommand cmd;

                    if (cn1.State != ConnectionState.Open)
                        cn1.Open();

                    int daysBack = Settings.Default.CorrelationLookBack * -1;
                    var sqlDate = DateTime.Now.AddDays(daysBack).ToString("yyyy-MM-dd");
                                        
                    string[] strArray = new string[Settings.Default.Symbols.Count];
                    Settings.Default.Symbols.CopyTo(strArray, 0);
                    string symbs = string.Join("','", strArray);

                    string query = @"SELECT date, symbol,avgS,avgSC,avgSR,avgSRC,adjclose, var1
                                        FROM `dailyCalcs` 
                                        WHERE date >= "+ sqlDate +
                                        " and symbol IN ('" + symbs + "')";
                    cmd = new MySqlCommand(query, cn1);
                    cmd.ExecuteNonQuery();

                    var _da = new MySqlDataAdapter(cmd);
                    _da.Fill(MyDataSet.tblDailyCalcs);

                    var _cb = new MySqlCommandBuilder(_da);

                    string lastUpQr = @"SELECT MAX(timestamp)
                                        FROM `dailyCalcs` 
                                        WHERE date >= " + sqlDate +
                                        " and symbol IN ('" + symbs + "')";

                    cmd = new MySqlCommand(lastUpQr, cn1);
                    object result = cmd.ExecuteScalar();
                    
                    if (result != null)
                        _toolStripLabel1(result.ToString());

                    cn1.Close();

                    _toolStripStatusLbl("got records from db: "+ MyDataSet.tblDailyCalcs.Rows.Count);                    
                }
            }
            
            RunDailyCalcs();

            CombineData();

            _fillDGV1(4);
        }
        
        void _buildGridTbl()
        {
            if (MyDataSet.tblGridData.Columns.Count > 2)
                return;

            MyDataSet.tblGridData.Columns.Add("index", typeof(string));
            MyDataSet.tblGridData.Columns.Add("Symbol", typeof(string));
            MyDataSet.tblGridData.Columns.Add("ClosePrx", typeof(decimal));
            MyDataSet.tblGridData.Columns.Add("ForecPrx", typeof(decimal));
            MyDataSet.tblGridData.Columns.Add("LastPrx", typeof(decimal));
            MyDataSet.tblGridData.Columns.Add(" ", typeof(string));
            MyDataSet.tblGridData.Columns.Add("avgS", typeof(decimal));
            MyDataSet.tblGridData.Columns.Add("avgSC", typeof(decimal));
            MyDataSet.tblGridData.Columns.Add("avgSR", typeof(decimal));
            MyDataSet.tblGridData.Columns.Add("avgSRC", typeof(decimal));
            MyDataSet.tblGridData.Columns.Add("pctCount", typeof(int));
            MyDataSet.tblGridData.Columns.Add("pctS", typeof(decimal));
            MyDataSet.tblGridData.Columns.Add("pctSC", typeof(decimal));
            MyDataSet.tblGridData.Columns.Add("pctSR", typeof(decimal));
            MyDataSet.tblGridData.Columns.Add("pctSRC", typeof(decimal));
        }

        public void CombineData()
        {
            int i = 1;

            foreach (var r in MyDataSet.SymbList)
            {               
                    DataRow dr = MyDataSet.tblGridData.NewRow();

                    int rN = -1;

                    if (!firstRun)
                    {                        
                        for (int rx = 0; rx <= MyDataSet.tblGridData.Rows.Count; rx++)
                        {
                            if (MyDataSet.tblGridData.Rows[rx][1].ToString() == r.Key)
                            {
                                rN = rx;
                                break;
                            }
                        }
                    }                   

                    if (rN >= 0)           
                    {
                        MyDataSet.tblGridData.Rows[rN]["index"] = 0;   //i++;

                        foreach (DataRow DCR in MyDataSet.tblDailyCalcs.Rows)
                        {
                            if (DCR[1].ToString() == r.Key.ToString())
                            {
                                MyDataSet.tblGridData.Rows[rN]["ClosePrx"] = Math.Round(Convert.ToDecimal(DCR["adjclose"]), roundPrx);
                                MyDataSet.tblGridData.Rows[rN]["ForecPrx"] = Math.Round(Convert.ToDecimal(DCR["adjclose"]) * (1 + (Convert.ToDecimal(DCR["avgSRC"]) /30 )), roundPrx);
                                MyDataSet.tblGridData.Rows[rN]["LastPrx"] = Math.Round(Convert.ToDecimal(DCR["adjclose"]), roundPrx);
                                MyDataSet.tblGridData.Rows[rN]["avgS"] = Math.Round(Convert.ToDecimal(DCR["avgS"]), roundData);
                                MyDataSet.tblGridData.Rows[rN]["avgSC"] = Math.Round(Convert.ToDecimal(DCR["avgSC"]), roundData);
                                MyDataSet.tblGridData.Rows[rN]["avgSR"] = Math.Round(Convert.ToDecimal(DCR["avgSR"]), roundData);
                                MyDataSet.tblGridData.Rows[rN]["avgSRC"] = Math.Round(Convert.ToDecimal(DCR["avgSRC"]), roundData);
                            }
                        }

                        MyDataSet.tblGridData.Rows[rN]["Symbol"] = r.Key;
                        MyDataSet.tblGridData.Rows[rN][" "] = " ";
                        MyDataSet.tblGridData.Rows[rN]["pctS"] = Math.Round(r.Value.pctAcc_aS, roundData);
                        MyDataSet.tblGridData.Rows[rN]["pctSC"] = Math.Round(r.Value.pctAcc_aSC, roundData);
                        MyDataSet.tblGridData.Rows[rN]["pctSR"] = Math.Round(r.Value.pctAcc_aSR, roundData);
                        MyDataSet.tblGridData.Rows[rN]["pctSRC"] = Math.Round(r.Value.pctAcc_aSRC, roundData);
                        MyDataSet.tblGridData.Rows[rN]["pctCount"] = r.Value.count;

                        WriteUpdateToGuiDB(r.Key.ToString(), 
                                           Convert.ToDecimal(MyDataSet.tblGridData.Rows[rN]["ClosePrx"]), 
                                           Convert.ToDecimal(MyDataSet.tblGridData.Rows[rN]["ForecPrx"]),
                                           Convert.ToDouble(MyDataSet.tblGridData.Rows[rN]["avgS"]));
                    }
                    else
                    {
                        dr["index"] = i++;

                        foreach (DataRow DCR in MyDataSet.tblDailyCalcs.Rows)
                        {
                            if (DCR[1].ToString() == r.Key.ToString())
                            {
                                dr["ClosePrx"] = Math.Round(Convert.ToDecimal(DCR["adjclose"]), roundPrx);
                                dr["ForecPrx"] = Math.Round(Convert.ToDecimal(DCR["adjclose"]) * (1 + (Convert.ToDecimal(DCR["avgSRC"])) /30), roundPrx);
                                dr["LastPrx"] = Math.Round(Convert.ToDecimal(DCR["adjclose"]), roundPrx);
                                dr["avgS"] = Math.Round(Convert.ToDecimal(DCR["avgS"]), roundData);
                                dr["avgSC"] = Math.Round(Convert.ToDecimal(DCR["avgSC"]), roundData);
                                dr["avgSR"] = Math.Round(Convert.ToDecimal(DCR["avgSR"]), roundData);
                                dr["avgSRC"] = Math.Round(Convert.ToDecimal(DCR["avgSRC"]), roundData);
                            }
                        }

                        dr["Symbol"] = r.Key;
                        dr[" "] = " ";
                        dr["pctS"] = Math.Round(r.Value.pctAcc_aS, roundData);
                        dr["pctSC"] = Math.Round(r.Value.pctAcc_aSC, roundData);
                        dr["pctSR"] = Math.Round(r.Value.pctAcc_aSR, roundData);
                        dr["pctSRC"] = Math.Round(r.Value.pctAcc_aSRC, roundData);
                        dr["pctCount"] = r.Value.count;
                                                
                        MyDataSet.tblGridData.Rows.Add(dr);

                       
                            WriteUpdateToGuiDB(dr["Symbol"].ToString(),
                                               Convert.ToDecimal(dr["ClosePrx"]),
                                               Convert.ToDecimal(dr["ForecPrx"]),
                                               Convert.ToDouble(dr["avgS"]));
                       
                    }               
            }

            _fillDGV1(4);
        }


        public void WriteUpdateToGuiDB(string symbol, decimal closePrx, decimal forePrx, double avgS)
        {
            using (MySqlConnection cn1 = new MySqlConnection(PriceLoader.connectMySQL))
            {
                MySqlCommand cmd;

                if (cn1.State != ConnectionState.Open)
                    cn1.Open();

                DateTime _date = DateTime.Now.Date;

                string qry1 = "SELECT COUNT(symbol) FROM data_gui_graph WHERE date = '" + _date.ToString("yyyy-MM-dd") + "' AND symbol = '" + symbol + "' AND forecast = '" + forePrx + "' AND F3 = '" + avgS + "'";
                cmd = new MySqlCommand(qry1, cn1);
                int outCountDone = Convert.ToInt32(cmd.ExecuteScalar());

                string qry = "SELECT COUNT(symbol) FROM data_gui_graph WHERE date = '" + _date.ToString("yyyy-MM-dd") + "' AND symbol = '" + symbol + "'";
                cmd = new MySqlCommand(qry, cn1);
                int outCount = Convert.ToInt32(cmd.ExecuteScalar());
                
                if (outCount == 0)
                {
                    //insert
                    qry = @"INSERT INTO data_gui_graph (symbol, date, price, forecast,F1, F2, F3) VALUES ('" + symbol + "','" + _date.ToString("yyyy-MM-dd") + "'," + closePrx + "," + forePrx + "," + forePrx + "," + closePrx + "," + avgS + ");";
                    cmd = new MySqlCommand(qry, cn1);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    //modify
                    qry = @"UPDATE data_gui_graph SET forecast = " + forePrx + " , F1 = " + forePrx + " , F3 = " + avgS + "  WHERE date = '" + _date.ToString("yyyy-MM-dd") + "' AND symbol = '" + symbol + "'";
                    cmd = new MySqlCommand(qry, cn1);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void RunDailyCalcs()
        {
            _toolStripStatusLbl("staring pct acc calcs...(" + MyDataSet.tblDailyCalcs.Rows.Count + ")rows");

            MyDataSet.SymbList.Clear();

            lock (MyDataSet.tblDailyCalcs)
            {

                foreach (DataRow sRow in MyDataSet.tblDailyCalcs.Rows)
                {
                    if (MyDataSet.SymbList.ContainsKey(sRow["symbol"].ToString()))
                    {
                        MyDataSet.Calcs _tsym = new MyDataSet.Calcs();
                        _tsym = MyDataSet.SymbList[sRow["symbol"].ToString()];
                        decimal prevPrice = 0;
                        int xd = 0;
                        while (prevPrice == 0 && xd < 10)
                        {
                            var results = from DataRow pRow in MyDataSet.dsHPrices.Tables[0].Rows
                                          where ((string)pRow["symbol"] == sRow["symbol"].ToString() && Convert.ToDateTime(pRow["date"]).AddDays(1) == Convert.ToDateTime(sRow["date"]))
                                          select (decimal)pRow["adjclose"]
                                          ;

                          
                            results.DefaultIfEmpty(0); 

                            if (results.FirstOrDefault() > 0)
                                prevPrice = results.FirstOrDefault();


                            xd++;
                            LogRunItLive(Convert.ToDateTime(sRow["date"]).AddDays(xd++ * -1).ToString("MM/dd/yy: ") + "price:1: " + sRow["symbol"].ToString() + "  = " + prevPrice);
                        }

                        if (prevPrice == 0)
                        {                           
                            continue;
                        }

                        decimal priceChg = (decimal)sRow["adjclose"] - prevPrice;

                        if (priceChg > 0)
                        {
                            if ((decimal)sRow["avgS"] > 0)
                                _tsym.pctAcc_aS = ((_tsym.pctAcc_aS * _tsym.count) + 1) / (_tsym.count + 1);
                            else
                                _tsym.pctAcc_aS = ((_tsym.pctAcc_aS * _tsym.count) + 0) / (_tsym.count + 1);

                            if ((decimal)sRow["avgSC"] > 0)
                                _tsym.pctAcc_aSC = ((_tsym.pctAcc_aSC * _tsym.count) + 1) / (_tsym.count + 1);
                            else
                                _tsym.pctAcc_aSC = ((_tsym.pctAcc_aSC * _tsym.count) + 0) / (_tsym.count + 1);

                            if ((decimal)sRow["avgSR"] > 0)
                                _tsym.pctAcc_aSR = ((_tsym.pctAcc_aSR * _tsym.count) + 1) / (_tsym.count + 1);
                            else
                                _tsym.pctAcc_aSR = ((_tsym.pctAcc_aSR * _tsym.count) + 0) / (_tsym.count + 1);

                            if ((decimal)sRow["avgSRC"] > 0)
                                _tsym.pctAcc_aSRC = ((_tsym.pctAcc_aSRC * _tsym.count) + 1) / (_tsym.count + 1);
                            else
                                _tsym.pctAcc_aSRC = ((_tsym.pctAcc_aSRC * _tsym.count) + 0) / (_tsym.count + 1);

                            _tsym.count++;

                            MyDataSet.SymbList[sRow["symbol"].ToString()] = _tsym;
                        }
                        else if (priceChg < 0)
                        {
                            if ((decimal)sRow["avgS"] > 0)
                                _tsym.pctAcc_aS = ((_tsym.pctAcc_aS * _tsym.count) + 0) / (_tsym.count + 1);
                            else
                                _tsym.pctAcc_aS = ((_tsym.pctAcc_aS * _tsym.count) + 1) / (_tsym.count + 1);

                            if ((decimal)sRow["avgSC"] > 0)
                                _tsym.pctAcc_aSC = ((_tsym.pctAcc_aSC * _tsym.count) + 0) / (_tsym.count + 1);
                            else
                                _tsym.pctAcc_aSC = ((_tsym.pctAcc_aSC * _tsym.count) + 1) / (_tsym.count + 1);

                            if ((decimal)sRow["avgSR"] > 0)
                                _tsym.pctAcc_aSR = ((_tsym.pctAcc_aSR * _tsym.count) + 0) / (_tsym.count + 1);
                            else
                                _tsym.pctAcc_aSR = ((_tsym.pctAcc_aSR * _tsym.count) + 1) / (_tsym.count + 1);

                            if ((decimal)sRow["avgSRC"] > 0)
                                _tsym.pctAcc_aSRC = ((_tsym.pctAcc_aSRC * _tsym.count) + 0) / (_tsym.count + 1);
                            else
                                _tsym.pctAcc_aSRC = ((_tsym.pctAcc_aSRC * _tsym.count) + 1) / (_tsym.count + 1);

                            _tsym.count++;
                            MyDataSet.SymbList[sRow["symbol"].ToString()] = _tsym;
                        }
                    }
                    else
                    {
                        MyDataSet.Calcs _tsym = new MyDataSet.Calcs();
                        decimal prevPrice = 0;
                        int xd = 0;
                        while (prevPrice == 0 && xd < 10)
                        {
                            var results = from DataRow pRow in MyDataSet.dsHPrices.Tables[0].Rows
                                          where ((string)pRow["symbol"] == sRow["symbol"].ToString() && Convert.ToDateTime(pRow["date"]).AddDays(1) == Convert.ToDateTime(sRow["date"]))   //.AddDays(xd++ * -1) !!!!!!!!!
                                          select (decimal)pRow["adjclose"];

                            results.DefaultIfEmpty(0); //.Count() > 0)

                            if (results.FirstOrDefault() > 0)
                                prevPrice = results.FirstOrDefault();

                            xd++;
                            LogRunItLive(Convert.ToDateTime(sRow["date"]).AddDays(xd++ * -1).ToString("MM/dd/yy: ") + "price:0: " + sRow["symbol"].ToString() + "  = " + prevPrice);
                        }

                        if (prevPrice == 0)
                        {
                            continue;
                        }

                        decimal priceChg = (decimal)sRow["adjclose"] - prevPrice;

                        if (priceChg > 0)
                        {
                            if ((decimal)sRow["avgS"] > 0)
                                _tsym.pctAcc_aS = 1;
                            else
                                _tsym.pctAcc_aS = 0;

                            if ((decimal)sRow["avgSC"] > 0)
                                _tsym.pctAcc_aSC = 1;
                            else
                                _tsym.pctAcc_aSC = 0;

                            if ((decimal)sRow["avgSR"] > 0)
                                _tsym.pctAcc_aSR = 1;
                            else
                                _tsym.pctAcc_aSR = 0;

                            if ((decimal)sRow["avgSRC"] > 0)
                                _tsym.pctAcc_aSRC = 1;
                            else
                                _tsym.pctAcc_aSRC = 0;

                            _tsym.count = 1;
                            MyDataSet.SymbList.Add(sRow["symbol"].ToString(), _tsym);
                        }
                        else if (priceChg < 0)
                        {
                            if ((decimal)sRow["avgS"] > 0)
                                _tsym.pctAcc_aS = 0;
                            else
                                _tsym.pctAcc_aS = 1;

                            if ((decimal)sRow["avgSC"] > 0)
                                _tsym.pctAcc_aSC = 0;
                            else
                                _tsym.pctAcc_aSC = 1;

                            if ((decimal)sRow["avgSR"] > 0)
                                _tsym.pctAcc_aSR = 0;
                            else
                                _tsym.pctAcc_aSR = 1;

                            if ((decimal)sRow["avgSRC"] > 0)
                                _tsym.pctAcc_aSRC = 0;
                            else
                                _tsym.pctAcc_aSRC = 1;

                            _tsym.count = 1;
                            MyDataSet.SymbList.Add(sRow["symbol"].ToString(), _tsym);
                        }
                        else
                            LogRunItLive("Zero price change on: " + sRow["symbol"].ToString() + "  " + sRow["date"].ToString());
                    }
                }
            }           
        }
        
        public void WaitForLoading()
        {
            DateTime startTime = DateTime.Now;
            while (!doneLHS) 
            {
                if (DateTime.Now > startTime.AddMinutes(20))
                {
                    toolStripStatusLabel1.Text = "wait for loading timed out...20 mins";
                    break;
                }
                toolStripStatusLabel1.Text = "waiting for loading to finish...";
                Thread.Sleep(500);
            }
        }
        
        public void RunUpdate(int min)
        {
           
        }

        public void _toolStripStatusLbl(string lblStatus)
        {
            MethodInvoker inv = delegate
            {
                this.toolStripStatusLabel1.Text = lblStatus;             
            };

            this.Invoke(inv);
        }

        public void _toolStripLabel1(string lbl)
        {
            MethodInvoker inv = delegate
            {
                this.label1.Text = lbl;             
            };

            if (!_formClosed)
                this.Invoke(inv);
        }

        public void _dgv1refresh()
        {
            MethodInvoker inv = delegate
            {
                this.dataGridView1.Refresh();
            };

            if (!_formClosed)
                this.Invoke(inv);
        }

        public void _fillDGV1(int sourceInt)
        {
            if (dataGridView1.DataSource != null && dataGridView1.DataBindings != null && MyDataSet.tblGridData.Rows.Count > 0)
            {
                _dgv1refresh();
                return;
            }
           

            MethodInvoker inv = delegate
            {               
                toolStripStatusLabel1.Text = "start filling dgv...";

                if (sourceInt == 0)
                    dataGridView1.DataSource = MyDataSet.tblDailyCalcs;
                else if (sourceInt == 1)
                    dataGridView1.DataSource = MyDataSet.dsHPrices.Tables[0];
                else if (sourceInt == 2)
                    dataGridView1.DataSource = MyDataSet.dsHSent.Tables[0];
                else if (sourceInt == 3)
                {
                    var q = from s in MyDataSet.SymbList 
                                                select new
                                                {
                                                    s.Key,
                                                    s.Value.pctAcc_aS,
                                                    s.Value.pctAcc_aSC,
                                                    s.Value.pctAcc_aSR,
                                                    s.Value.pctAcc_aSRC,
                                                    s.Value.count,
                                                    s.Value.correl
                                                };
                    dataGridView1.DataSource = q.ToList();
                }
                else if (sourceInt == 4)
                {
                    try
                    {
                        Monitor.TryEnter(MyDataSet.tblGridData);
                    }
                    catch
                    {
                        MessageBox.Show("tblGridData - Locked!");
                        return;
                    }

                    var binding = new BindingSource();                    
                    dataGridView1.DataSource = binding;
                    dataGridView1.DataSource = MyDataSet.tblGridData;
                    
                }
                else if (sourceInt == 5)
                    dataGridView1.DataSource = SentimentToDB.LinkList;
                
                dataGridView1.AutoResizeColumns();
                dataGridView1.Refresh();
                toolStripStatusLabel1.Text = "filled dgv";
                firstRun = false;

            };
            this.Invoke(inv);
        }

        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
           
        }

        public int GetDaysBackNeededForHistSentiment()
        {
            int daysBack = -1;

            using (MySqlConnection cn1 = new MySqlConnection(connectMySQL))
            {
                MySqlCommand cmd;
                int hsCount = 0;                
                while (hsCount < 50)
                {
                    if (cn1.State != ConnectionState.Open)
                        cn1.Open();

                    DateTime date = DateTime.Now.Date.AddDays(daysBack++ * -1);
                    string qry = "SELECT COUNT(url) from api_results WHERE artdate = '" + date.ToString("yyyy-MM-dd") + "'";
                    cmd = new MySqlCommand(qry, cn1);
                    hsCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cn1.Close();
                }
            }
            return daysBack;
        }
                
        public void LoadHistPrices(DateTime startDate)
        {
            lock (MyDataSet.dsHPrices)
            {
                using (MySqlConnection cn1 = new MySqlConnection(connectMySQL))
                {
                    MySqlCommand cmd;
                    if (cn1.State != ConnectionState.Open)
                        cn1.Open();

                    string[] strArray = new string[Settings.Default.Symbols.Count];
                    Settings.Default.Symbols.CopyTo(strArray, 0);
                    string symbs = string.Join("','", strArray);

                    string qry = "SELECT symbol, adjclose, date FROM dailyPrices WHERE date > '" + startDate.ToString("yyyy-MM-dd") + "' AND symbol IN ('" + symbs + "')";
                    cmd = new MySqlCommand(qry, cn1);

                    cmd.ExecuteNonQuery();

                    var _da = new MySqlDataAdapter(cmd);

                    _da.Fill(MyDataSet.dsHPrices);

                    var _cb = new MySqlCommandBuilder(_da);

                    cn1.Close();

                    Thread.Sleep(200);
                }
            }
        }

        public void LoadHistSentiment() 
        {
            int daysBack = GetDaysBackNeededForHistSentiment();

            for (int d = 0; d <= daysBack; d++)
            {                
                _toolStripStatusLbl("starting day: " + d);
                Thread WL = new Thread(() => Historical.StartHistorical_ps(DateTime.Now.AddDays(daysBack * -1), DateTime.Now)); // ClookbackDate));
                WL.Start();
            }
            _toolStripStatusLbl("All days HS started...");
        }

        public static void LogRunItLive(string msg)
        {
            using (StreamWriter writer = new StreamWriter("errors-exceptions-RIL-Log.txt", true))
            {
                writer.WriteLine(DateTime.Now.ToString("dd/MM/yy | hh:mm:ss :: ") + msg);
            }
        }

        private void eXITToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            this.Close();
        }

        private void RIN_FormClosing(object sender, EventArgs e)
        {
            rTimer.Stop();
            this.Dispose();
            _formClosed = true;
        }
    }

    public sealed class MyDataSet
    {        
        public static DataTable tblDailyCalcs = new DataTable();

        public static DataTable tblGridData = new DataTable();

        public static Dictionary<string, Calcs> SymbList = new Dictionary<string,Calcs>();

        public class Calcs
        {
            public double correl;
            public double pctAcc_aS;
            public double pctAcc_aSC;
            public double pctAcc_aSR;
            public double pctAcc_aSRC;
            public double score;
            public int count;
        }
                
        public static DataSet dsHPrices = new DataSet();
        private static object _lockHPrices = new object();
        public static void UpdateRow_HPrices(string key, string data)
        {
            lock (_lockHPrices)
            {
                DataRow dr = dsHPrices.Tables[0].Rows.Find(key);
                dr.AcceptChanges();
                dr.BeginEdit();
                dr["col"] = data;
                dr.EndEdit();
            }
        }

        public static DataSet dsHSent = new DataSet();
        private static object _lockHSent = new object();
        public static void UpdateRow_HSent(string key, string data)
        {
            lock (_lockHSent)
            {
                DataRow dr = dsHSent.Tables[0].Rows.Find(key);
                dr.AcceptChanges();
                dr.BeginEdit();
                dr["col"] = data;
                dr.EndEdit();
            }
        }

    }
}
