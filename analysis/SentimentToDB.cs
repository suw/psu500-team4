using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using MySql;
using MySql.Data.MySqlClient;
using AlchemyAPI;

namespace nlp_test1
{
    public partial class SentimentToDB : Form
    {
        public static string connectMySQL = "Server=host320.hostmonster.com;Database=fivninni_sweng500;Uid=fivninni_apimax;Pwd=make1tw0rk;Port=3306";

        public static List<string> LinkList = new List<string>();

        public SentimentToDB()
        {
            InitializeComponent();
            string url1 = @"www.bloomberg.com/news/2014-06-25/u-s-stock-index-futures-are-little-changed-before-data.html";
            ProcessUrl(url1, new DateTime(2014,6,25),"Bloomberg","us stock...");
        }

        
        public static void ProcessUrl(string url, DateTime date, string source, string title)
        {
            if (LinkList.Count < 1)
                LoadProcessedLinks(date);

            if (CheckIfUrlProcessed(url))  
                return;

            try
            {
                AlchemyAPI.AlchemyAPI alchemyObj = new AlchemyAPI.AlchemyAPI();
                alchemyObj.LoadAPIKey("api_key.txt");
                AlchemyAPI.AlchemyAPI_EntityParams prms = new AlchemyAPI.AlchemyAPI_EntityParams();
                prms.setSentiment(true);
                prms.setMaxRetrieve(1000);
                prms.setOutputMode(AlchemyAPI_BaseParams.OutputMode.XML);
                string in_xml = alchemyObj.URLGetRankedNamedEntities(url, prms);
                string mid_xml = in_xml.Replace("<sentiment>", "");
                string s_xml = mid_xml.Replace("</sentiment>", "");
                string xml_01 = s_xml.Replace(@"<type>Person</type>", @"<Stype>Person</Stype>");
                string xml_02 = xml_01.Replace(@"<type>PrintMedia</type>", @"<Stype>PrintMedia</Stype>");
                string xml_03 = xml_02.Replace(@"<type>JobTitle</type>", @"<Stype>JobTitle</Stype>");
                string xml_04 = xml_03.Replace(@"<type>FieldTerminology</type>", @"<Stype>FieldTerminology</Stype>");
                string xml_05 = xml_04.Replace(@"<type>Company</type>", @"<Stype>Company</Stype>");
                string xml_06 = xml_05.Replace(@"<type>City</type>", @"<Stype>City</Stype>");
                string xml_07 = xml_06.Replace(@"<type>State</type>", @"<Stype>State</Stype>");  //   
                string xml_08 = xml_07.Replace(@"<type>Country</type>", @"<Stype>Country</Stype>");
                string xml_09 = xml_08.Replace(@"<type>Automobile</type>", @"<Stype>Automobile</Stype>");
                string xml_10 = xml_09.Replace(@"<type>TelevisionStation</type>", @"<Stype>TelevisionStation</Stype>");
                string xml_11 = xml_10.Replace(@"<type>StateOrCounty</type>", @"<Stype>StateOrCounty</Stype>");
                string xml_12 = xml_11.Replace(@"<type>Organization</type>", @"<Stype>Organization</Stype>");
                string xml_13 = xml_12.Replace(@"<type>OperatingSystem</type>", @"<Stype>OperatingSystem</Stype>");
                string xml_14 = xml_13.Replace(@"<type>Crime</type>", @"<Stype>Crime</Stype>");
                string xml_15 = xml_14.Replace(@"<type>Technology</type>", @"<Stype>Technology</Stype>");
                string xml_16 = xml_15.Replace(@"<type>Facility</type>", @"<Stype>Facility</Stype>");
                string xml_17 = xml_16.Replace(@"<type>Continent</type>", @"<Stype>Continent</Stype>");
                string xml_18 = xml_17.Replace(@"<type>FinancialMarketIndex</type>", @"<Stype>FinancialMarketIndex</Stype>");
                string xml = xml_18.Replace(@"<type>HealthCondition</type>", @"<Stype>HealthCondition</Stype>");

                if (Historical.printAPIresultXML)
                {
                    try
                    {
                        File.Delete("f.xml");
                        File.WriteAllText("f.xml", xml);
                    }
                    catch { }
                }

                DataSet newN = new DataSet();
                newN.ReadXml(new StringReader(xml), XmlReadMode.Auto);
                                
                DataTable table = new DataTable();
                table = newN.Tables[2];

                lock (table)
                {
                    using (MySqlConnection cn1 = new MySqlConnection(connectMySQL))
                    {
                        MySqlCommand cmd;

                        foreach (DataRow row in table.Rows)        
                        {
                            cn1.Open();
                            string txt = row["text"].ToString().Replace("'", "").Replace("`", "").Replace("’", "");
                            string mxd = "";
                            try
                            {
                                mxd = row["mixed"].ToString().Replace("'", "").Replace("`", "").Replace("’", "");
                            }
                            catch { }

                            var sqlDate = date.Date.ToString("yyyy-MM-dd");
                            string query = @"Insert INTO api_results (Stype, artdate, source, url, api, relevance, score, mixed, count, text)  values ('" + row["Stype"] + "','" + sqlDate + "','" + source + "','" + url + "','Alchemy','" + row["relevance"] + "','" + row["score"] + "','" + mxd + "','" + row["count"] + "','" + txt + "')";
                            cmd = new MySqlCommand(query, cn1);
                            cmd.ExecuteNonQuery();
                            cn1.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter("errors-exceptions-Log.txt", true))
                {
                    writer.WriteLine(DateTime.Now.ToString("dd/MM/yy | hh:mm:ss :: ")+ url +" ||"  + ex.ToString());
                }
                return;
            }            
        }


        public static bool CheckIfUrlProcessed(string url)
        {
            if (LinkList.Contains(url))
                return true;
            else
                return false;
        }

        public static void LoadProcessedLinks(DateTime linkDate)
        {            
            using (MySqlConnection cn1 = new MySqlConnection(connectMySQL))
            {
                MySqlCommand cmd;
                if (cn1.State != ConnectionState.Open)
                    cn1.Open();

                string qry = "SELECT DISTINCT(url) FROM api_results WHERE artdate > '" + linkDate.Date.AddDays(-3).ToString("yyyy-MM-dd") + "'";
                cmd = new MySqlCommand(qry, cn1);
                try
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        LinkList.Add(reader["url"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    using (StreamWriter writer = new StreamWriter("errors-exceptions-Log.txt", true))
                    {
                        writer.WriteLine(DateTime.Now.ToString("dd/MM/yy | hh:mm:ss")+" ::  PROBLEM LOADING LINK LIST... (for: "+linkDate+")  ||" + ex.ToString());
                    }
                }
                cn1.Close();
            }
        }


        public static void WriteTableToApiResultsDB(DataTable table, DateTime date, string source, string url)
        {
            string connectMySQL = "Server=host320.hostmonster.com;Database=fivninni_sweng500;Uid=fivninni_apimax;Pwd=make1tw0rk;Port=3306";
            
            using (MySqlConnection cn1 = new MySqlConnection(connectMySQL))
            {
                MySqlCommand cmd;               

                foreach (DataRow row in table.Rows)       
                {
                    cn1.Open();
                    string text1 = row["text"].ToString().Replace("'", "");
                    string text2 = text1.Replace("`", ""); 
                    string txt = text2.Replace("’", "");
                    var sqlDate = date.Date.ToString("yyyy-MM-dd");
                    string query = @"Insert INTO api_results (Stype, artdate, source, url, api, relevance, score, mixed, count, text)  values ('" + row["Stype"] + "','" + sqlDate + "','" + source + "','" + url + "','Alchemy','" + row["relevance"] + "','" + row["score"] + "','" + row["mixed"] + "','" + row["count"] + "','" + txt + "')";
                    cmd = new MySqlCommand(query, cn1);
                    cmd.ExecuteNonQuery();
                    cn1.Close();
                }
            }
        }

        private void btnLaunchHist_Click(object sender, EventArgs e)
        {
           new Historical().Show();
        }

    }





}
