using System.Net.Http;
using System.Threading.Tasks;

namespace Coral {
    public static class CoralCancellationClient {
        private const string ENDPOINT = "cancel/";
        
        public static string Cancel(this CoralClient client, CoralCancellationParameters parameters) {
            return client.MakeRequest(
                CoralCancellationClient.ENDPOINT, HttpMethod.Post, query: parameters.GetQueryString()
            ).Result;
        }

        public static async Task<string> CancelAsync(this CoralClient client, CoralCancellationParameters parameters) {
            return await client.MakeRequest(
                CoralCancellationClient.ENDPOINT, HttpMethod.Post, query: parameters.GetQueryString()
            );
        }
    }

    public class CoralCancellationParameters {
        public string BookingCode { get; set; }

        public string GetQueryString() {
            return this.BookingCode;
        }
    }
}
