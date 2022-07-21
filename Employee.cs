using Microsoft.EntityFrameworkCore;
namespace EmployeeCompany.Models 
{
  public class Employee
  {
      public int Id { get; set; }
      public string? Name { get; set; }
      public string? Lastname { get; set; }
  }
  class EmployeeDb : DbContext
    {
        public EmployeeDb(DbContextOptions options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
    }
}