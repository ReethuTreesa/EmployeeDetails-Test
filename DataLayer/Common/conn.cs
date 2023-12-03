using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Common
{
    public class conn
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();

        public conn()
        {
            con.ConnectionString = "Data Source=LCCSERVER;Initial Catalog=steps;User ID=sa;Password=admin123";
            cmd.Connection = con;
        }

        public void openconnection()
        //using for eliminating the connection error
        {

            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
        }

        public SqlCommand getcommand
        {
            get
            {
                return cmd;
            }
        }
    }
}
