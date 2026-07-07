# ISTQB Concepts — Interview Cheat Sheet

## Test Levels

| Level | Who tests | What is tested | Example |
|---|---|---|---|
| **Unit** | Developer | Single method / function in isolation | `CalculateTotal()` returns correct sum |
| **Integration** | Developer / QA | Interaction between components | POST /booking saves data → GET /booking returns it |
| **System** | QA | Entire product against requirements | Full booking flow works end-to-end |
| **Acceptance (UAT)** | Client / PO | Business requirements met, ready for release | Client confirms the product does what was agreed |

## Test Types

### Functional vs Non-Functional

| Functional | Non-Functional |
|---|---|
| WHAT the system does | HOW WELL the system does it |
| Login works with valid credentials | Login responds in under 1 second |
| Booking is saved correctly | System handles 1000 concurrent users |

### Common Test Types

| Type | Description |
|---|---|
| **Smoke** | Quick check — does the build work at all? |
| **Regression** | Does a new change break existing functionality? |
| **Retesting** | Is the specific bug fixed? |
| **Exploratory** | Unscripted — learn, design and test simultaneously |
| **Performance** | Speed, stability, scalability under load |
| **Security** | Vulnerabilities, unauthorised access |

## Test Pyramid

```
        /\
       /E2E\          ← Few — slow, expensive, brittle
      /------\
     /  API   \       ← Some — faster, more stable
    /----------\
   /    Unit    \     ← Many — fast, cheap, isolated
  /______________\
```

**Rule:** More unit tests (fast/cheap) → fewer E2E tests (slow/expensive).
E2E tests are not bad — just use them sparingly for critical paths only.

## Black-box / White-box / Grey-box

| Type | Code knowledge | Who |
|---|---|---|
| **Black-box** | None — test from outside | QA |
| **White-box** | Full — test internal logic | Developer |
| **Grey-box** | Partial — know some internals | QA Automation |

## Verification vs Validation

| | Question | When |
|---|---|---|
| **Verification** | Are we building the product right? | During development |
| **Validation** | Are we building the right product? | At the end — does it meet user needs? |

## One-line Interview Answers

**"What is the test pyramid?"**
> "More unit tests at the base because they're fast and cheap, fewer E2E tests at the top because they're slow and brittle. The goal is maximum coverage at minimum cost."

**"What is the difference between functional and non-functional testing?"**
> "Functional tests verify WHAT the system does — features and business logic. Non-functional tests verify HOW WELL it does it — performance, security, reliability."

**"What level does a QA automation engineer work at?"**
> "Mainly System level — automated E2E and API tests — and sometimes Integration level when testing how components interact."
