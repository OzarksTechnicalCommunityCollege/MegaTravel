using MegaTravelAPI.Data;

var builder = WebApplication.CreateBuilder(args);
//var databaseConnectionString = builder.Configuration["MegaTravel:DatabaseConnectionString"];
var databaseConnectionString = "cis-db.ckwia8qkgyyj.us-east-1.rds.amazonaws.com; Database = MegaTravel; User Id = fordt; Password = G0dSaveTheQu33n;";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
