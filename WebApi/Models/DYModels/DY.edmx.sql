
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/28/2018 14:49:26
-- Generated from EDMX file: D:\Project\DCLYApp\WebApi\Models\DYModels\DY.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DYJXC];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ControllerActionRoleSet'
CREATE TABLE [dbo].[ControllerActionRoleSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ControllerActionId] int  NOT NULL,
    [RoleId] int  NOT NULL,
    [IsAllowed] bit  NOT NULL
);
GO

-- Creating table 'ControllerActionSet'
CREATE TABLE [dbo].[ControllerActionSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IsAllowedNonRoles] bit  NOT NULL,
    [IsAllowedAllRoles] bit  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [ControllerName] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [ControllerDisplayName] nvarchar(max)  NOT NULL,
    [ActionDisplayName] nvarchar(max)  NOT NULL,
    [IsApi] bit  NOT NULL
);
GO

-- Creating table 't_RoleSet'
CREATE TABLE [dbo].[t_RoleSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 't_UserRoleSet'
CREATE TABLE [dbo].[t_UserRoleSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [User] nvarchar(max)  NOT NULL,
    [Role] int  NOT NULL
);
GO





-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ControllerActionRoleSet'
ALTER TABLE [dbo].[ControllerActionRoleSet]
ADD CONSTRAINT [PK_ControllerActionRoleSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ControllerActionSet'
ALTER TABLE [dbo].[ControllerActionSet]
ADD CONSTRAINT [PK_ControllerActionSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 't_RoleSet'
ALTER TABLE [dbo].[t_RoleSet]
ADD CONSTRAINT [PK_t_RoleSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 't_UserRoleSet'
ALTER TABLE [dbo].[t_UserRoleSet]
ADD CONSTRAINT [PK_t_UserRoleSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO





-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------