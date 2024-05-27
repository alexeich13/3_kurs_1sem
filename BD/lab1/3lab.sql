alter table Customers
add node hierarchyid,
Level as node.GetLevel() PERSISTED,
HierarchyPath as node.ToString() PERSISTED;
select * from Customers;

go;

select * from Bookings

SELECT
    node.ToString() AS NodeAsString,
    node AS NodeAsBinary,
    Level AS Level,
    customer_id,
    first_name,
    last_name,
    email,
    phone,
	address
FROM
    Customers;

CREATE PROCEDURE UpdateCustomerHierarchy
AS
BEGIN
    DECLARE @Level hierarchyid;
	SELECT @Level = node FROM Customers WHERE customer_id = 2;
    UPDATE Customers
    SET
         node = hierarchyid::GetRoot()
     FROM (
        SELECT TOP 1 WITH TIES
            c.customer_id,
            ROW_NUMBER() OVER (ORDER BY COUNT(b.booking_id) DESC) AS RowNum
        FROM Customers c
        LEFT JOIN Bookings b ON c.customer_id = b.customer_id
        GROUP BY c.customer_id
        ORDER BY COUNT(b.booking_id) DESC
    ) AS MaxBookingCustomer
    WHERE Customers.customer_id = MaxBookingCustomer.customer_id;
	WITH CustomerBookingCount AS (
        SELECT
            c.customer_id,
            COUNT(b.booking_id) AS BookingCount
        FROM Customers c
        LEFT JOIN Bookings b ON c.customer_id = b.customer_id
        GROUP BY c.customer_id
        HAVING COUNT(b.booking_id) > 2
    )
	
    UPDATE Customers
    SET
        node = hierarchyid::GetRoot().GetDescendant(NULL, NULL)
    FROM CustomerBookingCount
    WHERE Customers.customer_id = CustomerBookingCount.customer_id
	AND node IS NULL;
	WITH SubCustomerBookingCount AS (
        SELECT
            c.customer_id,
            COUNT(b.booking_id) AS BookingCount
        FROM Customers c
        LEFT JOIN Bookings b ON c.customer_id = b.customer_id
        GROUP BY c.customer_id
        HAVING COUNT(b.booking_id) >= 0
    )
    UPDATE Customers
    SET
        node = @Level.GetDescendant(NULL, NULL)
    FROM SubCustomerBookingCount
    WHERE Customers.customer_id = SubCustomerBookingCount.customer_id
	and node is null;
END;

exec UpdateCustomerHierarchy;

drop procedure UpdateCustomerHierarchy;

select * from Customers;


CREATE PROCEDURE MoveSubtree
    @SourceNodeValue hierarchyid,
    @DestinationNodeValue hierarchyid
AS
BEGIN
    DECLARE @SubtreeRootValue hierarchyid
    SELECT @SubtreeRootValue = node
    FROM Customers
    WHERE node = @SourceNodeValue
    -- изменение узла родителя
    UPDATE Customers
    SET node = node.GetReparentedValue(@SubtreeRootValue, @DestinationNodeValue)
    WHERE node.IsDescendantOf(@SubtreeRootValue) = 1
END

DECLARE @SourceNodeValueToMove hierarchyid
DECLARE @DestinationNodeToMoveValue hierarchyid

SET @SourceNodeValueToMove = '/1/1/' 
SET @DestinationNodeToMoveValue = '/1/'
-- Вызов хранимой процедуры
EXEC MoveSubtree @SourceNodeValueToMove, @DestinationNodeToMoveValue

ALTER TABLE Customers
DROP COLUMN node;

ALTER TABLE Customers
DROP COLUMN Level;

ALTER TABLE Customers
DROP COLUMN HierarchyPath;

CREATE PROCEDURE UpdateCustomerHierarchyDes
AS
BEGIN
    DECLARE @Level hierarchyid;
	SELECT @Level = node FROM Customers WHERE customer_id = 8;
	UPDATE Customers
    SET
        node = @Level.GetDescendant(NULL, NULL)
    FROM Customers
	where customer_id = 9
end;

drop procedure UpdateCustomerHierarchyDes
exec UpdateCustomerHierarchyDes;