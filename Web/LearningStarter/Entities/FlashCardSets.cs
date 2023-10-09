using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Http;
using System;
namespace LearningStarter.Entities
{
    public class FlashCardSets
    {
        public int Id { get; set; }
        public string SetName { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public List<FlashCards> FlashCards { get; set; }
    }

    public class FlashCardSetsGetDto
    {
        public int Id { get; set; }
       
        public int GroupId { get; set; }
        public string SetName { get; set; }
        public List<FlashCardsGetDto> FlashCards { get; set; }
    }
    public class FlashCardSetsCreateDto
    {
        public string SetName { get; set; }
    }
    public class FlashCardSetsUpdateDto
    {
        public string SetName { get; set; }
    }

    public class FlashCardSetsEntityTypeConfiguration : IEntityTypeConfiguration<FlashCardSets>
    {
        public void Configure(EntityTypeBuilder<FlashCardSets> builder)
        {
            builder.ToTable("FlashCardSets");

            builder.HasMany(x => x.FlashCards).WithOne(x => x.FlashCardSet);
        }
    }

}



