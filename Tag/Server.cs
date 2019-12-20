using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;

namespace Tag
{
    class Server
    {
        private static readonly string IP = "185.179.136.127";
        private static readonly string port = "1433";
        public static bool CheckLogIn(string username, string password) {
            bool answer = false;
            string connectionString;
            
            using (SqlConnection conn = new SqlConnection())
            {
                connectionString = @"Data Source="+IP+","+port+ ";Initial Catalog=TagGame;User ID=TestLogin;Password=Password123;Connect Timeout=5";
                conn.ConnectionString = connectionString;
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) as Validation FROM dbo.Clients WHERE username = '" + username + "' AND password = '" + password +"';", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            if (reader["Validation"].ToString() == "1")
                            {
                                answer = true;
                            }
                        }
                    }
                }
                conn.Close();
            }
            return answer;
        }

        public static string GetTag() {
            string connectionString;

            using (SqlConnection conn = new SqlConnection())
            {
                connectionString = @"Data Source=" + IP + "," + port + ";Initial Catalog=TagGame;User ID=TestLogin;Password=Password123;Connect Timeout=5";
                conn.ConnectionString = connectionString;
                using (SqlCommand command = new SqlCommand("SELECT TOP 1 RTRIM(tag_username) as tag_username FROM dbo.TagTable ORDER BY time DESC;", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            return reader["tag_username"].ToString();
                        }
                    }
                }
                conn.Close();
            }
            return "System.Error";
        }

        public static List<string> GetListOfUsers(){

            List<string> list = new List<string>();

            string connectionString;

            using (SqlConnection conn = new SqlConnection())
            {
                connectionString = @"Data Source=" + IP + "," + port + ";Initial Catalog=TagGame;User ID=TestLogin;Password=Password123;Connect Timeout=5";
                conn.ConnectionString = connectionString;
                using (SqlCommand command = new SqlCommand("SELECT RTRIM(username) as username FROM dbo.Clients ORDER BY username ASC;", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if(reader["username"].ToString() != Client.Username)
                                    list.Add(reader["username"].ToString());
                            }
                        }
                    }
                }
                conn.Close();
            }

            return list;
        }

        public static bool SetTag(string currentTag, string nextTag) {
            if (Client.Username == currentTag)
            {
                string connectionString;

                string timeNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

                using (SqlConnection conn = new SqlConnection())
                {
                    connectionString = @"Data Source=" + IP + "," + port + ";Initial Catalog=TagGame;User ID=TestLogin;Password=Password123;Connect Timeout=5";
                    conn.ConnectionString = connectionString;
                    using (SqlCommand command = new SqlCommand("INSERT INTO dbo.TagTable (tag_username,time) VALUES ('"+nextTag+ "',convert(datetime,'" + timeNow+"'));", conn))
                    {
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                return true;
            }
            return false;
        }

        public static string GetLastTagTime() {
            string connectionString;

            using (SqlConnection conn = new SqlConnection())
            {
                connectionString = @"Data Source=" + IP + "," + port + ";Initial Catalog=TagGame;User ID=TestLogin;Password=Password123;Connect Timeout=5";
                conn.ConnectionString = connectionString;
                using (SqlCommand command = new SqlCommand("SELECT TOP 1 time FROM dbo.TagTable WHERE tag_username='" + Client.Username + "' ORDER BY time DESC;", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            return System.Convert.ToDateTime(reader["time"]).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        }
                    }
                }
                conn.Close();
            }
            return "Never";
        }

        public static string WhoTaggedYou(string lastTimeTagged) {
            string connectionString;

            using (SqlConnection conn = new SqlConnection())
            {
                connectionString = @"Data Source=" + IP + "," + port + ";Initial Catalog=TagGame;User ID=TestLogin;Password=Password123;Connect Timeout=5";
                conn.ConnectionString = connectionString;
                using (SqlCommand command = new SqlCommand("SELECT TOP 1 RTRIM(tag_username) as tag_name FROM dbo.TagTable WHERE time < '" + lastTimeTagged + "' ORDER BY time DESC;", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            return reader["tag_name"].ToString();
                        }
                    }
                }
                conn.Close();
            }
            return "Error";
        }

        public static bool AddUser(string username, string password) {
            string connectionString;

            bool not_exists = false;

            using (SqlConnection conn = new SqlConnection())
            {
                connectionString = @"Data Source=" + IP + "," + port + ";Initial Catalog=TagGame;User ID=TestLogin;Password=Password123;Connect Timeout=5";
                conn.ConnectionString = connectionString;
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) as Count FROM dbo.Clients WHERE username = '"+username+"'; ", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            if (reader["Count"].ToString() == "0")
                            {
                                not_exists = true;
                            }
                        }
                    }
                    conn.Close();
                }

                if (!not_exists)
                    return false;

                using (SqlCommand insert = new SqlCommand("INSERT INTO dbo.Clients (username,password) VALUES('" + username + "','" + password + "');", conn))
                {
                    conn.Open();
                    if (insert.ExecuteNonQuery() > 0)
                        return true;
                    conn.Close();
                }
            }
            return false;
        }

    }
}