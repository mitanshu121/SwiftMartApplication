
create table tblAdmin
(
AdminID nvarchar(50) primary key,
[Password] nvarchar(50),
FullName nvarchar(50)
)
select * from tblAdmin

select top 1 AdminID,Password,FullName from tblAdmin where AdminID=@AdminID and Password=@Password
USE GoMartDB;
GO


create table tblSeller
(
SellerID int identity(1,1) primary key,
SellerName nvarchar(50) unique,
SellerAge int,
SellerPhone nvarchar(10),
SellerPass nvarchar(50)
)
select top 1 SellerName,SellerPass from tblSeller where SellerName=@SellerName and SellerPass=@SellerPass

create table tblcategory
(
CatID int identity(1,1) primary key not null,
CategoryrName nvarchar(50) unique,
CategoryDesc nvarchar(50)
)
select* from tblcategory
insert into tblcategory(CategoryrName,CategoryDesc) values(@CategoryrName,@CategoryDesc)

create procedure spCatInsert
(
@CategoryName nvarchar(50),
@CategoryDesc nvarchar(50)
)
as
begin
insert into tblcategory(CategoryName,CategoryDesc) values(@CategoryName,@CategoryDesc)
end

select CatID as CategoryID, CategoryName,CategoryDesc as CategoryDescription from tblcategory 
EXEC sp_rename 'tblcategory.CategoryrName', 'CategoryName', 'COLUMN';

CREATE PROCEDURE spCatUpdate
(
    @CatID INT,
    @CategoryName NVARCHAR(50),
    @CategoryDesc NVARCHAR(50)
)
AS
BEGIN
    UPDATE tblcategory
    SET CategoryName = @CategoryName,
        CategoryDesc = @CategoryDesc
    WHERE CatID = @CatID;
END;
GO


create procedure spCatDelete
(
@CatID int
)
as
begin
Delete from tblcategory where CatID=@CatID 
end


------

IF COL_LENGTH('tblcategory', 'CategoryrName') IS NOT NULL
BEGIN
    EXEC sp_rename 'tblcategory.CategoryrName', 'CategoryName', 'COLUMN';
END
GO

-- Drop the old spCatInsert procedure if it exists
IF OBJECT_ID('spCatInsert', 'P') IS NOT NULL
    DROP PROCEDURE spCatInsert;

	IF OBJECT_ID('spCatUpdate', 'P') IS NOT NULL
    DROP PROCEDURE spCatUpdate;

	USE GoMartDB;
GO
IF OBJECT_ID('dbo.tblcategory', 'U') IS NOT NULL
    DROP TABLE dbo.tblcategory;
GO

CREATE TABLE tblcategory
(
    CatID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    CategoryName NVARCHAR(50) UNIQUE,
    CategoryDesc NVARCHAR(50)
);
GO


IF OBJECT_ID('dbo.spCatInsert', 'P') IS NOT NULL
    DROP PROCEDURE dbo.spCatInsert;
GO

CREATE PROCEDURE spCatInsert
(
    @CategoryName NVARCHAR(50),
    @CategoryDesc NVARCHAR(50)
)
AS
BEGIN
    INSERT INTO tblcategory(CategoryName, CategoryDesc)
    VALUES(@CategoryName, @CategoryDesc);
END;
GO

IF OBJECT_ID('dbo.spCatUpdate', 'P') IS NOT NULL
    DROP PROCEDURE dbo.spCatUpdate;
GO

CREATE PROCEDURE spCatUpdate
(
    @CatID INT,
    @CategoryName NVARCHAR(50),
    @CategoryDesc NVARCHAR(50)
)
AS
BEGIN
    UPDATE tblcategory
    SET CategoryName = @CategoryName, CategoryDesc = @CategoryDesc
    WHERE CatID = @CatID;
END;
GO

IF OBJECT_ID('dbo.spCatDelete', 'P') IS NOT NULL
    DROP PROCEDURE dbo.spCatDelete;
GO

CREATE PROCEDURE spCatDelete
(
    @CatID INT
)
AS
BEGIN
    DELETE FROM tblcategory
    WHERE CatID = @CatID;
END;
GO
use GoMartDB
select SellerName from tblSeller where SellerName=@SellerName

create procedure spSellerInsert
(
@SellerName nvarchar(50),
@SellerAge int,
@SellerPhone nvarchar(50),
@SellerPass nvarchar(50)
)
as
begin
insert into tblSeller(SellerName,SellerAge,SellerPhone,SellerPass) values(@SellerName,@SellerAge,@SellerPhone,@SellerPass)
end
go
--------------

CREATE PROCEDURE spSellerUpdate
(
    @SellerID int,
	@SellerName nvarchar(50),
	@SellerAge int,
	@SellerPhone nvarchar(50),
	@SellerPass nvarchar(50)
)
AS
BEGIN
    UPDATE tblSeller
    SET SellerName = @SellerName,
        SellerAge = @SellerAge,
		SellerPhone = @SellerPhone,
		SellerPass = @SellerPass
    WHERE SellerID = @SellerID;
END
GO

create procedure spSellerDelete
(
@SellerID int
)
as
begin
Delete from tblSeller where SellerID = @SellerID 
end

 select * from tblSeller

 use GoMartDB

 select * from tblAdmin
 select AdminID from tblAdmin where AdminID=@ID
 ----------------
 create procedure spAdminInsert
(
@AdminName nvarchar(50),
@AdminPass nvarchar(50),
@AdminID nvarchar(50)
)
as
begin
insert into tblAdmin(FullName,Password,AdminID) values(@AdminName,@AdminPass,@AdminID)
end
go


CREATE PROCEDURE spAdminUpdate
(
@AdminName nvarchar(50),
@AdminPass nvarchar(50),
@AdminID nvarchar(50)
)
AS
BEGIN
    UPDATE tblAdmin
    SET FullName = @AdminName,
	[Password]= @AdminPass
	WHERE AdminID = @AdminID
END
GO

alter procedure spAdminDelete
(
@AdminID nvarchar(50)
)
as
begin
Delete from tblAdmin where AdminID = @AdminID 
end
select * from tblcategory
-------
create table tblProduct
(
ProdID int identity(1,1) primary key not null,
ProdName nvarchar(50),
ProdCatID int,
ProdPrice decimal(10,2),
ProdQty int
)


Create procedure spGetCategory

as
begin
set nocount on;
select CatID,CategoryName from tblcategory order by CategoryName asc
end
go

Create procedure spGetCategory

as
begin
set nocount on;
select CatID,CategoryName from tblcategory order by CategoryName asc
end
go

-------
select* from tblProduct
create procedure spCheckDuplicateProduct
(
@ProdName nvarchar(50),
@ProdCatID int
)
as
begin
set nocount on;
select ProdName from tblProduct where ProdName=@ProdName and ProdCatID=@ProdCatID 
end
GO
-------
select* from tblProduct
create procedure spInsertProduct
(
@ProdName nvarchar(50),
@ProdCatID int,
@ProdPrice decimal(10,2),
@ProdQty int
)
as
begin
Insert into tblProduct (ProdName,ProdCatID,ProdPrice,ProdQty) values (@ProdName,@ProdCatID,@ProdPrice,@ProdQty)
end
GO
select * from tblProduct
create procedure spgetAllProductList
as
begin
set nocount on;
select t1.ProdID as ProductID,t1.ProdName as ProductName, t2.CategoryName as CategoryName,t1.ProdCatID as CategoryID, t1.ProdPrice as ProductPrice, t1.ProdQty as ProductQuantity from tblProduct as t1
inner join tblCategory as t2 on t1. ProdCatID=t2.CatID
order by t1.ProdName, t2.CategoryName asc
end
go

---------
alter procedure spUpdateProduct
(
@ProdID int,
@ProdName nvarchar(50),
@ProdCatID int,
@ProdPrice decimal(10,2),
@ProdQty int
)
as
begin
Update tblProduct set ProdName =@ProdName,ProdCatID=@ProdCatID,ProdPrice=@ProdPrice,ProdQty=@ProdQty
where  ProdID=@ProdID
end
GO
---------
create procedure spDeleteProduct
(
@ProdID int
)
as
begin
delete from tblProduct where ProdID=@ProdID
end
GO

------

create procedure spgetAllProductList_SearchByCat
(
@ProdCatID int
)
as
begin
set nocount on;
select t1.ProdID as ProductID,t1.ProdName as ProductName, t2.CategoryName as CategoryName,t1.ProdCatID as CategoryID, t1.ProdPrice as ProductPrice, t1.ProdQty as ProductQuantity from tblProduct as t1
inner join tblCategory as t2 on t1. ProdCatID=t2.CatID 
where t1.ProdCatID=@ProdCatID
order by t1.ProdName, t2.CategoryName asc
end
go
-------

create table tblBill
(
Bill_ID int primary key,
SellerID nvarchar(50),
SellDate nvarchar(50),
TotalAmt decimal(18,2)
)
select * from tblBill
UPDATE tblBill
SET TotalAmt = 200.00
WHERE Bill_ID = 1;

create procedure spInsertBill
(
@Bill_ID int ,
@SellerID nvarchar(50),
@SellDate nvarchar(50),
@TotalAmt decimal(18,2)
)
as
begin
insert into tblBill(Bill_ID,SellerID,SellDate,TotalAmt) values(@Bill_ID,@SellerID,@SellDate,@TotalAmt)
end
go

--------------

create procedure spGetBillList

as
begin
set nocount on;
select Bill_ID,SellerID,SellDate,TotalAmt from tblBill order by Bill_ID desc
end
go