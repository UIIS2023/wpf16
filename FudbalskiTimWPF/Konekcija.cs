using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;//dodato
namespace FudbalskiTimWPF
{
    internal class Konekcija
    {
        public SqlConnection KreirajKonekciju()
        {
            SqlConnectionStringBuilder ccSb = new SqlConnectionStringBuilder
            {
                DataSource = @"DESKTOP-SBDOD4R\SQLEXPRESS",
                InitialCatalog = "FudbalskiTim",
                IntegratedSecurity = true
            };
            string con = ccSb.ToString();
            SqlConnection konekcija = new SqlConnection(con);
            return konekcija;
        }
    }
}
