-- =========================================================
-- Скрипт создания базы данных для системы продажи обуви
-- СУБД: Microsoft SQL Server 2017+
-- Нормализация: 3НФ, ссылочная целостность через FK
-- =========================================================

IF DB_ID('ShoeStoreDB') IS NOT NULL
BEGIN
    ALTER DATABASE ShoeStoreDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE ShoeStoreDB;
END
GO

CREATE DATABASE ShoeStoreDB;
GO

USE ShoeStoreDB;
GO

-- =========================================================
-- Таблица ролей пользователей
-- =========================================================
CREATE TABLE Roles
(
    RoleId   INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE
);
GO

-- =========================================================
-- Таблица пользователей
-- =========================================================
CREATE TABLE Users
(
    UserId    INT IDENTITY(1,1) PRIMARY KEY,
    Login     NVARCHAR(50)  NOT NULL UNIQUE,
    Password  NVARCHAR(100) NOT NULL,
    FullName  NVARCHAR(150) NOT NULL,
    RoleId    INT           NOT NULL,
    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);
GO

-- =========================================================
-- Категории товаров
-- =========================================================
CREATE TABLE Categories
(
    CategoryId   INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL UNIQUE
);
GO

-- =========================================================
-- Производители
-- =========================================================
CREATE TABLE Manufacturers
(
    ManufacturerId   INT IDENTITY(1,1) PRIMARY KEY,
    ManufacturerName NVARCHAR(100) NOT NULL UNIQUE
);
GO

-- =========================================================
-- Поставщики
-- =========================================================
CREATE TABLE Suppliers
(
    SupplierId   INT IDENTITY(1,1) PRIMARY KEY,
    SupplierName NVARCHAR(100) NOT NULL UNIQUE
);
GO

-- =========================================================
-- Единицы измерения
-- =========================================================
CREATE TABLE Units
(
    UnitId   INT IDENTITY(1,1) PRIMARY KEY,
    UnitName NVARCHAR(30) NOT NULL UNIQUE
);
GO

-- =========================================================
-- Товары
-- =========================================================
CREATE TABLE Products
(
    ProductId       INT IDENTITY(1,1) PRIMARY KEY,
    ProductName     NVARCHAR(200)  NOT NULL,
    Description     NVARCHAR(1000) NULL,
    CategoryId      INT            NOT NULL,
    ManufacturerId  INT            NOT NULL,
    SupplierId      INT            NOT NULL,
    UnitId          INT            NOT NULL,
    Price           DECIMAL(10,2)  NOT NULL CHECK (Price >= 0),
    Quantity        INT            NOT NULL CHECK (Quantity >= 0),
    Discount        INT            NOT NULL DEFAULT 0 CHECK (Discount BETWEEN 0 AND 100),
    ImagePath       NVARCHAR(300)  NULL,
    CONSTRAINT FK_Products_Categories    FOREIGN KEY (CategoryId)     REFERENCES Categories(CategoryId),
    CONSTRAINT FK_Products_Manufacturers FOREIGN KEY (ManufacturerId) REFERENCES Manufacturers(ManufacturerId),
    CONSTRAINT FK_Products_Suppliers     FOREIGN KEY (SupplierId)     REFERENCES Suppliers(SupplierId),
    CONSTRAINT FK_Products_Units         FOREIGN KEY (UnitId)         REFERENCES Units(UnitId)
);
GO

-- =========================================================
-- Статусы заказов
-- =========================================================
CREATE TABLE OrderStatuses
(
    StatusId   INT IDENTITY(1,1) PRIMARY KEY,
    StatusName NVARCHAR(50) NOT NULL UNIQUE
);
GO

-- =========================================================
-- Пункты выдачи
-- =========================================================
CREATE TABLE PickupPoints
(
    PickupPointId INT IDENTITY(1,1) PRIMARY KEY,
    Address       NVARCHAR(300) NOT NULL UNIQUE
);
GO

-- =========================================================
-- Заказы
-- =========================================================
CREATE TABLE Orders
(
    OrderId        INT IDENTITY(1,1) PRIMARY KEY,
    OrderCode      NVARCHAR(20)  NOT NULL UNIQUE,  -- артикул заказа
    StatusId       INT           NOT NULL,
    PickupPointId  INT           NOT NULL,
    OrderDate      DATE          NOT NULL,
    DeliveryDate   DATE          NULL,
    UserId         INT           NULL,
    CONSTRAINT FK_Orders_Statuses     FOREIGN KEY (StatusId)      REFERENCES OrderStatuses(StatusId),
    CONSTRAINT FK_Orders_PickupPoints FOREIGN KEY (PickupPointId) REFERENCES PickupPoints(PickupPointId),
    CONSTRAINT FK_Orders_Users        FOREIGN KEY (UserId)        REFERENCES Users(UserId)
);
GO

-- =========================================================
-- Состав заказа (товары в заказе)
-- =========================================================
CREATE TABLE OrderItems
(
    OrderItemId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId     INT NOT NULL,
    ProductId   INT NOT NULL,
    Quantity    INT NOT NULL CHECK (Quantity > 0),
    CONSTRAINT FK_OrderItems_Orders   FOREIGN KEY (OrderId)   REFERENCES Orders(OrderId)   ON DELETE CASCADE,
    CONSTRAINT FK_OrderItems_Products FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);
GO

-- =========================================================
-- Заполнение справочников
-- =========================================================
INSERT INTO Roles (RoleName) VALUES
(N'Клиент'), (N'Менеджер'), (N'Администратор');

INSERT INTO Users (Login, Password, FullName, RoleId) VALUES
(N'client',  N'client',  N'Иванов Иван Иванович',         1),
(N'manager', N'manager', N'Петров Пётр Петрович',         2),
(N'admin',   N'admin',   N'Сидоров Сидор Сидорович',      3);

INSERT INTO Categories (CategoryName) VALUES
(N'Кроссовки'), (N'Ботинки'), (N'Туфли'), (N'Сапоги'), (N'Кеды'), (N'Сандалии');

INSERT INTO Manufacturers (ManufacturerName) VALUES
(N'Nike'), (N'Adidas'), (N'Puma'), (N'Reebok'), (N'New Balance'), (N'Asics');

INSERT INTO Suppliers (SupplierName) VALUES
(N'ООО "СпортОпт"'),
(N'ООО "ОбувьТорг"'),
(N'ИП "Иванов А.А."'),
(N'ООО "ШузМаркет"');

INSERT INTO Units (UnitName) VALUES
(N'пара'), (N'шт');

INSERT INTO OrderStatuses (StatusName) VALUES
(N'Новый'), (N'В обработке'), (N'Готов к выдаче'), (N'Завершён'), (N'Отменён');

INSERT INTO PickupPoints (Address) VALUES
(N'г. Москва, ул. Ленина, д. 1'),
(N'г. Москва, ул. Пушкина, д. 10'),
(N'г. Санкт-Петербург, Невский пр., д. 25'),
(N'г. Казань, ул. Баумана, д. 5');

-- =========================================================
-- Тестовые товары
-- =========================================================
INSERT INTO Products (ProductName, Description, CategoryId, ManufacturerId, SupplierId, UnitId, Price, Quantity, Discount, ImagePath) VALUES
(N'Air Max 90',        N'Классические кроссовки',                 1, 1, 1, 1, 8990.00, 15,  0, NULL),
(N'Ultraboost 22',     N'Беговые кроссовки',                      1, 2, 1, 1,12990.00,  8, 10, NULL),
(N'Suede Classic',     N'Замшевые кеды',                          5, 3, 2, 1, 5490.00,  0,  0, NULL),
(N'Classic Leather',   N'Кожаные кеды',                           5, 4, 2, 1, 6790.00, 22, 20, NULL),
(N'574 Core',          N'Кроссовки в стиле casual',               1, 5, 3, 1, 9990.00,  4,  5, NULL),
(N'Gel-Kayano 29',     N'Профессиональные беговые кроссовки',     1, 6, 1, 1,14990.00, 12, 25, NULL),
(N'Ботинки зимние',    N'Тёплые зимние ботинки',                  2, 2, 4, 1, 7990.00,  6,  0, NULL),
(N'Туфли офисные',     N'Классические мужские туфли',             3, 4, 2, 1, 4990.00, 18,  0, NULL),
(N'Сапоги демисезон',  N'Удобные демисезонные сапоги',            4, 3, 4, 1, 6990.00,  0, 30, NULL),
(N'Сандалии летние',   N'Лёгкие летние сандалии',                 6, 1, 3, 1, 3490.00, 25, 18, NULL);

-- =========================================================
-- Тестовые заказы
-- =========================================================
INSERT INTO Orders (OrderCode, StatusId, PickupPointId, OrderDate, DeliveryDate, UserId) VALUES
(N'ORD-00001', 1, 1, '2026-05-20', '2026-05-25', 1),
(N'ORD-00002', 2, 2, '2026-05-21', '2026-05-26', 1),
(N'ORD-00003', 4, 1, '2026-05-15', '2026-05-19', 1);

INSERT INTO OrderItems (OrderId, ProductId, Quantity) VALUES
(1, 1, 1),
(1, 4, 2),
(2, 2, 1),
(3, 8, 1);

GO

PRINT N'База данных ShoeStoreDB успешно создана.';
GO
