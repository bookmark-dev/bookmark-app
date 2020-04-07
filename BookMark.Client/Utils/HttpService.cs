using System;
using System.Net.Http;

namespace BookMark.Client.Utils {
	public class HttpService {
		private readonly HttpClientHandler handler;
		public HttpClient client { get; }
		public HttpService() {
			if (handler == null) {
				handler = new HttpClientHandler() {
					ServerCertificateCustomValidationCallback = 
						(sender, cert, chain, ssl_policy_errors) => {
							/* TODO: THIS IS A SECURITY PROBLEM! */
							return true;
						}
				};
			}
			if (client == null) {
				string base_url = Environment.GetEnvironmentVariable("RestApiUrl");
				if (base_url == null) {
					base_url = "http://localhost:5000";
				}
				client = new HttpClient(handler);
				client.BaseAddress = new Uri(base_url);
			}
		}
	}
}