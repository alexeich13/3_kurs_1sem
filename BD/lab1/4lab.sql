---2. Вычисление итогов предоставленных услуг для определенного вида услуги помесячно, за квартал, за полгода, за год.
-- Вычисление итогов помесячно
SELECT
    MONTH(bp.booking_date) AS month,
    YEAR(bp.booking_date) AS year,
    p.description AS package_description,
    COUNT(bp.booking_id) AS total_bookings,
    SUM(p.price) AS total_revenue
FROM
    Bookings bp
    JOIN Packages p ON bp.package_id = p.package_id
WHERE
    p.description = 'Package to Rome' 
GROUP BY
    YEAR(bp.booking_date),
    MONTH(bp.booking_date),
    p.description
ORDER BY
    year,
    month;

-- Вычисление итогов за квартал
SELECT
    DATEPART(QUARTER, bp.booking_date) AS quarter,
    YEAR(bp.booking_date) AS year,
    p.description AS package_description,
    COUNT(bp.booking_id) AS total_bookings,
    SUM(p.price) AS total_revenue
FROM
    Bookings bp
    JOIN Packages p ON bp.package_id = p.package_id
WHERE
    p.description = 'Package to Rome'  
GROUP BY
    YEAR(bp.booking_date),
    DATEPART(QUARTER, bp.booking_date),
    p.description
ORDER BY
    year,
    quarter;

-- Вычисление итогов за полгода 
SELECT
    CEILING(MONTH(bp.booking_date) / 6.0) AS half_year,
    YEAR(bp.booking_date) AS year,
    p.description AS package_description,
    COUNT(bp.booking_id) AS total_bookings,
    SUM(p.price) AS total_revenue
FROM
    Bookings bp
    JOIN Packages p ON bp.package_id = p.package_id
WHERE
    p.description = 'Package to Rome'
GROUP BY
    YEAR(bp.booking_date),
    CEILING(MONTH(bp.booking_date) / 6.0),
    p.description
ORDER BY
    year,
    half_year;

-- Вычисление итогов за год
SELECT
    YEAR(bp.booking_date) AS year,
    p.description AS package_description,
    COUNT(bp.booking_id) AS total_bookings,
    SUM(p.price) AS total_revenue
FROM
    Bookings bp
    JOIN Packages p ON bp.package_id = p.package_id
WHERE
    p.description = 'Package to Rome' 
GROUP BY
    YEAR(bp.booking_date),
    p.description
ORDER BY
    year;

WITH RomePackageStats AS (
    SELECT
        YEAR(bp.booking_date) AS year,
        MONTH(bp.booking_date) AS month,
        p.description AS package_description,
        COUNT(bp.booking_id) AS total_bookings,
        SUM(p.price) AS total_revenue
    FROM
        Bookings bp
        JOIN Packages p ON bp.package_id = p.package_id
    WHERE
        p.description = 'Package to Rome'
    GROUP BY
        YEAR(bp.booking_date),
        MONTH(bp.booking_date),
        p.description
),
QuarterStats AS (
    SELECT
        YEAR(bp.booking_date) AS year,
        DATEPART(QUARTER, bp.booking_date) AS quarter,
        p.description AS package_description,
        COUNT(bp.booking_id) AS total_bookings,
        SUM(p.price) AS total_revenue
    FROM
        Bookings bp
        JOIN Packages p ON bp.package_id = p.package_id
    WHERE
        p.description = 'Package to Rome'
    GROUP BY
        YEAR(bp.booking_date),
        DATEPART(QUARTER, bp.booking_date),
        p.description
),
HalfYearStats AS (
    SELECT
        YEAR(bp.booking_date) AS year,
        CEILING(MONTH(bp.booking_date) / 6.0) AS half_year,
        p.description AS package_description,
        COUNT(bp.booking_id) AS total_bookings,
        SUM(p.price) AS total_revenue
    FROM
        Bookings bp
        JOIN Packages p ON bp.package_id = p.package_id
    WHERE
        p.description = 'Package to Rome'
    GROUP BY
        YEAR(bp.booking_date),
        CEILING(MONTH(bp.booking_date) / 6.0),
        p.description
),
YearStats AS (
    SELECT
        YEAR(bp.booking_date) AS year,
        p.description AS package_description,
        COUNT(bp.booking_id) AS total_bookings,
        SUM(p.price) AS total_revenue
    FROM
        Bookings bp
        JOIN Packages p ON bp.package_id = p.package_id
    WHERE
        p.description = 'Package to Rome'
    GROUP BY
        YEAR(bp.booking_date),
        p.description
)
SELECT
    R.year,
    R.month,
    R.package_description,
    R.total_bookings,
    R.total_revenue,
    Q.quarter,
    H.half_year,
    Y.total_bookings AS total_bookings_year,
    Y.total_revenue AS total_revenue_year
FROM
    RomePackageStats R
    LEFT JOIN QuarterStats Q ON R.year = Q.year
    LEFT JOIN HalfYearStats H ON R.year = H.year
    LEFT JOIN YearStats Y ON R.year = Y.year
ORDER BY
    R.year,
    R.month;

--3.Вычисление итогов предоставленных услуг для определенного вида услуги за период: объем услуг; сравнение их с общим объемом услуг (в %);сравнение с наибольшим объемом услуг (в %).
DECLARE @ServiceDescription NVARCHAR(255) = 'Package to Rome'; 
DECLARE @StartDate DATE = '2023-01-01'; 
DECLARE @EndDate DATE = '2023-12-31'; 

-- Объем услуг за период
SELECT
    COUNT(bp.booking_id) AS total_bookings,
    SUM(p.price) AS total_revenue
FROM
    Bookings bp
    JOIN Packages p ON bp.package_id = p.package_id
WHERE
    p.description = @ServiceDescription
    AND bp.booking_date BETWEEN @StartDate AND @EndDate;

-- Сравнение с общим объемом услуг (в %)
go;
DECLARE @TotalServices INT;
DECLARE @TotalRevenue DECIMAL(10, 2);
DECLARE @ServiceDescription NVARCHAR(255) = 'Package to Rome'; 
DECLARE @StartDate DATE = '2023-01-01'; 
DECLARE @EndDate DATE = '2023-12-31'; 

SELECT
    @TotalServices = COUNT(bp.booking_id),
    @TotalRevenue = SUM(p.price)
FROM
    Bookings bp
    JOIN Packages p ON bp.package_id = p.package_id
WHERE
    bp.booking_date BETWEEN @StartDate AND @EndDate;

SELECT
    COUNT(bp.booking_id) AS total_bookings,
    SUM(p.price) AS total_revenue,
    CAST(COUNT(bp.booking_id) AS DECIMAL(10, 2)) / @TotalServices * 100 AS percentage_bookings,
    CAST(SUM(p.price) AS DECIMAL(10, 2)) / @TotalRevenue * 100 AS percentage_revenue
FROM
    Bookings bp
    JOIN Packages p ON bp.package_id = p.package_id
WHERE
    p.description = @ServiceDescription
    AND bp.booking_date BETWEEN @StartDate AND @EndDate;

-- Сравнение с наибольшим объемом услуг (в %)
go;
DECLARE @MaxServices INT;
DECLARE @MaxRevenue DECIMAL(10, 2);
DECLARE @ServiceDescription NVARCHAR(255) = 'Package to Rome'; 
DECLARE @StartDate DATE = '2023-01-01'; 
DECLARE @EndDate DATE = '2023-12-31'; 

SELECT TOP 1
    @MaxServices = total_bookings,
    @MaxRevenue = total_revenue
FROM (
    SELECT
        COUNT(bp.booking_id) AS total_bookings,
        SUM(p.price) AS total_revenue
    FROM
        Bookings bp
        JOIN Packages p ON bp.package_id = p.package_id
    WHERE
        bp.booking_date BETWEEN @StartDate AND @EndDate
    GROUP BY
        p.description
) AS subquery
ORDER BY
    total_bookings DESC;

SELECT
    COUNT(bp.booking_id) AS total_bookings,
    SUM(p.price) AS total_revenue,
    CAST(COUNT(bp.booking_id) AS DECIMAL(10, 2)) / @MaxServices * 100 AS percentage_bookings,
    CAST(SUM(p.price) AS DECIMAL(10, 2)) / @MaxRevenue * 100 AS percentage_revenue
FROM
    Bookings bp
    JOIN Packages p ON bp.package_id = p.package_id
WHERE
    p.description = @ServiceDescription
    AND bp.booking_date BETWEEN @StartDate AND @EndDate;
go;
DECLARE @ServiceDescription NVARCHAR(255) = 'Package to Rome'; 
DECLARE @StartDate DATE = '2023-01-01'; 
DECLARE @EndDate DATE = '2023-12-31'; 

-- Объем услуг за период
WITH PeriodStats AS (
    SELECT
        COUNT(bp.booking_id) AS total_bookings,
        SUM(p.price) AS total_revenue
    FROM
        Bookings bp
        JOIN Packages p ON bp.package_id = p.package_id
    WHERE
        p.description = @ServiceDescription
        AND bp.booking_date BETWEEN @StartDate AND @EndDate
),
-- Сравнение с общим объемом услуг (в %)
TotalStats AS (
    SELECT
        COUNT(bp.booking_id) AS total_services,
        SUM(p.price) AS total_revenue
    FROM
        Bookings bp
        JOIN Packages p ON bp.package_id = p.package_id
    WHERE
        bp.booking_date BETWEEN @StartDate AND @EndDate
),
-- Сравнение с наибольшим объемом услуг (в %)
MaxStats AS (
    SELECT TOP 1
        COUNT(bp.booking_id) AS max_services,
        SUM(p.price) AS max_revenue
    FROM
        Bookings bp
        JOIN Packages p ON bp.package_id = p.package_id
    WHERE
        bp.booking_date BETWEEN @StartDate AND @EndDate
    GROUP BY
        p.description
    ORDER BY
        max_services DESC
)

SELECT
    p.total_bookings AS total_bookings_period,
    p.total_revenue AS total_revenue_period,
    t.total_services AS total_services,
    t.total_revenue AS total_revenue,
    CAST(p.total_bookings AS DECIMAL(10, 2)) / t.total_services * 100 AS percentage_bookings_total,
    CAST(p.total_revenue AS DECIMAL(10, 2)) / t.total_revenue * 100 AS percentage_revenue_total,
    CAST(p.total_bookings AS DECIMAL(10, 2)) / m.max_services * 100 AS percentage_bookings_max,
    CAST(p.total_revenue AS DECIMAL(10, 2)) / m.max_revenue * 100 AS percentage_revenue_max
FROM
    PeriodStats p
    CROSS JOIN TotalStats t
    CROSS JOIN MaxStats m;


--4. Продемонстрируйте применение функции ранжирования ROW_NUMBER() для разбиения результатов запроса на страницы (по 20 строк на каждую страницу).
DECLARE @PageSize INT = 20;
DECLARE @PageNumber INT = 5;

SELECT
    customer_id,
    first_name,
    last_name,
    ROW_NUMBER() OVER (ORDER BY customer_id) AS row_num
FROM
    Customers
ORDER BY
    customer_id
OFFSET (@PageNumber - 1) * @PageSize ROWS -- Смещение для нужной страницы
FETCH NEXT @PageSize ROWS ONLY; -- Количество строк на странице

--5.Продемонстрируйте применение функции ранжирования ROW_NUMBER() для удаления дубликатов.
insert into Customers (customer_id,first_name, last_name, email, phone, address)
values
(100001, 'FirstName100000', 'LastName100000', 'email100000@example.com', '123-456-100000', 'Address100000');
select * from Customers;
WITH NumberedRows AS (
    SELECT
        customer_id,
        first_name,
        last_name,
        email,
        phone,
        address,
        ROW_NUMBER() OVER (PARTITION BY first_name, last_name, email, phone, address ORDER BY customer_id) AS row_num
    FROM
        Customers
)
DELETE FROM Customers
WHERE customer_id IN (
    SELECT customer_id
    FROM NumberedRows
    WHERE row_num > 1
);

SELECT * FROM Customers;

--6.Вернуть для каждого клиента количество услуг за последние 6 месяцев помесячно.

SELECT
    c.customer_id,
    c.first_name,
    c.last_name,
    COUNT(bp.booking_id) AS total_bookings,
    MONTH(bp.booking_date) AS month,
    YEAR(bp.booking_date) AS year
FROM
    Customers c
    JOIN Bookings bp ON c.customer_id = bp.customer_id
WHERE
    bp.booking_date >= DATEADD(MONTH, -6, GETDATE())
GROUP BY
    c.customer_id,
    c.first_name,
    c.last_name,
    MONTH(bp.booking_date),
    YEAR(bp.booking_date)
ORDER BY
    c.customer_id,
    year,
    month;


--7.Какая услуга была предоставлена наибольшее число раз для определенного вида? Вернуть для всех видов.

WITH ServiceCounts AS (
    SELECT
        p.description AS service_type,
        COUNT(bp.booking_id) AS total_bookings
    FROM
        Packages p
        JOIN Bookings bp ON p.package_id = bp.package_id
    GROUP BY
        p.description
)
SELECT
    service_type,
    total_bookings,
    RANK() OVER (ORDER BY total_bookings DESC) AS service_rank
FROM
    ServiceCounts;

select * from Customers;

