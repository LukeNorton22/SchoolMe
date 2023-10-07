using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Http;
using System;
namespace LearningStarter.Entities
{
    public class Tests
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public string TestName { get; set; }
        public List<TestQuestions> Questions { get; set; }
        
    }
    public class TestsCreateDto
    {
        
        public string TestName { get; set; }

    }

    public class TestsUpdateDto
    {
        public string TestName { get; set; }
    }
    public class TestsGetDto
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string TestName { get; set; }
        public List<TestQuestionsGetDto> Questions { get; set; }
    }
    public class TestsEntityTypeConfiguration : IEntityTypeConfiguration<Tests>
    {
        public void Configure(EntityTypeBuilder<Tests> builder)
        {
            builder.ToTable("Tests");
            builder.HasOne(x => x.Group).WithMany(x => x.Test);
            builder.HasMany(x => x.Questions).WithOne(x => x.Tests);
        }
    }

}
