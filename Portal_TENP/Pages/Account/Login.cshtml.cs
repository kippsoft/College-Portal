using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Portal_TENP.Data;
using Portal_TENP.Models;

namespace Portal_TENP.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _config;

        public LoginModel(IConfiguration config)
        {
            _config = config;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnPost()
        {
            string connString = _config.GetConnectionString("ECollegeDB");

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                // 1️⃣ Check Trainee
                string traineeQuery = @"SELECT * 
                                        FROM tblTrainees
                                        WHERE AdmissionNo=@user 
                                        AND PasswordHash=@pass
                                        AND IsActive=1";

                SqlCommand cmd = new SqlCommand(traineeQuery, con);
                cmd.Parameters.AddWithValue("@user", Username);
                cmd.Parameters.AddWithValue("@pass", Password);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    HttpContext.Session.SetString("UserType", "Trainee");
                    HttpContext.Session.SetString("AdmissionNo", dr["AdmissionNo"].ToString());
                    HttpContext.Session.SetString("Username", dr["Name"].ToString());
                    HttpContext.Session.SetString("DepartmentID", dr["DepartmentID"].ToString());

                    return RedirectToPage("/Dashboard");
                }

                dr.Close();

                // 2️⃣ Check Staff
                string staffQuery = @"SELECT * 
                                      FROM tblStaff
                                      WHERE StaffNo=@user
                                      AND PasswordHash=@pass
                                      AND IsActive=1";

                SqlCommand cmd2 = new SqlCommand(staffQuery, con);
                cmd2.Parameters.AddWithValue("@user", Username);
                cmd2.Parameters.AddWithValue("@pass", Password);

                SqlDataReader dr2 = cmd2.ExecuteReader();

                if (dr2.Read())
                {
                    string staffID = dr2["ID"].ToString();

                    HttpContext.Session.SetString("UserType", "Staff");
                    HttpContext.Session.SetString("StaffID", staffID);
                    HttpContext.Session.SetString("Username", dr2["StaffName"].ToString());
                    HttpContext.Session.SetString("DepartmentID", dr2["DepartmentID"].ToString());

                    dr2.Close();

                    // 3️⃣ Load Roles
                    string roleQuery = @"SELECT RoleID 
                                         FROM tblUserRoles
                                         WHERE StaffID=@StaffID";

                    SqlCommand roleCmd = new SqlCommand(roleQuery, con);
                    roleCmd.Parameters.AddWithValue("@StaffID", staffID);

                    SqlDataReader roleReader = roleCmd.ExecuteReader();

                    string roles = "";

                    while (roleReader.Read())
                    {
                        roles += roleReader["RoleID"].ToString() + ",";
                    }

                    HttpContext.Session.SetString("Roles", roles);

                    return RedirectToPage("/Dashboard");
                }
            }

            ErrorMessage = "Invalid Login Details";
            return Page();
        }
    }
}
