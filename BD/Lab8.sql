CREATE OR REPLACE TYPE book_obj AS OBJECT (
    book_id NUMBER,
    image BLOB,
    description CLOB,
    price NUMBER,
    quantity NUMBER,
    author_id NUMBER,
    polygraphy_id NUMBER,
    genre_id NUMBER,
    publish_date DATE,
    CONSTRUCTOR FUNCTION book_obj(self IN OUT NOCOPY book_obj, p_book_id NUMBER, p_price NUMBER) RETURN SELF AS RESULT,
    MAP MEMBER FUNCTION get_book_id RETURN NUMBER,
    MEMBER FUNCTION get_discounted_price(p_discount NUMBER) RETURN NUMBER,
    MEMBER PROCEDURE update_price(p_new_price NUMBER)
);

CREATE OR REPLACE TYPE BODY book_obj AS
    -- доп конструктор
    CONSTRUCTOR FUNCTION book_obj(self IN OUT NOCOPY book_obj, p_book_id NUMBER, p_price NUMBER) RETURN SELF AS RESULT IS
    BEGIN
        self.book_id := p_book_id;
        self.price := p_price;
        RETURN;
    END;
    -- метод
    MAP MEMBER FUNCTION get_book_id RETURN NUMBER IS
    BEGIN
        RETURN book_id;
    END;
    -- экземпляр функции
    MEMBER FUNCTION get_discounted_price(p_discount NUMBER) RETURN NUMBER IS
    BEGIN
        RETURN price * (1 - p_discount);
    END;
    -- экземпляр процедуры
    MEMBER PROCEDURE update_price(p_new_price NUMBER) IS
    BEGIN
        price := p_new_price;
    END;
END;

CREATE TABLE book_obj_table OF book_obj;

INSERT INTO book_obj_table
SELECT book_obj(book_id, image, description, price, quantity, author_id, polygraphy_id, genre_id, publish_date)
FROM Book;

SELECT * FROM book_obj_table;

CREATE VIEW book_obj_view AS
SELECT VALUE(b) AS book FROM book_obj_table b;

SELECT * FROM book_obj_table;


-- Создание индекса на атрибут
CREATE INDEX book_obj_table_price_idx ON book_obj_table(price);


DROP INDEX book_obj_table_price_idx;
-- Создание индекса на метод
CREATE INDEX book_obj_book_id_idx ON book_obj_table (book_id);

DROP INDEX book_obj_book_id_idx;

SELECT * FROM book_obj_table WHERE price > 50;

SELECT * FROM book_obj_table WHERE book_id = book_obj(123, null, null, null, null, null, null, null, null).get_book_id();


----2

CREATE OR REPLACE TYPE Author_obj AS OBJECT (
    author_id NUMBER,
    author_name VARCHAR2(255),
    author_surname VARCHAR2(255),
    author_patronymic VARCHAR2(255),
    CONSTRUCTOR FUNCTION Author_obj(self IN OUT NOCOPY Author_obj, p_author_id NUMBER, p_author_name VARCHAR2) RETURN SELF AS RESULT,
    MAP MEMBER FUNCTION get_author_id RETURN NUMBER,
    MEMBER FUNCTION get_full_name RETURN VARCHAR2,
    MEMBER PROCEDURE update_name(p_new_name VARCHAR2)
);


CREATE OR REPLACE TYPE BODY Author_obj AS
--доп конструктор
    CONSTRUCTOR FUNCTION Author_obj(self IN OUT NOCOPY Author_obj, p_author_id NUMBER, p_author_name VARCHAR2) RETURN SELF AS RESULT IS
    BEGIN
        self.author_id := p_author_id;
        self.author_name := p_author_name;
        RETURN;
    END;
--метод
    MAP MEMBER FUNCTION get_author_id RETURN NUMBER IS
    BEGIN
        RETURN author_id;
    END;
--экземпляр функции
    MEMBER FUNCTION get_full_name RETURN VARCHAR2 IS
    BEGIN
        RETURN author_name || ' ' || author_surname;
    END;
--экземпляр процедуры
    MEMBER PROCEDURE update_name(p_new_name VARCHAR2) IS
    BEGIN
        author_name := p_new_name;
    END;
END;

--перенос в таблицу
CREATE TABLE Author_obj_table OF Author_obj;

INSERT INTO Author_obj_table
SELECT Author_obj(author_id, author_name, author_surname, author_patronymic)
FROM Author;

--представление

CREATE VIEW Author_obj_view AS
SELECT VALUE(a) AS author FROM Author_obj_table a;

SELECT * FROM Author_obj_view;

-- Создание индексов
CREATE INDEX author_obj_id_idx ON Author_obj_table (author_id);

DROP INDEX author_obj_id_idx;

CREATE INDEX author_obj_name_idx ON Author_obj_table (author_name);


DROP INDEX author_obj_name_idx;

SELECT * FROM Author_obj_table WHERE author_id = 1;

SELECT * FROM Author_obj_table WHERE author_name = 'Лев';