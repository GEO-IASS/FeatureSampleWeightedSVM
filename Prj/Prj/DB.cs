using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
namespace Prj
{
    class DB
    {
        protected string conStr;//�����ַ���
        protected SqlConnection con;//���Ӷ���
        public DB()
        {
            conStr = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Data\\CC.mdf;Integrated Security=True;User Instance=True";
        }
        protected void open()
        {
            if (con == null)
            {
                con = new SqlConnection(conStr);
            }

            if (con.State.Equals(System.Data.ConnectionState.Closed))
            {
                con.Open();
                //Console.WriteLine("���ݿ�򿪣�");
            }
        }
        public void close()
        {
            if (con != null && !con.State.Equals(System.Data.ConnectionState.Closed))
                con.Close();
            //Console.WriteLine("���ݿ�رգ�");
        }
        public SqlDataReader getDataReader(string sql)
        {
            this.open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = sql;
            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }
        public DataTable getDataTable(string sql)
        {
            this.open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = sql;
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            da.SelectCommand = cmd;
            da.Fill(dt);
            this.close();
            return dt;
        }
    }
}
