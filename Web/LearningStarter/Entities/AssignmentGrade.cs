using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Http;
using System;
namespace LearningStarter.Entities;

public class AssignmentGrade 
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Grade { get; set; }
    public double AverageGrade { get; set; }

}

public class AssignmentGradeCreateDto
{
    public string Name { get; set; }
    public double Grade { get; set; }
    public double AverageGrade { get; set; }

}

public class AssignmentGradeGetDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Grade { get; set; }
    public double AverageGrade { get; set; }
}

public class AssignmentGradeUpdateDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public double Grade { get; set; }
    public double AverageGrade { get; set;}

}

public class AssignmentGradeEntityTypeConfiguration : IEntityTypeConfiguration<AssignmentGrade>
{
    public void Configure(EntityTypeBuilder<AssignmentGrade> builder)
    {
        builder.ToTable("AssignmentGrade");
    }
}