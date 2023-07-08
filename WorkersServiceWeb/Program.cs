var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<WorkersServiceWeb.Repositories.IWorkerRepository,
    WorkersServiceWeb.Repositories.WorkerRepository>(provider =>
    new WorkersServiceWeb.Repositories.WorkerRepository("Server = (localdb)\\mssqllocaldb;" +
    "Initial Catalog=WorkersDb; Integrated Security=True")); 
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
