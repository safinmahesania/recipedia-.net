namespace recipedia.Models.API
{
    public class APIResponse
    {
        public Object? Data { set; get; } = null;
        public int StatusCode { set; get; }
        public bool IsSuccess { set; get; }
        public String Message { set; get; }
        public IEnumerable<Object>? Errors { set; get; } = null;
    }
}
