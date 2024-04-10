using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Webs1te.Pages.Account
{

    public class LoginModel : PageModel

    {
        public string ErrorMessage { get; set; }
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(ILogger<LoginModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string connectionString = "Data Source=KIETPC;Initial Catalog=myuser;Integrated Security=True"; 
            string query = "SELECT COUNT(*) FROM Users WHERE Username=@Username AND Password=@Password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", Input.Username);
                command.Parameters.AddWithValue("@Password", Input.Password);

                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();

                    if (count == 1)
                    {
                        return RedirectToPage("/Clients/Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid username or password");
                        return Page();
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again later.");
                    _logger.LogError(ex, "An error occurred while processing login request.");
                    return Page();
                }
            }
        }
    }
}
