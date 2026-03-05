using Microsoft.Data.SqlClient;
using System.Data;

namespace Portal_TENP.Data
{
    public class Db
    {
        private readonly IConfiguration _config;

        public Db(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection ECollege()
        {
            return new SqlConnection(_config.GetConnectionString("ECollegeDB"));
        }

        public IDbConnection SIMS()
        {
            return new SqlConnection(_config.GetConnectionString("SIMSDB"));
        }
    }
}
