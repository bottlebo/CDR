using CDR_Exchange;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();

// Add services to the container.
builder.Services.AddControllers();
builder.AddHttpClients();
builder.AddServices();
builder.AddCors();
builder.AddDbCtx();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.AddSwagger();

var app = builder.Build();

app.Migrate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();