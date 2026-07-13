# BUG-005 — POST /booking with missing required field returns 500 instead of 400

**Status:** Known API behaviour (documented)  
**Reported by:** Venelin Krastev  

---

## Environment

- Windows 11
- Postman 11
- API: https://restful-booker.herokuapp.com

## Severity / Priority

| | |
|---|---|
| **Severity** | Medium |
| **Priority** | Low |

## Preconditions

- API is accessible at `https://restful-booker.herokuapp.com`
- No authentication required for `POST /booking`

## Steps to Reproduce

1. Send a `POST` request to `/booking`
2. Set `Content-Type: application/json` header
3. Send a payload with a missing required field — e.g. omit `firstname`:

```json
{
  "lastname": "Smith",
  "totalprice": 150,
  "depositpaid": true,
  "bookingdates": {
    "checkin": "2025-01-01",
    "checkout": "2025-01-05"
  }
}
```

4. Observe the response status code

## Expected Result

`400 Bad Request` — the server should validate the request body and return a client error indicating which field is missing.

## Actual Result

`500 Internal Server Error` is returned instead of `400 Bad Request`.

## Notes

Returning `500 Internal Server Error` for a missing request field is incorrect. A `500` indicates an unexpected server-side failure, which misleads API consumers into thinking the server crashed rather than that their request was malformed. The correct response is `400 Bad Request`, optionally with a body describing which field is missing (e.g. `{"error": "firstname is required"}`). In a production API this distinction is critical — client-side code that handles `400` vs `500` differently would mask validation errors as infrastructure failures.
