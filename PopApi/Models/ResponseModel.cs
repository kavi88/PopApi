namespace PopApi.Models
{
    public class ResponseModel<T>
    {
        public ResponseModel(bool isSuccess, T data)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = "";
        }

        public ResponseModel(bool isSuccess, string errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }

        public T Data { get; set; }
    }
}
