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
        public int CreatorId {  get; set; }
        public string Name { get; set; }
    }
    public class TestsCreateDto
    {
        public int GroupId { get; set; }
        public int CreatorId { get; set; }
        public string Name { get; set; }
    }

    public class TestsUpdateDto
    {
        public int GroupId { get; set; }
        public int CreatorId { get; set; }
        public string Name { get; set; }
    }
    public class TestsGetDto
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int CreatorId { get; set; }
        public string Name { get; set; }
    }
    public class TestsEntityTypeConfiguration : IEntityTypeConfiguration<Tests>
    {
        public void Configure(EntityTypeBuilder<Tests> builder)
        {
            builder.ToTable("Tests");
        }
    }

}
