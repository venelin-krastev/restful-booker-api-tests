# Test Plan — Restful Booker API

## Scope

**In scope:**
- GET /booking — list and filter bookings
- GET /booking/{id} — retrieve single booking
- POST /booking — create booking
- PUT /booking/{id} — full update
- PATCH /booking/{id} — partial update
- DELETE /booking/{id} — delete booking
- POST /auth — token generation

**Out of scope:**
- Load / performance testing
- Security penetration testing
- UI testing (API only)

## Test Types

| Type | When | Tool |
|---|---|---|
| Smoke | After every push to main | GitHub Actions |
| Regression | Before release | `dotnet test` full suite |
| Exploratory | New endpoints / edge cases | Postman |

## Environment

- **Base URL:** `https://restful-booker.herokuapp.com`
- **Tool:** Postman 11 (manual), RestSharp + NUnit (automated)
- **OS:** Windows 11
- **CI:** GitHub Actions — Ubuntu Latest

## Entry Criteria

- API is accessible and returning responses
- Automated suite builds without errors (`dotnet build`)

## Exit Criteria

- 0 critical bugs open
- All 30 automated tests passing in CI
- Smoke test green on main branch

## Risks

| Risk | Mitigation |
|---|---|
| restful-booker is a public shared API — data changes between runs | Tests always create fresh data before update/delete operations |
| Some IDs return 418 | Avoided by using test-created IDs only |
| `bookingid` serialisation bug (Infinity) | Handled via Regex extraction |

## Retesting vs Regression

- **Retesting:** Verify a specific bug is fixed
- **Regression:** Verify the fix has not broken existing functionality
