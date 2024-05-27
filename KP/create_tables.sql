-- Создание таблицы "Roles"
CREATE TABLE Roles (
    role_id INT IDENTITY(1,1) PRIMARY KEY,
    role_name NVARCHAR(50) NOT NULL
);

-- Создание таблицы "Destinations"
CREATE TABLE Destinations (
    destination_id INT IDENTITY(1,1) PRIMARY KEY,
    country_name NVARCHAR(255) NOT NULL,
    city_name NVARCHAR(255) NOT NULL
);

-- Создание таблицы "TourOperators"
CREATE TABLE TourOperators (
    tour_operator_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    contact_info NVARCHAR(255),
    website NVARCHAR(255)
);

-- Создание таблицы "Packages"
CREATE TABLE Packages (
    package_id INT IDENTITY(1,1) PRIMARY KEY,
    tour_operator_id INT,
    destination_id INT, 
    description NVARCHAR(MAX),
    price DECIMAL(10, 2) NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    FOREIGN KEY (tour_operator_id) REFERENCES TourOperators(tour_operator_id),
    FOREIGN KEY (destination_id) REFERENCES Destinations(destination_id) 
);

-- Создание таблицы "Users"
CREATE TABLE Users (
    users_id INT IDENTITY(1,1) PRIMARY KEY,
    first_name NVARCHAR(255) NOT NULL,
    last_name NVARCHAR(255) NOT NULL,
    login_user NVARCHAR(255) NOT NULL,
    password_user NVARCHAR(255) NOT NULL,
    email NVARCHAR(255),
    role_id INT,
    FOREIGN KEY (role_id) REFERENCES Roles(role_id)
);

-- Создание таблицы "Bookings"
CREATE TABLE Bookings (
    booking_id INT IDENTITY(1,1) PRIMARY KEY,
    users_id INT,
    package_id INT,
    booking_date DATE NOT NULL,
    payment_status NVARCHAR(50),
    FOREIGN KEY (users_id) REFERENCES Users(users_id),
    FOREIGN KEY (package_id) REFERENCES Packages(package_id)
);

-- Создание таблицы "Payments"
CREATE TABLE Payments (
    payment_id INT IDENTITY(1,1) PRIMARY KEY,
    booking_id INT ,
    payment_date DATE NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (booking_id) REFERENCES Bookings(booking_id)
);

INSERT INTO Roles (role_name) VALUES ('Пользователь');
-- Добавление роли "Администратор"
INSERT INTO Roles (role_name) VALUES ('Администратор');

-- Заполнение таблицы Destinations
INSERT INTO Destinations (country_name, city_name)
VALUES
    ('USA', 'New York'),
    ('France', 'Paris'),
    ('Italy', 'Rome'),
    ('Japan', 'Tokyo'),
    ('Brazil', 'Rio de Janeiro'),
    ('Australia', 'Sydney'),
    ('South Africa', 'Cape Town'),
    ('Canada', 'Toronto'),
    ('India', 'Mumbai'),
    ('China', 'Beijing');

-- Заполнение таблицы TourOperators
INSERT INTO TourOperators (name, contact_info, website)
VALUES
    ('ABC Travel', '123-456-7890', 'www.abctravel.com'),
    ('XYZ Tours', '987-654-3210', 'www.xyztours.com'),
    ('Global Adventures', '555-123-4567', 'www.globaladventures.com'),
    ('Sunshine Travel', '111-222-3333', 'www.sunshinetravel.com'),
    ('Peak Expeditions', '777-888-9999', 'www.peakexpeditions.com'),
    ('Ocean Voyages', '444-555-6666', 'www.oceanvoyages.com'),
    ('Skyline Tours', '666-777-8888', 'www.skylinetours.com'),
    ('Epic Journeys', '333-444-5555', 'www.epicjourneys.com'),
    ('Discover World', '888-999-0000', 'www.discoverworld.com'),
    ('Happy Travels', '222-333-4444', 'www.happytravels.com');

-- Заполнение таблицы Packages
-- Заполнение таблицы Packages с датами в будущем
INSERT INTO Packages (tour_operator_id, destination_id, description, price, start_date, end_date)
VALUES
    (1, 1, 'Explore the Big Apple', 1200.00, '2024-01-01', '2024-01-10'),
    (2, 3, 'Romantic Getaway in Paris', 2000.00, '2024-02-15', '2024-02-25'),
    (3, 5, 'Historical Tour of Rome', 1800.00, '2024-03-10', '2024-03-20'),
    (4, 7, 'Discover Tokyo', 2500.00, '2024-04-05', '2024-04-15'),
    (5, 9, 'Carnival in Rio de Janeiro', 1500.00, '2024-05-01', '2024-05-10'),
    (6, 2, 'Scenic Beauty of Sydney', 2200.00, '2024-06-15', '2024-06-25'),
    (7, 4, 'Safari Adventure in Cape Town', 3000.00, '2024-07-10', '2024-07-20'),
    (8, 6, 'Explore Toronto', 1700.00, '2024-08-01', '2024-08-10'),
    (9, 8, 'Mumbai Magic', 1900.00, '2024-09-15', '2024-09-25'),
    (10, 10, 'Great Wall of China', 2800.00, '2024-10-10', '2024-10-20');

-- Заполнение таблицы Users
INSERT INTO Users (first_name, last_name, login_user, password_user, email, role_id)
VALUES
    ('John', 'Doe', 'john_doe', 'password123', 'john.doe@example.com', 1),
    ('Jane', 'Smith', 'jane_smith', 'pass456', 'jane.smith@example.com', 1),
    ('Bob', 'Johnson', 'bob_johnson', 'secure789', 'bob.johnson@example.com', 1),
    ('Alice', 'Williams', 'alice_williams', 'strongpass', 'alice.williams@example.com', 1),
    ('Charlie', 'Davis', 'charlie_davis', 'safepassword', 'charlie.davis@example.com', 1),
    ('Eva', 'Anderson', 'eva_anderson', 'password321', 'eva.anderson@example.com', 1),
    ('Michael', 'Clark', 'michael_clark', 'mypassword', 'michael.clark@example.com', 1),
    ('Sophie', 'Brown', 'sophie_brown', 'mypassword123', 'sophie.brown@example.com', 1),
    ('David', 'Taylor', 'david_taylor', 'pass1234', 'david.taylor@example.com', 1),
    ('Emma', 'Jones', 'emma_jones', 'secret123', 'emma.jones@example.com', 1);

-- Заполнение таблицы Bookings
INSERT INTO Bookings (users_id, package_id, booking_date, payment_status)
VALUES
    (1, 1, '2023-04-20', 'Paid'),
    (2, 3, '2023-06-01', 'Pending'),
    (3, 5, '2023-07-15', 'Paid'),
    (4, 7, '2023-09-05', 'Pending'),
    (5, 9, '2023-11-01', 'Paid'),
    (6, 2, '2023-12-20', 'Pending'),
    (7, 4, '2024-01-10', 'Paid'),
    (8, 6, '2024-03-01', 'Pending'),
    (9, 8, '2024-04-15', 'Paid'),
    (10, 10, '2024-06-01', 'Paid');

-- Заполнение таблицы Payments
INSERT INTO Payments (booking_id, payment_date, amount)
VALUES
    (1, '2023-04-22', 1200.00),
    (3, '2023-07-17', 1800.00),
    (5, '2023-11-05', 1500.00),
    (7, '2024-01-15', 1700.00),
    (9, '2024-04-20', 2800.00),
    (10, '2024-06-03', 2800.00);
