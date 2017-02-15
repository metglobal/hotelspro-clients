using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Coral {
    public class CoralClient {
        private AuthenticationHeaderValue auth;
        protected string BaseURL { get; set; } 

        public CoralClient(string username, string password, string apiServer="https://api-test.hotelspro.com/api/v2/") {
            var authBytes = Encoding.ASCII.GetBytes(username + ":" + password);
            this.auth = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));
            this.BaseURL = apiServer;
        }

        public async Task<string> MakeRequest(string endpoint, HttpMethod method, string query="", ByteArrayContent body=null) {            
            using (var client = new HttpClient() {
                BaseAddress=new Uri(this.BaseURL)
            }) {
                client.DefaultRequestHeaders.Authorization = this.auth;
                HttpResponseMessage response = null;
                
                if (method == HttpMethod.Post) {
                    response = await client.PostAsync(endpoint + query, body);
                } else if (method == HttpMethod.Get) {
                    response = await client.GetAsync(endpoint + query);
                }
                
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
