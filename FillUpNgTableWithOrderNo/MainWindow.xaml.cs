using MST.MES;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FillUpNgTableWithOrderNo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }
        private static class bla
        {
            public static Dictionary<string, string> serialTOrder = new Dictionary<string, string>();
            public static List<string> serialsInNgTable = new List<string>();

            public static Dictionary<string, string> serialToOrderNo()
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                DateTime dateUntil = (DateTime.Now.AddDays(-100));
                string query = @"SELECT distinct serial_no, wip_entity_name,tester_id FROM MES.dbo.tb_tester_measurements WHERE datetime>@date AND tester_id<>0";

                using (SqlConnection conn = new SqlConnection(@"Data Source=MSTMS010;Initial Catalog=MES;User Id=mes;Password=mes;"))
                {
                    using (var cmd = conn.CreateCommand())
                    {

                        var parameter = cmd.CreateParameter();
                        parameter.ParameterName = "@date";
                        parameter.Value = dateUntil;
                        cmd.Parameters.Add(parameter);

                        cmd.CommandText = query;
                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                string serial = SafeGetString(rdr, "serial_no");
                                if (string.IsNullOrEmpty(serial)) continue;
                                string orderNo = SafeGetString(rdr, "wip_entity_name");
                                result.Add(serial, orderNo);
                            }
                        }
                    }
                }
                return result;
            }

            public static string SafeGetString(SqlDataReader reader, string colName)
            {
                int colIndex = reader.GetOrdinal(colName);
                if (!reader.IsDBNull(colIndex))
                    return reader.GetString(colIndex);
                return string.Empty;
            }

            public static List<string> getList ()
            {
                List<string> result = new List<string>();

                return result;
            }

            internal static void UpdateOrder(string serial, string v)
            {
                throw new NotImplementedException();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //BAZOOKA
            bla.serialTOrder = bla.serialToOrderNo();
            bla.serialsInNgTable = bla.getList();

            foreach (var serial in bla.serialsInNgTable)
            {
                bla.UpdateOrder(serial, bla.serialTOrder[serial]);
            }
        }

    }
}
