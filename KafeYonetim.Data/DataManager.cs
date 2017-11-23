using KafeYonetim.Lib;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace KafeYonetim.Data
{

    public class DataManager
    {
        private static string connStr = "Data Source=DESKTOP-S3O5AOR;Initial Catalog=KafeYonetim;Integrated Security=True";

        private static SqlConnection CreateConnection()
        {
            var connection = new SqlConnection(connStr);
            connection.Open();

            return connection;
        }

        public static Kafe AktifKafeyiGetir()
        {
            using (var connection = CreateConnection())
            {
                var command = new SqlCommand("SELECT TOP 1 * FROM KAfe ", connection);

                using (var result = command.ExecuteReader())
                {
                    result.Read();
                    var kafe = new Kafe((int)result["Id"], result["Ad"].ToString(), result["AcilisSaati"].ToString(), result["KapanisSaati"].ToString());
                    kafe.Durum = (KafeDurum)result["Durum"];

                    return kafe;
                }
            }
        }

        public static void KafeAdiniGetir()
        {
            using (var connection = CreateConnection())
            {

                var command = new SqlCommand("SELECT TOP 1 Ad FROM KAfe ", connection);
                var result = (string)command.ExecuteScalar();

                Console.WriteLine($"Kafe Adı: {result}");

            }

            Console.ReadLine();

        }

        public static void UrunFiyatiniGetir()
        {
            using (var connection = CreateConnection())
            {

                Console.WriteLine("Ürün adı yazınız: ");
                string urunAdi = Console.ReadLine();

                //var command = new SqlCommand($"SELECT Fiyat FROM Urunler WHERE Ad = '{urunAdi}' ", connection);

                var command = new SqlCommand($"SELECT Fiyat FROM Urunler WHERE Ad = @Ad", connection);

                command.Parameters.AddWithValue("@Ad", urunAdi);

                var result = (double)command.ExecuteScalar();

                Console.WriteLine($"{urunAdi} ürününün fiyatı: {result}");

            }

            Console.ReadLine();

        }

        public static Tuple<int, int> MasaSayisi()
        {
            using (var connection = CreateConnection())
            {
                var command = new SqlCommand("SELECT COUNT(*) AS MasaSayisi, SUM(KisiSayisi) AS KisiSayisi FROM Masa", connection);

                var reader = command.ExecuteReader();

                reader.Read();

                var tuple = new Tuple<int, int>((int)reader["MasaSayisi"], (int)reader["KisiSayisi"]);

                return tuple;

                //return new Tuple<int, int>((int)reader["MasaSayisi"], (int)reader["KisiSayisi"]);               
                //return new MasaKisiSayisi { MasaSayisi = (int)reader["MasaSayisi"], KisiSayisi=(int)reader["KisiSayisi"]};
            }
        }

        public static List<Calisan> CalisanAra(string isim)
        {
            List<Calisan> calisanlar = new List<Calisan>();
            using (SqlConnection connection = CreateConnection())
            {
                SqlCommand command = new SqlCommand("select * from Calisan where Isim like @isim+'%'", connection);
                command.Parameters.AddWithValue("@isim", isim);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        calisanlar.Add(new Calisan(reader["Isim"].ToString(), (DateTime)reader["IseGirisTarihi"], AktifKafeyiGetir()));
                    }
                }
            }
            return calisanlar;
        }

        public static double GarsonBahsisToplami()
        {
            using (var connection = CreateConnection())
            {
                var command = new SqlCommand("SELECT SUM(Bahsis) FROM Garson", connection);

                double result = (double)command.ExecuteScalar();

                return result;
            }
        }

        public static int GarsonSayisi()
        {
            using (var connection = CreateConnection())
            {
                var command = new SqlCommand("SELECT COUNT(*) FROM Garson", connection);

                int result = (int)command.ExecuteScalar();

                return result;
            }
        }

        public static List<Calisan> CalisanlariSayfaliGetir(int sayfa)
        {
            List<Calisan> calisanlar = new List<Calisan>();
            using (SqlConnection connection = CreateConnection())
            {
                float toplamSayfaSayisi = Convert.ToSingle(CalisanSayisiniGetir()) / 20;
                SqlCommand commandCalisanlar = new SqlCommand("SELECT * FROM Calisan C Join CalisanGorev G on C.GorevID = G.Id ORDER BY C.Id ASC OFFSET @baslangic ROWS FETCH FIRST 20 ROWS ONLY", connection);
                if (sayfa <= Math.Ceiling(toplamSayfaSayisi))
                {
                    commandCalisanlar.Parameters.AddWithValue("@baslangic", (sayfa - 1) * 20);
                    using (SqlDataReader reader = commandCalisanlar.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Calisan calisan = new Calisan(reader["Isim"].ToString(), (DateTime)reader["IseGirisTarihi"], AktifKafeyiGetir());
                            calisan.Gorev.GorevAdi = reader["GorevAdi"].ToString();
                            calisanlar.Add(calisan);
                        }
                    }
                }
            }
            return calisanlar;
        }

        public static string ToplamBahsisler()
        {
            string Garson_Bahsis = "";
            using (SqlConnection connection = CreateConnection())
            {
                SqlCommand command = new SqlCommand("select Count(Id) GarsonSayisi, Sum(Bahsis) ToplamBahsis from Garson", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Garson_Bahsis = $"Garson Sayisi: {reader["GarsonSayisi"].ToString()}, Toplam Bahşiş: {reader["ToplamBahsis"]}";
                    }
                }
            }
            return Garson_Bahsis;
        }

        public static List<Garson> GarsonlariGetir()
        {
            List<Garson> garsonlar = new List<Garson>();
            using (SqlConnection connection = CreateConnection())
            {
                SqlCommand command = new SqlCommand("Select C.Isim, C.IseGirisTarihi, G.Bahsis from Calisan C join Garson G on C.GorevTabloId = G.Id where GorevId = 2", connection);
                using (SqlDataReader result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        garsonlar.Add(new Garson(result["Isim"].ToString(), (DateTime)result["IseGirisTarihi"], AktifKafeyiGetir()));
                    }
                }

            }
            return garsonlar;
        }

        public static object CalisanSayisiniGetir()
        {
            using (var connection = CreateConnection())
            {
                var command = new SqlCommand("SELECT COUNT(*) FROM Calisan", connection);

                int result = Convert.ToInt32(command.ExecuteScalar());

                return result;
            }
        }

        public static List<Garson> GarsonListele()
        {
            using (var conn = CreateConnection())
            {
                var command = new SqlCommand("SELECT Isim , IseGirisTarihi, Bahsis FROM Calisan INNER JOIN Garson ON Calisan.GorevTabloId = Garson.Id WHERE Calisan.GorevId = 2", conn);

                var list = new List<Garson>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var garson = new Garson(reader["Isim"].ToString(), (DateTime)reader["IseGirisTarihi"], AktifKafeyiGetir());
                        garson.Bahsis = Convert.ToInt32(reader["Bahsis"]);

                        list.Add(garson);
                    }

                    return list;
                }
            }
        }

        public static int CalisanSayfaSayisiniGetir(decimal sayfadakiKayitSayisi = 20)
        {
            using (var connection = CreateConnection())
            {
                var command = new SqlCommand("CalisanSayfaSayisiHesapla", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@SayfadakiOgeSayisi", sayfadakiKayitSayisi);

                int sayfaSayisi = Convert.ToInt32(command.ExecuteScalar());

                return sayfaSayisi;
            }
        }

        public static List<Calisan> CalisanListesiniGetir(int sayfaNumarasi = 1, int sayfadakiKayitSayisi = 20)
        {
            using (var connection = CreateConnection())
            {
                var command = new SqlCommand("SayfaSayisinaGoreCalisanGetir", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@sayfaNumarasi", sayfaNumarasi);
                command.Parameters.AddWithValue("@sayfadakiKayitSayisi", sayfadakiKayitSayisi);

                using (var reader = command.ExecuteReader())
                {
                    var list = new List<Calisan>();

                    while (reader.Read())
                    {
                        var calisan = new Calisan(reader["Isim"].ToString(), (DateTime)reader["IseGirisTarihi"], DataManager.AktifKafeyiGetir());

                        calisan.Gorev.GorevAdi = reader["GorevAdi"].ToString();

                        list.Add(calisan);
                    }

                    return list;
                }
            }
        }

        public static int AsciEkle(Asci asci)
        {
            using (var connection = CreateConnection())
            {
                //var command = new SqlCommand("asciEkle", connection);
                var command = new SqlCommand($@"
                            INSERT INTO Asci(Puan) VALUES(@puan);
                DECLARE @id int;
                SET @id = scope_identity();
                INSERT INTO Calisan(Isim, IseGirisTarihi, MesaideMi, KafeId, Durum, GorevId, GorevTabloId) VALUES(@isim, @IseGirisTarihi, @MesaideMi, @kafeId, @Durum, @GorevId, @id); SELECT scope_identity()", connection);

                command.Parameters.AddWithValue("@puan", FakeData.NumberData.GetNumber(0, 5));
                command.Parameters.AddWithValue("@isim", asci.Isim);
                command.Parameters.AddWithValue("@IseGirisTarihi", asci.IseGirisTarihi);
                command.Parameters.AddWithValue("@MesaideMi", asci.MesaideMi);
                command.Parameters.AddWithValue("@KafeId", asci.Kafe.Id);
                command.Parameters.AddWithValue("@Durum", asci.Durum);
                command.Parameters.AddWithValue("@GorevId", 1);

                //command.CommandType = System.Data.CommandType.StoredProcedure;

                var result = Convert.ToInt32(command.ExecuteScalar());

                return result;
            }
        }

        public static int BulasikciEkle(Bulasikci bulasikci)
        {
            using (var connection = CreateConnection())
            {
                //var command = new SqlCommand("BulasikciEkle", connection);
                var command = new SqlCommand($@"
                            INSERT INTO Bulasikci(HijyenPuani) VALUES(@hijyenPuan);
                DECLARE @id int;
                SET @id = scope_identity();
                INSERT INTO Calisan(Isim, IseGirisTarihi, MesaideMi, KafeId, Durum, GorevId, GorevTabloId) VALUES(@isim, @IseGirisTarihi, @MesaideMi, @kafeId, @Durum, @GorevId, @id); SELECT scope_identity()", connection);

                command.Parameters.AddWithValue("@hijyenPuan", FakeData.NumberData.GetNumber(0, 5));
                command.Parameters.AddWithValue("@isim", bulasikci.Isim);
                command.Parameters.AddWithValue("@IseGirisTarihi", bulasikci.IseGirisTarihi);
                command.Parameters.AddWithValue("@MesaideMi", bulasikci.MesaideMi);
                command.Parameters.AddWithValue("@KafeId", bulasikci.Kafe.Id);
                command.Parameters.AddWithValue("@Durum", bulasikci.Durum);
                command.Parameters.AddWithValue("@GorevId", 3);

                //command.CommandType = System.Data.CommandType.StoredProcedure;

                var result = Convert.ToInt32(command.ExecuteScalar());

                return result;
            }
        }

        public static List<Urun> UrunListesiniGetir()
        {
            using (var connection = CreateConnection())
            {
                var command = new SqlCommand("SELECT * FROM Urunler", connection);
                return UrunListesiHazirla(command.ExecuteReader());
            }

        }

        public static List<Urun> StoktaOlmayanUrunlerinListesiniGetir()
        {
            using (var connection = CreateConnection())
            {
                var command = new SqlCommand("SELECT * FROM Urunler WHERE StoktaVarMi = 'false'", connection);

                return UrunListesiHazirla(command.ExecuteReader());
            }

        }

        public static List<Urun> DegerdenYuksekFiyatliUrunleriGetir(double esikDeger)
        {
            using (var connection = CreateConnection())
            {
                var command = new SqlCommand("SELECT * FROM Urunler WHERE fiyat > @deger", connection);
                command.Parameters.AddWithValue("@deger", esikDeger);

                return UrunListesiHazirla(command.ExecuteReader());
            }
        }

        private static List<Urun> UrunListesiHazirla(SqlDataReader reader)
        {
            var urunListesi = new List<Urun>();

            using (reader)
            {
                while (reader.Read())
                {

                    var urun = new Urun((int)reader["Id"], reader["ad"].ToString()
                                        , (double)reader["Fiyat"]
                                        , (bool)reader["StoktaVarMi"]);


                    urunListesi.Add(urun);
                }

            }

            return urunListesi;

        }

        public static void KapatilmamimsBaglanti()
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    Console.Write("Bir değer giriniz: ");

                    var deger = Console.ReadLine();
                    var command = new SqlCommand("SELECT * FROM Urunler WHERE fiyat > @deger", connection);
                    command.Parameters.AddWithValue("@deger", double.Parse(deger));

                    using (var result = command.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            Console.WriteLine($"Ad: {result["Ad"]}");
                        }

                        Console.ReadLine();
                    }


                    var command2 = new SqlCommand("SELECT * FROM Urunler", connection);
                    //command.Parameters.AddWithValue("@deger", double.Parse(deger));

                    using (var result2 = command2.ExecuteReader())
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("HATA!");
                Console.WriteLine($"{ex.Message}");
            }

            Console.ReadLine();
        }

        public static bool UrunGir(string ad, double fiyat, bool stokDurum)
        {
            using (var connection = CreateConnection())
            {
                var command = new SqlCommand("INSERT INTO Urunler (ad, fiyat, stoktavarmi) VALUES (@ad, @fiyat, @stoktaVarMi)", connection);
                command.Parameters.AddWithValue("@ad", ad);
                command.Parameters.AddWithValue("@fiyat", fiyat);
                command.Parameters.AddWithValue("@stoktaVarMi", stokDurum);

                var result = command.ExecuteNonQuery();

                if (result > 0)
                {
                    return true;
                }

                return false;
            }

        }

        public static bool UrunGir(Urun urun)
        {
            using (var connection = CreateConnection())
            {
                var command = new SqlCommand("INSERT INTO Urunler (ad, fiyat, stoktavarmi) VALUES (@ad, @fiyat, @stoktaVarMi)", connection);
                command.Parameters.AddWithValue("@ad", urun.Ad);
                command.Parameters.AddWithValue("@fiyat", urun.Fiyat);
                command.Parameters.AddWithValue("@stoktaVarMi", urun.StoktaVarmi);

                var result = command.ExecuteNonQuery();

                if (result > 0)
                {
                    return true;
                }

                return false;
            }

        }

        //public static void UrunGir()
        //{
        //    using (var connection = CreateConnection())
        //    {

        //        Console.Write("Ürün Adını giriniz: ");
        //        var ad = (isWindows)? textBox1.Text : Console.ReadLine();

        //        Console.Write("Ürün fiyatını giriniz: ");
        //        var fiyat = double.Parse(Console.ReadLine());

        //        Console.Write("Ürün stokta var mı? (e/h): ");
        //        var stok = (Console.ReadLine() == "e") ? true : false;

        //        var command = new SqlCommand("INSERT INTO Urunler (ad, fiyat, stoktavarmi) VALUES (@ad, @fiyat, @stoktaVarMi)", connection);
        //        command.Parameters.AddWithValue("@ad", ad);
        //        command.Parameters.AddWithValue("@fiyat", fiyat);
        //        command.Parameters.AddWithValue("@stoktaVarMi", stok);

        //        var result = command.ExecuteNonQuery();

        //        if (result > 0)
        //        {
        //            Console.WriteLine("Kayıt eklendi.");
        //        }

        //    }

        //    Console.ReadLine();
        //}

        public static int SecilenUrunleriSil(string idList)
        {
            using (var connection = CreateConnection())
            {
                var command = new SqlCommand($"DELETE FROM Urunler WHERE ID IN ({idList}) ", connection);

                return command.ExecuteNonQuery();
            }
        }

        //public static int MasaEkle(string masaNo, int kafeId)
        //{
        //    return MasaEkle(masaNo, kafeId, DateTime.Now);
        //}

        //public static int MasaEkle(string masaNo, int kafeId, DateTime eklenmeTarihi)
        //{
        //    return 0;
        //}

        public static int MasaEkle(Masa masa)
        {
            using (var connection = CreateConnection())
            {
                var command = new SqlCommand("INSERT INTO Masa (MasaNo, KafeId, Durum, KisiSayisi) VALUES (@masaNo, @kafeId, @durum, @kisiSayisi);SELECT scope_identity()", connection);

                command.Parameters.AddWithValue("@masaNo", masa.MasaNo);
                command.Parameters.AddWithValue("@kafeId", masa.Kafe.Id);
                command.Parameters.AddWithValue("@durum", masa.Durum);
                command.Parameters.AddWithValue("@kisiSayisi", masa.KisiSayisi);

                int result = Convert.ToInt32(command.ExecuteScalar());

                return result;
            }
        }

        public static int GarsonEkle(Garson garson)
        {
            using (var connection = CreateConnection())
            {
                var commandGarson = new SqlCommand($@" 
                            INSERT INTO Garson (Bahsis) VALUES (@bahsis); 
                            DECLARE @id int;
                            SET @id= scope_identity();
                            INSERT INTO Calisan (Isim, IseGirisTarihi, MesaideMi, KafeId, Durum, GorevId, GorevTabloId) VALUES (@Isim, @IseGirisTarihi, @MesaideMi, @KafeId, @Durum, @GorevId, @id); SELECT scope_identity()", connection);
                commandGarson.Parameters.AddWithValue("@bahsis", FakeData.NumberData.GetNumber(5, 100));
                commandGarson.Parameters.AddWithValue("@Isim", garson.Isim);
                commandGarson.Parameters.AddWithValue("@IseGirisTarihi", garson.IseGirisTarihi);
                commandGarson.Parameters.AddWithValue("@MesaideMi", garson.MesaideMi);
                commandGarson.Parameters.AddWithValue("@KafeId", garson.Kafe.Id);
                commandGarson.Parameters.AddWithValue("@Durum", garson.Durum);
                commandGarson.Parameters.AddWithValue("@GorevId", 2);

                var result = Convert.ToInt32(commandGarson.ExecuteScalar());

                return result;
            }
        }
    }
}
