using GastuakApi;
using GastuakApi.Repositorioak;

/// <summary>
/// API aplikazioaren abiarazpena eta middleware/zerbitzuen konfigurazioa.
/// </summary>
/// <remarks>
/// Hemen konfiguratzen dira:
/// - CORS politika (frontend-etik deitzeko)
/// - Swagger (garapen ingurunean)
/// - NHibernate (SessionFactory + middleware-a)
/// - Controller mapaketa
/// </remarks>

var builder = WebApplication.CreateBuilder(args);

// CORS konfigurazioa (frontend-etik deitzeko)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

// Swagger (OpenAPI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI: NHibernate eta repositorioak
builder.Services.AddSingleton(NHibernateHelper.SessionFactory);
builder.Services.AddTransient<FamiliaRepository>();
builder.Services.AddTransient<ErabiltzaileaRepository>();
builder.Services.AddTransient<ProduktuaRepository>();

var app = builder.Build();

// Garapen ingurunean Swagger aktibatu
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

// NHibernate saioa request bakoitzean kudeatzeko middleware-a
app.UseMiddleware<NHibernateSessionMiddleware>();

app.MapControllers();

app.Run();
