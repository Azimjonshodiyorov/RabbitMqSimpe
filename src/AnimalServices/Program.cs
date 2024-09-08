using AnimalServices.DataContext;
using AnimalServices.Services;
using AnimalServices.Services.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RabbitDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
//config rabbitMq
builder.Services.AddMassTransit(x =>
    {
        //outbox pattern
        x.AddEntityFrameworkOutbox<RabbitDbContext>(o =>
        {
            //10 sonya kutish habar yuborishdan oldin
            o.QueryDelay = TimeSpan.FromSeconds(10);
            o.UsePostgres();
            o.UseBusOutbox();
        });
        x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("animal", false));
        
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(builder.Configuration["RabbitMq:Host"],"/", host =>
            {
                host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
                host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
            });
            cfg.ConfigureEndpoints(context);
        });
    }
);
builder.Services.AddCors(options =>
{
    options.AddPolicy("customPolicy", x =>
    {
        x.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("http://localhost:8081");
    });
});

var app = builder.Build();

app.UseCors("customPolicy");
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
    endpoints.MapControllers(); // bu yerdagi konrollerlarni xaritalash
});
try
{
    DbInitializer.InitDb(app);
}
catch (Exception e)
{

    Console.WriteLine(e);
}
app.Run();