USE [master]
GO
/****** Object:  Database [KajolBezol]    Script Date: 10/18/2023 11:18:45 AM ******/
CREATE DATABASE [KajolBezol]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'KajolBezol_Data', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\KajolBezol.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'KajolBezol_Log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\KajolBezol.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [KajolBezol] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [KajolBezol].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [KajolBezol] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [KajolBezol] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [KajolBezol] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [KajolBezol] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [KajolBezol] SET ARITHABORT OFF 
GO
ALTER DATABASE [KajolBezol] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [KajolBezol] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [KajolBezol] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [KajolBezol] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [KajolBezol] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [KajolBezol] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [KajolBezol] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [KajolBezol] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [KajolBezol] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [KajolBezol] SET  ENABLE_BROKER 
GO
ALTER DATABASE [KajolBezol] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [KajolBezol] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [KajolBezol] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [KajolBezol] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [KajolBezol] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [KajolBezol] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [KajolBezol] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [KajolBezol] SET RECOVERY FULL 
GO
ALTER DATABASE [KajolBezol] SET  MULTI_USER 
GO
ALTER DATABASE [KajolBezol] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [KajolBezol] SET DB_CHAINING OFF 
GO
ALTER DATABASE [KajolBezol] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [KajolBezol] SET TARGET_RECOVERY_TIME = 120 SECONDS 
GO
ALTER DATABASE [KajolBezol] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [KajolBezol] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'KajolBezol', N'ON'
GO
ALTER DATABASE [KajolBezol] SET QUERY_STORE = ON
GO
ALTER DATABASE [KajolBezol] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [KajolBezol]
GO
/****** Object:  User [kajolbezoldbuser]    Script Date: 10/18/2023 11:18:46 AM ******/
CREATE USER [kajolbezoldbuser] FOR LOGIN [kajolbezoldbuser] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[AppSettings]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppSettings](
	[KeyId] [int] IDENTITY(1,1) NOT NULL,
	[KeyName] [nvarchar](100) NOT NULL,
	[KeyValue] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK__AppSetti__21F5BE4732E2E94C] PRIMARY KEY CLUSTERED 
(
	[KeyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BuyRequests]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BuyRequests](
	[RequestId] [int] IDENTITY(2210,1) NOT NULL,
	[UserId] [int] NULL,
	[amount] [decimal](18, 0) NULL,
	[RequestTime] [datetime2](7) NOT NULL,
	[Cancelled] [bit] NULL,
	[Fulfilled] [bit] NULL,
	[CommittedOperation] [bit] NULL,
	[TransactionId] [int] NULL,
 CONSTRAINT [PK__BuyReque__33A8517A29E54C77] PRIMARY KEY CLUSTERED 
(
	[RequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Calendar]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Calendar](
	[NonWorkingDate] [varchar](15) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[NonWorkingDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellRequests]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellRequests](
	[RequestId] [int] IDENTITY(2210,1) NOT NULL,
	[UserId] [int] NULL,
	[amount] [decimal](18, 0) NULL,
	[RequestTime] [datetime2](7) NOT NULL,
	[Cancelled] [bit] NULL,
	[Fulfilled] [bit] NULL,
	[CommittedOperation] [bit] NULL,
	[TransactionId] [int] NULL,
 CONSTRAINT [PK__SellRequ__33A8517A2A2B3FBD] PRIMARY KEY CLUSTERED 
(
	[RequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TradeTransactions]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TradeTransactions](
	[TransactionId] [int] IDENTITY(2466,1) NOT NULL,
	[TransactionDate] [datetime] NULL,
 CONSTRAINT [PK__TradeTra__55433A6BCE2752EF] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionDetails]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionDetails](
	[TransactionId] [int] NULL,
	[RequestId] [int] NULL,
	[TransactionTypen] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[WhatsAppNumber] [int] NOT NULL,
	[UserName] [varchar](max) NULL,
	[Email] [varchar](max) NULL,
	[PassCode] [nvarchar](max) NOT NULL,
	[UserRole] [nvarchar](max) NULL,
	[FullName] [nvarchar](max) NULL,
	[DeviceKey] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[WhatsAppNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[BuyRequests]  WITH CHECK ADD  CONSTRAINT [FK_UserId_WhatsAppNumber] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([WhatsAppNumber])
GO
ALTER TABLE [dbo].[BuyRequests] CHECK CONSTRAINT [FK_UserId_WhatsAppNumber]
GO
/****** Object:  StoredProcedure [dbo].[sp_AppVersion]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_AppVersion] 
    
AS
    SELECT KeyValue as AppVersion FROM AppSettings where KeyName='AppVersion'
GO
/****** Object:  StoredProcedure [dbo].[sp_Authenticate]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Authenticate](
	@UserName NVARCHAR(MAX),
	@Password NVARCHAR(MAX),
	@DeviceKey NVARCHAR(MAX)
)
AS 
BEGIN

UPDATE Users SET  DeviceKey = @DeviceKey  WHERE UserName = @UserName AND PassCode = @Password

SELECT UserName,WhatsAppNumber,Email,FullName,UserRole,DeviceKey FROM Users WHERE UserName = @UserName AND PassCode = @Password

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Cancel]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_Cancel]
	-- Add the parameters for the stored procedure here
	@userId INT
AS

BEGIN
	
	IF OBJECT_ID('#TempTrades') IS NOT NULL 
		DROP TABLE #TempTrades

	CREATE TABLE #TempTrades
	(
		RequestId INT,
		UserId INT,
		amount DECIMAL ,
		RequestTime DATETIME2,
		Cancelled BIT,
		Fulfilled BIT,
		CommittedOperation BIT,
		TransactionId INT,
		TradeType NVARCHAR(MAX)
	)

	DECLARE @reqId INT


	INSERT INTO #TempTrades EXEC usp_GetPendingTrades

	IF EXISTS (SELECT RequestId FROM #TempTrades where UserId = @userId) 
	
	BEGIN
		SET @reqId = (SELECT RequestId FROM #TempTrades where UserId = @userId) 
		UPDATE BuyRequests SET Cancelled = 1 WHERE RequestId = @reqId 
		UPDATE SellRequests SET Cancelled = 1 WHERE RequestId = @reqId 
		RETURN 1
	END

	RETURN 0
    
END
GO
/****** Object:  StoredProcedure [dbo].[sp_getPercentageFee]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_getPercentageFee]
    @percentageFee int OUTPUT 
AS
BEGIN
	IF EXISTS (SELECT KeyName,KeyValue from AppSettings where KeyName = 'FeeStructure')
	BEGIN
		IF EXISTS (SELECT KeyValue from AppSettings where KeyName = 'Fees')
		BEGIN
			SELECT @PercentageFee=KeyValue from AppSettings where KeyName = 'Fees'
			RETURN @PercentageFee
		END
	END
	RETURN NULL
END
GO
/****** Object:  StoredProcedure [dbo].[sp_IsOpen]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_IsOpen]
AS
BEGIN
	DECLARE @openTime NVARCHAR (MAX)
	DECLARE @OpenDate NVARCHAR (MAX)
	DECLARE @OpenDateTime NVARCHAR (MAX)
	DECLARE @closeTime NVARCHAR (MAX)
	DECLARE @CloseDateTime NVARCHAR(MAX)
	DECLARE @IsOpen BIT
	DECLARE @Holiday NVARCHAR(MAX) = NULL
	DECLARE @dayNumber INT;
	SET @dayNumber = DATEPART(DW, GETDATE());
	

	SELECT @openTime = KeyValue from AppSettings where AppSettings.KeyName = 'OpenTime'
	SET @openDate = convert(varchar, getdate(), 23) 
	SET @OpenDateTime = @openDate + ' ' + @openTime
	SELECT @closeTime = KeyValue from AppSettings where AppSettings.KeyName = 'CloseTime'
	SET @CloseDateTime = @openDate + ' ' + @closeTime

	IF EXISTS (SELECT NonWorkingDate FROM Calendar where NonWorkingDate = CONVERT(VARCHAR, getdate(),110))
	
	BEGIN
		 SET @Holiday = (SELECT NonWorkingDate FROM Calendar where NonWorkingDate = CONVERT(VARCHAR, getdate(),110))
		 SET @IsOpen = 0
	END
	
	ELSE IF (SYSDATETIME() < @OpenDateTime OR SYSDATETIME() > @CloseDateTime)
	
		SET @IsOpen = 0
	
	ELSE IF  (@dayNumber = 1 OR @dayNumber = 7) 

		SET @IsOpen = 0
	ELSE
		SET @IsOpen = 1
	
	SELECT @OpenDateTime as OpenDateTime, @CloseDateTime as CloseDateTime, @Holiday as Holiday, @IsOpen as IsOpen, GETDATE() as TimeRequested 

	RETURN @IsOpen
	

END
GO
/****** Object:  StoredProcedure [dbo].[sp_MyRequests]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[sp_MyRequests]
	@UserName INT
	
AS
BEGIN
	DECLARE @openDate nvarchar(MAX)
	DECLARE @openTime nvarchar(MAX)
	DECLARE @OpenDateTime nvarchar(MAX)
	DECLARE @closeTime nvarchar(max)
	DECLARE @CloseDateTime nvarchar(MAX)
	DECLARE @openTimeStamp datetime2
	DECLARE @closeTimeStamp datetime2


	SELECT @openTime = KeyValue from AppSettings where AppSettings.KeyName = 'OpenTime'
	SET @openDate = convert(varchar, getdate(), 23) 
	SET @OpenDateTime = @openDate + ' ' + @openTime
	SELECT @closeTime = KeyValue from AppSettings where AppSettings.KeyName = 'CloseTime'
	SET @CloseDateTime = @openDate + ' ' + @closeTime

	
	SELECT RequestId,UserId,amount,RequestTime,Cancelled,Fulfilled,CommittedOperation,TransactionId,'Sell' as TradeType FROM SellRequests 
	where RequestTime >= @OpenDateTime
	AND RequestTime <= @CloseDateTime  
	AND UserId = @UserName
	
	UNION
	
	SELECT RequestId,UserId,amount,RequestTime,Cancelled,Fulfilled,CommittedOperation,TransactionId,'Buy' as TradeType FROM BuyRequests 
	where RequestTime >= @OpenDateTime
	AND RequestTime <= @CloseDateTime 
	AND UserId = @UserName
	
	ORDER BY RequestTime ASC
	
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UserhasNoBids]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UserhasNoBids]
	@userId INT
 
 AS

 IF OBJECT_ID('#TempTrades') IS NOT NULL 
	DROP TABLE #TempTrades


CREATE TABLE #TempTrades
	(
		RequestId INT,
		UserId INT,
		amount DECIMAL ,
		RequestTime DATETIME2,
		Cancelled BIT,
		Fulfilled BIT,
		CommittedOperation BIT,
		TransactionId INT,
		TradeType NVARCHAR(MAX)
	)

INSERT INTO #TempTrades EXEC usp_GetPendingTrades 

IF EXISTS (SELECT 1 FROM #TempTrades WHERE UserId = @userId) RETURN 0

RETURN 1
GO
/****** Object:  StoredProcedure [dbo].[spBuyRequest]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[spBuyRequest](
    @UserName int,
	@Amount decimal
)
AS


IF OBJECT_ID('#TempTrades') IS NOT NULL 
	DROP TABLE #TempTrades


DECLARE @ThisRequestId INT
DECLARE @MatchRequest INT
DECLARE @TransactionId INT


CREATE TABLE #TempTrades
	(
		RequestId INT,
		UserId INT,
		amount DECIMAL ,
		RequestTime DATETIME2,
		Cancelled BIT,
		Fulfilled BIT,
		CommittedOperation BIT,
		TransactionId INT,
		TradeType NVARCHAR(MAX)
	)

------------------------POPULATE TRADE TABLE------------------

INSERT INTO #TempTrades EXEC usp_GetPendingTrades 


INSERT INTO BuyRequests (UserId,amount,RequestTime,Cancelled,Fulfilled,CommittedOperation,TransactionId)
	VALUES (@UserName,@Amount,GETDATE(),0,0,0,NULL)

SET @ThisRequestId =  (SELECT SCOPE_IDENTITY())


IF EXISTS (SELECT TOP 1 RequestId FROM #TempTrades tt WHERE tt.amount = @Amount AND tt.UserId != @UserName AND tt.TradeType = 'Sell'  ORDER BY tt.RequestId ASC) 

BEGIN 

	SET @MatchRequest = (SELECT TOP 1 RequestId FROM #TempTrades tt WHERE tt.amount = @Amount AND tt.UserId != @UserName AND tt.TradeType = 'Sell'  ORDER BY tt.RequestId ASC)
	
	INSERT INTO TradeTransactions (TransactionDate) VALUES (SYSDATETIME())

	SET @TransactionId =  (SELECT SCOPE_IDENTITY())
	
	UPDATE BuyRequests SET TransactionId = @TransactionId, CommittedOperation = 1 WHERE RequestId =  @ThisRequestId
	UPDATE SellRequests SET TransactionId = @TransactionId, CommittedOperation = 1 WHERE RequestId =  @MatchRequest

END 

DROP TABLE #TempTrades

SELECT RequestId,UserId,amount,RequestTime,Cancelled,Fulfilled,CommittedOperation,TransactionId,'Buy' as TradeType FROM BuyRequests
WHERE RequestId = @ThisRequestId
UNION
SELECT RequestId,UserId,amount,RequestTime,Cancelled,Fulfilled,CommittedOperation,TransactionId,'Sell' as TradeType FROM SellRequests
WHERE RequestId = @MatchRequest
GO
/****** Object:  StoredProcedure [dbo].[spPrivacyPolicy]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spPrivacyPolicy] 
    
AS
    SELECT KeyValue as PrivacyPolicy FROM AppSettings where KeyName='PrivacyPolicy'
GO
/****** Object:  StoredProcedure [dbo].[spSellRequest]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[spSellRequest](
    @UserName int,
	@Amount decimal
)
AS



IF OBJECT_ID('#TempTrades') IS NOT NULL 
	DROP TABLE #TempTrades


DECLARE @ThisRequestId INT
DECLARE @MatchRequest INT
DECLARE @TransactionId INT


CREATE TABLE #TempTrades
	(
		RequestId INT,
		UserId INT,
		amount DECIMAL ,
		RequestTime DATETIME2,
		Cancelled BIT,
		Fulfilled BIT,
		CommittedOperation BIT,
		TransactionId INT,
		TradeType NVARCHAR(MAX)
	)

------------------------POPULATE TRADE TABLE------------------

INSERT INTO #TempTrades EXEC usp_GetPendingTrades 


INSERT INTO SellRequests (UserId,amount,RequestTime,Cancelled,Fulfilled,CommittedOperation,TransactionId)
	VALUES (@UserName,@Amount,GETDATE(),0,0,0,NULL)

SET @ThisRequestId =  (SELECT SCOPE_IDENTITY())


IF EXISTS (SELECT TOP 1 RequestId FROM #TempTrades tt WHERE tt.amount = @Amount AND tt.UserId != @UserName AND tt.TradeType = 'Buy'  ORDER BY tt.RequestId ASC) 

BEGIN 

	SET @MatchRequest = (SELECT TOP 1 RequestId FROM #TempTrades tt WHERE tt.amount = @Amount AND tt.UserId != @UserName AND tt.TradeType = 'Buy'  ORDER BY tt.RequestId ASC)
	
	INSERT INTO TradeTransactions (TransactionDate) VALUES (SYSDATETIME())

	SET @TransactionId =  (SELECT SCOPE_IDENTITY())
	
	UPDATE SellRequests SET TransactionId = @TransactionId, CommittedOperation = 1 WHERE RequestId =  @ThisRequestId
	UPDATE BuyRequests SET TransactionId = @TransactionId, CommittedOperation = 1 WHERE RequestId =  @MatchRequest

END 

DROP TABLE #TempTrades

SELECT RequestId,UserId,amount,RequestTime,Cancelled,Fulfilled,CommittedOperation,TransactionId,'Sell' as TradeType FROM SellRequests
WHERE RequestId = @ThisRequestId
UNION
SELECT RequestId,UserId,amount,RequestTime,Cancelled,Fulfilled,CommittedOperation,TransactionId,'Buy' as TradeType FROM BuyRequests
WHERE RequestId = @MatchRequest
GO
/****** Object:  StoredProcedure [dbo].[spUsePolicy]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUsePolicy] 
    
AS
    SELECT KeyValue as UsePolicy FROM AppSettings where KeyName='UsePolicy'
GO
/****** Object:  StoredProcedure [dbo].[spWhatsAppcontact]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spWhatsAppcontact] 
    
AS
    SELECT KeyValue as WhatsAppBrokerContact FROM AppSettings where KeyName='WhatsAppBrokerContact'
GO
/****** Object:  StoredProcedure [dbo].[usp_GetPendingTrades]    Script Date: 10/18/2023 11:18:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetPendingTrades]
AS
BEGIN
	DECLARE @openDate nvarchar(MAX)
	DECLARE @openTime nvarchar(MAX)
	DECLARE @OpenDateTime nvarchar(MAX)
	DECLARE @closeTime nvarchar(max)
	DECLARE @CloseDateTime nvarchar(MAX)
	DECLARE @openTimeStamp datetime2
	DECLARE @closeTimeStamp datetime2


	SELECT @openTime = KeyValue from AppSettings where AppSettings.KeyName = 'OpenTime'
	SET @openDate = convert(varchar, getdate(), 23) 
	SET @OpenDateTime = @openDate + ' ' + @openTime
	SELECT @closeTime = KeyValue from AppSettings where AppSettings.KeyName = 'CloseTime'
	SET @CloseDateTime = @openDate + ' ' + @closeTime

	
	SELECT RequestId,UserId,amount,RequestTime,Cancelled,Fulfilled,CommittedOperation,TransactionId,'Sell' as TradeType FROM SellRequests 
	where RequestTime >= @OpenDateTime
	AND RequestTime <= @CloseDateTime  
	AND Cancelled = 0 AND Fulfilled = 0 AND CommittedOperation = 0 AND TransactionId IS NULL 
	
	UNION
	
	SELECT RequestId,UserId,amount,RequestTime,Cancelled,Fulfilled,CommittedOperation,TransactionId,'Buy' as TradeType FROM BuyRequests 
	where RequestTime >= @OpenDateTime
	AND RequestTime <= @CloseDateTime 
	AND Cancelled = 0 AND Fulfilled = 0 AND CommittedOperation = 0 AND TransactionId IS NULL
	ORDER BY RequestTime ASC
	
END
GO
USE [master]
GO
ALTER DATABASE [KajolBezol] SET  READ_WRITE 
GO
