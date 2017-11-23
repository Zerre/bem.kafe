USE [KafeYonetim]
GO
/****** Object:  StoredProcedure [dbo].[CalisanSayfaSayisiHesapla]    Script Date: 23.11.2017 11:30:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CalisanSayfaSayisiHesapla] 
	@SayfadakiOgeSayisi decimal
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT CEILING((SELECT COUNT(*) FROM Calisan) / @SayfadakiOgeSayisi)
END
