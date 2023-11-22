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

        builder.HasOne(q => q.Assignments)
          .WithMany(t => t.Grades)
          .HasForeignKey(q => q.AssignmentId);
        builder.HasOne(x => x.User); 

    }
}