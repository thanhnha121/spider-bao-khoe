using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderNews.Lib
{
    public class SqlHelper : IDisposable
    {
        readonly SqlConnection _conn;
        SqlDataReader _reader;
        public SqlDataReader Reader
        {
            get { return _reader; }
        }

        public SqlHelper(string connString)
        {
            _conn = new SqlConnection(connString);
            _conn.Open();
        }

        public void OpenReader(string tableName, Dictionary<string, string> pr)
        {
            string sql = "select * from " + tableName + " where 1=1";
            if (pr != null)
            {
                foreach (var item in pr)
                {
                    sql += " and " + item.Key + "=" + item.Value;
                }
            }
            SqlCommand cmd = new SqlCommand(sql, _conn);
            _reader = cmd.ExecuteReader();
        }
        public void OpenReader(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, _conn);
            _reader = cmd.ExecuteReader();
        }
        public void ClodeReader()
        {
            try
            {
                if (_reader.IsClosed == false)
                    _reader.Close();
            }
            catch
            {
                // ignored
            }
            finally
            {
                try
                {
                    _reader.Dispose();
                }
                catch
                {
                    // ignored
                }
            }
        }

        public void ExecuteCommand(string sql, Dictionary<string, string> pr)
        {
            SqlCommand cmd = new SqlCommand(sql, _conn);
            if (pr != null)
                foreach (var item in pr)
                {
                    cmd.Parameters.AddWithValue("@" + item.Key, item.Value);
                }
            cmd.CommandTimeout = 0;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        public void ExecuteCommand(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.CommandTimeout = 0;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        public int GetMaxOrdGroupArticleAssign(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.CommandTimeout = 0;

            int result = 0;
            try
            {
                result = (int)cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                // ignored
            }
            cmd.Dispose();
            return result;
        }

        public bool CheckExistBySql(string tableName, Dictionary<string, string> pr)
        {
            string sql = "select count(*) from " + tableName + " where 1=1";
            if (pr != null)
                foreach (var item in pr)
                {
                    sql += " and " + item.Key + "=" + item.Value;
                }
            SqlCommand cmd = new SqlCommand(sql, _conn);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int val = Convert.ToInt32(reader[0]);
                reader.Close();
                cmd.Dispose();
                return val > 0;
            }
            else
            {
                reader.Close();
                cmd.Dispose();
                return false;
            }
        }
        public bool CheckArticleExistBySourceUrl(string sourceUrl)
        {
            string sql = "select count(*) from Article where SourceUrl=@SourceUrl";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@SourceUrl", sourceUrl);

            int val = (int)cmd.ExecuteScalar();

            cmd.Dispose();

            return val > 0;
        }
        public bool CheckArticleExistByTitle(string title)
        {
            string sql = "select count(*) from Article where Title=@Title";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@Title", title);

            int val = (int)cmd.ExecuteScalar();

            cmd.Dispose();

            return val > 0;
        }

        public bool Add(string tableName, Dictionary<string, string> pr)
        {
            string sql = "";

            sql += "insert " + tableName + "(";
            int d = 0;
            if (pr != null)
                foreach (var item in pr)
                {
                    if (d++ == 0)
                        sql += item.Key;
                    else
                        sql += "," + item.Key;
                }
            sql += ") values(";
            d = 0;
            if (pr != null)
                foreach (var item in pr)
                {
                    if (d++ == 0)
                        sql += "@" + item.Key;
                    else
                        sql += ",@" + item.Key;
                }
            sql += ")";

            SqlCommand cmd = new SqlCommand(sql, _conn);
            if (pr != null)
                foreach (var item in pr)
                {
                    cmd.Parameters.AddWithValue("@" + item.Key, item.Value);
                }
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            return true;
        }
        // OUTPUT INSERTED.ID
        public int AddToGetInsertId(string tableName, Dictionary<string, string> pr, string autoId)
        {
            string sql = "";

            sql += "insert " + tableName + "(";
            int d = 0;
            if (pr != null)
                foreach (var item in pr)
                {
                    if (d++ == 0)
                        sql += item.Key;
                    else
                        sql += "," + item.Key;
                }
            sql += ") OUTPUT " + autoId + "  values(";
            d = 0;
            if (pr != null)
                foreach (var item in pr)
                {
                    if (d++ == 0)
                        sql += "@" + item.Key;
                    else
                        sql += ",@" + item.Key;
                }
            sql += ")";

            SqlCommand cmd = new SqlCommand(sql, _conn);
            if (pr != null)
                foreach (var item in pr)
                {
                    cmd.Parameters.AddWithValue("@" + item.Key, item.Value);
                }
            int insertedId = (int)cmd.ExecuteScalar();
            cmd.Dispose();
            return insertedId;
        }

        public DataTable Get(string tableName, Dictionary<string, string> pr)
        {
            string sql = "select * from " + tableName + " where 1=1";
            if (pr != null)
                foreach (var item in pr)
                {
                    sql += " and " + item.Key + "=" + item.Value;
                }
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sql, _conn);
            da.Fill(dt);
            da.Dispose();
            return dt;
        }
        public DataTable Get(string sql)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sql, _conn);
            da.Fill(dt);
            da.Dispose();
            return dt;
        }
        public void Dispose()
        {
            _conn.Close();
            _conn.Dispose();
        }

    }
}
