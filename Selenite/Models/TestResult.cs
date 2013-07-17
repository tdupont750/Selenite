namespace Selenite.Models
{
    public class TestResult
    {
        public ResultStatus Status { get; set; }
        public string CollectionName { get; set; }
        public string TestName { get; set; }
        public string Url { get; set; }
        public string TraceResult { get; set; }
    }
}
