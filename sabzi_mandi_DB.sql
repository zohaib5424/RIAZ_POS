USE [POS_Sabzi_Mandi]
GO
/****** Object:  Table [dbo].[Bill]    Script Date: 12/21/2019 5:58:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bill](
	[id] [nvarchar](250) NOT NULL,
	[Bill_Type] [nvarchar](250) NULL,
	[Bill_Id] [nvarchar](250) NULL,
	[Bill_Date] [date] NULL,
	[Bill_Time] [time](7) NULL,
	[Net_Total_Amount] [decimal](18, 3) NULL,
	[Discount_Type] [nvarchar](250) NULL,
	[Discount] [decimal](18, 3) NULL,
	[Discount_Amount] [decimal](18, 3) NULL,
	[Tax_Type] [nvarchar](250) NULL,
	[Tax_Amount] [decimal](18, 3) NULL,
	[Service_Charges] [decimal](18, 3) NULL,
	[Voucher_Or_Coupon_Id] [nvarchar](250) NULL,
	[Vouchar_Coupon_Discount_Amount] [decimal](18, 3) NULL,
	[Total_Amount] [decimal](18, 3) NULL,
	[Paid_Amount] [decimal](18, 3) NULL,
	[Balance] [decimal](18, 3) NULL,
	[_Description] [nvarchar](250) NULL,
	[Customer_Vendor_Id] [nvarchar](250) NULL,
	[Payment_Method] [nvarchar](250) NULL,
	[Card_Number] [nvarchar](250) NULL,
	[Card_Holder_Name] [nvarchar](250) NULL,
	[Card_Transaction_No] [nvarchar](250) NULL,
	[Card_Type] [nvarchar](250) NULL,
	[Month] [nvarchar](250) NULL,
	[Year] [nvarchar](250) NULL,
	[Security_Code] [nvarchar](250) NULL,
	[Cheque_No] [nvarchar](250) NULL,
	[Bank_Name] [nvarchar](250) NULL,
	[Bank_Account_No] [nvarchar](250) NULL,
	[Payment_Note] [nvarchar](250) NULL,
	[AddedBy_UserId] [nvarchar](250) NULL,
	[_Date] [date] NULL,
	[_Time] [time](7) NULL,
	[Status] [nvarchar](250) NULL,
 CONSTRAINT [PK__Bill__3213E83F21735ADE] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Bill_Details]    Script Date: 12/21/2019 5:58:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Bill_Details](
	[id] [nvarchar](250) NOT NULL,
	[Bill_Id] [nvarchar](250) NULL,
	[Item_Id] [nvarchar](250) NULL,
	[Qty] [decimal](18, 3) NULL,
	[Unit_Cost] [decimal](18, 3) NULL,
	[Total_Amount] [decimal](18, 3) NULL,
	[Manufacturing_Date] [date] NULL,
	[Expiry_Date] [date] NULL,
	[Batch_Or_Lot_Number] [nvarchar](250) NULL,
	[Status] [nvarchar](250) NULL,
	[item_type] [nvarchar](250) NULL,
	[update_status] [varchar](250) NULL,
	[update_date] [date] NULL,
 CONSTRAINT [PK__Bill_Det__3213E83F7157BD4B] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Bill_Voucher_Or_Coupons_Details]    Script Date: 12/21/2019 5:58:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bill_Voucher_Or_Coupons_Details](
	[id] [nvarchar](250) NOT NULL,
	[Bill_Id] [nvarchar](250) NULL,
	[Voucher_Coupon_Code] [nvarchar](250) NULL,
	[Amount] [decimal](18, 3) NULL,
	[Status] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Brands]    Script Date: 12/21/2019 5:58:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brands](
	[id] [nvarchar](250) NOT NULL,
	[Brand_Name] [nvarchar](250) NULL,
	[AddedBy_UserId] [nvarchar](250) NULL,
	[_Date] [date] NULL,
	[_Time] [time](7) NULL,
	[Status] [nvarchar](250) NULL,
 CONSTRAINT [PK__Brands__3213E83F09A11AE8] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Categories]    Script Date: 12/21/2019 5:58:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[id] [nvarchar](250) NOT NULL,
	[parent_location] [nvarchar](250) NULL,
	[child_location] [nvarchar](250) NULL,
	[status] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Customer_Or_Vendor]    Script Date: 12/21/2019 5:58:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customer_Or_Vendor](
	[id] [nvarchar](250) NOT NULL,
	[Customer_Vendor_Type] [nvarchar](250) NULL,
	[customer_Or_Vendor_Name] [nvarchar](250) NULL,
	[Contact_Number] [nvarchar](250) NULL,
	[_Address] [nvarchar](250) NULL,
	[AddedBy_UserId] [nvarchar](250) NULL,
	[_Date] [date] NULL,
	[_Time] [time](7) NULL,
	[status] [nvarchar](250) NULL,
	[parent] [varchar](250) NULL,
	[Percentage] [varchar](250) NULL,
 CONSTRAINT [PK__Customer__3213E83F3292DE12] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[items]    Script Date: 12/21/2019 5:58:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[items](
	[id] [nvarchar](250) NOT NULL,
	[sku] [nvarchar](250) NULL,
	[Name] [nvarchar](250) NULL,
	[Brand_Id] [nvarchar](250) NULL,
	[Unit_Id] [nvarchar](250) NULL,
	[Category_Id] [nvarchar](250) NULL,
	[Barcode] [nvarchar](250) NULL,
	[Picture] [varbinary](max) NULL,
	[Purchase_Price] [decimal](18, 3) NULL,
	[Retail_Price] [decimal](18, 3) NULL,
	[Have_Mfg_Or_Exp_Date] [nvarchar](250) NULL,
	[Have_Batch_Or_Lot_No] [nvarchar](250) NULL,
	[AlertQty] [decimal](18, 3) NULL,
	[Qty] [decimal](18, 3) NULL,
	[Detail] [nvarchar](250) NULL,
	[AddedBy_UserId] [nvarchar](250) NULL,
	[_Date] [date] NULL,
	[_Time] [time](7) NULL,
	[Status] [nvarchar](250) NULL,
	[weight] [nvarchar](250) NULL,
	[update_status] [varchar](250) NULL,
	[update_date] [date] NULL,
	[Mazdori] [decimal](18, 3) NULL,
 CONSTRAINT [PK__items__3213E83F10A76A68] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[locations]    Script Date: 12/21/2019 5:58:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[locations](
	[id] [nvarchar](250) NOT NULL,
	[parent_location] [nvarchar](250) NULL,
	[child_location] [nvarchar](250) NULL,
	[status] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[login]    Script Date: 12/21/2019 5:58:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[login](
	[id] [nvarchar](250) NULL,
	[username] [nvarchar](250) NULL,
	[user_id] [nvarchar](250) NULL,
	[password] [nvarchar](250) NULL,
	[pattern] [nvarchar](250) NULL,
	[usertype] [nvarchar](250) NULL,
	[AddedBy_UserId] [nvarchar](250) NULL,
	[_date] [date] NULL,
	[_time] [time](7) NULL,
	[status] [nvarchar](250) NULL,
	[active_password_type] [nvarchar](250) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[loginlogs]    Script Date: 12/21/2019 5:58:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[loginlogs](
	[Id] [varchar](250) NOT NULL,
	[Login_Userid] [varchar](250) NULL,
	[Login_Date] [date] NULL,
	[Login_Time] [time](7) NULL,
	[Status] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[mml]    Script Date: 12/21/2019 5:58:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[mml](
	[id] [varchar](250) NOT NULL,
	[lk] [varchar](250) NULL,
	[dfrom] [date] NULL,
	[dto] [date] NULL,
	[status] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[printed_bills_log]    Script Date: 12/21/2019 5:58:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[printed_bills_log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Bill_Id] [nvarchar](250) NULL,
	[Print_Status] [nvarchar](250) NULL,
	[AddedBy_UserId] [nvarchar](250) NULL,
	[_Date] [date] NULL,
	[_Time] [time](7) NULL,
	[Status] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Report]    Script Date: 12/21/2019 5:58:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report](
	[id] [nvarchar](250) NULL,
	[Report_Id] [nvarchar](250) NULL,
	[report_date] [date] NULL,
	[Category_Id] [nvarchar](250) NULL,
	[Credit_Amount] [decimal](18, 3) NULL,
	[Recovery_Amount] [decimal](18, 3) NULL,
	[Expense_Amount] [decimal](18, 3) NULL,
	[Discount_Amount] [decimal](18, 3) NULL,
	[Net_Total] [decimal](18, 3) NULL,
	[Total_Profit] [decimal](18, 3) NULL,
	[Total_Sale_Amount] [decimal](18, 3) NULL,
	[Balance] [decimal](18, 3) NULL,
	[AddedBy_UserId] [nvarchar](250) NULL,
	[_Date] [date] NULL,
	[_Time] [time](7) NULL,
	[status] [nvarchar](250) NULL,
	[returnamount] [decimal](18, 2) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Report_Details]    Script Date: 12/21/2019 5:58:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report_Details](
	[id] [nvarchar](250) NULL,
	[Report_Id] [nvarchar](250) NULL,
	[Item_Id] [nvarchar](250) NULL,
	[Quantity_In_Stock] [decimal](18, 3) NULL,
	[Purchase_Amount] [decimal](18, 3) NULL,
	[Sale_Amount] [decimal](18, 3) NULL,
	[Profit_Per_Unit] [decimal](18, 3) NULL,
	[Sale_Qty] [decimal](18, 3) NULL,
	[Total_Profit] [decimal](18, 3) NULL,
	[Total_Sale_Amount] [decimal](18, 3) NULL,
	[status] [nvarchar](250) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[settings]    Script Date: 12/21/2019 5:58:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[settings](
	[id] [nvarchar](250) NOT NULL,
	[setting_code] [nvarchar](250) NULL,
	[setting] [nvarchar](max) NULL,
	[value] [nvarchar](250) NULL,
	[AddedBy_UserId] [nvarchar](250) NULL,
	[_date] [date] NULL,
	[_time] [time](7) NULL,
	[status] [nvarchar](250) NULL,
 CONSTRAINT [PK__settings__3213E83F94886E62] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Taxes]    Script Date: 12/21/2019 5:58:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Taxes](
	[id] [nvarchar](250) NOT NULL,
	[Tax_Name] [nvarchar](250) NULL,
	[Tax_Percent] [decimal](18, 3) NULL,
	[AddedBy_UserId] [nvarchar](250) NULL,
	[_Date] [date] NULL,
	[_Time] [time](7) NULL,
	[Status] [nvarchar](250) NULL,
 CONSTRAINT [PK__Taxes__3213E83F574E6BAB] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Units]    Script Date: 12/21/2019 5:58:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Units](
	[id] [nvarchar](250) NOT NULL,
	[Unit_Name] [nvarchar](250) NULL,
	[Short_Name] [nvarchar](250) NULL,
	[AddedBy_UserId] [nvarchar](250) NULL,
	[_Date] [date] NULL,
	[_Time] [time](7) NULL,
	[Status] [nvarchar](250) NULL,
 CONSTRAINT [PK__Units__3213E83F51374683] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Voucher_Or_Coupons]    Script Date: 12/21/2019 5:58:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Voucher_Or_Coupons](
	[id] [nvarchar](250) NOT NULL,
	[Voucher_Coupon_Code] [nvarchar](250) NULL,
	[Amount] [decimal](18, 3) NULL,
	[FromDate] [date] NULL,
	[FromTime] [time](7) NULL,
	[ToDate] [date] NULL,
	[ToTime] [time](7) NULL,
	[_Description] [nvarchar](250) NULL,
	[Status] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
