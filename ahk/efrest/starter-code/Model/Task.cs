namespace api.Model
{
    // DO NOT CHANGE ANYTHING
    // NE VALTOZTASS MEG SEMMIT
    public class Task
    {
        public Task(int id, string title, bool done, string status)
        {
            Id = id;
            Title = title;
            Done = done;
            Status = status;
        }

        public int Id { get; }
        public string Title { get; }
        public bool Done { get; }
        public string Status { get; }
    }
}
