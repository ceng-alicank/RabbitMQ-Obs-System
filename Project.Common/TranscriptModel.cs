namespace Project.Common
{
    public class TranscriptModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }
        public TranscriptModel()
        {
            Id = Guid.NewGuid();
        }
    }
}