-- QA Verification Queries — Restful Booker
-- These queries mirror the automated API tests and would be used
-- to verify data integrity directly in the database.

-- ============================================================
-- 1. Count all bookings
-- ============================================================
SELECT COUNT(*) AS total_bookings
FROM bookings;

-- ============================================================
-- 2. Find a booking by guest name
-- ============================================================
SELECT *
FROM bookings
WHERE firstname = 'Test'
  AND lastname = 'User';

-- ============================================================
-- 3. Find bookings with depositpaid = false (potential issue)
-- ============================================================
SELECT id, firstname, lastname, totalprice
FROM bookings
WHERE depositpaid = 0
ORDER BY totalprice DESC;

-- ============================================================
-- 4. Count bookings grouped by depositpaid status
-- ============================================================
SELECT depositpaid, COUNT(*) AS booking_count
FROM bookings
GROUP BY depositpaid;

-- ============================================================
-- 5. Find bookings with missing additionalneeds (NULL check)
-- ============================================================
SELECT id, firstname, lastname
FROM bookings
WHERE additionalneeds IS NULL;

-- ============================================================
-- 6. Top 5 most expensive bookings
-- ============================================================
SELECT id, firstname, lastname, totalprice
FROM bookings
ORDER BY totalprice DESC
LIMIT 5;

-- ============================================================
-- 7. Bookings with checkout before checkin (data integrity bug)
-- ============================================================
SELECT id, firstname, lastname, checkin, checkout
FROM bookings
WHERE checkout < checkin;

-- ============================================================
-- 8. JOIN example — bookings with guest details
-- (hypothetical schema with separate guests table)
-- ============================================================
SELECT guests.name, guests.email, bookings.totalprice, bookings.checkin
FROM bookings
INNER JOIN guests ON bookings.guest_id = guests.id
WHERE bookings.totalprice > 100
ORDER BY bookings.checkin DESC;

-- ============================================================
-- 9. Average booking price per deposit status
-- ============================================================
SELECT depositpaid,
       COUNT(*)              AS total,
       ROUND(AVG(totalprice), 2) AS avg_price,
       MAX(totalprice)       AS max_price,
       MIN(totalprice)       AS min_price
FROM bookings
GROUP BY depositpaid;

-- ============================================================
-- 10. Find duplicate bookings (same guest, same dates)
-- ============================================================
SELECT firstname, lastname, checkin, checkout, COUNT(*) AS duplicates
FROM bookings
GROUP BY firstname, lastname, checkin, checkout
HAVING COUNT(*) > 1;
