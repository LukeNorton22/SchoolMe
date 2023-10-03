using LearningStarter.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LearningStarter.Entities
{
    public class FlashCardSetsFlashCard
    {
        public int Id { get; set; }
        public int FlashCardSetsId { get; set; }
        public FlashCardSets FlashCardSets { get; set; }

        public int FlashCardsId { get; set; }
        public FlashCards FlashCards{ get; set; }
    }
    public class FlashCardSetsFlashCardGetDto
    {
        public int Id { get; set; }
        public string Question { get; set; }

        public string Answer { get; set; }
    }
}

    public class FlashCardSetsFlashCardEntityTypeConfiguration : IEntityTypeConfiguration<FlashCardSetsFlashCard>
    {
        public void Configure(EntityTypeBuilder<FlashCardSetsFlashCard> builder)
        {
            builder.ToTable("FlashCardSetsFlashCard");

            builder.HasOne(x => x.FlashCardSets)
                .WithMany(x => x.FlashCards);

            builder.HasOne(x => x.FlashCards)
                .WithMany();
        }
    }


