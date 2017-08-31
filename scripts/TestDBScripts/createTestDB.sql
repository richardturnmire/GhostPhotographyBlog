USE master
GO

IF EXISTS(SELECT * FROM sys.databases WHERE name='GhostPhotoDBTest')
DROP DATABASE GhostPhotoDBTest
GO

CREATE DATABASE GhostPhotoDBTest
GO

USE GhostPhotoDBTest
GO
