using NUnit.Framework;
using RestSharp;
using System.Net;
using Newtonsoft.Json.Linq;

namespace RestfulBookerApiTests;

[TestFixture]
public class FilterAndSchemaTests
{
    private RestClient client;
    private const string BaseUrl = "https://restful-booker.herokuapp.com";

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        client = new RestClient(BaseUrl);
        client.AddDefaultHeader("Accept", "application/json");
    }

    [OneTimeTearDown]
    public void OneTimeTeardown()
    {
        client?.Dispose();
    }

    [TestCase("John")]
    [TestCase("Sally")]
    [TestCase("Jim")]
    public void GetBookings_FilteredByFirstName_ReturnsOk(string firstname)
    {
        var request = new RestRequest("/booking", Method.Get);
        request.AddQueryParameter("firstname", firstname);

        var response = client.Execute(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
            $"Filtering by firstname='{firstname}' should return 200 OK");
    }

    [TestCase("Smith")]
    [TestCase("Brown")]
    [TestCase("Jones")]
    public void GetBookings_FilteredByLastName_ReturnsOk(string lastname)
    {
        var request = new RestRequest("/booking", Method.Get);
        request.AddQueryParameter("lastname", lastname);

        var response = client.Execute(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
            $"Filtering by lastname='{lastname}' should return 200 OK");
    }

    [Test]
    public void GetBooking_ByValidId_ReturnsAllRequiredFields()
    {
        // Arrange — create a booking to guarantee a valid ID
        var createRequest = new RestRequest("/booking", Method.Post);
        createRequest.AddBody(
            "{\"firstname\":\"Schema\",\"lastname\":\"Test\",\"totalprice\":150,\"depositpaid\":true," +
            "\"bookingdates\":{\"checkin\":\"2025-06-01\",\"checkout\":\"2025-06-05\"}}",
            ContentType.Json);
        var createResponse = client.Execute(createRequest);
        var match = System.Text.RegularExpressions.Regex.Match(createResponse.Content!, "\"bookingid\":(\\d+)");
        var bookingId = match.Groups[1].Value;

        // Act
        var request = new RestRequest($"/booking/{bookingId}", Method.Get);
        var response = client.Execute(request);
        var body = JObject.Parse(response.Content!);

        // Assert — all required schema fields must be present
        Assert.That(body["firstname"], Is.Not.Null, "Response must contain 'firstname'");
        Assert.That(body["lastname"], Is.Not.Null, "Response must contain 'lastname'");
        Assert.That(body["totalprice"], Is.Not.Null, "Response must contain 'totalprice'");
        Assert.That(body["depositpaid"], Is.Not.Null, "Response must contain 'depositpaid'");
        Assert.That(body["bookingdates"], Is.Not.Null, "Response must contain 'bookingdates'");
        Assert.That(body["bookingdates"]!["checkin"], Is.Not.Null, "bookingdates must contain 'checkin'");
        Assert.That(body["bookingdates"]!["checkout"], Is.Not.Null, "bookingdates must contain 'checkout'");
    }
}
