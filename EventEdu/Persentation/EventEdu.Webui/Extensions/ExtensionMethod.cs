namespace EventEdu.Webui.Extensions
{
	public static class ExtentionMethod
	{
		public static string GetReturnUrl(this HttpRequest httpRequest)
		{
			string? returnUrl = httpRequest.Headers["Referer"];

			if (returnUrl == null)
				return "/";

			return returnUrl;
		}
	}
}
