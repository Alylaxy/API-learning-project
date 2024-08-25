using API.GameStore.Data;
using API.GameStore.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var conexao = builder.Configuration.GetConnectionString("GameStoreConnection");
builder.Services.AddSqlite<GameStoreContext>(conexao);

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapearEndpoints();

app.Run();