namespace Vids.Model
{
    public enum ResultStatusValues
    {
        OK,
        Error,
    }

    public class Result
    {
        public ResultStatusValues Status { get; set; } = ResultStatusValues.OK;
        public int Code { get; set; } = 0;
        public string Message { get; set; } = "Success";
    }

    public class Result<T> : Result
    {
        public T? Data { get; set; }
        public int Count { get; set; } = 0;
    }

    public class PageResult<T>
    {
        public T? Data { get; set; }
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 0;
    }
}
