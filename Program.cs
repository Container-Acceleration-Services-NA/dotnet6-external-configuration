using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using EmployeeCompany.Models;


var builder = WebApplication.CreateBuilder(args);

// Swagger config
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<EmployeeDb>(options => options.UseInMemoryDatabase("items"));
builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo {
         Title = "Employee API",
         Description = "A dummy employee API",
         Version = "v1" });
});

// Add Healtchecks
builder.Services.AddHealthChecks();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// // Swagger Endpoint
app.UseSwagger();
app.UseSwaggerUI(c =>
{
   c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee API V1");
});

// Healtcheck endpoint
app.MapHealthChecks("/healthz");


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// Get Employees
app.MapGet("/employees", async (EmployeeDb db) => await db.Employees.ToListAsync());

// Create Employees
app.MapPost("/employees", async (EmployeeDb db, Employee employee) =>
{
    await db.Employees.AddAsync(employee);
    await db.SaveChangesAsync();
    return Results.Created($"/employee/{employee.Id}", employee);
});

// Get Employee by id
app.MapGet("/employee/{id}", async (EmployeeDb db, int id) => await db.Employees.FindAsync(id));

// Update Employee
app.MapPut("/employee/{id}", async (EmployeeDb db, Employee updateemployee, int id) =>
{
    var employee = await db.Employees.FindAsync(id);
    if (employee is null) return Results.NotFound();
    employee.Name = updateemployee.Name;
    employee.Lastname = updateemployee.Lastname;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Delete Employee
app.MapDelete("/employee/{id}", async (EmployeeDb db, int id) =>
{
  var employee = await db.Employees.FindAsync(id);
  if (employee is null)
  {
    return Results.NotFound();
  }
  db.Employees.Remove(employee);
  await db.SaveChangesAsync();
  return Results.Ok();
});

app.Run();
