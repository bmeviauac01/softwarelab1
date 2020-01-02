using System.Collections.Generic;

namespace api.Controllers.Dto
{
    // DO NOT CHANGE ANYTHING
    // NE VALTOZTASS MEG SEMMIT
    public class PagedTaskList
    {
        public PagedTaskList(IReadOnlyCollection<Model.Task> items, int count, int? nextId, string nextUrl)
        {
            this.Items = items;
            this.Count = count;
            this.NextId = nextId;
            this.NextUrl = nextUrl;
        }
        public IReadOnlyCollection<Model.Task> Items { get; }
        public int Count { get; }
        public int? NextId { get; }
        public string NextUrl { get; }
    }
}
