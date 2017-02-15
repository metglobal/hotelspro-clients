using System.Net.Http;
using System.Threading.Tasks;

namespace Coral {
    public static class CoralProvisionClient {
        private const string ENDPOINT = "provision/";
        
        public static string Provision(this CoralClient client, CoralProvisionParameters parameters) {
            return client.MakeRequest(
                CoralProvisionClient.ENDPOINT, HttpMethod.Post, query: parameters.GetQueryString()
            ).Result;
        }

        public static async Task<string> ProvisionAsync(this CoralClient client, CoralProvisionParameters parameters) {
            return await client.MakeRequest(
                CoralProvisionClient.ENDPOINT, HttpMethod.Post, query: parameters.GetQueryString()
            );
        }
    }

    public class CoralProvisionParameters {
        public string ProductCode { get; set; }

        public string GetQueryString() {
            return this.ProductCode;
        }
    }
}
