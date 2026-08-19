namespace E_Commerce.Services.AuthAPI.Models.DTOs
{
    public class ResultDto
    {
        public object? Result { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
