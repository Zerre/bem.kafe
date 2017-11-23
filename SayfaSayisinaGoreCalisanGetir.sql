USE [KafeYonetim]
GO
/****** Object:  StoredProcedure [dbo].[SayfaSayisinaGoreCalisanGetir]    Script Date: 23.11.2017 11:31:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SayfaSayisinaGoreCalisanGetir] 
	@sayfaNumarasi int,
	@sayfadakiKayitSayisi int
AS
BEGIN
	SELECT Calisan.*, CalisanGorev.GorevAdi FROM Calisan
	INNER JOIN CalisanGorev ON Calisan.GorevId = CalisanGorev.Id
ORDER BY Calisan.Id
OFFSET ((@sayfaNumarasi -1) * @sayfadakiKayitSayisi) ROWS
FETCH NEXT @sayfadakiKayitSayisi ROWS ONLY
END
