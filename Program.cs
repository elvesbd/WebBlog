using WebBlog.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
builder.Services.AddDbContext<BlogDataContext>();

var app = builder.Build();
app.MapControllers();
Console.WriteLine($"Versão do C#: {Environment.Version}");
app.Run();
