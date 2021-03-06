USE [iotdatabase]
GO
/****** Object:  Table [dbo].[foodconsumptionpattern]    Script Date: 30-05-2016 21:26:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[foodconsumptionpattern](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[menuitem] [varchar](50) NULL,
	[day] [varchar](50) NULL,
	[season] [varchar](50) NULL,
	[weight] [int] NULL,
	[footfall] [int] NULL,
	[daytype] [bit] NULL,
	[leftoverpersent] [int] NULL,
	[dayofweek] [int] NULL,
	[consumpdate] [date] NULL,
	[avgtemperature] [decimal](18, 0) NULL,
 CONSTRAINT [PK_foodconsumptionpattern] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[footfalldata]    Script Date: 30-05-2016 21:26:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[footfalldata](
	[capturedate] [date] NOT NULL,
	[footfall] [int] NULL,
 CONSTRAINT [PK_footfalldata] PRIMARY KEY CLUSTERED 
(
	[capturedate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[iotsinktable]    Script Date: 30-05-2016 21:26:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[iotsinktable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[deviceId] [varchar](50) NULL,
	[windSpeed] [decimal](18, 0) NULL,
	[counter] [int] NULL,
	[telemetrytime] [datetime] NULL,
 CONSTRAINT [PK_iotsinktable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PublicHolidays]    Script Date: 30-05-2016 21:26:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PublicHolidays](
	[Holiday] [date] NOT NULL,
	[DayOfWeek] [varchar](50) NOT NULL,
	[dow] [int] NULL,
 CONSTRAINT [PK_PublicHolidays] PRIMARY KEY CLUSTERED 
(
	[Holiday] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
