using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly string _connectionString;

    public EmployeeController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new ArgumentNullException("DefaultConnection", "Connection string is missing in appsettings.json");
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployees()
    {
        var employees = new List<Employee>();

        try
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT EmployeeID, Name, Email, Department FROM Employees";

                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            employees.Add(new Employee
                            {
                                EmployeeID = reader.IsDBNull(reader.GetOrdinal("EmployeeID")) ? null : reader["EmployeeID"].ToString(),
                                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader["Name"].ToString(),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader["Email"].ToString(),
                                Department = reader.IsDBNull(reader.GetOrdinal("Department")) ? null : reader["Department"].ToString()
                            });
                        }
                    }
                }
            }
            return Ok(employees);
        }
        catch (SqlException ex)
        {
            return StatusCode(500, $"Database error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

public class Employee
{
    public string? EmployeeID { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Department { get; set; }
}
