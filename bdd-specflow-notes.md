# BDD and SpecFlow — Quick Reference

## What is BDD?

**BDD (Behavior Driven Development)** — a way of writing tests in plain language understood by the entire team: QA, developers, product owners, and clients.

Instead of technical test method names, scenarios are written in **Gherkin** — a human-readable format.

## Gherkin Keywords

| Keyword | Meaning |
|---|---|
| `Given` | Initial state — precondition |
| `When` | User action |
| `Then` | Expected result |
| `And` | Continuation of the previous step |

## Example — Feature File

```gherkin
Feature: Booking API

  Scenario: Get all bookings returns 200 OK
    Given the booking API is accessible
    When I send a GET request to /booking
    Then the response status code should be 200

  Scenario: Create booking with valid data returns booking ID
    Given the booking API is accessible
    When I send a POST request to /booking with valid payload
    Then the response should contain a bookingid
    And the response status code should be 200

  Scenario: Get non-existent booking returns 404
    Given the booking API is accessible
    When I send a GET request to /booking/999999
    Then the response status code should be 404
```

## Example — Step Definitions (C# / SpecFlow)

```csharp
[Binding]
public class BookingApiSteps
{
    private RestClient client = new RestClient("https://restful-booker.herokuapp.com");
    private RestResponse response;

    [Given(@"the booking API is accessible")]
    public void GivenTheBookingApiIsAccessible()
    {
        // API is a public endpoint — no setup needed
    }

    [When(@"I send a GET request to /booking")]
    public void WhenISendAGetRequestToBooking()
    {
        var request = new RestRequest("/booking", Method.Get);
        response = client.Execute(request);
    }

    [Then(@"the response status code should be (.*)")]
    public void ThenTheResponseStatusCodeShouldBe(int statusCode)
    {
        Assert.That((int)response.StatusCode, Is.EqualTo(statusCode),
            $"Expected status code {statusCode} but got {(int)response.StatusCode}");
    }

    [Then(@"the response should contain a bookingid")]
    public void ThenTheResponseShouldContainABookingid()
    {
        Assert.That(response.Content, Does.Contain("bookingid"),
            "Response should contain a bookingid field");
    }
}
```

## BDD vs NUnit — When to Use Which

| | NUnit | SpecFlow / BDD |
|---|---|---|
| Who reads the tests | Developers / QA only | Entire team including PO and client |
| Speed of writing | Faster | Slower |
| Best for | Technical tests, unit, API | Business scenarios, acceptance tests |

## One-line Interview Answers

**"What is the advantage of BDD?"**
> "Scenarios are written in Gherkin — readable by the entire team. Product Owners can read and validate tests without knowing C#. This reduces misunderstandings between business requirements and what is actually tested."

**"When would you use SpecFlow instead of NUnit?"**
> "When business stakeholders need to participate in defining test scenarios. For purely technical tests, NUnit is faster and simpler."
