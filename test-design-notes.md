# Test Design Techniques — Quick Reference

## Equivalence Partitioning
Divide input data into groups expected to behave the same way.
Test one value per group — if one passes/fails, all in the group will too.

**Example — age field accepting 18-65:**
| Partition | Example value | Expected |
|---|---|---|
| Below minimum | 17 | Rejected |
| Valid range | 30 | Accepted |
| Above maximum | 66 | Rejected |

## Boundary Value Analysis (BVA)
Test values at and around the boundary — where bugs most commonly hide.

**Example — field accepting 1-100:**
| Value | Reason |
|---|---|
| 0 | Just below minimum |
| 1 | Minimum (boundary) |
| 2 | Just above minimum |
| 99 | Just below maximum |
| 100 | Maximum (boundary) |
| 101 | Just above maximum |

## Test Types

| Type | Description |
|---|---|
| **Happy path** | Valid data, main flow — expected to pass |
| **Negative test** | Invalid data or wrong action — system should reject gracefully |
| **Edge case** | Unusual but valid input — double click, empty string, max length |
| **Smoke test** | Quick check that core features work after a deploy |
| **Regression test** | Verify existing functionality not broken by a new change |
| **Exploratory test** | Simultaneous learning, design and execution — no fixed script |
| **End-to-end test** | Full user journey from start to finish across all layers |

## Test Case Structure

```
Title:          [Short descriptive name]
Preconditions:  [System state before test]
Steps:          [Numbered actions]
Expected result:[What should happen]
Actual result:  [What happened — filled after execution]
Status:         Pass / Fail
```
