using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BookMark.Client.Utils {
	public static class SessionService {
		public static void ToJson(this ISession session, string key, object value) {
			session.SetString(key, JsonConvert.SerializeObject(value));
		}
		public static T FromJson<T>(this ISession session, string key) {
			var value = session.GetString(key);
			return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
		}
	}
}