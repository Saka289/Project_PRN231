USE [master]
GO
/****** Object:  Database [Web_Order]    Script Date: 3/27/2024 2:43:06 PM ******/
CREATE DATABASE [Web_Order]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Web_Order', FILENAME = N'/var/opt/mssql/data/Web_Order.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Web_Order_log', FILENAME = N'/var/opt/mssql/data/Web_Order_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Web_Order] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Web_Order].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Web_Order] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Web_Order] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Web_Order] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Web_Order] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Web_Order] SET ARITHABORT OFF 
GO
ALTER DATABASE [Web_Order] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [Web_Order] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Web_Order] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Web_Order] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Web_Order] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Web_Order] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Web_Order] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Web_Order] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Web_Order] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Web_Order] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Web_Order] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Web_Order] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Web_Order] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Web_Order] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Web_Order] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Web_Order] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [Web_Order] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Web_Order] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Web_Order] SET  MULTI_USER 
GO
ALTER DATABASE [Web_Order] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Web_Order] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Web_Order] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Web_Order] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Web_Order] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Web_Order] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Web_Order] SET QUERY_STORE = ON
GO
ALTER DATABASE [Web_Order] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Web_Order]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 3/27/2024 2:43:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 3/27/2024 2:43:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[OrderDetailId] [uniqueidentifier] NOT NULL,
	[OrderId] [uniqueidentifier] NOT NULL,
	[ProductId] [nvarchar](max) NOT NULL,
	[UnitPrice] [decimal](10, 2) NOT NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[OrderDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 3/27/2024 2:43:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderId] [uniqueidentifier] NOT NULL,
	[UserId] [nvarchar](max) NOT NULL,
	[OrderDate] [datetime2](7) NOT NULL,
	[OrderTotal] [decimal](10, 2) NOT NULL,
	[CouponCode] [nvarchar](max) NULL,
	[Discount] [decimal](10, 2) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[ShippedDate] [datetime2](7) NOT NULL,
	[RequiredDate] [datetime2](7) NOT NULL,
	[PaymentStatus] [nvarchar](max) NOT NULL,
	[OrderIdString] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderDetails_OrderId]    Script Date: 3/27/2024 2:43:07 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrderDetails_OrderId] ON [dbo].[OrderDetails]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (N'') FOR [OrderIdString]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Orders_OrderId]
GO
USE [master]
GO
ALTER DATABASE [Web_Order] SET  READ_WRITE 
GO
