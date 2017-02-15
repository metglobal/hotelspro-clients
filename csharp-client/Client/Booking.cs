using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Coral {
    ////////////
    /* CLIENT */
    ////////////
    public static class CoralBookingClient {
        private const string ENDPOINT = "book/";

        public static string Booking(this CoralClient client, CoralBookingParameters parameters) {
            return client.MakeRequest(
                CoralBookingClient.ENDPOINT, HttpMethod.Post, 
                query: parameters.GetQueryString(), body: parameters.GetPostBody()
            ).Result;
        }

        public static async Task<string> BookingAsync(this CoralClient client, CoralBookingParameters parameters) {
            return await client.MakeRequest(
                CoralBookingClient.ENDPOINT, HttpMethod.Post, 
                query: parameters.GetQueryString(), body: parameters.GetPostBody()
            );
        }
    }

    public abstract class CoralBookingParameters {
        public string Code { get; set; }
        public List<string> Name { get; set; }

        public virtual string GetQueryString() {
            return this.Code;
        }

        public virtual ByteArrayContent GetPostBody() {
            var names = new List<KeyValuePair<string, string>>();
            foreach (var name in this.Name) {
                names.Add(new KeyValuePair<string, string>("name", name));
            }
            return new FormUrlEncodedContent(names.ToArray());
        }
    }

    public class CoralPrepaidBookingParameters : CoralBookingParameters { }

    public class CoralPayAtHotelBookingParameters : CoralBookingParameters {
        public string CardCVC { get; set; }
        public DateTime CardExpiration { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        public override ByteArrayContent GetPostBody() {
            // cannot use a Dictionary<string, string> here, as there will be multiple values with key "name"
            var parameters = new List<KeyValuePair<string, string>>();
            foreach (var name in this.Name) {
                parameters.Add(new KeyValuePair<string, string>("name", name));
            }
            parameters.Add(new KeyValuePair<string, string>("card_cvc", this.CardCVC));
            parameters.Add(new KeyValuePair<string, string>("card_expiration", this.CardExpiration.ToString("yyyy-MM")));
            parameters.Add(new KeyValuePair<string, string>("card_holder_name", this.CardHolderName));
            parameters.Add(new KeyValuePair<string, string>("card_number", this.CardNumber));
            parameters.Add(new KeyValuePair<string, string>("card_type", this.CardType));
            parameters.Add(new KeyValuePair<string, string>("email", this.EmailAddress));
            parameters.Add(new KeyValuePair<string, string>("phone_number", PhoneNumber));
            
            return new FormUrlEncodedContent(parameters.ToArray());
        }
    }
}
