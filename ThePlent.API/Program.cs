using Microsoft.EntityFrameworkCore;
using ThePlant.EF;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("https://localhost:7016", "http://localhost:5069");

// Add services to the container.
ThePlant.EF.Services.DependencyRegistration.RegisterDependency(builder.Services, builder.Configuration);
ThePlant.API.Services.DependencyRegistration.RegisterDependency(builder.Services, builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();


app.Run();
