namespace ExpenseTracker.Application.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Data { get; } 
        public string? ErrorMessage { get; }
        public ErrorType ErrorType { get; }

        private Result(bool isSuccess, T data, string? errorMessage, ErrorType errorType)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
            ErrorType = errorType;
        }

        public static Result<T> Success (T data) => new Result<T>(true, data, null, ErrorType.None);
        public static Result<T> Failure(string errorMessage, ErrorType errorType = ErrorType.Failure) 
                         => new Result<T>(false, default, errorMessage, errorType);
        

    }
}
