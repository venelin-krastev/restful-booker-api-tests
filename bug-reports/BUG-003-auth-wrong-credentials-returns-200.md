# BUG-003 — POST /auth with wrong password returns 200 OK instead of 401 Unauthorized

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
| **Severity** | High |
| **Priority** | High |

## Preconditions

- API is accessible at `https://restful-booker.herokuapp.com`

## Steps to Reproduce

1. Send `POST` request to `/auth`
2. Use the following request body with an incorrect password:
```json
{
  "username": "admin",
  "password": "wrongpassword"
}
```
3. Observe the response status code and body

## Expected Result

`401 Unauthorized` — per REST convention, failed authentication should return 401.

## Actual Result

`200 OK` is returned with the following body:
```json
{
  "reason": "Bad credentials"
}
```

## Notes

This is a known non-standard behaviour of restful-booker. Returning `200 OK` for failed authentication is misleading — a `200` status signals success at the HTTP level, forcing clients to parse the response body to detect failure instead of relying on the status code. In a production system this would be a high severity finding as it can mask authentication failures in monitoring and logging tools.
