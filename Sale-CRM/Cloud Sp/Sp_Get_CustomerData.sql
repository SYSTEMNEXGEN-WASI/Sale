USE [sngonclo_BMS]
GO
/****** Object:  StoredProcedure [dbo].[Sp_Get_CustomerData]    Script Date: 10/11/2019 11:13:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[Sp_Get_CustomerData]
@DealerCode char(5)

AS

Begin

Select

 C.CusCode
,C.CusDesc
,C.Address1
,C.CellNo
,C.AccountCode
,C.CusTypeCode
,C.NIC
From 

Customer C
where
	DealerCode = @DealerCode

End

