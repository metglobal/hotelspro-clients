using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Coral {
    public static class CoralHotelAvailabilityClient {
        private const string ENDPOINT = "hotel-availability/";
        
        public static string HotelAvailability(this CoralClient client, CoralHotelAvailabilityParameters parameters) {
            return client.MakeRequest(
                CoralHotelAvailabilityClient.ENDPOINT, HttpMethod.Get, query: parameters.GetQueryString()
            ).Result;
        }

        public static async Task<string> HotelAvailabilityAsync(this CoralClient client, CoralHotelAvailabilityParameters parameters) {
            return await client.MakeRequest(
                CoralHotelAvailabilityClient.ENDPOINT, HttpMethod.Get, query: parameters.GetQueryString()
            );
        }
    }

    public class CoralHotelAvailabilityParameters {
        public string SearchCode { get; set; }
        public string HotelCode { get; set; }

        public string GetQueryString() {
            return String.Format("?search_code={0}&hotel_code={1}", this.SearchCode, this.HotelCode);
        }
    }
}
