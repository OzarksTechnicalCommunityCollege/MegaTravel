USE [MegaTravel]
GO

/****** Object:  Table [dbo].[Trip]    Script Date: 8/15/2022 4:26:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Trip](
	[TripID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[AgentID] [int] NOT NULL,
	[TripName] [varchar](50) NOT NULL,
	[Location] [varchar](50) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[NumAdults] [int] NOT NULL,
	[NumChildren] [int] NOT NULL,
 CONSTRAINT [PK_Trip] PRIMARY KEY CLUSTERED 
(
	[TripID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Trip]  WITH CHECK ADD  CONSTRAINT [FK_Trip_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([AgentID])
GO

ALTER TABLE [dbo].[Trip] CHECK CONSTRAINT [FK_Trip_Agent]
GO

ALTER TABLE [dbo].[Trip]  WITH CHECK ADD  CONSTRAINT [FK_Trip_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO

ALTER TABLE [dbo].[Trip] CHECK CONSTRAINT [FK_Trip_User]
GO


