
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/19/2018 10:14:22
-- Generated from EDMX file: D:\Project\DCLYApp\WebApi\Models\EMTModels\EMT.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [EMT];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[CustomWorkFlowEntrySet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomWorkFlowEntrySet];
GO
IF OBJECT_ID(N'[dbo].[CustomWorkFlowSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomWorkFlowSet];
GO
IF OBJECT_ID(N'[dbo].[GYLCLogSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GYLCLogSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------



-- Creating table 'CustomWorkFlowEntrySet'
CREATE TABLE [dbo].[CustomWorkFlowEntrySet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PFID] int  NOT NULL,
    [Index] int  NOT NULL,
    [FWorkProcedure] int  NOT NULL,
    [IsDeleted] bit  NOT NULL
);
GO

-- Creating table 'CustomWorkFlowSet'
CREATE TABLE [dbo].[CustomWorkFlowSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FGH] nvarchar(max)  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [Creator] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'GYLCLogSet'
CREATE TABLE [dbo].[GYLCLogSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LogTime] datetime  NOT NULL,
    [User] nvarchar(max)  NOT NULL,
    [Before] nvarchar(max)  NOT NULL,
    [Fgh] nvarchar(max)  NOT NULL,
    [After] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'CustomWorkFlowEntrySet'
ALTER TABLE [dbo].[CustomWorkFlowEntrySet]
ADD CONSTRAINT [PK_CustomWorkFlowEntrySet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustomWorkFlowSet'
ALTER TABLE [dbo].[CustomWorkFlowSet]
ADD CONSTRAINT [PK_CustomWorkFlowSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GYLCLogSet'
ALTER TABLE [dbo].[GYLCLogSet]
ADD CONSTRAINT [PK_GYLCLogSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------