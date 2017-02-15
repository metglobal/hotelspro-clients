using System.Net.Http;
using System.Threading.Tasks;

namespace Coral {
    public static class CoralAvailabilityClient {
        private const string ENDPOINT = "availability/";
        
        public static string Availability(this CoralClient client, CoralAvailabilityParameters parameters) {
            return client.MakeRequest(
                CoralAvailabilityClient.ENDPOINT, HttpMethod.Get, query: parameters.GetQueryString()
            ).Result;
        }

        public static async Task<string> AvailabilityAsync(this CoralClient client, CoralAvailabilityParameters parameters) {
            return await client.MakeRequest(
                CoralAvailabilityClient.ENDPOINT, HttpMethod.Get, query: parameters.GetQueryString()
            );
        }
    }

    public class CoralAvailabilityParameters {
        public string ProductCode { get; set; }

        public string GetQueryString() {
            return this.ProductCode;
        }
    }
}
