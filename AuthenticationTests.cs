using NUnit.Framework;
using RestSharp;
using System.Net;
using Newtonsoft.Json.Linq;

namespace RestfulBookerApiTests;

[TestFixture]
public class AuthenticationTests
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

    [Test]
    public void AuthenticateWithValidCredentialsReturnsOkWithToken()
    {
        var request = new RestRequest("/auth", Method.Post);
        request.AddBody("{\"username\":\"admin\",\"password\":\"password123\"}", ContentType.Json);

        var response = client.Execute(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
            "POST /auth with valid credentials should return 200 OK");

        var body = JObject.Parse(response.Content!);
        Assert.That(body["token"]?.ToString(), Is.Not.Null.And.Not.Empty,
            "Response should contain a non-empty token");
    }

    [Test]
    public void AuthenticateWithWrongPasswordReturnsTokenIsInvalidReason()
    {
        var request = new RestRequest("/auth", Method.Post);
        request.AddBody("{\"username\":\"admin\",\"password\":\"wrongpassword\"}", ContentType.Json);

        var response = client.Execute(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
            "POST /auth returns 200 even for invalid credentials — error is in the body");

        var body = JObject.Parse(response.Content!);
        Assert.That(body["reason"]?.ToString(), Is.EqualTo("Bad credentials"),
            "Wrong password should return reason: Bad credentials");
    }

    [Test]
    public void AuthenticateWithWrongUsernameReturnsBadCredentials()
    {
        var request = new RestRequest("/auth", Method.Post);
        request.AddBody("{\"username\":\"unknownuser\",\"password\":\"password123\"}", ContentType.Json);

        var response = client.Execute(request);

        var body = JObject.Parse(response.Content!);
        Assert.That(body["reason"]?.ToString(), Is.EqualTo("Bad credentials"),
            "Unknown username should return reason: Bad credentials");
    }

    [Test]
    public void AuthenticateWithEmptyCredentialsReturnsBadCredentials()
    {
        var request = new RestRequest("/auth", Method.Post);
        request.AddBody("{\"username\":\"\",\"password\":\"\"}", ContentType.Json);

        var response = client.Execute(request);

        var body = JObject.Parse(response.Content!);
        Assert.That(body["reason"]?.ToString(), Is.EqualTo("Bad credentials"),
            "Empty credentials should return reason: Bad credentials");
    }

    [Test]
    public void TokenReturnedByAuthCanBeUsedForAuthenticatedRequests()
    {
        var authRequest = new RestRequest("/auth", Method.Post);
        authRequest.AddBody("{\"username\":\"admin\",\"password\":\"password123\"}", ContentType.Json);
        var authResponse = client.Execute(authRequest);
        var token = JObject.Parse(authResponse.Content!)["token"]!.ToString();

        var createRequest = new RestRequest("/booking", Method.Post);
        createRequest.AddBody(
            "{\"firstname\":\"Token\",\"lastname\":\"Test\",\"totalprice\":50,\"depositpaid\":true," +
            "\"bookingdates\":{\"checkin\":\"2025-10-01\",\"checkout\":\"2025-10-05\"}}",
            ContentType.Json);
        var createMatch = System.Text.RegularExpressions.Regex.Match(
            client.Execute(createRequest).Content!, "\"bookingid\":(\\d+)");
        var bookingId = createMatch.Groups[1].Value;

        var updateRequest = new RestRequest($"/booking/{bookingId}", Method.Put);
        updateRequest.AddHeader("Cookie", $"token={token}");
        updateRequest.AddBody(
            "{\"firstname\":\"Updated\",\"lastname\":\"Test\",\"totalprice\":99,\"depositpaid\":true," +
            "\"bookingdates\":{\"checkin\":\"2025-10-01\",\"checkout\":\"2025-10-05\"}}",
            ContentType.Json);
        var updateResponse = client.Execute(updateRequest);

        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK),
            "A valid token should allow updating a booking — 200 OK expected");

        var updatedBody = JObject.Parse(updateResponse.Content!);
        Assert.That(updatedBody["firstname"]?.ToString(), Is.EqualTo("Updated"),
            "Updated firstname should be reflected in the response");
    }
}
