using System;
using System.Net.Http;

namespace BookMark.Client.Utils {
	public class HttpService {
		private static readonly HttpService service = new HttpService();
		private readonly HttpClientHandler handler;
		private readonly HttpClient client;
		public static HttpService Service {
			get { return service; }
		}
		public static HttpClient Client {
			get { return Service.client; }
		}
		private HttpService() {
			if (handler == null) {
				handler = new HttpClientHandler() {
					ServerCertificateCustomValidationCallback = 
						(sender, cert, chain, ssl_policy_errors) => {
							// Console.WriteLine($"Sender: {sender}");
							// Console.WriteLine($"Certificate: {cert}");
							// Console.WriteLine($"Chain: {chain}");
							// Console.WriteLine($"SSL Policy Errors: {ssl_policy_errors}");
							return true;
						}
				};
			}
			if (client == null) {
				string base_url = Environment.GetEnvironmentVariable("RestApiUrl");
				if (base_url == null) {
					base_url = "https://localhost:5001";
				}
				client = new HttpClient(handler);
				client.BaseAddress = new Uri(base_url);
			}
		}
	}
}