using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Http;
using System;
using System.Text.Json.Serialization;

namespace LearningStarter.Entities;

    public class Assignments
    {
    public int Id { get; set; }
    public string AssignmentName { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; }
    
    public List<AssignmentGrade> Grades { get; set; }
    }
public class AssignmentsCreateDto
{
    public string AssignmentName { get; set; }

}

public class AssignmentsGetDto
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string AssignmentName { get; set; }
    public List<AssignmentGradeGetDto> Grades { get; set; }
}

public class AssignmentsUpdateDto
{
    public string AssignmentName { get; set; }

}

public class AssignmentEntityTypeConfiguration : IEntityTypeConfiguration<Assignments>
{
    public void Configure(EntityTypeBuilder<Assignments> builder)
    {
        builder.ToTable("Assignments");

        builder.HasOne(x => x.Group).WithMany(x => x.Assignments);
        builder.HasMany(x => x.Grades).WithOne(x => x.Assignments);

    }
}

