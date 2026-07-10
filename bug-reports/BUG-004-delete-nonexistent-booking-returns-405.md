# BUG-004 — DELETE /booking/{id} on non-existent ID returns 405 instead of 404

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
- The booking ID used does not exist in the system

## Steps to Reproduce

1. Generate auth token — send `POST /auth` with valid credentials
2. Send `DELETE` request to `/booking/999999`
3. Add header: `Cookie: token=<token_value>`
4. Observe the response status code

## Expected Result

`404 Not Found` — the resource does not exist, so the server should indicate the ID was not found.

## Actual Result

`405 Method Not Allowed` is returned instead of `404 Not Found`.

## Notes

This is a known non-standard behaviour of restful-booker. Returning `405 Method Not Allowed` implies the DELETE method is not supported on this endpoint, which is misleading — DELETE is a valid and supported method. The correct response for a non-existent resource is `404 Not Found`. In a production system this would confuse API consumers and make error handling more complex, as `405` and `404` require different client-side responses.
