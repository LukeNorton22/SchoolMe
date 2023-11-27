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
        SeedGroupUser(dataContext);
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
            CreatorId = 1
            
        };
        var seededGroup2 = new Group
        {
            GroupName = "American History 101",
            Description = "Group for students in Professor Alberdean's American History 101 section 2 class.",
            CreatorId = 3

        };
        var seededGroup3 = new Group
        {
            GroupName = "College Algebra 201",
            Description = "Professor Leonard's college algebra 201 section 1 class",
            CreatorId = 2

        };

        dataContext.Set<Group>().Add(seededGroup1);
        dataContext.Set<Group>().Add(seededGroup2);
        dataContext.Set<Group>().Add(seededGroup3);
        dataContext.SaveChanges();
    }
    private static void SeedGroupUser(DataContext dataContext)
    {
        if (dataContext.Set<GroupUser>().Any())
        {
            return;
        }

        var seededGroupUser1 = new GroupUser
        {
            GroupId = 1,
            UserId = 1,
            CreatorId = 1,

        };
        var seededGroupUser2 = new GroupUser
        {
            GroupId = 1,
            UserId = 2,
            CreatorId = 1,

        };
        var seededGroupUser3 = new GroupUser
        {
            GroupId = 1,
            UserId = 3,
            CreatorId = 1,

        };
        //add RobertTurner to group 2
       // var seededGroupUser4 = new GroupUser
       // {
          //  GroupId = 2,
          //  UserId = 3,
         //   CreatorId = 3,

        //};
        //seed LukeNorton to group 3
        var seededGroupUser5 = new GroupUser
        {
            GroupId = 3,
            UserId = 2,
            CreatorId = 2,

        };
        //seeding members for group 1 
        var seededGroupUser6 = new GroupUser
        {
            GroupId = 1,
            UserId = 4,
            CreatorId = 1,

        };
        var seededGroupUser7 = new GroupUser
        {
            GroupId = 1,
            UserId = 5,
            CreatorId = 1,

        };
        var seededGroupUser8 = new GroupUser
        {
            GroupId = 1,
            UserId = 6,
            CreatorId = 1,

        };
        var seededGroupUser9 = new GroupUser
        {
            GroupId = 1,
            UserId = 7,
            CreatorId = 1,


        };
        //seeding for group 2 
        var seededGroupUser10 = new GroupUser
        {
            GroupId = 2,
            UserId = 2,
            CreatorId = 3,

        };
        var seededGroupUser11 = new GroupUser
        {
            GroupId = 2,
            UserId = 1,
            CreatorId = 3,

        };
        var seededGroupUser12 = new GroupUser
        {
            GroupId = 2,
            UserId = 4,
            CreatorId = 3,

        };

        var seededGroupUser13 = new GroupUser
        {
            GroupId = 3,
            UserId = 1,
            CreatorId = 2,

        };
        var seededGroupUser14 = new GroupUser
        {
            GroupId = 3,
            UserId = 3,
            CreatorId = 2,

        };
        var seededGroupUser15 = new GroupUser
        {
            GroupId = 3,
            UserId = 5,
            CreatorId = 2,

        };
        var seededGroupUser16 = new GroupUser
        {
            GroupId = 2,
            UserId = 5,
            CreatorId = 3,

        };
        var seededGroupUser17 = new GroupUser
        {
            GroupId = 2,
            UserId = 3,
            CreatorId = 3,

        };
        var seededGroupUser18 = new GroupUser
        {
            GroupId = 2,
            UserId = 6,
            CreatorId = 3,

        };
        var seededGroupUser19 = new GroupUser
        {
            GroupId = 2,
            UserId = 7,
            CreatorId = 3,

        };
        dataContext.Set<GroupUser>().Add(seededGroupUser1);
        dataContext.Set<GroupUser>().Add(seededGroupUser2); 
        dataContext.Set<GroupUser>().Add(seededGroupUser3);
       // dataContext.Set<GroupUser>().Add(seededGroupUser4);
        dataContext.Set<GroupUser>().Add(seededGroupUser5);
        dataContext.Set<GroupUser>().Add(seededGroupUser6);
        dataContext.Set<GroupUser>().Add(seededGroupUser7);
        dataContext.Set<GroupUser>().Add(seededGroupUser8);
        dataContext.Set<GroupUser>().Add(seededGroupUser9);
        dataContext.Set<GroupUser>().Add(seededGroupUser10);
        dataContext.Set<GroupUser>().Add(seededGroupUser11);
        dataContext.Set<GroupUser>().Add(seededGroupUser12);
        dataContext.Set<GroupUser>().Add(seededGroupUser13);
        dataContext.Set<GroupUser>().Add(seededGroupUser14);
        dataContext.Set<GroupUser>().Add(seededGroupUser15);
        dataContext.Set<GroupUser>().Add(seededGroupUser16);
        dataContext.Set<GroupUser>().Add(seededGroupUser17);
        dataContext.Set<GroupUser>().Add(seededGroupUser18);
        dataContext.Set<GroupUser>().Add(seededGroupUser19);
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
        var seededMessage2 = new Messages
        {
            GroupId = 1,
            Content = "Hey guys,thanks for adding me to the group!",
            CreatedAt = "12:54 PM",
            UserId = 2,

        };
        var seededMessage3 = new Messages
        {
            GroupId = 1,
            Content = "What's up! Goodluck to everyone this semester!",
            CreatedAt = "12:55 PM",
            UserId = 3,

        };
        var seededMessage4 = new Messages
        {
            GroupId = 2,
            Content = "Hey guys is someone making a flashcard set for the first exam?",
            CreatedAt = "3:05 PM",
            UserId = 5,

        };
        var seededMessage5 = new Messages
        {
            GroupId = 2,
            Content = "Yup I just finished it, go check it out.",
            CreatedAt = "3:14 PM",
            UserId = 6,

        };
       
        var seededMessage7 = new Messages
        {
            GroupId = 2,
            Content = "Thanks John!",
            CreatedAt = "3:20 PM",
            UserId = 7,

        };

        dataContext.Set<Messages>().Add(seededMessage1);
        dataContext.Set<Messages>().Add(seededMessage2);
        dataContext.Set<Messages>().Add(seededMessage3);
        dataContext.Set<Messages>().Add(seededMessage4);
        dataContext.Set<Messages>().Add(seededMessage5);
      
        dataContext.Set<Messages>().Add(seededMessage7);
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
            UserId = 1
        };
        var seededFlashCardSet2 = new FlashCardSets
        {
            SetName = "Exam 1 Flashcards",
            GroupId = 2,
            UserId = 6,
        };
        var seededFlashCardSet3 = new FlashCardSets
        {
            SetName = "Exam 2 Flashcards",
            GroupId = 2,
            UserId = 3,
        };
        var seededFlashCardSet4 = new FlashCardSets
        {
            SetName = "Exam 3 Flashcards",
            GroupId = 2,
            UserId = 3,
        };
        var seededFlashCardSet5 = new FlashCardSets
        {
            SetName = "Exam 4 Flashcards",
            GroupId = 2,
            UserId = 3,
        };

        dataContext.Set<FlashCardSets>().Add(seededFlashCardSet1);
        dataContext.Set<FlashCardSets>().Add(seededFlashCardSet2);
        dataContext.Set<FlashCardSets>().Add(seededFlashCardSet3);
        dataContext.Set<FlashCardSets>().Add(seededFlashCardSet4);
        dataContext.Set<FlashCardSets>().Add(seededFlashCardSet5);
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
        var seededFlashCard2 = new FlashCards
        {
            FlashCardSetId = 2,
            Question = "What was the Sugar Act?",
            Answer = "Placed taxes on imported goods that was not taxed before",
        };
        var seededFlashCard3 = new FlashCards
        {
            FlashCardSetId = 2,
            Question = "What was the Stamp Act?",
            Answer = "Stamp act put taxes on documents and printed items",
        };
        var seededFlashCard4 = new FlashCards
        {
            FlashCardSetId = 2,
            Question = "What was Name of British General that decided on an attack on Breed's Hill?",
            Answer = "Gage",
        };
        var seededFlashCard5 = new FlashCards
        {
            FlashCardSetId = 2,
            Question = "Name the individuals were loyal to the independence of the colonies?",
            Answer = "Patriots",
        };

        dataContext.Set<FlashCards>().Add(seededFlashCard1);
        dataContext.Set<FlashCards>().Add(seededFlashCard2);
        dataContext.Set<FlashCards>().Add(seededFlashCard3);
        dataContext.Set<FlashCards>().Add(seededFlashCard4);
        dataContext.Set<FlashCards>().Add(seededFlashCard5);
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
            UserId= 1
        };
        var seededTest2 = new Tests
        {
            TestName = "Exam 1 Practice Test",
            GroupId = 2,
            UserId = 3
        };
        var seededTest3 = new Tests
        {
            TestName = "Exam 2 Practice Test",
            GroupId = 2,
            UserId = 3
        };
        var seededTest4 = new Tests
        {
            TestName = "Exam 3 Practice Test",
            GroupId = 2,
            UserId = 3
        };
        var seededTest5 = new Tests
        {
            TestName = "Exam 4 Practice Test",
            GroupId = 2,
            UserId = 3
        };
        dataContext.Set<Tests>().Add(seededTest1);
        dataContext.Set<Tests>().Add(seededTest2);
        dataContext.Set<Tests>().Add(seededTest3);
        dataContext.Set<Tests>().Add(seededTest4);
        dataContext.Set<Tests>().Add(seededTest5);

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
        var seededTestQuestion2 = new TestQuestions
        {
            TestId = 2,
            Question = "What placed taxes on imported goods that was not taxed before ",
            Answer = "The Sugar Act",
        };
        var seededTestQuestion3 = new TestQuestions
        {
            TestId = 2,
            Question = "What put taxes on documents and printed items",
            Answer = "The Stamp Act",
        };
        var seededTestQuestion4 = new TestQuestions
        {
            TestId = 2,
            Question = "What was Name of British General that decided on an attack on Breed's Hill?",
            Answer = "Gage",
        };
        var seededTestQuestion5 = new TestQuestions
        {
            TestId = 2,
            Question = "Name the individuals were loyal to the independence of the colonies?",
            Answer = "Patriots",
        };
        dataContext.Set<TestQuestions>().Add(seededTestQuestion1);
        dataContext.Set<TestQuestions>().Add(seededTestQuestion2);
        dataContext.Set<TestQuestions>().Add(seededTestQuestion3);
        dataContext.Set<TestQuestions>().Add(seededTestQuestion4);
        dataContext.Set<TestQuestions>().Add(seededTestQuestion5);
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
            UserId = 1,
           
            
        };
        var seededAssignment2 = new Assignments
        {
            GroupId = 2,
            AssignmentName = "Exam 1",
            UserId = 4,


        };
        var seededAssignment3 = new Assignments
        {
            GroupId = 2,
            AssignmentName = "Exam 2",
            UserId = 3,

        };
        var seededAssignment4 = new Assignments
        {
            GroupId = 2,
            AssignmentName = "Exam 3",
            UserId = 3,

       
        };
        var seededAssignment5 = new Assignments
        {
            GroupId = 2,
            AssignmentName = "Exam 4",
            UserId = 3,


        };

        dataContext.Set<Assignments>().Add(seededAssignment1);
        dataContext.Set<Assignments>().Add(seededAssignment2);
        dataContext.Set<Assignments>().Add(seededAssignment3);
        dataContext.Set<Assignments>().Add(seededAssignment4);
        dataContext.Set<Assignments>().Add(seededAssignment5);
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
            Grades = 100,
            userId=1,
            userName= "LoganStewart",
        };
        var seededAssignmentGrade2 = new AssignmentGrade
        {
            AssignmentId = 2,
            Grades = 91,
            userId = 2,
            userName = "LukeNorton",
        };
        var seededAssignmentGrade3 = new AssignmentGrade
        {
            AssignmentId = 2,
            Grades = 85,
            userId = 3,
            userName = "RobertTurner",
        };
        var seededAssignmentGrade4 = new AssignmentGrade
        {
            AssignmentId = 2,
            Grades = 60,
            userId = 5,
            userName = "JohhnyBoy",
        };
        var seededAssignmentGrade5 = new AssignmentGrade
        {
            AssignmentId = 2,
            Grades = 72,
            userId = 6,
            userName = "RichardMeyers",
        };
        var seededAssignmentGrade6 = new AssignmentGrade
        {
            AssignmentId = 2,
            Grades = 93,
            userId = 7,
            userName = "OliviaBurnette",
        };
        var seededAssignmentGrade7 = new AssignmentGrade
        {
            AssignmentId = 3,
            Grades = 98,
            userId = 7,
            userName = "OliviaBurnette",
        };
        var seededAssignmentGrade8 = new AssignmentGrade
        {
            AssignmentId = 3,
            Grades = 88,
            userId = 6,
            userName = "RichardMeyers",
        };
        var seededAssignmentGrade9 = new AssignmentGrade
        {
            AssignmentId = 3,
            Grades = 92,
            userId = 5,
            userName = "JohhnyBoy",
        };
        var seededAssignmentGrade10 = new AssignmentGrade
        {
            AssignmentId = 3,
            Grades = 91,
            userId = 3,
            userName = "RobertTurner",
        };

        dataContext.Set<AssignmentGrade>().Add(seededAssignmentGrade1);
        dataContext.Set<AssignmentGrade>().Add(seededAssignmentGrade2);
        dataContext.Set<AssignmentGrade>().Add(seededAssignmentGrade3);
        dataContext.Set<AssignmentGrade>().Add(seededAssignmentGrade4);
        dataContext.Set<AssignmentGrade>().Add(seededAssignmentGrade5);
        dataContext.Set<AssignmentGrade>().Add(seededAssignmentGrade6);
        dataContext.Set<AssignmentGrade>().Add(seededAssignmentGrade7);
        dataContext.Set<AssignmentGrade>().Add(seededAssignmentGrade8);
        dataContext.Set<AssignmentGrade>().Add(seededAssignmentGrade9);
        dataContext.Set<AssignmentGrade>().Add(seededAssignmentGrade10);
        dataContext.SaveChanges(); 
    }







    private static async Task SeedUsers(DataContext dataContext, UserManager<User> userManager)
    {
        var numUsers = dataContext.Users.Count();

        if (numUsers == 0)
        {
            var seededUser = new User
            {
                FirstName = "Logan",
                LastName = "Stewart",
                UserName = "LoganStewart",
                
            };

            await userManager.CreateAsync(seededUser, "Password");
            await dataContext.SaveChangesAsync();
        }
    }


private static async Task SeedsUsers(DataContext dataContext, UserManager<User> userManager)
{
    var numUsers = dataContext.Users.Count();


        var seededUser1 = new User
        {
            FirstName = "Luke",
            LastName = "Norton",
            UserName = "LukeNorton",
        };

        // Seed the second user
        var seededUser2 = new User
        {
            FirstName = "Robert",
            LastName = "Turner",
            UserName = "RobertTurner",
        };
        var seededUser3 = new User
        {
            FirstName = "Jonathan",
            LastName = "Wellmeyer",
            UserName = "JonathanWellmeyer",
        };
        var seededUser4 = new User
        {
            FirstName = "John",
            LastName = "Smith",
            UserName = "JohnnyBoy",
        };
        var seededUser5 = new User
        {
            FirstName = "Richard",
            LastName = "Meyers",
            UserName = "RichardMeyers",
        };

        var seededUser6 = new User
        {
            FirstName = "Olivia",
            LastName = "Burnette",
            UserName = "OliviaBurnette",
        };


        await userManager.CreateAsync(seededUser1, "Password");

        // Create the second user using UserManager
        await userManager.CreateAsync(seededUser2, "Password");
        await userManager.CreateAsync(seededUser3, "Password");
        await userManager.CreateAsync(seededUser4, "Password");
        await userManager.CreateAsync(seededUser5, "Password");
        await userManager.CreateAsync(seededUser6, "Password");
        await dataContext.SaveChangesAsync();
    
   }
}
