using System.Reflection;
using WalletApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup=>
{
    var xmlCommentsFiles = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFiles);
    setup.IncludeXmlComments(xmlCommentsFullPath);
});

var res = builder.Configuration;

builder.Services.WalletServicesExtension(res);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthorization();


app.UseEndpoints(endpoints => 
{ 
    endpoints.MapControllers();
});

app.Run();
