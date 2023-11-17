using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using LearningStarter.Data;
using LearningStarter.Entities;
using LearningStarter.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LearningStarter;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors();
        services.AddControllers();

        services.AddHsts(options =>
        {
            options.MaxAge = TimeSpan.MaxValue;
            options.Preload = true;
            options.IncludeSubDomains = true;
        });

        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddIdentity<User, Role>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 8;
                    options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
                    options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
                    options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
                })
            .AddEntityFrameworkStores<DataContext>();

        services.AddMvc();

        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

        services.AddAuthorization();

        // Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Learning Starter Server",
                Version = "v1",
                Description = "Description for the API goes here.",
            });

            c.CustomOperationIds(apiDesc => apiDesc.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name : null);
            c.MapType(typeof(IFormFile), () => new OpenApiSchema { Type = "file", Format = "binary" });
        });

        services.AddSpaStaticFiles(config =>
        {
            config.RootPath = "learning-starter-web/build";
        });

        services.AddHttpContextAccessor();

        // configure DI for application services
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext dataContext)
    {
        dataContext.Database.EnsureDeleted();
        dataContext.Database.EnsureCreated();
        
        app.UseHsts();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSpaStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        // global cors policy
        app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger(options =>
        {
            options.SerializeAsV2 = true;
        });

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Learning Starter Server API V1");
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(x => x.MapControllers());

        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "learning-starter-web";
            if (env.IsDevelopment())
            {
                spa.UseProxyToSpaDevelopmentServer("http://localhost:3001");
            }
        });
        
        using var scope = app.ApplicationServices.CreateScope();
        var userManager = scope.ServiceProvider.GetService<UserManager<User>>();

        SeedUsers(dataContext, userManager).Wait();
        SeedsUsers(dataContext, userManager).Wait();
        SeedGroup(dataContext);
        SeedMessage(dataContext);
        SeedFlasCardSet(dataContext);
        SeedFlashCards(dataContext);
        SeedTests(dataContext);
        SeedTestQuestions(dataContext);
        SeedAssignments(dataContext);
        SeedAssignmentGrades(dataContext);
    }


    private static void SeedGroup(DataContext dataContext)
    {
        if (dataContext.Set<Group>().Any())
        {
            return;
        }

        var seededGroup1 = new Group
        {
            GroupName = "CMPS 285",
            Description = "Group for students in CMPS 285",
        };

        dataContext.Set<Group>().Add(seededGroup1);
        dataContext.SaveChanges();
    }

    private static void SeedMessage(DataContext dataContext)
    {
        if (dataContext.Set<Messages>().Any())
        {
            return;
        }

        var seededMessage1 = new Messages
        {
            GroupId = 1,
            Content = "What is up guys! So glad to be in CMPS 285!",
            CreatedAt = "12:52 PM",
            UserId = 1,
            
        };

        dataContext.Set<Messages>().Add(seededMessage1);
        dataContext.SaveChanges();
    }

    private static void SeedFlasCardSet(DataContext dataContext)
    {
        if (dataContext.Set<FlashCardSets>().Any())
        {
            return;
        }

        var seededFlashCardSet1 = new FlashCardSets
        {
            SetName = "Midterm study material",
            GroupId = 1,
           
        };

        dataContext.Set<FlashCardSets>().Add(seededFlashCardSet1);
        dataContext.SaveChanges();
    }

    private static void SeedFlashCards(DataContext dataContext)
    {
        if (dataContext.Set<FlashCards>().Any())
        {
            return;
        }

        var seededFlashCard1 = new FlashCards
        {
            FlashCardSetId = 1,
            Question = "What is scrum",
            Answer = "A framework for getting work done with agile development methods",
        };

        dataContext.Set<FlashCards>().Add(seededFlashCard1);
        dataContext.SaveChanges();
    }

    private static void SeedTests(DataContext dataContext)
    {
        if (dataContext.Set<Tests>().Any())
        {
            return;
        }

        var seededTest1 = new Tests
        {
            TestName = "Midterm Material",
            GroupId = 1,
        };

        dataContext.Set<Tests>().Add(seededTest1);
        dataContext.SaveChanges();
    }

    private static void SeedTestQuestions(DataContext dataContext)
    {
        if (dataContext.Set<TestQuestions>().Any())
        {
            return;
        }

        var seededTestQuestion1 = new TestQuestions
        {
            TestId = 1,
            Question = "What is a primary key",
            Answer = "A unique identifier for a specific table and record in a database",
        };

        dataContext.Set<TestQuestions>().Add(seededTestQuestion1);
        dataContext.SaveChanges();
    }

    private static void SeedAssignments(DataContext dataContext)
    {
        if (dataContext.Set<Assignments>().Any())
        {
            return;
        }

        var seededAssignment1 = new Assignments
        {
            GroupId = 1,
            AssignmentName = "Midterm Presentations",
        };

        dataContext.Set<Assignments>().Add(seededAssignment1);
        dataContext.SaveChanges();
    }

    private static void SeedAssignmentGrades(DataContext dataContext)
    {
        if (dataContext.Set<AssignmentGrade>().Any())
        {
            return;
        }

        var seededAssignmentGrade1 = new AssignmentGrade
        {
            AssignmentId = 1,
            Grades = 100
        };

        dataContext.Set<AssignmentGrade>().Add(seededAssignmentGrade1);
        dataContext.SaveChanges(); 
    }







    private static async Task SeedUsers(DataContext dataContext, UserManager<User> userManager)
    {
        var numUsers = dataContext.Users.Count();

        if (numUsers == 0)
        {
            var seededUser = new User
            {
                FirstName = "Seeded",
                LastName = "User",
                UserName = "admin",
                
            };

            await userManager.CreateAsync(seededUser, "Password");
            await dataContext.SaveChangesAsync();
        }
    }


private static async Task SeedsUsers(DataContext dataContext, UserManager<User> userManager)
{
    var numUsers = dataContext.Users.Count();

    
        var seededUser = new User
        {
            FirstName = "Luke",
            LastName = "Norton",
            UserName = "LukeNorton",

        };

        await userManager.CreateAsync(seededUser, "Password");
        await dataContext.SaveChangesAsync();
    
   }
}
