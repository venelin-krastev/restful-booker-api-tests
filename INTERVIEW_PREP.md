# Interview Prep — QA Automation

## 2026-07-08 — FilterAndSchemaTests.cs

---

**Q1: Why did you use `[TestCase]` with hardcoded names like "John" and "Smith" instead of pulling test data from an external source?**
A: `[TestCase]` is ideal for small, known datasets where the values themselves are part of the test specification — I want to explicitly document which firstnames and lastnames the filter endpoint must handle.
For larger datasets an external source like a CSV or database would be more maintainable, but here simplicity wins because the API is a practice project with predictable data.
The tradeoff is clear: `[TestCase]` is readable and self-documenting, external data sources are more flexible but add complexity that isn't justified here.

---

**Q2: Why is `RestClient` created in `[OneTimeSetUp]` instead of `[SetUp]`?**
A: `[OneTimeSetUp]` runs once for the entire test class, while `[SetUp]` runs before every single test — creating and disposing an HTTP client on every test is wasteful and slows the suite down.
`RestClient` is thread-safe and designed to be reused across requests, so sharing one instance across all tests in the class is the correct pattern.
The `[OneTimeTearDown]` disposes it cleanly after all tests finish, so there are no resource leaks.

---

**Q3: What edge cases are NOT covered in the filter tests?**
A: The filter tests only assert that the status code is `200 OK` — they don't verify whether the returned bookings actually match the filter criteria, which is a gap.
There are no tests for filtering with a name that doesn't exist in the database, which should return an empty array rather than an error.
Special characters, SQL injection strings, or extremely long names in the query parameter are also not tested — these would be important negative and security edge cases in a production system.

---

**Q4: How would you extend this test suite for CI/CD?**
A: The suite already runs on GitHub Actions via a workflow that triggers on every push to main — `dotnet test` executes all tests in a headless environment on Ubuntu.
I would add test result reporting with `--logger trx` and upload the `.trx` file as a build artifact so the team can review failures without reading raw logs.
For larger teams I would also add a step that fails the pipeline if test coverage drops below a defined threshold, enforcing quality gates before merge.

---

**Q5: What could cause the schema validation test to be flaky?**
A: The test creates a fresh booking in the Arrange step to guarantee a valid ID — if the `POST /booking` call fails due to network issues on the shared public API, the Regex match will fail and the test throws a confusing error unrelated to schema validation.
The public restful-booker API occasionally returns the `bookingid` as `Infinity` due to a known serialisation bug, which is why Regex extraction is used instead of JSON deserialisation — without this workaround the test would fail intermittently.
A third flakiness risk is that the public API is shared by all users worldwide, so a booking created in the Arrange step could theoretically be deleted by another user before the GET request in the Act step.

---

**Q6: There are no explicit waits in this file — why, and when would you need them?**
A: API tests don't need explicit waits because HTTP requests are synchronous — `client.Execute(request)` blocks until the response is received, so there's no timing uncertainty.
Explicit waits with `WebDriverWait` are needed in UI/Selenium tests where the browser renders asynchronously and elements may not be in the DOM yet when the test tries to interact with them.
Using `Thread.Sleep` in any context is an anti-pattern because it wastes fixed time regardless of how fast or slow the system responds — `WebDriverWait` polls until the condition is true, which is both faster and more reliable.

---

**Q7: How does this test class fit into the test pyramid?**
A: `FilterAndSchemaTests.cs` sits in the middle layer of the pyramid — API/Integration level — which is faster and more stable than E2E Selenium tests but broader than unit tests.
These tests verify that the HTTP layer, business logic, and data serialisation all work together correctly, without the overhead of a browser.
In a balanced pyramid there should be more of these API tests than E2E tests, because they're faster to run, easier to maintain, and give precise failure messages when something breaks.

---

**Q8: In `GetBooking_ByValidId_ReturnsAllRequiredFields`, you assert that fields are `Not.Null` — why not also assert their values?**
A: The test's intent is schema validation — verifying that all required fields are present in the response, not that they contain specific values.
Asserting specific values would couple the test to the data created in the Arrange step, making the test more brittle and harder to read as a schema contract.
If I wanted to also validate values I would add separate data-integrity assertions after the schema checks, keeping the two concerns clearly separated and making failure messages more informative.
