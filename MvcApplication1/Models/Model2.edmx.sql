
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/14/2021 06:29:46
-- Generated from EDMX file: C:\Users\MNWEB\Desktop\New folder\MvcApplication1 - Copy\MvcApplication1\MvcApplication1\Models\Model2.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [admin_db1];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[usr1].[ADMINUSER]', 'U') IS NOT NULL
    DROP TABLE [usr1].[ADMINUSER];
GO
IF OBJECT_ID(N'[usr1].[AFTXN]', 'U') IS NOT NULL
    DROP TABLE [usr1].[AFTXN];
GO
IF OBJECT_ID(N'[usr1].[FRENCHISE]', 'U') IS NOT NULL
    DROP TABLE [usr1].[FRENCHISE];
GO
IF OBJECT_ID(N'[usr1].[FRENCHISEBAL]', 'U') IS NOT NULL
    DROP TABLE [usr1].[FRENCHISEBAL];
GO
IF OBJECT_ID(N'[usr1].[FRENCHISEREPORT]', 'U') IS NOT NULL
    DROP TABLE [usr1].[FRENCHISEREPORT];
GO
IF OBJECT_ID(N'[usr1].[FUTXN]', 'U') IS NOT NULL
    DROP TABLE [usr1].[FUTXN];
GO
IF OBJECT_ID(N'[usr1].[GAMEPOOL]', 'U') IS NOT NULL
    DROP TABLE [usr1].[GAMEPOOL];
GO
IF OBJECT_ID(N'[usr1].[GAMESLOT]', 'U') IS NOT NULL
    DROP TABLE [usr1].[GAMESLOT];
GO
IF OBJECT_ID(N'[usr1].[Recipt]', 'U') IS NOT NULL
    DROP TABLE [usr1].[Recipt];
GO
IF OBJECT_ID(N'[usr1].[TOTALSALE]', 'U') IS NOT NULL
    DROP TABLE [usr1].[TOTALSALE];
GO
IF OBJECT_ID(N'[usr1].[USERBAL]', 'U') IS NOT NULL
    DROP TABLE [usr1].[USERBAL];
GO
IF OBJECT_ID(N'[usr1].[USERTBL]', 'U') IS NOT NULL
    DROP TABLE [usr1].[USERTBL];
GO
IF OBJECT_ID(N'[usr1].[YANTRA]', 'U') IS NOT NULL
    DROP TABLE [usr1].[YANTRA];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ADMINUSERs'
CREATE TABLE [dbo].[ADMINUSERs] (
    [AUID] int IDENTITY(1,1) NOT NULL,
    [USERNAME] nvarchar(max)  NULL,
    [PASSWORD] nvarchar(max)  NULL
);
GO

-- Creating table 'AFTXNs'
CREATE TABLE [dbo].[AFTXNs] (
    [TXNID] int IDENTITY(1,1) NOT NULL,
    [AUID] int  NULL,
    [FUID] int  NULL,
    [DEBIT] nvarchar(max)  NULL,
    [CREDIT] nvarchar(max)  NULL,
    [TXNDATE] nvarchar(max)  NULL,
    [TXNTIME] nvarchar(max)  NULL
);
GO

-- Creating table 'FRENCHISEs'
CREATE TABLE [dbo].[FRENCHISEs] (
    [FUID] int IDENTITY(1,1) NOT NULL,
    [AUID] int  NULL,
    [FUNAME] nvarchar(max)  NULL,
    [USERNAME] nvarchar(max)  NULL,
    [PASSWORD] nvarchar(max)  NULL,
    [CONTACTNO] nvarchar(max)  NULL
);
GO

-- Creating table 'FRENCHISEBALs'
CREATE TABLE [dbo].[FRENCHISEBALs] (
    [FWID] int IDENTITY(1,1) NOT NULL,
    [BALANCE] nvarchar(max)  NULL,
    [FUID] int  NULL
);
GO

-- Creating table 'FUTXNs'
CREATE TABLE [dbo].[FUTXNs] (
    [TXNID] int IDENTITY(1,1) NOT NULL,
    [FUID] int  NULL,
    [UID] int  NULL,
    [DEBIT] nvarchar(max)  NULL,
    [CREDIT] nvarchar(max)  NULL,
    [TXNDATE] nvarchar(max)  NULL,
    [TXNTIME] nvarchar(max)  NULL
);
GO

-- Creating table 'GAMEPOOLs'
CREATE TABLE [dbo].[GAMEPOOLs] (
    [GPID] int IDENTITY(1,1) NOT NULL,
    [GSID] int  NULL,
    [GSTIME] nvarchar(max)  NULL,
    [GSENDTIME] nvarchar(max)  NULL,
    [GPDATE] nvarchar(max)  NULL,
    [GPRESULT] nvarchar(max)  NULL,
    [YID] int  NULL,
    [JACKPOT] nvarchar(max)  NULL
);
GO

-- Creating table 'GAMESLOTs'
CREATE TABLE [dbo].[GAMESLOTs] (
    [GSID] int IDENTITY(1,1) NOT NULL,
    [GSTIME] nvarchar(max)  NULL,
    [GSENDTIME] nvarchar(max)  NULL
);
GO

-- Creating table 'Recipts'
CREATE TABLE [dbo].[Recipts] (
    [RID] int IDENTITY(1,1) NOT NULL,
    [FUID] int  NULL,
    [UUID] int  NULL,
    [RTYPE] nvarchar(max)  NULL,
    [Y1] nvarchar(max)  NULL,
    [Y2] nvarchar(max)  NULL,
    [Y3] nvarchar(max)  NULL,
    [Y4] nvarchar(max)  NULL,
    [Y5] nvarchar(max)  NULL,
    [Y6] nvarchar(max)  NULL,
    [Y7] nvarchar(max)  NULL,
    [Y8] nvarchar(max)  NULL,
    [Y9] nvarchar(max)  NULL,
    [Y10] nvarchar(max)  NULL,
    [GSID] nvarchar(max)  NULL,
    [GSTIME] nvarchar(max)  NULL,
    [GSDATE] nvarchar(max)  NULL,
    [RGTIME] nvarchar(max)  NULL,
    [RVAL] nvarchar(max)  NULL,
    [RWINVAL] nvarchar(max)  NULL,
    [RSTATUS] nvarchar(max)  NULL
);
GO

-- Creating table 'TOTALSALEs'
CREATE TABLE [dbo].[TOTALSALEs] (
    [TSID] int IDENTITY(1,1) NOT NULL,
    [GSID] int  NULL,
    [GPID] int  NULL,
    [GPSTIME] nvarchar(max)  NULL,
    [GPSENDTIME] nvarchar(max)  NULL,
    [GPDATE] nvarchar(max)  NULL,
    [Y1] nvarchar(max)  NULL,
    [Y2] nvarchar(max)  NULL,
    [Y3] nvarchar(max)  NULL,
    [Y4] nvarchar(max)  NULL,
    [Y5] nvarchar(max)  NULL,
    [Y6] nvarchar(max)  NULL,
    [Y7] nvarchar(max)  NULL,
    [Y8] nvarchar(max)  NULL,
    [Y9] nvarchar(max)  NULL,
    [Y10] nvarchar(max)  NULL
);
GO

-- Creating table 'USERBALs'
CREATE TABLE [dbo].[USERBALs] (
    [UWID] int IDENTITY(1,1) NOT NULL,
    [BALANCE] nvarchar(max)  NULL,
    [UID] int  NULL,
    [FUID] int  NULL
);
GO

-- Creating table 'USERTBLs'
CREATE TABLE [dbo].[USERTBLs] (
    [UID] int IDENTITY(1,1) NOT NULL,
    [FUID] nvarchar(max)  NULL,
    [UUNAME] nvarchar(max)  NULL,
    [USERNAME] nvarchar(max)  NULL,
    [PASSWORD] nvarchar(max)  NULL,
    [CONTACTNO] nvarchar(max)  NULL
);
GO

-- Creating table 'YANTRAs'
CREATE TABLE [dbo].[YANTRAs] (
    [YID] int IDENTITY(1,1) NOT NULL,
    [YANTRANAME] nvarchar(max)  NULL,
    [YANTRASELLPRICE] decimal(18,0)  NULL,
    [YANTRAIMAGEURL] nvarchar(max)  NULL
);
GO

-- Creating table 'FRENCHISEREPORTs'
CREATE TABLE [dbo].[FRENCHISEREPORTs] (
    [FRID] int IDENTITY(1,1) NOT NULL,
    [FUID] int  NULL,
    [DATE] nvarchar(max)  NULL,
    [OB] nvarchar(max)  NULL,
    [AR] nvarchar(max)  NULL,
    [AP] nvarchar(max)  NULL,
    [AW] nvarchar(max)  NULL,
    [CB] nvarchar(max)  NULL,
    [OCPL] nvarchar(max)  NULL,
    [MCCB] nvarchar(max)  NULL,
    [MCC] nvarchar(max)  NULL,
    [MCD] nvarchar(max)  NULL,
    [MP] nvarchar(max)  NULL,
    [MW] nvarchar(max)  NULL,
    [MCPL] nvarchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [AUID] in table 'ADMINUSERs'
ALTER TABLE [dbo].[ADMINUSERs]
ADD CONSTRAINT [PK_ADMINUSERs]
    PRIMARY KEY CLUSTERED ([AUID] ASC);
GO

-- Creating primary key on [TXNID] in table 'AFTXNs'
ALTER TABLE [dbo].[AFTXNs]
ADD CONSTRAINT [PK_AFTXNs]
    PRIMARY KEY CLUSTERED ([TXNID] ASC);
GO

-- Creating primary key on [FUID] in table 'FRENCHISEs'
ALTER TABLE [dbo].[FRENCHISEs]
ADD CONSTRAINT [PK_FRENCHISEs]
    PRIMARY KEY CLUSTERED ([FUID] ASC);
GO

-- Creating primary key on [FWID] in table 'FRENCHISEBALs'
ALTER TABLE [dbo].[FRENCHISEBALs]
ADD CONSTRAINT [PK_FRENCHISEBALs]
    PRIMARY KEY CLUSTERED ([FWID] ASC);
GO

-- Creating primary key on [TXNID] in table 'FUTXNs'
ALTER TABLE [dbo].[FUTXNs]
ADD CONSTRAINT [PK_FUTXNs]
    PRIMARY KEY CLUSTERED ([TXNID] ASC);
GO

-- Creating primary key on [GPID] in table 'GAMEPOOLs'
ALTER TABLE [dbo].[GAMEPOOLs]
ADD CONSTRAINT [PK_GAMEPOOLs]
    PRIMARY KEY CLUSTERED ([GPID] ASC);
GO

-- Creating primary key on [GSID] in table 'GAMESLOTs'
ALTER TABLE [dbo].[GAMESLOTs]
ADD CONSTRAINT [PK_GAMESLOTs]
    PRIMARY KEY CLUSTERED ([GSID] ASC);
GO

-- Creating primary key on [RID] in table 'Recipts'
ALTER TABLE [dbo].[Recipts]
ADD CONSTRAINT [PK_Recipts]
    PRIMARY KEY CLUSTERED ([RID] ASC);
GO

-- Creating primary key on [TSID] in table 'TOTALSALEs'
ALTER TABLE [dbo].[TOTALSALEs]
ADD CONSTRAINT [PK_TOTALSALEs]
    PRIMARY KEY CLUSTERED ([TSID] ASC);
GO

-- Creating primary key on [UWID] in table 'USERBALs'
ALTER TABLE [dbo].[USERBALs]
ADD CONSTRAINT [PK_USERBALs]
    PRIMARY KEY CLUSTERED ([UWID] ASC);
GO

-- Creating primary key on [UID] in table 'USERTBLs'
ALTER TABLE [dbo].[USERTBLs]
ADD CONSTRAINT [PK_USERTBLs]
    PRIMARY KEY CLUSTERED ([UID] ASC);
GO

-- Creating primary key on [YID] in table 'YANTRAs'
ALTER TABLE [dbo].[YANTRAs]
ADD CONSTRAINT [PK_YANTRAs]
    PRIMARY KEY CLUSTERED ([YID] ASC);
GO

-- Creating primary key on [FRID] in table 'FRENCHISEREPORTs'
ALTER TABLE [dbo].[FRENCHISEREPORTs]
ADD CONSTRAINT [PK_FRENCHISEREPORTs]
    PRIMARY KEY CLUSTERED ([FRID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------