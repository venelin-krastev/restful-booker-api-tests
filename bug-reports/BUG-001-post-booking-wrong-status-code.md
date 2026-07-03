# BUG-001 — POST /booking returns 200 OK instead of 201 Created

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
| **Priority** | High |

## Preconditions

- API is accessible at `https://restful-booker.herokuapp.com`
- Valid booking payload is prepared

## Steps to Reproduce

1. Send `POST` request to `/booking`
2. Use the following request body:
```json
{
  "firstname": "Test",
  "lastname": "User",
  "totalprice": 200,
  "depositpaid": true,
  "bookingdates": {
    "checkin": "2025-07-01",
    "checkout": "2025-07-05"
  }
}
```
3. Observe the response status code

## Expected Result

`201 Created` with new `bookingid` in response body — per REST convention, resource creation should return 201.

## Actual Result

`200 OK` is returned instead of `201 Created`.

## Notes

This is a known non-standard behaviour of restful-booker. The automated test suite asserts on the actual `200 OK` response rather than the REST convention `201 Created`. In a production system this would be flagged for correction.
