-- Tạo bảng Users
CREATE TABLE Users (
    IdUser INT IDENTITY(1,1) PRIMARY KEY,        -- Khóa chính tự động tăng    
    Email NVARCHAR(100) NOT NULL,                -- Đây là email và cũng là tài khoản đăng nhập
    PasswordHash NVARCHAR(255) NOT NULL,         -- Đây là mật khẩu user (md5)                                                           
    IsAdmin INT DEFAULT 0,                       -- Role tài khoản (0.User, 1.Admin) 
    IsEmailVerified BIT DEFAULT 0,               -- Đã xác minh email chưa (hệ thống gửi email để verify email)
    TokenPassword NVARCHAR(255),                 -- Đây là mật khẩu (token) để lấy lại mật khẩu             
    Stt  INT DEFAULT 0,							 -- Trạng thái tài khoản  (0. Bình thường, 1. Bị khoá)              
    CreatedAt DATETIME DEFAULT GETDATE(),        -- Này không cần quan tâm vì nó tự update 
    Ip varchar(20)                               -- Lưu lại địa chỉ IP khi tạo tài khoản
);

-- Thêm ràng buộc duy nhất cho Email 
ALTER TABLE Users ADD CONSTRAINT UQ_Email UNIQUE (Email);

-- Tạo bảng Categories
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,    -- Khóa chính tự động tăng     
    CategoryName NVARCHAR(100) NOT NULL             
);

-- Tạo bảng Promotions (Khuyến mãi)
CREATE TABLE Promotions (
    PromotionID INT IDENTITY(1,1) PRIMARY KEY,     -- Khóa chính tự động tăng
    PromotionName NVARCHAR(255) NOT NULL,          -- Tên khuyến mãi
    Description NVARCHAR(1000),                    -- Mô tả khuyến mãi
    DiscountAmount DECIMAL(10, 2),                 -- Số tiền giảm giá (hoặc tỷ lệ giảm giá)
    DiscountPercentage DECIMAL(5, 2),              -- Tỷ lệ giảm giá (nếu là giảm theo phần trăm)
    StartDate DATE NOT NULL,                       -- Ngày bắt đầu khuyến mãi
    EndDate DATE NOT NULL                          -- Ngày kết thúc khuyến mãi
);

-- Tạo bảng Sizes (Kích thước)
CREATE TABLE Sizes (
    SizeID INT IDENTITY(1,1) PRIMARY KEY,           -- Khóa chính, tự động tăng
    Size NVARCHAR(50) NOT NULL                      -- Kích thước
);

-- Tạo bảng Colors (Màu sắc)
CREATE TABLE Colors (
    ColorID INT IDENTITY(1,1) PRIMARY KEY,          -- Khóa chính, tự động tăng
    Color NVARCHAR(50) NOT NULL                     -- Màu sắc
);

-- Tạo bảng Products
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,    -- Khóa chính tự động tăng    
    ProductName NVARCHAR(255) NOT NULL,             
    Price DECIMAL(10, 2) NOT NULL,                                                   
    Description NVARCHAR(1000),                     
    ImageURL NVARCHAR(255),                     -- link hình ảnh    
    CategoryID INT,                             -- Khoá ngoại đến bảng Categories    
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID) ON DELETE CASCADE
);

-- Tạo bảng AddressUser
CREATE TABLE AddressUser (
    IdAddress INT IDENTITY(1,1) PRIMARY KEY,     -- Khóa chính, tự động tăng     
    IdUser INT,                                  -- Khóa ngoại đến bảng Users     
    FullName NVARCHAR(50),                            
    Phone NVARCHAR(15),                               
    Province NVARCHAR(255),                           
    Town NVARCHAR(255),                               
    Block NVARCHAR(255),                              
    SpecificAddress NVARCHAR(255),                    
    CONSTRAINT FK_AddressUser_Users FOREIGN KEY (IdUser) REFERENCES Users(IdUser) ON DELETE CASCADE
);

-- Tạo bảng Orders
CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,       -- Khóa chính tự động tăng
    UserID INT,                                  -- Khóa ngoại đến bảng Users (người dùng đặt hàng)
    AddressID INT,                               -- Khóa ngoại đến bảng AddressUser (địa chỉ giao hàng)
    OrderDate DATE NOT NULL,                     -- Ngày đặt hàng
    status INT,                                  -- Trạng thái đơn hàng(1.Đang xử lí, 0.Đã huỷ, 2. Thành công)
    CONSTRAINT FK_Orders_Users FOREIGN KEY (UserID) REFERENCES Users(IdUser),
    CONSTRAINT FK_Orders_AddressUser FOREIGN KEY (AddressID) REFERENCES AddressUser(IdAddress)
);

-- Tạo bảng OrderDetails (chi tiết đơn hàng)
CREATE TABLE OrderDetails (
    OrderDetailID INT IDENTITY(1,1) PRIMARY KEY,    -- Khóa chính, tự động tăng 
    OrderID INT,                                    -- Khóa ngoại đến bảng Orders
    ProductID INT,                                  -- Khóa ngoại đến bảng Products
    Quantity INT NOT NULL,                          
    UnitPrice DECIMAL(10, 2) NOT NULL,              
    CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE,
    CONSTRAINT FK_OrderDetails_Products FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);

-- Bảng Stock (Lưu thông tin bán nhập kho)
CREATE TABLE Stock (
    StockID INT IDENTITY(1,1) PRIMARY KEY,           -- Khóa chính tự động tăng
    ProductID INT,                                   -- Khóa ngoại đến bảng Products
    StockType INT,                                   -- Loại giao dịch (1.Nhập vào, 0.bán ra)
    Quantity INT NOT NULL,                           -- Số lượng nhập vào, bán ra
    TransactionDate DATETIME DEFAULT GETDATE(),      -- Ngày giao dịch (ngày nhập vào, bán ra)
    Notes NVARCHAR(500),                             -- Ghi chú cho giao dịch (nếu cần)
    CONSTRAINT FK_Stock_Products FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);

-- Tạo bảng ProductPromotions (liên kết sản phẩm với khuyến mãi)
CREATE TABLE ProductPromotions (
    ProductPromotionID INT IDENTITY(1,1) PRIMARY KEY, -- Khóa chính tự động tăng
    ProductID INT,                                    -- Khóa ngoại đến bảng Products
    PromotionID INT,                                  -- Khóa ngoại đến bảng Promotions
    CONSTRAINT FK_ProductPromotions_Products FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE,
    CONSTRAINT FK_ProductPromotions_Promotions FOREIGN KEY (PromotionID) REFERENCES Promotions(PromotionID) ON DELETE CASCADE
);

-- Tạo bảng Payments (Nhận thông tin từ API)
CREATE TABLE Payments (
    PaymentID INT IDENTITY(1,1) PRIMARY KEY,      -- Khóa chính tự động tăng
    OrderID INT,                                  -- Khóa ngoại đến bảng Orders
    PaymentAmount DECIMAL(10, 2) NOT NULL,        -- Số tiền thanh toán
    PaymentDate DATETIME NOT NULL,                -- Ngày thanh toán
    PaymentMethod INT,                            -- Phương thức thanh toán (1- Thanh toán ATM) (0- Thanh toán tiền mặt)
    PaymentStatus INT,                            -- Trạng thái thanh toán (1-Completed, 2 - Pending, 0 - Failed)
    TransactionID NVARCHAR(255),                  -- Mã giao dịch từ API bên thứ ba
    CONSTRAINT FK_Payments_Orders FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE
);


-- Bảng trung gian ProductSizes
CREATE TABLE ProductSizes (
    ProductSizeID INT IDENTITY(1,1) PRIMARY KEY, -- Khóa chính
    ProductID INT,                               -- Khóa ngoại đến bảng Products
    SizeID INT,                                  -- Khóa ngoại đến bảng Sizes
    CONSTRAINT FK_ProductSizes_Products FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE,
    CONSTRAINT FK_ProductSizes_Sizes FOREIGN KEY (SizeID) REFERENCES Sizes(SizeID) ON DELETE CASCADE
);

-- Bảng trung gian ProductColors
CREATE TABLE ProductColors (
    ProductColorID INT IDENTITY(1,1) PRIMARY KEY, -- Khóa chính
    ProductID INT,                                -- Khóa ngoại đến bảng Products
    ColorID INT,                                  -- Khóa ngoại đến bảng Colors
    CONSTRAINT FK_ProductColors_Products FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE,
    CONSTRAINT FK_ProductColors_Colors FOREIGN KEY (ColorID) REFERENCES Colors(ColorID) ON DELETE CASCADE
);





