using Mec126.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Mec126.Common.Extensions
{
	public static class ControllerExtensions
	{
		public static ActionResult<ApiResponse<T>> ValidationFail<T>(this ControllerBase controller)
		{
			var errors = controller.ModelState.Values
				.SelectMany(v => v.Errors)
				.Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? "Invalid value." : e.ErrorMessage);

			return controller.BadRequest(ApiResponse<T>.Fail("Validation failed.", errors));
		}
	}
}
