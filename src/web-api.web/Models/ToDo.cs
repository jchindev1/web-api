namespace web_api.web.Models
{
    public class ToDo
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public bool IsCompleted { get; set; }
        public ToDo(int id, string? title, bool isCompleted)
        {
            Id = id;
            Title = title;
            IsCompleted = isCompleted;
        }
        public ToDo() { } // Parameterless constructor for deserialization
        public string? Description { get; set; }

    }
}
