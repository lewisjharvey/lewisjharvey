USE [StaffPortal]
GO

/****** Object:  Table [dbo].[Portal_EventLog]    Script Date: 04/12/2011 13:54:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Portal_EventLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[Time] [datetime] NOT NULL,
	[EventLogTypeId] [int] NOT NULL,
	[Metadata] [nvarchar](255) NULL,
 CONSTRAINT [PK_Portal_EventLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[Portal_EventLogType]    Script Date: 04/12/2011 13:54:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Portal_EventLogType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Portal_EventLogType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


-- Add the default EventLogTypes
INSERT INTO [StaffPortal].[dbo].[Portal_EventLogType]
           ([Name])
     VALUES
           ('Helpdesk - Outstanding Calls - Visit')
GO
INSERT INTO [StaffPortal].[dbo].[Portal_EventLogType]
           ([Name])
     VALUES
           ('Helpdesk - Call - Visit')
GO

