using XorTag;
using XorTag.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Scan(scan => scan.FromAssemblyOf<Program>()
    .AddClasses()
    .AsSelf()
    .AsImplementedInterfaces());

builder.Services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
builder.Services.AddSingleton<IIdGenerator, IdGenerator>();
builder.Services.AddSingleton<INameGenerator, NameGenerator>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();
app.MapRazorPages();

app.Run();
