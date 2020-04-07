using System;
using System.Net.Http;

namespace BookMark.Client.Utils {
	public class HttpService {
		public HttpClient client { get; }
		public HttpService() {
			if (client == null) {
				string base_url = Environment.GetEnvironmentVariable("RestApiUrl");
				if (base_url == null) {
					base_url = "http://localhost:5000";
				}
				client = new HttpClient();
				client.BaseAddress = new Uri(base_url);
			}
		}
	}
}