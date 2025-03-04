var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();  // ✅ Registers controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();  // ✅ Maps all controller routes

app.Run();
