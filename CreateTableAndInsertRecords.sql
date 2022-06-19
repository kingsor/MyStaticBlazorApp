
USE [ShoppingListDb]
GO

/****** Object:  Table [dbo].[ShoppingList]    Script Date: 26/05/2022 17:15:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ShoppingList]') AND type in (N'U'))
DROP TABLE [dbo].[ShoppingList]
GO

/****** Object:  Table [dbo].[ShoppingList]    Script Date: 26/05/2022 17:15:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ShoppingList](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Quantity] [int] NOT NULL,
	[CreatedAt] [datetime2] NOT NULL default GETDATE(),
	[UpdatedAt] [datetime2] NOT NULL default GETDATE(),
	[DeletedAt] [datetime2] default NULL
 CONSTRAINT [PK_ShoppingList] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


INSERT INTO [dbo].[ShoppingList]
           ([Name]
           ,[Description]
           ,[Quantity])
     VALUES
           ('Strawberries'
           ,'16oz package of fresh organic strawberries'
           ,5),
           ('Sliced bread'
           ,'Loaf of fresh sliced wheat bread'
           ,6),
           ('Apples'
           ,'Bag of 7 fresh McIntosh apples'
           ,8)
GO


SELECT 
  COUNT(Id) as TotalRecords,
  SUM (CASE
    WHEN CreatedAt = UpdatedAt and DeletedAt is null THEN 1
    ELSE 0
  END) AS CreatedRecords,
  SUM (CASE
    WHEN CreatedAt <> UpdatedAt and DeletedAt is null THEN 1
    ELSE 0
  END) AS UpdatedRecords,
  SUM (CASE
    WHEN DeletedAt is not null THEN 1
    ELSE 0
  END) AS DeletedRecords
FROM ShoppingList

