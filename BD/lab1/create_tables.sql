-- Создание таблицы "Destinations" с названием города и страны
CREATE TABLE Destinations (
    destination_id INT PRIMARY KEY,
    country_name NVARCHAR(255) NOT NULL,
    city_name NVARCHAR(255) NOT NULL
);

-- Создание таблицы "Туроператоры" (Tour Operators)
CREATE TABLE TourOperators (
    tour_operator_id INT PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    contact_info NVARCHAR(255),
    website NVARCHAR(255)
);

-- Создание таблицы "Путевки" (Packages)
CREATE TABLE Packages (
    package_id INT PRIMARY KEY,
    tour_operator_id INT,
    destination_id INT, 
    description NVARCHAR(MAX),
    price DECIMAL(10, 2) NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    FOREIGN KEY (tour_operator_id) REFERENCES TourOperators(tour_operator_id),
    FOREIGN KEY (destination_id) REFERENCES Destinations(destination_id) 
);

-- Создание таблицы "Клиенты" (Customers)
CREATE TABLE Customers (
    customer_id INT PRIMARY KEY,
    first_name NVARCHAR(255) NOT NULL,
    last_name NVARCHAR(255) NOT NULL,
    email NVARCHAR(255),
    phone NVARCHAR(20),
    address NVARCHAR(MAX)
);

-- Создание таблицы "Бронирования" (Bookings)
CREATE TABLE Bookings (
    booking_id INT PRIMARY KEY,
    customer_id INT,
    package_id INT,
    booking_date DATE NOT NULL,
    payment_status NVARCHAR(50),
    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id),
    FOREIGN KEY (package_id) REFERENCES Packages(package_id)
);

-- Создание таблицы "Платежи" (Payments)
CREATE TABLE Payments (
    payment_id INT PRIMARY KEY,
    booking_id INT,
    payment_date DATE NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (booking_id) REFERENCES Bookings(booking_id)
);