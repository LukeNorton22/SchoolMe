namespace LearningStarter.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public Group Group { get; set; }
        public User Sender {  get; set; }
    }
}
