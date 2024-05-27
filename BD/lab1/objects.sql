--Представление "Список бронирований с подробностями":
CREATE VIEW BookingDetails AS
SELECT
    B.booking_id,
    C.first_name AS customer_first_name,
    C.last_name AS customer_last_name,
    P.package_id,
    P.description AS package_description,
    P.price AS package_price,
    B.booking_date,
    B.payment_status
FROM Bookings B
INNER JOIN Customers C ON B.customer_id = C.customer_id
INNER JOIN Packages P ON B.package_id = P.package_id;

select * from BookingDetails;

--Представление "Список доступных путевок":
CREATE VIEW AvailablePackages AS
SELECT
    P.package_id,
    P.description AS package_description,
    P.price AS package_price,
    P.start_date,
    P.end_date,
    T.name AS tour_operator_name,
    D.country_name,
    D.city_name
FROM Packages P
INNER JOIN TourOperators T ON P.tour_operator_id = T.tour_operator_id
INNER JOIN Destinations D ON P.destination_id = D.destination_id
LEFT JOIN Bookings B ON P.package_id = B.package_id
WHERE B.booking_id IS NULL;

select * from AvailablePackages;

--Представление "Список бронирований с оплатой":
CREATE VIEW BookingsWithPayment AS
SELECT
    B.booking_id,
    C.first_name AS customer_first_name,
    C.last_name AS customer_last_name,
    P.package_id,
    P.description AS package_description,
    B.booking_date,
    B.payment_status,
    PY.payment_date,
    PY.amount AS payment_amount
FROM Bookings B
INNER JOIN Customers C ON B.customer_id = C.customer_id
INNER JOIN Packages P ON B.package_id = P.package_id
LEFT JOIN Payments PY ON B.booking_id = PY.booking_id;

select * from BookingsWithPayment;

-- Создание процедуры для поиска доступных путевок
CREATE PROCEDURE SearchPackages
    @destination NVARCHAR(255),
    @start_date DATE,
    @end_date DATE,
    @max_price DECIMAL(10, 2)
AS
BEGIN
    SELECT
        P.package_id,
        P.description AS package_description,
        P.price AS package_price,
        P.start_date,
        P.end_date,
        T.name AS tour_operator_name,
        D.country_name,
        D.city_name
    FROM Packages P
    INNER JOIN TourOperators T ON P.tour_operator_id = T.tour_operator_id
    INNER JOIN Destinations D ON P.destination_id = D.destination_id
    LEFT JOIN Bookings B ON P.package_id = B.package_id
    WHERE B.booking_id IS NULL
        AND D.country_name = @destination
        AND P.start_date >= @start_date
        AND P.end_date <= @end_date
        AND P.price <= @max_price;
END;

EXEC SearchPackages
    @destination = 'Italy',
    @start_date = '2023-01-01',
    @end_date = '2023-12-31',
    @max_price = 2000.00;
-- Создание процедуры для отмены бронирования
CREATE PROCEDURE CancelBooking
    @booking_id INT
AS
BEGIN
    DELETE FROM Payments
    WHERE booking_id = @booking_id;
    DELETE FROM Bookings
    WHERE booking_id = @booking_id;
END;

exec CancelBooking @booking_id = 10;
select *from Bookings;
-- Создание процедуры для обновления информации о туроператоре
CREATE PROCEDURE UpdateTourOperator
    @tour_operator_id INT,
    @name NVARCHAR(255),
    @contact_info NVARCHAR(255),
    @website NVARCHAR(255)
AS
    BEGIN
        UPDATE TourOperators
        SET name = @name, contact_info = @contact_info, website = @website
        WHERE tour_operator_id = @tour_operator_id;
    END;

EXEC UpdateTourOperator
    @tour_operator_id = 9,
    @name = 'NTourCo',
    @contact_info = 'NContact',
    @website = 'www.Ntourco.com';

select * from TourOperators;

-- Создание процедуры для обновления информации о клиенте
CREATE PROCEDURE UpdateCustomer
    @customer_id INT,
    @first_name NVARCHAR(255),
    @last_name NVARCHAR(255),
    @email NVARCHAR(255),
    @phone NVARCHAR(20),
    @address NVARCHAR(MAX)
AS
    BEGIN
        UPDATE Customers
        SET first_name = @first_name, last_name = @last_name, email = @email, phone = @phone, address = @address
        WHERE customer_id = @customer_id;
    END;

EXEC UpdateCustomer
    @customer_id = 2,  
    @first_name = 'Updated John',
    @last_name = 'Updated Doe',
    @email = 'updatedjohn.doe@email.com',
    @phone = '987-654-3210',
    @address = '456 Oak St, Town';

select * from Customers;
-- Создание процедуры обновления информации о платеже
CREATE PROCEDURE UpdatePayment
    @payment_id INT,
    @booking_id INT,
    @payment_date DATE,
    @amount DECIMAL(10, 2)
AS
    BEGIN
        UPDATE Payments
        SET booking_id = @booking_id, payment_date = @payment_date, amount = @amount
        WHERE payment_id = @payment_id;
    END;

EXEC UpdatePayment
    @payment_id = 1,  
    @booking_id = 2,  
    @payment_date = '2023-02-20',
    @amount = 750.00;
select * from Payments

-- Вставка 100,000 случайных клиентов в таблицу Customers
DECLARE @Counter INT = 1;
WHILE @Counter <= 100000
BEGIN
    INSERT INTO Customers (customer_id, first_name, last_name, email, phone, address)
    VALUES (
        @Counter, 
        'FirstName' + CAST(@Counter AS NVARCHAR(10)),
        'LastName' + CAST(@Counter AS NVARCHAR(10)),
        'email' + CAST(@Counter AS NVARCHAR(10)) + '@example.com',
        '123-456-' + CAST(@Counter AS NVARCHAR(10)),
        'Address' + CAST(@Counter AS NVARCHAR(10))
    );
    SET @Counter = @Counter + 1;
END;

SELECT first_name FROM Customers WHERE last_name = 'LastName123';

CREATE INDEX idx_customer_ln ON Customers(last_name);

drop index idx_customer_ln on Customers; 


-- Вставка данных в таблицу Destinations
INSERT INTO Destinations (destination_id, country_name, city_name)
VALUES
(1, 'USA', 'New York'),
(2, 'France', 'Paris'),
(3, 'Italy', 'Rome'),
(4, 'Japan', 'Tokyo'),
(5, 'Australia', 'Sydney'),
(6, 'Canada', 'Vancouver'),
(7, 'Spain', 'Barcelona'),
(8, 'Germany', 'Berlin'),
(9, 'Brazil', 'Rio de Janeiro'),
(10, 'China', 'Beijing');
INSERT INTO Destinations (destination_id, country_name, city_name)
VALUES
(11, 'Greece', 'Athens'),
(12, 'Italy', 'Venice'),
(13, 'Spain', 'Seville'),
(14, 'France', 'Nice'),
(15, 'USA', 'Los Angeles'),
(16, 'Australia', 'Sydney'),
(17, 'Japan', 'Kyoto'),
(18, 'Brazil', 'Rio de Janeiro'),
(19, 'Canada', 'Vancouver'),
(20, 'China', 'Shanghai'),
(21, 'Germany', 'Munich'),
(22, 'India', 'Mumbai'),
(23, 'South Africa', 'Cape Town'),
(24, 'Mexico', 'Cancun'),
(25, 'Thailand', 'Bangkok'),
(26, 'Russia', 'St. Petersburg'),
(27, 'United Kingdom', 'Edinburgh'),
(28, 'Netherlands', 'Amsterdam'),
(29, 'Turkey', 'Istanbul'),
(30, 'Argentina', 'Buenos Aires'),
(31, 'Switzerland', 'Zurich'),
(32, 'Egypt', 'Cairo'),
(33, 'Portugal', 'Lisbon'),
(34, 'New Zealand', 'Auckland'),
(35, 'Norway', 'Oslo'),
(36, 'Sweden', 'Stockholm'),
(37, 'Denmark', 'Copenhagen'),
(38, 'South Korea', 'Seoul'),
(39, 'Iceland', 'Reykjavik'),
(40, 'Singapore', 'Singapore');

-- Вставка данных в таблицу TourOperators
INSERT INTO TourOperators (tour_operator_id, name, contact_info, website)
VALUES
(1, 'TourCo1', 'Contact1', 'www.tourco1.com'),
(2, 'TourCo2', 'Contact2', 'www.tourco2.com'),
(3, 'TourCo3', 'Contact3', 'www.tourco3.com'),
(4, 'TourCo4', 'Contact4', 'www.tourco4.com'),
(5, 'TourCo5', 'Contact5', 'www.tourco5.com'),
(6, 'TourCo6', 'Contact6', 'www.tourco6.com'),
(7, 'TourCo7', 'Contact7', 'www.tourco7.com'),
(8, 'TourCo8', 'Contact8', 'www.tourco8.com'),
(9, 'TourCo9', 'Contact9', 'www.tourco9.com'),
(10, 'TourCo10', 'Contact10', 'www.tourco10.com');
INSERT INTO TourOperators (tour_operator_id, name, contact_info, website)
VALUES
(11, 'ExploreWorld', 'info@exploreworld.com', 'www.exploreworld.com'),
(12, 'TravelExperts', 'contact@travelexperts.com', 'www.travelexperts.com'),
(13, 'AdventuresUnlimited', 'adventures@unlimited.com', 'www.adventuresunlimited.com'),
(14, 'GlobalGetaways', 'global@getaways.com', 'www.globalgetaways.com'),
(15, 'EpicJourneys', 'info@epicjourneys.com', 'www.epicjourneys.com'),
(16, 'DiscoverDestinations', 'discover@destinations.com', 'www.discoverdestinations.com'),
(17, 'WanderlustTravel', 'wanderlust@travel.com', 'www.wanderlusttravel.com'),
(18, 'JourneyMasters', 'journey@masters.com', 'www.journeymasters.com'),
(19, 'InfinityVoyages', 'info@infinityvoyages.com', 'www.infinityvoyages.com'),
(20, 'DreamyDestinations', 'dreamy@destinations.com', 'www.dreamydestinations.com'),
(21, 'VividVentures', 'vivid@ventures.com', 'www.vividventures.com'),
(22, 'RoamingRovers', 'roaming@rovers.com', 'www.roamingrovers.com'),
(23, 'ExoticExplorations', 'exotic@explorations.com', 'www.exoticexplorations.com'),
(24, 'SunsetAdventures', 'sunset@adventures.com', 'www.sunsetadventures.com'),
(25, 'PacificParadise', 'pacific@paradise.com', 'www.pacificparadise.com'),
(26, 'MountainHighTours', 'mountain@tours.com', 'www.mountainhightours.com'),
(27, 'OceanOdysseys', 'ocean@odysseys.com', 'www.oceanodysseys.com'),
(28, 'EnchantingEscapes', 'enchanting@escapes.com', 'www.enchantingescapes.com'),
(29, 'MysticJourneys', 'mystic@journeys.com', 'www.mysticjourneys.com'),
(30, 'PinnaclePlaces', 'pinnacle@places.com', 'www.pinnacleplaces.com'),
(31, 'TropicalTreasures', 'tropical@treasures.com', 'www.tropicaltreasures.com'),
(32, 'CitySlickerTours', 'cityslicker@tours.com', 'www.cityslickertours.com'),
(33, 'HeritageHolidays', 'heritage@holidays.com', 'www.heritageholidays.com'),
(34, 'NorthernLightsTravel', 'northernlights@travel.com', 'www.northernlightstravel.com'),
(35, 'DynamicDiscoveries', 'dynamic@discoveries.com', 'www.dynamicdiscoveries.com'),
(36, 'SereneSojourns', 'serene@sojourns.com', 'www.serenesojourns.com');

-- Вставка данных в таблицу Packages
INSERT INTO Packages (package_id, tour_operator_id, destination_id, description, price, start_date, end_date)
VALUES
(1, 1, 3, 'Package to Rome', 1500.00, '2023-01-01', '2023-01-10'),
(2, 2, 1, 'Package to New York', 2000.00, '2023-02-01', '2023-02-10'),
(3, 3, 2, 'Package to Paris', 1800.00, '2023-03-01', '2023-03-10'),
(4, 4, 5, 'Package to Sydney', 2200.00, '2023-04-01', '2023-04-10'),
(5, 5, 4, 'Package to Tokyo', 2500.00, '2023-05-01', '2023-05-10'),
(6, 6, 6, 'Package to Vancouver', 1700.00, '2023-06-01', '2023-06-10'),
(7, 7, 7, 'Package to Barcelona', 1900.00, '2023-07-01', '2023-07-10'),
(8, 8, 8, 'Package to Berlin', 1600.00, '2023-08-01', '2023-08-10'),
(9, 9, 9, 'Package to Rio de Janeiro', 2300.00, '2023-09-01', '2023-09-10'),
(10, 10, 10, 'Package to Beijing', 2100.00, '2023-10-01', '2023-10-10');

INSERT INTO Packages (package_id, tour_operator_id, destination_id, description, price, start_date, end_date)
VALUES
(11, 1, 3, 'Available Package to Rome', 1200.00, '2023-01-01', '2023-01-10'),
(12, 2, 1, 'Available Package to New York', 1800.00, '2023-02-01', '2023-02-10'),
(13, 3, 2, 'Available Package to Paris', 1500.00, '2023-03-01', '2023-03-10'),
(14, 4, 5, 'Available Package to Sydney', 2000.00, '2023-04-01', '2023-04-10'),
(15, 5, 4, 'Available Package to Tokyo', 2200.00, '2023-05-01', '2023-05-10');
INSERT INTO Packages (package_id, tour_operator_id, destination_id, description, price, start_date, end_date)
VALUES
(21, 11, 13, 'Historic Seville Tour', 1200.00, '2023-01-15', '2023-01-22'),
(22, 12, 6, 'Beach Retreat in Sydney', 1800.00, '2023-02-05', '2023-02-12'),
(23, 13, 16, 'Cultural Extravaganza in Kyoto', 1500.00, '2023-03-10', '2023-03-17'),
(24, 14, 5, 'Luxury Getaway to Nice', 2000.00, '2023-04-02', '2023-04-09'),
(25, 15, 15, 'Hollywood Dreams in Los Angeles', 2200.00, '2023-05-20', '2023-05-27'),
(26, 16, 10, 'Shanghai Skyline Adventure', 1700.00, '2023-06-15', '2023-06-22'),
(27, 17, 7, 'Barcelona Bliss', 1900.00, '2023-07-08', '2023-07-15'),
(28, 18, 8, 'Munich Magic', 1600.00, '2023-08-12', '2023-08-19'),
(29, 19, 9, 'Samba Rhythms in Rio de Janeiro', 2300.00, '2023-09-01', '2023-09-08'),
(30, 20, 3, 'Beijing Explorer', 2100.00, '2023-10-10', '2023-10-17'),
(31, 21, 11, 'Santorini Serenity', 1400.00, '2023-11-05', '2023-11-12'),
(32, 22, 26, 'Mumbai Marvels', 2000.00, '2023-12-02', '2023-12-09'),
(33, 23, 25, 'Cape Town Safari Adventure', 2500.00, '2024-01-15', '2024-01-22'),
(34, 24, 4, 'Cancun Sun & Sand Retreat', 1800.00, '2024-02-05', '2024-02-12'),
(35, 25, 14, 'Bangkok Cultural Immersion', 1900.00, '2024-03-10', '2024-03-17'),
(36, 26, 27, 'White Nights in St. Petersburg', 1600.00, '2024-04-02', '2024-04-09'),
(37, 27, 28, 'Edinburgh Escapade', 2100.00, '2024-05-20', '2024-05-27'),
(38, 28, 17, 'Amsterdam Adventure', 1700.00, '2024-06-15', '2024-06-22'),
(39, 29, 1, 'Istanbul Delights', 2200.00, '2024-07-08', '2024-07-15'),
(40, 30, 30, 'Buenos Aires Tango Experience', 2000.00, '2024-08-12', '2024-08-19');
-- Вставка данных в таблицу Bookings
INSERT INTO Bookings (booking_id, customer_id, package_id, booking_date, payment_status)
VALUES
(1, 1, 3, '2023-01-05', 'Pending'),
(2, 2, 1, '2023-02-10', 'Paid'),
(3, 3, 2, '2023-03-15', 'Pending'),
(4, 4, 5, '2023-04-20', 'Paid'),
(5, 5, 4, '2023-05-25', 'Pending'),
(6, 6, 6, '2023-06-30', 'Paid'),
(7, 7, 7, '2023-07-05', 'Pending'),
(8, 8, 8, '2023-08-10', 'Paid'),
(9, 9, 9, '2023-09-15', 'Pending'),
(10, 10, 10, '2023-10-20', 'Paid');

INSERT INTO Bookings (booking_id, customer_id, package_id, booking_date, payment_status)
VALUES
(11, 1, 4, '2023-01-05', 'Pending'),
(12, 1, 1, '2023-02-10', 'Paid'),
(13, 1, 2, '2023-03-15', 'Pending'),
(14, 1, 4, '2023-04-20', 'Paid'),
(15, 2, 4, '2023-05-25', 'Pending'),
(16, 2, 5, '2023-06-30', 'Paid'),
(17, 2, 5, '2023-07-05', 'Pending');
INSERT INTO Bookings (booking_id, customer_id, package_id, booking_date, payment_status)
VALUES
(21, 11, 23, '2023-01-02', 'Pending'),
(22, 12, 36, '2023-02-12', 'Paid'),
(23, 13, 31, '2023-03-20', 'Pending'),
(24, 14, 29, '2023-04-25', 'Paid'),
(25, 15, 38, '2023-05-30', 'Pending'),
(26, 16, 22, '2023-06-18', 'Paid'),
(27, 17, 15, '2023-07-05', 'Pending'),
(28, 18, 28, '2023-08-10', 'Paid'),
(29, 19, 37, '2023-09-15', 'Pending'),
(30, 20, 30, '2023-10-20', 'Paid'),
(31, 21, 24, '2023-11-22', 'Pending'),
(32, 22, 14, '2023-12-28', 'Paid'),
(33, 23, 40, '2024-01-05', 'Pending'),
(34, 24, 22, '2024-02-14', 'Paid'),
(35, 25, 26, '2024-03-22', 'Pending'),
(36, 26, 27, '2024-04-30', 'Paid'),
(37, 27, 33, '2024-05-05', 'Pending'),
(38, 28, 10, '2024-06-10', 'Paid'),
(39, 29, 31, '2024-07-15', 'Pending'),
(40, 30, 35, '2024-08-20', 'Paid');
INSERT INTO Bookings (booking_id, customer_id, package_id, booking_date, payment_status)
VALUES
(41, 1, 1, '2023-07-13', 'Paid');

-- Вставка данных в таблицу Payments
INSERT INTO Payments (payment_id, booking_id, payment_date, amount)
VALUES
(1, 1, '2023-01-10', 1500.00),
(2, 2, '2023-02-15', 2000.00),
(3, 3, '2023-03-20', 1800.00),
(4, 4, '2023-04-25', 2200.00),
(5, 5, '2023-05-30', 2500.00),
(6, 6, '2023-07-05', 1700.00),
(7, 7, '2023-07-10', 1900.00),
(8, 8, '2023-08-15', 1600.00),
(9, 9, '2023-09-20', 2300.00),
(10, 10, '2023-10-25', 2100.00);

INSERT INTO Payments (payment_id, booking_id, payment_date, amount)
VALUES
(21, 21, '2023-01-10', 1200.00),
(22, 22, '2023-02-15', 1800.00),
(23, 23, '2023-03-22', 1500.00),
(24, 24, '2023-04-30', 2000.00),
(25, 25, '2023-06-05', 2200.00),
(26, 26, '2023-07-12', 1700.00),
(27, 27, '2023-08-18', 1900.00),
(28, 28, '2023-09-25', 1600.00),
(29, 29, '2023-10-30', 2300.00),
(30, 30, '2023-12-05', 2100.00),
(31, 31, '2024-01-10', 1400.00),
(32, 32, '2024-02-18', 2000.00),
(33, 33, '2024-03-25', 2500.00),
(34, 34, '2024-04-30', 1800.00),
(35, 35, '2024-06-05', 1900.00),
(36, 36, '2024-07-12', 1600.00),
(37, 37, '2024-08-18', 2100.00),
(38, 38, '2024-09-25', 1700.00),
(39, 39, '2024-10-30', 2200.00),
(40, 40, '2024-12-05', 2000.00);


DELETE FROM Bookings WHERE customer_id = 1;

-- Удаление клиента с ID 1
DELETE FROM Customers WHERE customer_id = 1;