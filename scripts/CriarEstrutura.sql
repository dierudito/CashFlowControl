USE master
go
CREATE DATABASE CashFlowControl
go
USE CashFlowControl

GO

CREATE TABLE CashFlows (
	Id uniqueidentifier not null,
	ReleaseDate date not null,
	OpeningBalance money not null,
	TotalCredits money not null,
	TotalDebits money not null,
	ClosingBalance money not null,
    CONSTRAINT PK_CashFlows PRIMARY KEY (ID),
	CONSTRAINT UQ_CashFlows_ReleaseDate UNIQUE (ReleaseDate)
)


CREATE TABLE Categories (
	Id uniqueidentifier not null,
	Name varchar(50) not null,
	Description varchar(255) not null,
    CONSTRAINT PK_Categories PRIMARY KEY (ID)
)
GO

CREATE TABLE Accounts (
	Id uniqueidentifier not null,
	Name varchar(50) not null,
	Description varchar(255) not null,
    CONSTRAINT PK_Accounts PRIMARY KEY (ID)
)
GO

CREATE TABLE Transactions (
	Id uniqueidentifier not null,
	Type tinyint not null,
	Amount money not null,
	CashFlowId uniqueidentifier not null,
	AccountId uniqueidentifier null,
	CategoryId uniqueidentifier null,
	Description nvarchar(500) null,
    CONSTRAINT PK_Transactions PRIMARY KEY (ID),
	CONSTRAINT FK_Accounts FOREIGN KEY (AccountId) REFERENCES Accounts(Id),
	CONSTRAINT FK_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
	CONSTRAINT FK_CashFlows FOREIGN KEY (CashFlowId) REFERENCES CashFlows(Id)
)