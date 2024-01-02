
/****** Object:  Table [dbo].[OtpData]    Script Date: 5/16/2023 9:37:03 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OtpData]') AND type in (N'U'))
DROP TABLE [dbo].[OtpData]
GO

/****** Object:  Table [dbo].[OtpData]    Script Date: 5/16/2023 9:37:03 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OtpData](
	[ID] [uniqueidentifier] NOT NULL,
	[Key] [nvarchar](100) NOT NULL,
	[OTP] [nvarchar](10) NOT NULL,
	[ExpireAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


