# Restful Booker API Tests

Automated API test suite for [restful-booker.herokuapp.com](https://restful-booker.herokuapp.com) — a hotel booking API built for QA practice.

**Base URL:** `https://restful-booker.herokuapp.com`

## Tech Stack

- **C# / .NET 10**
- **NUnit** — test runner
- **RestSharp 114** — HTTP client
- **Newtonsoft.Json** — JSON parsing

## Test Coverage

| Class | Endpoint(s) | Tests |
|---|---|---|
| `GetBookingsTests.cs` | GET /booking, GET /booking/{id} | 5 |
| `CreateBookingTests.cs` | POST /booking | 5 |
| `AuthenticationTests.cs` | POST /auth | 5 |
| `UpdateBookingTests.cs` | PUT + PATCH /booking/{id} | 4 |
| `DeleteBookingTests.cs` | DELETE /booking/{id} | 4 |

**Total: 23 tests**

## What Is Tested

- Status codes and response body correctness
- Authentication flow — token generation and usage via `Cookie: token=<value>` header
- CRUD operations — Create, Read, Update, Delete
- Error handling — missing fields, invalid credentials, missing token
- PUT vs PATCH behavior — full replace vs partial update

## Authentication

The API uses token-based auth. After calling `POST /auth` with valid credentials, the token is passed on subsequent requests using the **Cookie header pattern**:

```
Cookie: token=<token_value>
```

RestSharp example:
```csharp
request.AddHeader("Cookie", $"token={token}");
```

## Booking ID Extraction — Regex Workaround

The `POST /booking` response occasionally contains a `bookingid` field rendered as `Infinity` due to a known restful-booker serialisation bug. Deserialising this with standard JSON parsers throws an exception. To work around it, the `bookingid` is extracted from the raw response string using a Regex:

```csharp
var match = Regex.Match(response.Content, @"""bookingid""\s*:\s*(\d+)");
int bookingId = int.Parse(match.Groups[1].Value);
```

This guarantees a valid integer ID regardless of the JSON parser's handling of non-standard numeric values.

## Non-Standard Status Codes

restful-booker deviates from REST conventions in several ways. The table below documents the actual behaviour:

| Scenario | Expected (REST convention) | Actual (restful-booker) |
|---|---|---|
| Successful booking creation (`POST /booking`) | 201 Created | **200 OK** |
| Successful booking deletion (`DELETE /booking/{id}`) | 204 No Content | **201 Created** |
| Wrong password on `POST /auth` | 401 Unauthorized | **200 OK** with `{"reason":"Bad credentials"}` body |
| Delete non-existent booking (`DELETE /booking/{id}`) | 404 Not Found | **405 Method Not Allowed** |

These quirks make the API a useful exercise in asserting on actual behaviour rather than assumed conventions.

## Additional Notes

- Missing required fields (e.g. `firstname`) on create return `500` instead of `400`
- Tests always create a fresh booking before update/delete operations to guarantee a valid, known ID
- Some pre-existing booking IDs return `418` — avoided by using test-created IDs only

## Run Tests

```bash
dotnet test
```
