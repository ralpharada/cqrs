namespace Tellus.Core.Events
{
    public class ResultEvent : IEvent
    {
        public bool Success { get; }
        public object Data { get; } = null!;
        public long? TotalRows { get; }
        public Guid? Id { get; } = null!;
        public ResultEvent() { }
        public ResultEvent(bool _success, object _data, long? totalRows = null, Guid? id = null)
        {
            Success = _success;
            Data = _data;
            TotalRows = totalRows;
            Id = id;
        }
    }
}
