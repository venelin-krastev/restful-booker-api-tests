# BUG-002 — DELETE /booking/{id} returns 201 Created instead of 204 No Content

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
| **Priority** | Medium |

## Preconditions

- API is accessible at `https://restful-booker.herokuapp.com`
- A valid auth token has been generated via `POST /auth`
- A booking exists with a known ID (created via `POST /booking`)

## Steps to Reproduce

1. Generate auth token — send `POST /auth` with valid credentials
2. Create a booking — send `POST /booking` with valid payload, note the returned `bookingid`
3. Send `DELETE` request to `/booking/{bookingid}`
4. Add header: `Cookie: token=<token_value>`
5. Observe the response status code

## Expected Result

`204 No Content` — per REST convention, successful deletion returns 204 with no response body.

## Actual Result

`201 Created` is returned instead of `204 No Content`.

## Notes

This is a known non-standard behaviour of restful-booker. The automated test suite asserts on the actual `201 Created` response. In a production system this would be flagged — returning `201 Created` on a DELETE operation is misleading as 201 conventionally means a resource was created, not deleted.
