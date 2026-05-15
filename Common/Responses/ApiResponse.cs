namespace Mec126.Common.Responses
{
	public class ApiResponse<T>
	{
		public bool Success { get; init; }
		public string? Message { get; init; }
		public T? Data { get; init; }
		public IEnumerable<string>? Errors { get; init; }

		public static ApiResponse<T> Ok(T data, string? message = null) =>
			new() { Success = true, Data = data, Message = message };

		public static ApiResponse<T> Fail(string message, IEnumerable<string>? errors = null) =>
			new() { Success = false, Message = message, Errors = errors };
	}

	public class ApiResponse : ApiResponse<object>
	{
		public static ApiResponse Ok(string? message = null) =>
			new() { Success = true, Message = message };

		public static new ApiResponse Fail(string message, IEnumerable<string>? errors = null) =>
			new() { Success = false, Message = message, Errors = errors };
	}
}
