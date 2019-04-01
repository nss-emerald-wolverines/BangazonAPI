DELETE FROM OrderProduct;
DELETE FROM ComputerEmployee;
DELETE FROM EmployeeTraining;
DELETE FROM Employee;
DELETE FROM TrainingProgram;
DELETE FROM Computer;
DELETE FROM Department;
DELETE FROM [Order];
DELETE FROM PaymentType;
DELETE FROM Product;
DELETE FROM ProductType;
DELETE FROM Customer;


ALTER TABLE Employee DROP CONSTRAINT [FK_EmployeeDepartment];
ALTER TABLE ComputerEmployee DROP CONSTRAINT [FK_ComputerEmployee_Employee];
ALTER TABLE ComputerEmployee DROP CONSTRAINT [FK_ComputerEmployee_Computer];
ALTER TABLE EmployeeTraining DROP CONSTRAINT [FK_EmployeeTraining_Employee];
ALTER TABLE EmployeeTraining DROP CONSTRAINT [FK_EmployeeTraining_Training];
ALTER TABLE Product DROP CONSTRAINT [FK_Product_ProductType];
ALTER TABLE Product DROP CONSTRAINT [FK_Product_Customer];
ALTER TABLE PaymentType DROP CONSTRAINT [FK_PaymentType_Customer];
ALTER TABLE [Order] DROP CONSTRAINT [FK_Order_Customer];
ALTER TABLE [Order] DROP CONSTRAINT [FK_Order_Payment];
ALTER TABLE OrderProduct DROP CONSTRAINT [FK_OrderProduct_Product];
ALTER TABLE OrderProduct DROP CONSTRAINT [FK_OrderProduct_Order];


DROP TABLE IF EXISTS OrderProduct;
DROP TABLE IF EXISTS ComputerEmployee;
DROP TABLE IF EXISTS EmployeeTraining;
DROP TABLE IF EXISTS Employee;
DROP TABLE IF EXISTS TrainingProgram;
DROP TABLE IF EXISTS Computer;
DROP TABLE IF EXISTS Department;
DROP TABLE IF EXISTS [Order];
DROP TABLE IF EXISTS PaymentType;
DROP TABLE IF EXISTS Product;
DROP TABLE IF EXISTS ProductType;
DROP TABLE IF EXISTS Customer;


CREATE TABLE Department (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(55) NOT NULL,
	Budget 	INTEGER NOT NULL
);

CREATE TABLE Employee (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	FirstName VARCHAR(55) NOT NULL,
	LastName VARCHAR(55) NOT NULL,
	DepartmentId INTEGER NOT NULL,
	IsSuperVisor BIT NOT NULL DEFAULT(0),
    CONSTRAINT FK_EmployeeDepartment FOREIGN KEY(DepartmentId) REFERENCES Department(Id)
);

CREATE TABLE Computer (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	PurchaseDate DATETIME NOT NULL,
	DecomissionDate DATETIME,
	Make VARCHAR(55) NOT NULL,
	Manufacturer VARCHAR(55) NOT NULL
);

CREATE TABLE ComputerEmployee (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	EmployeeId INTEGER NOT NULL,
	ComputerId INTEGER NOT NULL,
	AssignDate DATETIME NOT NULL,
	UnassignDate DATETIME,
    CONSTRAINT FK_ComputerEmployee_Employee FOREIGN KEY(EmployeeId) REFERENCES Employee(Id),
    CONSTRAINT FK_ComputerEmployee_Computer FOREIGN KEY(ComputerId) REFERENCES Computer(Id)
);


CREATE TABLE TrainingProgram (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(255) NOT NULL,
	StartDate DATETIME NOT NULL,
	EndDate DATETIME NOT NULL,
	MaxAttendees INTEGER NOT NULL
);

CREATE TABLE EmployeeTraining (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	EmployeeId INTEGER NOT NULL,
	TrainingProgramId INTEGER NOT NULL,
    CONSTRAINT FK_EmployeeTraining_Employee FOREIGN KEY(EmployeeId) REFERENCES Employee(Id),
    CONSTRAINT FK_EmployeeTraining_Training FOREIGN KEY(TrainingProgramId) REFERENCES TrainingProgram(Id)
);

CREATE TABLE ProductType (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(55) NOT NULL
);

CREATE TABLE Customer (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	FirstName VARCHAR(55) NOT NULL,
	LastName VARCHAR(55) NOT NULL
);

CREATE TABLE Product (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	ProductTypeId INTEGER NOT NULL,
	CustomerId INTEGER NOT NULL,
	Price INTEGER NOT NULL,
	Title VARCHAR(255) NOT NULL,
	[Description] VARCHAR(255) NOT NULL,
	Quantity INTEGER NOT NULL,
    CONSTRAINT FK_Product_ProductType FOREIGN KEY(ProductTypeId) REFERENCES ProductType(Id),
    CONSTRAINT FK_Product_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id)
);


CREATE TABLE PaymentType (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	AcctNumber INTEGER NOT NULL,
	[Name] VARCHAR(55) NOT NULL,
	CustomerId INTEGER NOT NULL,
    CONSTRAINT FK_PaymentType_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id)
);

CREATE TABLE [Order] (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	CustomerId INTEGER NOT NULL,
	PaymentTypeId INTEGER,
    CONSTRAINT FK_Order_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id),
    CONSTRAINT FK_Order_Payment FOREIGN KEY(PaymentTypeId) REFERENCES PaymentType(Id)
);

CREATE TABLE OrderProduct (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	OrderId INTEGER NOT NULL,
	ProductId INTEGER NOT NULL,
    CONSTRAINT FK_OrderProduct_Product FOREIGN KEY(ProductId) REFERENCES Product(Id),
    CONSTRAINT FK_OrderProduct_Order FOREIGN KEY(OrderId) REFERENCES [Order](Id));

Insert into Customer (Firstname ,Lastname)
Values('Miriam' , 'Goldstein')

Insert into Customer (Firstname ,Lastname)
Values('Mark' , 'Coldbloom')

Insert into Customer (Firstname ,Lastname)
Values('Sorin' , 'Heights')

Insert into Customer (Firstname ,Lastname)
Values('Coral' , 'Rose')

Insert into Customer (Firstname ,Lastname)
Values('Jojo' , 'Belstein')

Insert into Customer (Firstname ,Lastname)
Values('Korin' , 'Beldandy')

Insert into Customer (Firstname ,Lastname)
Values('Kars' , 'Flunderberg')

INSERT INTO ProductType (Name)
VALUES ('Poultry');

INSERT INTO ProductType (Name)
VALUES ('Fish');

INSERT INTO ProductType (Name)
VALUES ('Pork');

INSERT INTO ProductType (Name)
VALUES ('Beef');

INSERT INTO ProductType (Name)
VALUES ('Lamb');

INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, [Description], Quantity)
VALUES (1, 1, 4.99, 'Chicken Thighs', 'Extra Lean Chicken Thighs', 7);

INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, [Description], Quantity)
VALUES (1, 1, 5.99, 'Chicken Breasts', 'Skinless Chicken Breasts', 11);

INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, [Description], Quantity)
VALUES (1, 1, 6.99, 'Roasted Chicken', 'Organic Whole Roasted Chicken', 4);

INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, [Description], Quantity)
VALUES (2, 1, 50.99, 'green Lobseter' ,' a Super Rare Breed of Lobster', 1);

INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, [Description], Quantity)
VALUES (2, 1, 25.99, 'Blue Lobster' ,' a Rare Breed of Lobster', 2);

INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, [Description], Quantity)
VALUES (2, 1, 10.99, 'red Lobster' ,' a Common Breed of Lobster', 5);

INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, [Description], Quantity)
VALUES (2, 4, 40.99, 'Shark Fins' ,'A great Ingredient for soup', 1);

INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, [Description], Quantity)
VALUES (2, 2, 5.99, 'Cod' ,'A Common Fish', 15);

INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, [Description], Quantity)
VALUES (4, 1, 15.99, 'Porterhouse', 'Butcher Choice', 4),
(5, 1, 16.99, 'Lamb Chops', 'Organic', 2),
(4, 1, 9.99, 'Sirloin', 'Top Sirloin', 4),
(4, 1, 11.99, 'Ribeye', 'Ready to Grill', 3),
(4, 1, 12.99, 'T-Bone', 'Juicy', 3);

INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, [Description], Quantity)
VALUES (3, 1, 9.99, 'Rosted Ham' ,'a lovley Pound of  ham' ,3);

INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, [Description], Quantity)
VALUES (3, 1, 7.99, 'Pickiled Pigs Feet' ,'From the famous galoshues maaaket' ,1);


Insert into PaymentType (AcctNumber ,[Name] ,CustomerId)
Values(267345 , 'Visa', 1);

Insert into PaymentType (AcctNumber ,[Name] ,CustomerId)
Values(0786544 , 'Mastercard', 4);

Insert into PaymentType (AcctNumber ,[Name] ,CustomerId)
Values(89467095 , 'Debit', 2);

Insert into PaymentType (AcctNumber ,[Name] ,CustomerId)
Values(267345 , 'Visa', 2);

Insert into PaymentType (AcctNumber ,[Name] ,CustomerId)
Values(89467095 , 'American Express', 6);

Insert into PaymentType (AcctNumber ,[Name] ,CustomerId)
Values(89467095 , 'MasterCard', 5);

Insert into PaymentType (AcctNumber ,[Name] ,CustomerId)
Values(89467095 , 'Debit', 1);

Insert into PaymentType (AcctNumber ,[Name] ,CustomerId)
Values(89467095 , 'Visa', 3);

Insert into PaymentType (AcctNumber ,[Name] ,CustomerId)
Values(89467095 , 'Visa', 7);

Insert into PaymentType (AcctNumber ,[Name] ,CustomerId)
Values(89467095 , 'American Express', 2);

Insert into PaymentType (AcctNumber ,[Name] ,CustomerId)
Values(89467095 , 'Mastercard', 1);

Insert into PaymentType (AcctNumber ,[Name] ,CustomerId)
Values(89467095 , 'Discover', 6);

Insert into PaymentType (AcctNumber ,[Name] ,CustomerId)
Values(89467095 , 'Visa', 5);

Insert into PaymentType (AcctNumber ,[Name] ,CustomerId)
Values(89467095 , 'Discover', 3);

Insert into PaymentType (AcctNumber ,[Name] ,CustomerId)
Values(89467095 , 'Visa', 4);

INSERT INTO [Order] (CustomerId, PaymentTypeId)
VALUES
(1, 1),
(1, 7),
(1, 11),
(2, 3),
(2, 4),
(2, 10),
(3, 8),
(3, 14),
(4, 2),
(4, 15),
(5, 6),
(5, 13),
(6, 5),
(6, 12),
(7, 9);


INSERT INTO OrderProduct (OrderId, ProductId)
VALUES (1, 1),
(1, 3),
(2, 1),
(2, 4),
(3, 2),
(4, 2),
(4, 3),
(5, 6),
(5, 14),
(6, 11),
(7, 2),
(7, 5),
(7, 12),
(8, 3),
(9, 14),
(9, 11),
(10, 4),
(11, 9),
(12, 8),
(13, 5),
(14, 13),
(15, 1);
