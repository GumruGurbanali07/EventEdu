using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Services
{
	public class StringLocalizerService
	{
		private readonly IStringLocalizer _stringLocalizer;
		public StringLocalizerService(IStringLocalizerFactory stringLocalizerFactory)
		{
			_stringLocalizer = stringLocalizerFactory.Create("SharedResources", "EventEdu.Webui");
		}
		public string GetValue(string key)
		{
			return _stringLocalizer.GetString(key);
		}
	}
}
