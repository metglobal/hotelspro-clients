using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Coral {
    public static class CoralBookingsClient {
        private const string ENDPOINT = "bookings/";
        
        public static string GetBookings(this CoralClient client, CoralBookingsParameters parameters) {
            return client.MakeRequest(
                CoralBookingsClient.ENDPOINT, HttpMethod.Get, query: parameters.GetQueryString()
            ).Result;
        }

        public static async Task<string> BookingsAsync(this CoralClient client, CoralBookingsParameters parameters) {
            return await client.MakeRequest(
                CoralBookingsClient.ENDPOINT, HttpMethod.Get, query: parameters.GetQueryString()
            );
        }
    }

    public abstract class CoralBookingsParameters {
        public abstract string GetQueryString();
    }

    public class CoralBookingsSingleParameters : CoralBookingsParameters {
        public string Code { get; set; }

        public override string GetQueryString() { return this.Code; }
    }

    public class CoralBookingsListParameters : CoralBookingsParameters {
        public string ProviderCode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public CoralBookingsListParameters() {
            this.ProviderCode = null;
            this.FromDate = DateTime.MaxValue;
            this.ToDate = DateTime.FromFileTimeUtc(0);
        }

        public override string GetQueryString() {
            List<string> parameters = new List<string>();
            if (this.ProviderCode != null) 
                parameters.Add("provider_code=" + this.ProviderCode);
            if (this.FromDate != DateTime.MaxValue) 
                parameters.Add("from_date=" + this.FromDate.ToString("yyyy-MM-dd"));
            if (this.ToDate != DateTime.FromFileTimeUtc(0)) 
                parameters.Add("to_date=" + this.ToDate.ToString("yyyy-MM-dd"));
            return String.Join("&", parameters);
        }
    }
}
