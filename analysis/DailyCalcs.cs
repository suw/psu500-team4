using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql;
using MySql.Data.MySqlClient;

namespace nlp_test1
{
    public partial class DailyCalcs : Form
    {
        public static int daysToProcess;

        public static int cn1connectionsOpen = 0;

        public DailyCalcs()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {            
            DateTime[] dates = { dateTimePicker1.Value.Date, dateTimePicker2.Value.Date };
            backgroundWorker1.RunWorkerAsync(dates);             
        }

        public void calculate(DateTime startDate, DateTime endDate)
        {
            daysToProcess = Convert.ToInt32((endDate.Date - startDate.Date).Days);
                       
                for (int x = 0; x <= daysToProcess; x++)   //// LIMIT THREADS - for sql connections...
                {
                    Thread LHS = new Thread(() => calcDay(startDate.AddDays(x)));
                    LHS.Start();
                    //calcDay(startDate.AddDays(x));
                    string status = "running day: " + startDate.AddDays(x).ToShortDateString() + "   (" + x.ToString() + "/" + daysToProcess.ToString() + ")";
                    _StatusLbl(status); 
                }
        }

        public static void calculate_ps(DateTime startDate, DateTime endDate)
        {
            if (startDate.Date != endDate.Date)
            {
                daysToProcess = Convert.ToInt32((endDate.Date - startDate.Date).Days);


                int x = 0;
                Parallel.For(0, daysToProcess, new ParallelOptions { MaxDegreeOfParallelism = 5 },
                  i =>
                  {
                      calcDay(startDate.Date.AddDays(x));
                      x++;
                  });                                
            }
            else
            {
                Thread LHS = new Thread(() => calcDay(endDate.Date));
                LHS.Start();
            }
        } 

        public static void calcDay(DateTime date)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("errors-connections-Log.txt", true))
                {
                    writer.WriteLine(DateTime.Now.ToString("MM/dd/yy | hh:mm:ss :: ") + "called...");
                }
            }
            catch { }


            while (cn1connectionsOpen > 5)
            {
                Thread.Sleep(1000);
                using (StreamWriter writer = new StreamWriter("errors-connections-Log.txt", true))
                {
                    writer.WriteLine(DateTime.Now.ToString("MM/dd/yy | hh:mm:ss :: ") + "too many open connections (> 5) ...sleeping...");
                }
            }


            using (MySqlConnection cn1 = new MySqlConnection(RunItLive.connectMySQL))
            {
                MySqlCommand cmd;
                if(cn1.State != ConnectionState.Open)
                    cn1.Open();

                cn1connectionsOpen++;

                string qry = "SELECT COUNT(symbol) FROM dailyCalcs WHERE date = '" + date.ToString("yyyy-MM-dd") +  "'";
                cmd = new MySqlCommand(qry, cn1);
                int outCount = Convert.ToInt32(cmd.ExecuteScalar());
                               
                string query;

                if (outCount == 0)
                {
                    query = @"SET OPTION SQL_BIG_SELECTS = 1;
                    INSERT INTO dailyCalcs (symbol, date, avgS, avgSC, avgSR, avgSRC, adjclose)
                    SELECT 
                    s.symbol, 
                    a.artdate, 
                    AVG(a.score) as avgScore, 
                    AVG(a.score*a.count) as avgSC,
                    AVG(a.score*a.relevance) as avgSR,
                    AVG(a.score*a.relevance*a.count) as avgSRC,
                    p.adjclose
                    FROM `api_results` a
                    join `symbols` s ON s.name = a.text
                    join `dailyPrices` p ON p.symbol = s.symbol AND p.date = a.artdate
                    WHERE a.artdate = '" + date.ToString("yyyy-MM-dd") + @"'
                    AND (a.Stype = 'Organization' OR a.Stype = 'Company')
                    group by a.artdate, s.symbol";
                }
                else
                {
                    query = @"

                    DELETE FROM dailyCalcs WHERE date = '" + date.ToString("yyyy-MM-dd") +  @"' ;

                    SET OPTION SQL_BIG_SELECTS = 1;
                    INSERT INTO dailyCalcs (symbol, date, avgS, avgSC, avgSR, avgSRC, adjclose)
                    SELECT 
                    s.symbol, 
                    a.artdate, 
                    AVG(a.score) as avgScore, 
                    AVG(a.score*a.count) as avgSC,
                    AVG(a.score*a.relevance) as avgSR,
                    AVG(a.score*a.relevance*a.count) as avgSRC,
                    p.adjclose
                    FROM `api_results` a
                    join `symbols` s ON s.name = a.text
                    join `dailyPrices` p ON p.symbol = s.symbol AND p.date = a.artdate
                    WHERE a.artdate = '" + date.ToString("yyyy-MM-dd") + @"'
                    AND (a.Stype = 'Organization' OR a.Stype = 'Company')
                    group by a.artdate, s.symbol";
                }


                cmd = new MySqlCommand(query, cn1);
                var cmdOut = cmd.ExecuteNonQuery();
                
                cn1.Close();
                Thread.Sleep(200);
                cn1connectionsOpen--;
            }
        }

        public void _StatusLbl(string lblText)
        {
            MethodInvoker inv = delegate
            {
                this.lblStatus.Text = lblText;              //this.index.ToString(); 
            };

            this.Invoke(inv);
        }

        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            DateTime[] dates = (DateTime[])e.Argument;
            calculate(dates[0], dates[1]);
        }
    }
}
