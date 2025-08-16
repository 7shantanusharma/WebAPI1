using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace WebAPI1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "admin,user")]
    public class CountryController : ControllerBase
    {
        private DataTable dataTable = new DataTable();

        private readonly IConfiguration _config;

        public CountryController(IConfiguration configuration)
        {
            _config = configuration;
        }

        [HttpGet("GetCountries")]
        public string GetCountries()
        {
            string? connString = _config["ConnectionString:DBSetting"];

            string query = "select * from Country order by CountryName";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                // create data adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // this will query your database and return the result to your datatable
                da.Fill(dataTable);
                conn.Close();
                da.Dispose();
            }

            return JsonConvert.SerializeObject(dataTable, Formatting.Indented);
        }

        [HttpGet("TestData")]
        [Authorize(Roles = "admin")]
        public string TestData()
        {
            return "Success";
        }
    }
}
