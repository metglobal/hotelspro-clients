using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Coral {
    public static class CoralSearchClient {
        private const string ENDPOINT = "search/";
        
        public static string Search(this CoralClient client, CoralSearchParameters parameters) {
            return client.MakeRequest(
                CoralSearchClient.ENDPOINT, HttpMethod.Post, 
                query: parameters.GetQueryString(), body: parameters.GetPostBody()
            ).Result;
        }

        public static async Task<string> SearchAsync(this CoralClient client, CoralSearchParameters parameters) {
            return await client.MakeRequest(
                CoralSearchClient.ENDPOINT, HttpMethod.Post, 
                query: parameters.GetQueryString(), body: parameters.GetPostBody()
            );
        }
    }

    public class CoralSearchParameters {
        public List<string> Pax { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string ClientNationality { get; set; }
        public string Currency { get; set; }

        public virtual string GetQueryString() {
            return String.Format(
                "?pax={0}&checkin={1:yyyy-MM-dd}&checkout={2:yyyy-MM-dd}&client_nationality={3}&currency={4}",
                String.Join("&pax=", this.Pax), this.CheckIn, this.CheckOut, this.ClientNationality, this.Currency
            );
        }

        public virtual ByteArrayContent GetPostBody() { return new StringContent(""); }
    }

    public class CoralHotelSearchParameters : CoralSearchParameters {
        public List<string> HotelCodes { get; set; }

        public override ByteArrayContent GetPostBody() {
            return new FormUrlEncodedContent(
                new Dictionary<string, string>() {
                    { "hotel_code", String.Join(",", this.HotelCodes) }
                }
            );
        }
    }

    public class CoralDestinationSearchParameters : CoralSearchParameters {
        public string DestinationCode { get; set; }

        public override string GetQueryString() {
            return base.GetQueryString()
                + String.Format("&destination_code={0}", this.DestinationCode);
        }
    }

    public class CoralLocationSearchParameters : CoralSearchParameters {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Radius { get; set; }

        public override string GetQueryString() {
            return base.GetQueryString()
                + String.Format("&lat={0}&lon={1}&radius={2}", this.Latitude, this.Longitude, this.Radius);
        }
    }
}
