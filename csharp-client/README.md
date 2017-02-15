## HotelsPro C# Client

This client is intended to serve as an example implementation of the HotelsPro dynamic search API. Relevant documentation for the API can be accessed [here](api2.hotelspro.com/docs/).

The client itself has no dependencies, however running the tests requires the [Newtonsoft.Json](http://www.newtonsoft.com/json) package. This is stated in the `project.json` file and doing a `dotnet restore` will take care of it for you.

Running `dotnet run` will run the tests as written out in the `ConsoleApplication.Program::Main` method. Do not forget to pass your credentials in the client constructor before running the program.
