using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Http;
using System;
namespace LearningStarter.Entities;

public class AssignmentGrade
{
    public int Id { get; set; }
    public int AssignmentId { get; set; }
    public int Grades { get; set; }
    public Assignments Assignments { get; set; }
    public int userId { get;set;}
    public string userName { get; set; }
    public User User { get; set; }
}

public class AssignmentGradeCreateDto
{   
    
    public int Grades { get; set; }
    public int userId { get; set; }
    public string userName { get; set; }
}

public class AssignmentGradeGetDto
{
    public int Id { get; set; }
    public int AssignmentId {get; set;}
    public int Grades { get; set; }
    public int userId { get; set; }
    public string userName { get; set; }
}


public class AssignmentGradeUpdateDto
{
    public int Grades { get; set; }
    public int userId { get; set; }
    public string userName { get; set; }

}


public class AssignmentGradeEntityTypeConfiguration : IEntityTypeConfiguration<AssignmentGrade>
{
    public void Configure(EntityTypeBuilder<AssignmentGrade> builder)
    {
        builder.ToTable("AssignmentGrade");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.userId)
            .OnDelete(DeleteBehavior.Restrict); // Adjust the cascade action if needed
        builder.HasOne(x => x.Assignments)  // Add this line for the Assignments relationship
           .WithMany(a => a.Grades)
           .HasForeignKey(x => x.AssignmentId)
           .OnDelete(DeleteBehavior.Cascade);

    }
}