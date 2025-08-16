using Microsoft.Data.SqlClient;
using System.Data;
using WebAPI1.Models;

namespace WebAPI1.Service
{
    public class CommonService(IConfiguration configuration)
    {
        private DataTable? data;
        string? connString = configuration["ConnectionString:DBSetting"];

        public byte IsAuthenticatedUser(LoginRequest loginRequestModel)
        {
            data = new DataTable();
            byte userId = 0;

            string query = "select ID from [User] where Username = @Username and Password=@Password";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Username", loginRequestModel.Username);
                cmd.Parameters.AddWithValue("@Password", loginRequestModel.Password);

                conn.Open();

                // create data adapter
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userId = byte.Parse(reader["ID"].ToString());
                    }
                }

                conn.Close();
            }

            return userId;
        }

        public List<Roles> GetUserRoles(byte userId)
        {
            List<Roles> roles = new List<Roles>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetRoles", conn);

                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();

                // create data adapter
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        roles.Add(new Roles
                        {
                            RoleName = reader["RoleName"].ToString()
                        });
                    }
                }

                conn.Close();
            }

            return roles;
        }
    }
}
