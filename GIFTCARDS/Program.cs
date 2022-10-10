
using GiftCards;
using GiftCards.metodos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddDbContext<SampleContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<SecurityManager>();
builder.Services.AddTransient<GenerarCodigoCupon>();
//builder.Services.AddTransient<IEmpleadoRepository, EmpleadoRepository>();
//builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();

List<string> CorsOriginAllowed = builder.Configuration.GetSection("AllowedOrigins").Get<List<string>>();
string[] origins = CorsOriginAllowed != null ? CorsOriginAllowed.ToArray() : new string[] { "*" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        .WithOrigins(origins)
        .AllowAnyMethod()
        .AllowAnyHeader()
        );
});

var app = builder.Build();
app.UseCors("CorsPolicy");
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.UseCors(x => x
//              .AllowAnyMethod()
//              //.AllowAnyOrigin()
//              .AllowAnyHeader()
//              .SetIsOriginAllowed(origin => true) // allow any origin
//              .AllowCredentials()); // allow credentials
app.MapControllers();

app.Run();