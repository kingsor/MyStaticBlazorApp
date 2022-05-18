/****** Object:  Table [dbo].[ShoppingList]    Script Date: 17/05/2022 19:26:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ShoppingList](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK_ShoppingList] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


USE [ShoppingListDb]
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


