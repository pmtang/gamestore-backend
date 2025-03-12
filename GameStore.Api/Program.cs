using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);

//add cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    
              .AllowAnyMethod()   
              .AllowAnyHeader();   
    });
});
//add cors

var app = builder.Build();

app.UseCors("AllowAll"); // use cors strategy

app.MapGamesEndpoints();
app.MapGenresEndpoints();

await app.MigradteDbAsync();

app.Run();
