using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quartz;
using SurveyMicroServices;
using SurveyMicroServices.Models;
using Quartz.Impl;
using Quartz.Spi;
using SurveyMicroServices.Jobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireUppercase = false;


    options.User.RequireUniqueEmail = true;

    options.SignIn.RequireConfirmedEmail = true;


}
).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


builder.Services.AddScoped<Survey>();

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var jobKey = new JobKey("UpdateSurveyStatusJob");
    q.AddJob<UpdateSurveyStatusJob>(opts=>opts.WithIdentity(jobKey));
    q.AddTrigger(opts=>opts
    .ForJob(jobKey)
    .WithIdentity("UpdateSurveyStatusJob-trigger")
    .WithSimpleSchedule(x=>x.WithIntervalInHours(1).RepeatForever()));
});

builder.Services.AddSingleton<IJobFactory, JobFactory>(); 
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


var app = builder.Build();

app.UseCors("AllowAll");

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
