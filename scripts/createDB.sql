USE master
GO

IF EXISTS(SELECT * FROM sys.databases WHERE name='GhostPhotoDB')
DROP DATABASE GhostPhotoDB
GO

CREATE DATABASE GhostPhotoDB
GO

USE GhostPhotoDB
GO
