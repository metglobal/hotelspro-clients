using System;
using System.Collections.Generic;
using Coral;
using Newtonsoft.Json;

namespace ConsoleApplication
{
    public class CoralClientTester {
        CoralClient Client { get; set; }

        public CoralClientTester(CoralClient client) {
            this.Client = client;
        }

        public string RunDestinationSearch() {
            /*
            '19064': 'Istanbul',
            '10588': 'Moscow',
            '10869': 'Copenhagen',
            '11260': 'London',
            '12d83': 'Amsterdam',
            '13035': 'Vienna',
            '137d9': 'Brussels',
            '142aa': 'Munich',
            '148dd': 'Frankfurt/Main',
            '14bc7': 'Berlin',
            '154f0': 'Dublin',
            '161cf': 'Paris',
            '18edc': 'Dubai',
            '19bbc': 'Prague',
            '1ac3a': 'Madrid',
            '1b3b6': 'Barcelona',
            '1bcde': 'Venice',
            '1c11e': 'Rome',
            '1c56f': 'Milan',
            '1c88f': 'Florence',
            '1d336': 'Singapore',
            '1dd8b': 'Miami',
            '1ddaf': 'Orlando',
            '1e5f6': 'Las Vegas',
            '1e812': 'New York'
            */
            string destinationCode = "11260";

            Console.WriteLine("Searching destination {0}...", destinationCode);
            var searchParameters = new CoralDestinationSearchParameters() {
                Pax=new List<string>(new string[] {"2,5"}),
                CheckIn=new DateTime(2017, 3, 1),
                CheckOut=new DateTime(2017, 3, 3),
                ClientNationality="US",
                Currency="USD",
                DestinationCode=destinationCode
            };
            
            return this.Client.Search(searchParameters);
        }

        public string RunHotelSearch() {
            List<string> hotelCodeList = new List<string>(
                new string[] {"114e0a", "103e12"}
            );

            Console.WriteLine("Searching hotels {0}...", String.Join(", ", hotelCodeList));
            var searchParameters = new CoralHotelSearchParameters() {
                Pax=new List<string>(new string[] {"2,5"}),
                CheckIn=new DateTime(2017, 3, 1),
                CheckOut=new DateTime(2017, 3, 3),
                ClientNationality="US",
                Currency="USD",
                HotelCodes=hotelCodeList
            };
            
            return this.Client.Search(searchParameters);
        }

        public string RunLocationSearch() {
            // Testing for Istanbul...
            string latitude = "41.0445";
            string longitude = "28.9941";
            string radius = "2000";  // in meters

            Console.WriteLine("Searching location ({0}, {1}) with radius {2} meters...", latitude, longitude, radius);
            var searchParameters = new CoralLocationSearchParameters() {
                Pax=new List<string>(new string[] {"2,5"}),
                CheckIn=new DateTime(2017, 3, 1),
                CheckOut=new DateTime(2017, 3, 3),
                ClientNationality="US",
                Currency="USD",
                Latitude=latitude,
                Longitude=longitude,
                Radius=radius
            };
            
            return this.Client.Search(searchParameters);
        }

        public string RunHotelAvailabilityCheck(string hotelCode, string searchCode) {
            Console.WriteLine("Making hotel availability request for hotel {0} in search {1}", hotelCode, searchCode);
            var hotelAvailabilityParameters = new CoralHotelAvailabilityParameters() {
                SearchCode=searchCode,
                HotelCode=hotelCode
            };
            
            return this.Client.HotelAvailability(hotelAvailabilityParameters);
        }

        public string RunAvailabilityCheck(string productCode) {
            Console.WriteLine("Making availability check for product: {0}", productCode);
            var availabilityParameters = new CoralAvailabilityParameters() {
                ProductCode=productCode
            };

            return this.Client.Availability(availabilityParameters);
        }

        public string RunProvisioning(string productCode) {
            Console.WriteLine("Making provisioning request for product: {0}", productCode);
            var provisioningParameters = new CoralProvisionParameters() {
                ProductCode=productCode
            };

            return this.Client.Provision(provisioningParameters);
        }

        public string RunPrepaidBooking(string code) {
            Console.WriteLine("Making prepaid booking request for product: {0}", code);
            var bookingParameters = new CoralPrepaidBookingParameters() {
                Name=new List<string>() { "1,Foobar,Baz,adult", "1,Gooder,Gas,adult", "1,Baadur,Gur,child,5" },
                Code=code
            };

            return this.Client.Booking(bookingParameters);
        }

        public string RunPayAtHotelBooking(string code) {
            Console.WriteLine("Making pay-at-hotel booking request for product: {0}", code);
            var bookingParameters = new CoralPayAtHotelBookingParameters() {
                Name=new List<string>() { "1,Foobar,Baz,adult", "1,Gooder,Gas,adult", "1,Baadur,Gur,child,5" },
                Code=code,
                CardCVC="XXX",
                CardExpiration=new System.DateTime(2025, 5, 30),
                CardHolderName="Foobar Baz",
                CardNumber="XXXXXXXXXXXXXXXX",
                CardType="Visa",
                EmailAddress="foobar@baz.com",
                PhoneNumber="+XXXXXXXXXXX",
            };

            return this.Client.Booking(bookingParameters);
        }

        public string RunGetBookingList() {
            var getBookingsParameters = new CoralBookingsListParameters();
            return this.Client.GetBookings(getBookingsParameters);
        }

        public string RunGetSingleBooking(string code) {
            var getBookingsParameters = new CoralBookingsSingleParameters() {
                Code=code
            };
            return this.Client.GetBookings(getBookingsParameters);
        }

        public string RunCancellation(string code) {
            Console.WriteLine("Making cancellation request for booking: {0}", code);
            var cancellationParameters = new CoralCancellationParameters() {
                BookingCode=code
            };
            return this.Client.Cancel(cancellationParameters);
        }

        /////////////////////////////////
        // FLOW TESTS
        //
        // Refer to https://api2.hotelspro.com/docs/dynamic_api/index.html#workflow
        // for more info on workflows.

        public string TestFlow3() {
            var searchResponse = this.RunHotelSearch();
            var searchResults = JsonConvert.DeserializeObject<dynamic>(searchResponse);
            if (searchResults["count"] == 0) {
                Console.WriteLine("No results were found.");
                Environment.Exit(1);
            }
            
            dynamic firstResult = searchResults["results"][0];
            string searchCode = searchResults["code"];
            string hotelCode = firstResult["hotel_code"];
            string productCode = firstResult["products"][0]["code"];

            var hotelAvailabilityResponse = this.RunHotelAvailabilityCheck(hotelCode, searchCode);
            var hotelAvailabilityResults = JsonConvert.DeserializeObject<dynamic>(hotelAvailabilityResponse);

            productCode = hotelAvailabilityResults["results"][0]["code"];

            var provisioningResponse = this.RunProvisioning(productCode);
            var provisioningResults = JsonConvert.DeserializeObject<dynamic>(provisioningResponse);

            string provisionCode = provisioningResults["code"];
            bool payAtHotel = provisioningResults["pay_at_hotel"];

            var bookingResponse = "";
            if (payAtHotel)
                bookingResponse = this.RunPayAtHotelBooking(provisionCode);
            else
                bookingResponse = this.RunPrepaidBooking(provisionCode);
            return bookingResponse;
        }

        public string TestFlow2() {
            // Search
            var searchResponse = this.RunDestinationSearch();
            var searchResults = JsonConvert.DeserializeObject<dynamic>(searchResponse);
            if (searchResults["count"] == 0) {
                Console.WriteLine("No results were found.");
                Environment.Exit(1);
            }
            dynamic firstResult = searchResults["results"][0];
            string searchCode = searchResults["code"];
            string hotelCode = firstResult["hotel_code"];
            string productCode = firstResult["products"][0]["code"];

            // Hotel Availability
            var hotelAvailabilityResponse = this.RunHotelAvailabilityCheck(hotelCode, searchCode);
            var hotelAvailabilityResults = JsonConvert.DeserializeObject<dynamic>(hotelAvailabilityResponse);
            productCode = hotelAvailabilityResults["results"][0]["code"];

            // Check Availability
            var checkAvailabilityResponse = this.RunAvailabilityCheck(productCode);
            var checkAvailabilityResults = JsonConvert.DeserializeObject<dynamic>(checkAvailabilityResponse);
            // results from the "Check Availability" endpoint are unused in further steps
            // it simply provides up-to-date information about the product

            // Provisioning
            var provisioningResponse = this.RunProvisioning(productCode);
            var provisioningResults = JsonConvert.DeserializeObject<dynamic>(provisioningResponse);
            string provisionCode = provisioningResults["code"];
            bool payAtHotel = provisioningResults["pay_at_hotel"];

            // Book
            var bookingResponse = "";
            if (payAtHotel)
                bookingResponse = this.RunPayAtHotelBooking(provisionCode);
            else
                bookingResponse = this.RunPrepaidBooking(provisionCode);
            return bookingResponse;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            // TODO: enter credentials here
            var client = new CoralClient("username", "password");
            var tester = new CoralClientTester(client);
            string response;

            // response = tester.TestFlow3();
            // Console.WriteLine(response);

            response = tester.TestFlow2();
            Console.WriteLine(response);
            var flow2results = JsonConvert.DeserializeObject<dynamic>(response);
            string bookingCode = flow2results["code"];
            if (bookingCode != null && bookingCode != "") {
                response = tester.RunCancellation(bookingCode);
                Console.WriteLine(response);
            }

            // response = tester.RunLocationSearch();
        }
    }
}
