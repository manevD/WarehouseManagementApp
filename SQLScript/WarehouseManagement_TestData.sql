/*
=============================================================
WarehouseManagement - TEST DATA
One SQL script for DE / MK / HR / EN

CHANGE ONLY:
    SET @Language = 'mk';
Supported:
    mk = Macedonian
    de = German
    hr = Croatian
    en = English

The script:
- inserts Categories
- Products
- Warehouses
- WarehouseLocations
- Suppliers
- Customers
- PurchaseOrders + Items
- Orders + Items
- Stock
- StockMovements
- Inventories + Items

It does NOT delete existing data.
Test values use a unique timestamp suffix.
=============================================================
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @Language nvarchar(2) = 'mk'; -- mk / de / hr / en

IF @Language NOT IN ('mk', 'de', 'hr', 'en')
    THROW 50000, 'Invalid language. Use mk, de, hr or en.', 1;

DECLARE @Now datetime2 = GETUTCDATE();
DECLARE @Suffix nvarchar(30) =
    CONVERT(nvarchar(8), @Now, 112) +
    REPLACE(CONVERT(nvarchar(8), @Now, 108), ':', '');

BEGIN TRY
    BEGIN TRANSACTION;

    /* =========================================================
       1. CATEGORIES
       ========================================================= */

    DECLARE @Categories TABLE
    (
        Id int,
        CategoryNo int
    );

    INSERT INTO Categories
    (
        Name,
        Description,
        IsActive,
        CreatedAt
    )
    SELECT
        CASE src.CategoryNo
            WHEN 1 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Електроника'
                    WHEN 'de' THEN N'Elektronik'
                    WHEN 'hr' THEN N'Elektronika'
                    WHEN 'en' THEN N'Electronics'
                END
            WHEN 2 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Канцелариски материјали'
                    WHEN 'de' THEN N'Bürobedarf'
                    WHEN 'hr' THEN N'Uredski materijal'
                    WHEN 'en' THEN N'Office Supplies'
                END
            WHEN 3 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Алат и опрема'
                    WHEN 'de' THEN N'Werkzeuge und Ausrüstung'
                    WHEN 'hr' THEN N'Alati i oprema'
                    WHEN 'en' THEN N'Tools and Equipment'
                END
            WHEN 4 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Потрошен материјал'
                    WHEN 'de' THEN N'Verbrauchsmaterial'
                    WHEN 'hr' THEN N'Potrošni materijal'
                    WHEN 'en' THEN N'Consumables'
                END
        END,
        CASE src.CategoryNo
            WHEN 1 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Компјутерска и електронска опрема'
                    WHEN 'de' THEN N'Computer- und Elektronikprodukte'
                    WHEN 'hr' THEN N'Računalna i elektronička oprema'
                    WHEN 'en' THEN N'Computer and electronic products'
                END
            WHEN 2 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Производи за канцеларија'
                    WHEN 'de' THEN N'Produkte für das Büro'
                    WHEN 'hr' THEN N'Proizvodi za ured'
                    WHEN 'en' THEN N'Products for the office'
                END
            WHEN 3 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Рачни алати и опрема'
                    WHEN 'de' THEN N'Handwerkzeuge und Ausrüstung'
                    WHEN 'hr' THEN N'Ručni alati i oprema'
                    WHEN 'en' THEN N'Hand tools and equipment'
                END
            WHEN 4 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Материјали за секојдневна употреба'
                    WHEN 'de' THEN N'Materialien für den täglichen Gebrauch'
                    WHEN 'hr' THEN N'Materijali za svakodnevnu upotrebu'
                    WHEN 'en' THEN N'Materials for everyday use'
                END
        END,
        1,
        @Now
    FROM
    (
        VALUES (1), (2), (3), (4)
    ) src(CategoryNo);

    INSERT INTO @Categories (Id, CategoryNo)
    SELECT c.Id,
           v.CategoryNo
    FROM Categories c
    INNER JOIN
    (
        VALUES
        (
            CASE @Language
                WHEN 'mk' THEN N'Електроника'
                WHEN 'de' THEN N'Elektronik'
                WHEN 'hr' THEN N'Elektronika'
                WHEN 'en' THEN N'Electronics'
            END, 1
        ),
        (
            CASE @Language
                WHEN 'mk' THEN N'Канцелариски материјали'
                WHEN 'de' THEN N'Bürobedarf'
                WHEN 'hr' THEN N'Uredski materijal'
                WHEN 'en' THEN N'Office Supplies'
            END, 2
        ),
        (
            CASE @Language
                WHEN 'mk' THEN N'Алат и опрема'
                WHEN 'de' THEN N'Werkzeuge und Ausrüstung'
                WHEN 'hr' THEN N'Alati i oprema'
                WHEN 'en' THEN N'Tools and Equipment'
            END, 3
        ),
        (
            CASE @Language
                WHEN 'mk' THEN N'Потрошен материјал'
                WHEN 'de' THEN N'Verbrauchsmaterial'
                WHEN 'hr' THEN N'Potrošni materijal'
                WHEN 'en' THEN N'Consumables'
            END, 4
        )
    ) v(Name, CategoryNo)
        ON c.Name = v.Name;

    /* =========================================================
       2. WAREHOUSES
       ========================================================= */

    DECLARE @Warehouses TABLE
    (
        Id int,
        WarehouseNo int
    );

    INSERT INTO Warehouses
    (
        Name,
        Address,
        City,
        PostalCode,
        Country,
        Description,
        IsActive,
        CreatedAt
    )
    SELECT
        CASE src.WarehouseNo
            WHEN 1 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Главен магацин'
                    WHEN 'de' THEN N'Hauptlager'
                    WHEN 'hr' THEN N'Glavno skladište'
                    WHEN 'en' THEN N'Main Warehouse'
                END
            WHEN 2 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Резервен магацин'
                    WHEN 'de' THEN N'Reservemagazin'
                    WHEN 'hr' THEN N'Rezervno skladište'
                    WHEN 'en' THEN N'Reserve Warehouse'
                END
        END,
        CASE src.WarehouseNo
            WHEN 1 THEN N'Индустриска зона 10'
            WHEN 2 THEN N'Индустриска зона 20'
        END,
        CASE src.WarehouseNo
            WHEN 1 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Скопје'
                    WHEN 'de' THEN N'Skopje'
                    WHEN 'hr' THEN N'Skopje'
                    WHEN 'en' THEN N'Skopje'
                END
            WHEN 2 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Битола'
                    WHEN 'de' THEN N'Bitola'
                    WHEN 'hr' THEN N'Bitola'
                    WHEN 'en' THEN N'Bitola'
                END
        END,
        CASE src.WarehouseNo WHEN 1 THEN N'1000' ELSE N'7000' END,
        CASE @Language
            WHEN 'mk' THEN N'Северна Македонија'
            WHEN 'de' THEN N'Nordmazedonien'
            WHEN 'hr' THEN N'Sjeverna Makedonija'
            WHEN 'en' THEN N'North Macedonia'
        END,
        CASE src.WarehouseNo
            WHEN 1 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Главен магацин за дневни операции.'
                    WHEN 'de' THEN N'Hauptlager für den täglichen Betrieb.'
                    WHEN 'hr' THEN N'Glavno skladište za svakodnevno poslovanje.'
                    WHEN 'en' THEN N'Main warehouse for daily operations.'
                END
            WHEN 2 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Резервен магацин за дополнителни залихи.'
                    WHEN 'de' THEN N'Reservelager für zusätzliche Bestände.'
                    WHEN 'hr' THEN N'Rezervno skladište za dodatne zalihe.'
                    WHEN 'en' THEN N'Reserve warehouse for additional stock.'
                END
        END,
        1,
        @Now
    FROM (VALUES (1), (2)) src(WarehouseNo);

    INSERT INTO @Warehouses (Id, WarehouseNo)
    SELECT w.Id,
           CASE w.Name
               WHEN CASE @Language WHEN 'mk' THEN N'Главен магацин' WHEN 'de' THEN N'Hauptlager' WHEN 'hr' THEN N'Glavno skladište' WHEN 'en' THEN N'Main Warehouse' END THEN 1
               WHEN CASE @Language WHEN 'mk' THEN N'Резервен магацин' WHEN 'de' THEN N'Reservemagazin' WHEN 'hr' THEN N'Rezervno skladište' WHEN 'en' THEN N'Reserve Warehouse' END THEN 2
           END
    FROM Warehouses w
    WHERE w.CreatedAt = @Now
      AND w.Name IN
      (
          CASE @Language WHEN 'mk' THEN N'Главен магацин' WHEN 'de' THEN N'Hauptlager' WHEN 'hr' THEN N'Glavno skladište' WHEN 'en' THEN N'Main Warehouse' END,
          CASE @Language WHEN 'mk' THEN N'Резервен магацин' WHEN 'de' THEN N'Reservemagazin' WHEN 'hr' THEN N'Rezervno skladište' WHEN 'en' THEN N'Reserve Warehouse' END
      );

    /* =========================================================
       3. LOCATIONS
       ========================================================= */

    DECLARE @Locations TABLE
    (
        Id int,
        WarehouseNo int,
        LocationNo int
    );

    INSERT INTO WarehouseLocations
    (
        Code,
        Name,
        Description,
        WarehouseId,
        IsActive
    )
    SELECT
        CONCAT(
            CASE src.WarehouseNo WHEN 1 THEN 'A' ELSE 'B' END,
            '-',
            RIGHT('0' + CONVERT(varchar(2), src.LocationNo), 2)
        ),
        CASE src.LocationNo
            WHEN 1 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Електроника'
                    WHEN 'de' THEN N'Elektronik'
                    WHEN 'hr' THEN N'Elektronika'
                    WHEN 'en' THEN N'Electronics'
                END
            WHEN 2 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Алат'
                    WHEN 'de' THEN N'Werkzeuge'
                    WHEN 'hr' THEN N'Alati'
                    WHEN 'en' THEN N'Tools'
                END
            WHEN 3 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Резервна зона'
                    WHEN 'de' THEN N'Reservezone'
                    WHEN 'hr' THEN N'Rezervna zona'
                    WHEN 'en' THEN N'Reserve Area'
                END
        END,
        CASE @Language
            WHEN 'mk' THEN N'Тест локација'
            WHEN 'de' THEN N'Testlagerplatz'
            WHEN 'hr' THEN N'Testna skladišna lokacija'
            WHEN 'en' THEN N'Test warehouse location'
        END,
        w.Id,
        1
    FROM (VALUES (1),(2)) wh(WarehouseNo)
    CROSS JOIN (VALUES (1),(2),(3)) src(LocationNo)
    INNER JOIN @Warehouses w
        ON w.WarehouseNo = wh.WarehouseNo;

    INSERT INTO @Locations (Id, WarehouseNo, LocationNo)
    SELECT l.Id,
           CASE LEFT(l.Code,1) WHEN 'A' THEN 1 ELSE 2 END,
           TRY_CONVERT(int, RIGHT(l.Code,2))
    FROM WarehouseLocations l
    WHERE l.Code LIKE '[AB]-[0-9][0-9]'
      AND l.IsActive = 1
      AND l.Description =
          CASE @Language
              WHEN 'mk' THEN N'Тест локација'
              WHEN 'de' THEN N'Testlagerplatz'
              WHEN 'hr' THEN N'Testna skladišna lokacija'
              WHEN 'en' THEN N'Test warehouse location'
          END;

    /* =========================================================
       4. SUPPLIERS
       ========================================================= */

    DECLARE @Suppliers TABLE
    (
        Id int,
        SupplierNo int
    );

    INSERT INTO Suppliers
    (
        CompanyName,
        ContactPerson,
        Email,
        Phone,
        Street,
        PostalCode,
        City,
        Country,
        VatNumber,
        SupplierNumber,
        Note,
        IsActive,
        CreatedAt
    )
    SELECT
        CASE src.SupplierNo
            WHEN 1 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Балкан Техника ДОО'
                    WHEN 'de' THEN N'Balkan Technik GmbH'
                    WHEN 'hr' THEN N'Balkan Tehnika d.o.o.'
                    WHEN 'en' THEN N'Balkan Technology Ltd.'
                END
            WHEN 2 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Еуро Канцеларија ДОО'
                    WHEN 'de' THEN N'Euro Büro GmbH'
                    WHEN 'hr' THEN N'Euro Ured d.o.o.'
                    WHEN 'en' THEN N'Euro Office Ltd.'
                END
            WHEN 3 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Про Алат ДОО'
                    WHEN 'de' THEN N'Pro Werkzeug GmbH'
                    WHEN 'hr' THEN N'Pro Alat d.o.o.'
                    WHEN 'en' THEN N'Pro Tools Ltd.'
                END
        END,
        CASE src.SupplierNo
            WHEN 1 THEN N'Александар Петров'
            WHEN 2 THEN N'Марина Илиевска'
            WHEN 3 THEN N'Иван Стојанов'
        END,
        CONCAT('supplier', src.SupplierNo, '_', @Suffix, '@example.com'),
        CONCAT('+389 70 55 1', RIGHT('00' + CONVERT(varchar(2), src.SupplierNo), 2)),
        CASE src.SupplierNo
            WHEN 1 THEN N'Булевар Македонија 15'
            WHEN 2 THEN N'Улица Деловна 22'
            WHEN 3 THEN N'Индустриска 8'
        END,
        N'1000',
        N'Скопје',
        CASE @Language
            WHEN 'mk' THEN N'Северна Македонија'
            WHEN 'de' THEN N'Nordmazedonien'
            WHEN 'hr' THEN N'Sjeverna Makedonija'
            WHEN 'en' THEN N'North Macedonia'
        END,
        CONCAT(N'MK', RIGHT('000000000' + CONVERT(nvarchar(9), src.SupplierNo), 9)),
        CONCAT(N'SUP-', RIGHT('00' + CONVERT(nvarchar(2), src.SupplierNo), 2), '-', @Suffix),
        CASE @Language
            WHEN 'mk' THEN N'Тест добавувач за демонстрација.'
            WHEN 'de' THEN N'Testlieferant für die Demonstration.'
            WHEN 'hr' THEN N'Testni dobavljač za demonstraciju.'
            WHEN 'en' THEN N'Test supplier for demonstration.'
        END,
        CASE WHEN src.SupplierNo = 3 THEN 0 ELSE 1 END,
        @Now
    FROM (VALUES (1),(2),(3)) src(SupplierNo);

    INSERT INTO @Suppliers (Id, SupplierNo)
    SELECT s.Id, TRY_CONVERT(int, SUBSTRING(s.SupplierNumber, 5, 2))
    FROM Suppliers s
    WHERE s.SupplierNumber LIKE 'SUP-[0-9][0-9]-%'
      AND s.CreatedAt = @Now;

    /* =========================================================
       5. CUSTOMERS
       ========================================================= */

    DECLARE @Customers TABLE
    (
        Id int,
        CustomerNo int
    );

    INSERT INTO Customers
    (
        CompanyName,
        ContactPerson,
        Email,
        Phone,
        Street,
        PostalCode,
        City,
        Country,
        VatNumber,
        CustomerNumber,
        Note,
        IsActive,
        CreatedAt
    )
    SELECT
        CASE src.CustomerNo
            WHEN 1 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Скопје Сервис ДОО'
                    WHEN 'de' THEN N'Skopje Service GmbH'
                    WHEN 'hr' THEN N'Skopje Servis d.o.o.'
                    WHEN 'en' THEN N'Skopje Service Ltd.'
                END
            WHEN 2 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Балкан Маркет ДОО'
                    WHEN 'de' THEN N'Balkan Markt GmbH'
                    WHEN 'hr' THEN N'Balkan Market d.o.o.'
                    WHEN 'en' THEN N'Balkan Market Ltd.'
                END
            WHEN 3 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Техно Плус ДОО'
                    WHEN 'de' THEN N'Techno Plus GmbH'
                    WHEN 'hr' THEN N'Tehno Plus d.o.o.'
                    WHEN 'en' THEN N'Techno Plus Ltd.'
                END
            WHEN 4 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Орион Инженеринг ДОО'
                    WHEN 'de' THEN N'Orion Engineering GmbH'
                    WHEN 'hr' THEN N'Orion Inženjering d.o.o.'
                    WHEN 'en' THEN N'Orion Engineering Ltd.'
                END
        END,
        CASE src.CustomerNo
            WHEN 1 THEN N'Петар Николов'
            WHEN 2 THEN N'Елена Трајкова'
            WHEN 3 THEN N'Даниел Костов'
            WHEN 4 THEN N'Стефан Ристов'
        END,
        CONCAT('customer', src.CustomerNo, '_', @Suffix, '@example.com'),
        CONCAT('+389 71 66 2', RIGHT('00' + CONVERT(varchar(2), src.CustomerNo), 2)),
        CASE src.CustomerNo
            WHEN 1 THEN N'Улица Сервисна 10'
            WHEN 2 THEN N'Булевар Партизански 44'
            WHEN 3 THEN N'Улица Техничка 7'
            WHEN 4 THEN N'Индустриска 12'
        END,
        N'1000',
        CASE src.CustomerNo
            WHEN 1 THEN N'Скопје'
            WHEN 2 THEN N'Скопје'
            WHEN 3 THEN N'Битола'
            WHEN 4 THEN N'Скопје'
        END,
        CASE @Language
            WHEN 'mk' THEN N'Северна Македонија'
            WHEN 'de' THEN N'Nordmazedonien'
            WHEN 'hr' THEN N'Sjeverna Makedonija'
            WHEN 'en' THEN N'North Macedonia'
        END,
        CONCAT(N'MK', RIGHT('000000000' + CONVERT(nvarchar(9), 100 + src.CustomerNo), 9)),
        CONCAT(N'CUST-', RIGHT('00' + CONVERT(varchar(2), src.CustomerNo), 2), '-', @Suffix),
        CASE @Language
            WHEN 'mk' THEN N'Тест клиент за демонстрација.'
            WHEN 'de' THEN N'Testkunde für die Demonstration.'
            WHEN 'hr' THEN N'Testni kupac za demonstraciju.'
            WHEN 'en' THEN N'Test customer for demonstration.'
        END,
        CASE WHEN src.CustomerNo = 4 THEN 0 ELSE 1 END,
        @Now
    FROM (VALUES (1),(2),(3),(4)) src(CustomerNo);

    INSERT INTO @Customers (Id, CustomerNo)
    SELECT c.Id, TRY_CONVERT(int, SUBSTRING(c.CustomerNumber, 6, 2))
    FROM Customers c
    WHERE c.CustomerNumber LIKE 'CUST-[0-9][0-9]-%'
      AND c.CreatedAt = @Now;

    /* =========================================================
       6. PRODUCTS
       ========================================================= */

    DECLARE @Products TABLE
    (
        Id int,
        ProductNo int
    );

    INSERT INTO Products
    (
        SKU,
        Name,
        Description,
        Barcode,
        PurchasePrice,
        SalePrice,
        MinimumStock,
        CategoryId,
        IsActive,
        CreatedAt
    )
    SELECT
        CONCAT('TEST-', RIGHT('00' + CONVERT(varchar(2), src.ProductNo), 2), '-', @Suffix),
        CASE src.ProductNo
            WHEN 1 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Безжичен глушец'
                    WHEN 'de' THEN N'Kabellose Maus'
                    WHEN 'hr' THEN N'Bežični miš'
                    WHEN 'en' THEN N'Wireless Mouse'
                END
            WHEN 2 THEN
                CASE @Language
                    WHEN 'mk' THEN N'USB тастатура'
                    WHEN 'de' THEN N'USB-Tastatur'
                    WHEN 'hr' THEN N'USB tipkovnica'
                    WHEN 'en' THEN N'USB Keyboard'
                END
            WHEN 3 THEN
                CASE @Language
                    WHEN 'mk' THEN N'HDMI кабел 2m'
                    WHEN 'de' THEN N'HDMI-Kabel 2m'
                    WHEN 'hr' THEN N'HDMI kabel 2m'
                    WHEN 'en' THEN N'HDMI Cable 2m'
                END
            WHEN 4 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Ласерски печатач'
                    WHEN 'de' THEN N'Laserdrucker'
                    WHEN 'hr' THEN N'Laserski pisač'
                    WHEN 'en' THEN N'Laser Printer'
                END
            WHEN 5 THEN
                CASE @Language
                    WHEN 'mk' THEN N'А4 хартија'
                    WHEN 'de' THEN N'A4-Papier'
                    WHEN 'hr' THEN N'A4 papir'
                    WHEN 'en' THEN N'A4 Paper'
                END
            WHEN 6 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Хемиско пенкало'
                    WHEN 'de' THEN N'Kugelschreiber'
                    WHEN 'hr' THEN N'Kemijska olovka'
                    WHEN 'en' THEN N'Ballpoint Pen'
                END
            WHEN 7 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Електричен шрафцигер'
                    WHEN 'de' THEN N'Elektrischer Schraubendreher'
                    WHEN 'hr' THEN N'Električni odvijač'
                    WHEN 'en' THEN N'Electric Screwdriver'
                END
            WHEN 8 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Комплет рачни алати'
                    WHEN 'de' THEN N'Handwerkzeug-Set'
                    WHEN 'hr' THEN N'Set ručnih alata'
                    WHEN 'en' THEN N'Hand Tool Set'
                END
            WHEN 9 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Заштитни ракавици'
                    WHEN 'de' THEN N'Schutzhandschuhe'
                    WHEN 'hr' THEN N'Zaštitne rukavice'
                    WHEN 'en' THEN N'Protective Gloves'
                END
            WHEN 10 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Изолир трака'
                    WHEN 'de' THEN N'Isolierband'
                    WHEN 'hr' THEN N'Izolir traka'
                    WHEN 'en' THEN N'Electrical Tape'
                END
            WHEN 11 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Мрежен прекинувач 8 порти'
                    WHEN 'de' THEN N'8-Port-Netzwerk-Switch'
                    WHEN 'hr' THEN N'8-portni mrežni preklopnik'
                    WHEN 'en' THEN N'8-Port Network Switch'
                END
            WHEN 12 THEN
                CASE @Language
                    WHEN 'mk' THEN N'Етернет кабел 10m'
                    WHEN 'de' THEN N'Ethernet-Kabel 10m'
                    WHEN 'hr' THEN N'Ethernet kabel 10m'
                    WHEN 'en' THEN N'Ethernet Cable 10m'
                END
        END,
        CASE @Language
            WHEN 'mk' THEN N'Тест производ за магацински податоци.'
            WHEN 'de' THEN N'Testprodukt für Lagerdaten.'
            WHEN 'hr' THEN N'Testni proizvod za skladišne podatke.'
            WHEN 'en' THEN N'Test product for warehouse data.'
        END,
        CONCAT('380', RIGHT('000000000000' + CONVERT(varchar(12), 100000000000 + src.ProductNo), 12)),
        CASE src.ProductNo
            WHEN 1 THEN 8.50
            WHEN 2 THEN 12.00
            WHEN 3 THEN 4.50
            WHEN 4 THEN 145.00
            WHEN 5 THEN 3.20
            WHEN 6 THEN 0.60
            WHEN 7 THEN 55.00
            WHEN 8 THEN 75.00
            WHEN 9 THEN 4.00
            WHEN 10 THEN 1.50
            WHEN 11 THEN 38.00
            WHEN 12 THEN 8.00
        END,
        CASE src.ProductNo
            WHEN 1 THEN 15.90
            WHEN 2 THEN 22.90
            WHEN 3 THEN 9.90
            WHEN 4 THEN 189.00
            WHEN 5 THEN 5.90
            WHEN 6 THEN 1.20
            WHEN 7 THEN 79.00
            WHEN 8 THEN 109.00
            WHEN 9 THEN 7.90
            WHEN 10 THEN 3.50
            WHEN 11 THEN 59.00
            WHEN 12 THEN 14.90
        END,
        CASE src.ProductNo
            WHEN 1 THEN 10
            WHEN 2 THEN 10
            WHEN 3 THEN 15
            WHEN 4 THEN 3
            WHEN 5 THEN 20
            WHEN 6 THEN 50
            WHEN 7 THEN 5
            WHEN 8 THEN 4
            WHEN 9 THEN 20
            WHEN 10 THEN 30
            WHEN 11 THEN 5
            WHEN 12 THEN 10
        END,
        c.Id,
        CASE WHEN src.ProductNo = 10 THEN 0 ELSE 1 END,
        @Now
    FROM (VALUES
        (1),(2),(3),(4),(5),(6),
        (7),(8),(9),(10),(11),(12)
    ) src(ProductNo)
    INNER JOIN @Categories c
        ON c.CategoryNo =
            CASE
                WHEN src.ProductNo BETWEEN 1 AND 4 THEN 1
                WHEN src.ProductNo BETWEEN 5 AND 6 THEN 2
                WHEN src.ProductNo BETWEEN 7 AND 8 THEN 3
                ELSE 4
            END;

    INSERT INTO @Products (Id, ProductNo)
    SELECT p.Id,
           TRY_CONVERT(int, SUBSTRING(p.SKU, 7, 2))
    FROM Products p
    WHERE p.SKU LIKE 'TEST-[0-9][0-9]-%'
      AND p.CreatedAt = @Now;

    /* =========================================================
       7. PURCHASE ORDERS + ITEMS
       ========================================================= */

    DECLARE @PurchaseOrders TABLE
    (
        Id int,
        PurchaseNo int
    );

    INSERT INTO PurchaseOrders
    (
        OrderNumber,
        SupplierId,
        OrderDate,
        Status,
        ReferenceNumber,
        Note,
        Total,
        CreatedAt,
        UpdatedAt
    )
    SELECT
        CONCAT('PO-TEST-', RIGHT('00' + CONVERT(varchar(2), src.PurchaseNo), 2), '-', @Suffix),
        s.Id,
        DATEADD(day, -src.PurchaseNo * 3, @Now),
        CASE src.PurchaseNo
            WHEN 1 THEN 4 -- Geliefert
            WHEN 2 THEN 3 -- TeilweiseGeliefert
            WHEN 3 THEN 2 -- Bestellt
            ELSE 1        -- Entwurf
        END,
        CONCAT('SUP-REF-', src.PurchaseNo, '-', @Suffix),
        CASE @Language
            WHEN 'mk' THEN N'Тест нарачка кон добавувач.'
            WHEN 'de' THEN N'Testbestellung an Lieferanten.'
            WHEN 'hr' THEN N'Testna narudžba dobavljaču.'
            WHEN 'en' THEN N'Test purchase order to supplier.'
        END,
        0,
        @Now,
        @Now
    FROM (VALUES (1),(2),(3),(4)) src(PurchaseNo)
    INNER JOIN @Suppliers s
        ON s.SupplierNo = CASE src.PurchaseNo WHEN 4 THEN 3 ELSE src.PurchaseNo END;

    INSERT INTO @PurchaseOrders (Id, PurchaseNo)
    SELECT po.Id, TRY_CONVERT(int, SUBSTRING(po.OrderNumber, 9, 2))
    FROM PurchaseOrders po
    WHERE po.OrderNumber LIKE 'PO-TEST-[0-9][0-9]-%'
      AND po.CreatedAt = @Now;

    DECLARE @POItems TABLE
    (
        PurchaseOrderId int,
        ProductId int,
        Quantity decimal(18,3),
        UnitPrice decimal(18,2),
        DeliveredQuantity decimal(18,3)
    );

    INSERT INTO PurchaseOrderItems
    (
        PurchaseOrderId,
        ProductId,
        Quantity,
        UnitPrice,
        DeliveredQuantity
    )
    OUTPUT
        inserted.PurchaseOrderId,
        inserted.ProductId,
        inserted.Quantity,
        inserted.UnitPrice,
        inserted.DeliveredQuantity
    INTO @POItems
    SELECT
        po.Id,
        p.Id,
        CASE p.ProductNo
            WHEN 1 THEN 30
            WHEN 2 THEN 25
            WHEN 3 THEN 40
            WHEN 4 THEN 10
        END,
        pr.PurchasePrice,
        CASE po.PurchaseNo
            WHEN 1 THEN
                CASE p.ProductNo
                    WHEN 1 THEN 30 WHEN 2 THEN 25 WHEN 3 THEN 40 WHEN 4 THEN 10
                END
            WHEN 2 THEN
                CASE p.ProductNo
                    WHEN 1 THEN 15 WHEN 2 THEN 10 WHEN 3 THEN 20 WHEN 4 THEN 5
                END
            ELSE 0
        END
    FROM @PurchaseOrders po
    INNER JOIN
    (
        VALUES (1),(2),(3),(4)
    ) x(ProductNo)
        ON 1 = 1
    INNER JOIN @Products p
        ON p.ProductNo = x.ProductNo
    INNER JOIN Products pr
        ON pr.Id = p.Id;

    UPDATE po
    SET Total =
    (
        SELECT COALESCE(SUM(i.Quantity * i.UnitPrice), 0)
        FROM PurchaseOrderItems i
        WHERE i.PurchaseOrderId = po.Id
    )
    FROM PurchaseOrders po
    INNER JOIN @PurchaseOrders x
        ON x.Id = po.Id;

    /* =========================================================
       8. ORDERS + ITEMS
       ========================================================= */

    DECLARE @Orders TABLE
    (
        Id int,
        OrderNo int
    );

    INSERT INTO Orders
    (
        OrderNumber,
        CustomerId,
        OrderDate,
        Status,
        ReferenceNumber,
        DeliveryStreet,
        DeliveryPostalCode,
        DeliveryCity,
        DeliveryCountry,
        Note,
        Total,
        CreatedAt,
        UpdatedAt
    )
    SELECT
        CONCAT('SO-TEST-', RIGHT('00' + CONVERT(varchar(2), src.OrderNo), 2), '-', @Suffix),
        c.Id,
        DATEADD(day, -src.OrderNo, @Now),
        CASE src.OrderNo
            WHEN 1 THEN 6 -- Abgeschlossen
            WHEN 2 THEN 5 -- Versendet
            WHEN 3 THEN 2 -- Bestätigt
            ELSE 1        -- Entwurf
        END,
        CONCAT('CUST-REF-', src.OrderNo, '-', @Suffix),
        N'Улица Клиентска 5',
        N'1000',
        N'Скопје',
        CASE @Language
            WHEN 'mk' THEN N'Северна Македонија'
            WHEN 'de' THEN N'Nordmazedonien'
            WHEN 'hr' THEN N'Sjeverna Makedonija'
            WHEN 'en' THEN N'North Macedonia'
        END,
        CASE @Language
            WHEN 'mk' THEN N'Тест продажна нарачка.'
            WHEN 'de' THEN N'Testkundenauftrag.'
            WHEN 'hr' THEN N'Testna prodajna narudžba.'
            WHEN 'en' THEN N'Test customer order.'
        END,
        0,
        @Now,
        @Now
    FROM (VALUES (1),(2),(3),(4)) src(OrderNo)
    INNER JOIN @Customers c
        ON c.CustomerNo = src.OrderNo;

    INSERT INTO @Orders (Id, OrderNo)
    SELECT o.Id, TRY_CONVERT(int, SUBSTRING(o.OrderNumber, 9, 2))
    FROM Orders o
    WHERE o.OrderNumber LIKE 'SO-TEST-[0-9][0-9]-%'
      AND o.CreatedAt = @Now;

    INSERT INTO OrderItems
    (
        OrderId,
        ProductId,
        Quantity,
        UnitPrice,
        WarehouseLocationId
    )
    SELECT
        o.Id,
        p.Id,
        CASE o.OrderNo
            WHEN 1 THEN CASE p.ProductNo WHEN 1 THEN 2 WHEN 4 THEN 1 WHEN 7 THEN 2 ELSE 3 END
            WHEN 2 THEN CASE p.ProductNo WHEN 2 THEN 2 WHEN 5 THEN 10 ELSE 4 END
            WHEN 3 THEN CASE p.ProductNo WHEN 3 THEN 5 WHEN 8 THEN 1 ELSE 2 END
            ELSE CASE p.ProductNo WHEN 11 THEN 2 WHEN 12 THEN 5 ELSE 1 END
        END,
        pr.SalePrice,
        l.Id
    FROM @Orders o
    INNER JOIN @Products p
        ON p.ProductNo IN
        (
            CASE o.OrderNo WHEN 1 THEN 1 WHEN 2 THEN 2 WHEN 3 THEN 3 ELSE 11 END,
            CASE o.OrderNo WHEN 1 THEN 4 WHEN 2 THEN 5 WHEN 3 THEN 8 ELSE 12 END
        )
    INNER JOIN Products pr
        ON pr.Id = p.Id
    INNER JOIN @Locations l
        ON l.WarehouseNo = 1
        AND l.LocationNo =
            CASE
                WHEN p.ProductNo IN (1,2,3,4) THEN 1
                WHEN p.ProductNo IN (7,8) THEN 2
                ELSE 3
            END;

    UPDATE o
    SET Total =
    (
        SELECT COALESCE(SUM(i.Quantity * i.UnitPrice), 0)
        FROM OrderItems i
        WHERE i.OrderId = o.Id
    )
    FROM Orders o
    INNER JOIN @Orders x
        ON x.Id = o.Id;

    /* =========================================================
       9. STOCK
       ========================================================= */

    INSERT INTO Stocks
    (
        Quantity,
        ReservedQuantity,
        ProductId,
        WarehouseLocationId,
        UpdatedAt
    )
    SELECT
        CASE p.ProductNo
            WHEN 1 THEN 25
            WHEN 2 THEN 18
            WHEN 3 THEN 55
            WHEN 4 THEN 8
            WHEN 5 THEN 12
            WHEN 6 THEN 80
            WHEN 7 THEN 7
            WHEN 8 THEN 3
            WHEN 9 THEN 30
            WHEN 10 THEN 0
            WHEN 11 THEN 12
            WHEN 12 THEN 22
        END,
        CASE p.ProductNo
            WHEN 1 THEN 5
            WHEN 2 THEN 2
            WHEN 3 THEN 10
            WHEN 4 THEN 2
            ELSE 0
        END,
        p.Id,
        l.Id,
        @Now
    FROM @Products p
    INNER JOIN @Locations l
        ON l.WarehouseNo = 1
        AND l.LocationNo =
            CASE
                WHEN p.ProductNo BETWEEN 1 AND 4 THEN 1
                WHEN p.ProductNo BETWEEN 7 AND 8 THEN 2
                ELSE 3
            END;

    INSERT INTO Stocks
    (
        Quantity,
        ReservedQuantity,
        ProductId,
        WarehouseLocationId,
        UpdatedAt
    )
    SELECT
        CASE p.ProductNo
            WHEN 1 THEN 10
            WHEN 2 THEN 12
            WHEN 3 THEN 20
            WHEN 4 THEN 4
            WHEN 5 THEN 15
            WHEN 6 THEN 40
            ELSE 5
        END,
        0,
        p.Id,
        l.Id,
        @Now
    FROM @Products p
    INNER JOIN @Locations l
        ON l.WarehouseNo = 2
        AND l.LocationNo =
            CASE
                WHEN p.ProductNo BETWEEN 1 AND 4 THEN 1
                WHEN p.ProductNo BETWEEN 7 AND 8 THEN 2
                ELSE 3
            END
    WHERE p.ProductNo <= 8;

    /* =========================================================
       10. STOCK MOVEMENTS
       ========================================================= */

    DECLARE @MainLocation int =
    (
        SELECT TOP 1 Id
        FROM @Locations
        WHERE WarehouseNo = 1 AND LocationNo = 1
    );

    DECLARE @ToolLocation int =
    (
        SELECT TOP 1 Id
        FROM @Locations
        WHERE WarehouseNo = 1 AND LocationNo = 2
    );

    DECLARE @ReserveLocation int =
    (
        SELECT TOP 1 Id
        FROM @Locations
        WHERE WarehouseNo = 1 AND LocationNo = 3
    );

    DECLARE @Supplier1 int =
    (
        SELECT Id FROM @Suppliers WHERE SupplierNo = 1
    );

    DECLARE @Supplier2 int =
    (
        SELECT Id FROM @Suppliers WHERE SupplierNo = 2
    );

    DECLARE @PO1 int =
    (
        SELECT Id FROM @PurchaseOrders WHERE PurchaseNo = 1
    );

    DECLARE @PO2 int =
    (
        SELECT Id FROM @PurchaseOrders WHERE PurchaseNo = 2
    );

    INSERT INTO StockMovements
    (
        ProductId,
        Quantity,
        Type,
        FromWarehouseLocationId,
        ToWarehouseLocationId,
        SupplierId,
        PurchaseOrderId,
        ReferenceNumber,
        Note,
        UserId,
        CreatedAt
    )
    VALUES
    (
        (SELECT Id FROM @Products WHERE ProductNo = 1),
        30,
        1,
        NULL,
        @MainLocation,
        @Supplier1,
        @PO1,
        CONCAT('IN-', @Suffix),
        CASE @Language
            WHEN 'mk' THEN N'Прием на стока од добавувач.'
            WHEN 'de' THEN N'Wareneingang vom Lieferanten.'
            WHEN 'hr' THEN N'Ulaz robe od dobavljača.'
            WHEN 'en' THEN N'Goods received from supplier.'
        END,
        NULL,
        DATEADD(day, -8, @Now)
    ),
    (
        (SELECT Id FROM @Products WHERE ProductNo = 2),
        15,
        1,
        NULL,
        @MainLocation,
        @Supplier2,
        @PO2,
        CONCAT('IN-', @Suffix, '-02'),
        CASE @Language
            WHEN 'mk' THEN N'Делумна испорака.'
            WHEN 'de' THEN N'Teil-Lieferung.'
            WHEN 'hr' THEN N'Djelomična isporuka.'
            WHEN 'en' THEN N'Partial delivery.'
        END,
        NULL,
        DATEADD(day, -6, @Now)
    ),
    (
        (SELECT Id FROM @Products WHERE ProductNo = 7),
        5,
        3,
        @MainLocation,
        @ToolLocation,
        NULL,
        NULL,
        CONCAT('MOVE-', @Suffix),
        CASE @Language
            WHEN 'mk' THEN N'Преместување во зона за алати.'
            WHEN 'de' THEN N'Umlagerung in den Werkzeugbereich.'
            WHEN 'hr' THEN N'Premještanje u zonu alata.'
            WHEN 'en' THEN N'Transfer to tools area.'
        END,
        NULL,
        DATEADD(day, -4, @Now)
    ),
    (
        (SELECT Id FROM @Products WHERE ProductNo = 4),
        2,
        2,
        @MainLocation,
        NULL,
        NULL,
        NULL,
        CONCAT('OUT-', @Suffix),
        CASE @Language
            WHEN 'mk' THEN N'Испорака кон клиент.'
            WHEN 'de' THEN N'Warenausgang zum Kunden.'
            WHEN 'hr' THEN N'Izlaz robe prema kupcu.'
            WHEN 'en' THEN N'Goods issue to customer.'
        END,
        NULL,
        DATEADD(day, -2, @Now)
    ),
    (
        (SELECT Id FROM @Products WHERE ProductNo = 10),
        5,
        5,
        NULL,
        @ReserveLocation,
        NULL,
        NULL,
        CONCAT('CORR-', @Suffix),
        CASE @Language
            WHEN 'mk' THEN N'Корекција на залиха.'
            WHEN 'de' THEN N'Bestandskorrektur.'
            WHEN 'hr' THEN N'Korekcija zalihe.'
            WHEN 'en' THEN N'Stock correction.'
        END,
        NULL,
        DATEADD(day, -1, @Now)
    );

    /* =========================================================
       11. INVENTORIES + ITEMS
       ========================================================= */

    DECLARE @Inventories TABLE
    (
        Id int,
        InventoryNo int
    );

    INSERT INTO Inventories
    (
        WarehouseId,
        Status,
        Note,
        UserId,
        CreatedAt,
        CompletedAt
    )
    SELECT
        w.Id,
        CASE src.InventoryNo
            WHEN 1 THEN 3 -- Abgeschlossen
            WHEN 2 THEN 2 -- InBearbeitung
            ELSE 1        -- Entwurf
        END,
        CASE @Language
            WHEN 'mk' THEN N'Тест попис на залиха.'
            WHEN 'de' THEN N'Testinventur des Lagerbestands.'
            WHEN 'hr' THEN N'Testni popis zaliha.'
            WHEN 'en' THEN N'Test inventory count.'
        END,
        NULL,
        DATEADD(day, -src.InventoryNo, @Now),
        CASE
            WHEN src.InventoryNo = 1 THEN DATEADD(hour, -12, @Now)
            ELSE NULL
        END
    FROM (VALUES (1),(2),(3)) src(InventoryNo)
    INNER JOIN @Warehouses w
        ON w.WarehouseNo = CASE src.InventoryNo WHEN 3 THEN 2 ELSE 1 END;

    INSERT INTO @Inventories (Id, InventoryNo)
    SELECT i.Id,
           ROW_NUMBER() OVER (ORDER BY i.CreatedAt DESC)
    FROM Inventories i
    WHERE i.CreatedAt IN
    (
        DATEADD(day, -1, @Now),
        DATEADD(day, -2, @Now),
        DATEADD(day, -3, @Now)
    );

    INSERT INTO InventoryItems
    (
        InventoryId,
        ProductId,
        WarehouseLocationId,
        SystemQuantity,
        CountedQuantity,
        Difference
    )
    SELECT
        inv.Id,
        p.Id,
        l.Id,
        s.Quantity,
        CASE inv.InventoryNo
            WHEN 1 THEN s.Quantity
            WHEN 2 THEN
                CASE
                    WHEN p.ProductNo = 1 THEN s.Quantity - 2
                    WHEN p.ProductNo = 4 THEN s.Quantity + 1
                    ELSE s.Quantity
                END
            ELSE NULL
        END,
        CASE inv.InventoryNo
            WHEN 1 THEN 0
            WHEN 2 THEN
                CASE
                    WHEN p.ProductNo = 1 THEN -2
                    WHEN p.ProductNo = 4 THEN 1
                    ELSE 0
                END
            ELSE 0
        END
    FROM @Inventories inv
    INNER JOIN @Products p
        ON p.ProductNo IN (1,4,7)
    INNER JOIN @Locations l
        ON l.WarehouseNo =
            CASE inv.InventoryNo WHEN 3 THEN 2 ELSE 1 END
        AND l.LocationNo =
            CASE
                WHEN p.ProductNo IN (1,4) THEN 1
                ELSE 2
            END
    INNER JOIN Stocks s
        ON s.ProductId = p.Id
        AND s.WarehouseLocationId = l.Id;

    /* =========================================================
       SUMMARY
       ========================================================= */

    COMMIT TRANSACTION;

    SELECT
        @Language AS Language,
        (SELECT COUNT(*) FROM @Categories) AS CategoriesInserted,
        (SELECT COUNT(*) FROM @Warehouses) AS WarehousesInserted,
        (SELECT COUNT(*) FROM @Locations) AS LocationsInserted,
        (SELECT COUNT(*) FROM @Suppliers) AS SuppliersInserted,
        (SELECT COUNT(*) FROM @Customers) AS CustomersInserted,
        (SELECT COUNT(*) FROM @Products) AS ProductsInserted,
        (SELECT COUNT(*) FROM @PurchaseOrders) AS PurchaseOrdersInserted,
        (SELECT COUNT(*) FROM @Orders) AS OrdersInserted,
        (SELECT COUNT(*) FROM Stocks WHERE UpdatedAt >= @Now) AS StocksInserted,
        (SELECT COUNT(*) FROM StockMovements WHERE CreatedAt >= DATEADD(second, -1, @Now)) AS MovementsInserted,
        (SELECT COUNT(*) FROM @Inventories) AS InventoriesInserted;

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
