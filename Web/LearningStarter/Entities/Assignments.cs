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
    public string Name { get; set; }
    public int GroupId { get; set; }
    public int AverageGrade { get; set; }
    public Group Group { get; set; }
    [JsonIgnore]
    public List<AssignmentGrade> Grade { get; set; }
    }
public class AssignmentsCreateDto
{
    public string Name { get; set; }

}

public class AssignmentsGetDto
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public int AverageGrade { get; set; }
    public string Name { get; set; }
    public List<AssignmentGradeGetDto> Grade { get; set; }
}

public class AssignmentsUpdateDto
{
    public string Name { get; set; }

}

public class AssignmentEntityTypeConfiguration : IEntityTypeConfiguration<Assignments>
{
    public void Configure(EntityTypeBuilder<Assignments> builder)
    {
        builder.ToTable("Assignments");

        builder.HasMany(x => x.Grade).WithOne(x => x.Assignments);

        builder.HasOne(x => x.Group)
               .WithMany(x => x.Assignments);
    }
}

