using KafeYonetim.Data;
using KafeYonetim.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeYonetim.Sunum.AnaUygulama
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.Clear();

                Console.WriteLine("1. Ürün Listesini Getir");
                Console.WriteLine("2. Eşik Değerden Yüksek Fiyatlı Ürünlerin Listesini Getir");
                Console.WriteLine("3. Ürün Ekle");
                Console.WriteLine("4. Stokta olmayan ürünleri listele");
                Console.WriteLine("5. Ürün Sil");
                Console.WriteLine("6. Masa Ekle");
                Console.WriteLine("7. Masa Sayısı");
                Console.WriteLine("8. Garson Ekle");
                Console.WriteLine("9. Asçı Ekle");
                Console.WriteLine("10. Bulaşıkçı Ekle");
                Console.WriteLine("11. Çalışanları Listele");
                Console.WriteLine("12. Çalışan Sayısını Getir");
                Console.WriteLine("13. Garson Listele");
                Console.WriteLine("14. Garson Bahşişleri");
                Console.WriteLine("15. Çalışanları Sayfala");
                Console.WriteLine("16. Çalışan Ara");
                Console.WriteLine();
                Console.Write("Bir seçim yapınız (çıkmak için H harfine basınız): ");
                var secim = Console.ReadLine();

                switch (secim)
                {
                    case "1": ButunUrunlerListesiniYazdir(); Console.ReadLine(); break;
                    case "2": DegerdenYuksekFiyatliUrunleriGetir(); break;
                    case "3": UrunGir(); break;
                    case "4": StoktaOlmayanUrunleriListele(); break;
                    case "5": UrunSil(); break;
                    case "6": MasaEkle(); break;
                    case "7": MasaSayisi(); break;
                    case "8": GarsonEkle(); break;
                    case "9": AsciEkle(); break;
                    case "10": BulasikciEkle(); break;
                    case "11": CalisanListesiniGetir(); break;
                    case "12": CalisanSayisiniGetir(); break;
                    case "13": GarsonListele(); break;
                    case "14": ToplamGarsonBahsisleri(); break;
                    case "15": CalisanlariSayfaliListesi(); break;
                    case "16": CalisanIsimAra(); break;
                    //case "16": CalisanEkle(); break;
                    case "h": return;
                    default:
                        break;
                }

            } while (true);
        }

        private static void CalisanIsimAra()
        {
            Console.Clear();
            CalisanListesiniEkranaYazdir(DataManager.CalisanAra(Console.ReadLine()));
            Console.ReadLine();
        }

        private static void CalisanlariSayfaliListesi()
        {
            double toplamSayfa = Math.Ceiling(Convert.ToSingle(DataManager.CalisanSayisiniGetir()) / 20);
            Console.WriteLine($"Toplam {toplamSayfa} sayfa var");
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Sayfa Sayısını Yazın yada üst menü için C'ye basın..");
                try
                {
                    int sayfa = int.Parse(Console.ReadLine());

                    if (sayfa <= toplamSayfa)
                    {
                        foreach (var calisan in DataManager.CalisanlariSayfaliGetir(sayfa))
                        {
                            Console.WriteLine($"Adı:{calisan.Isim.PadRight(13)} İşe Giriş Tarihi: {calisan.IseGirisTarihi.ToString("dd.MM.yyyy").PadRight(13)} Görevi: {calisan.Gorev.GorevAdi}");
                        }

                        Console.WriteLine($"Sayfa: {sayfa}/{toplamSayfa}");
                    }
                    else
                    {
                        Console.WriteLine($"Böyle bir sayfa yok, 1 ile {toplamSayfa} arasında bir sayi giriniz..");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Sayfalar arasında dolaşmak için Enter'a basın ve sayfa sayısnı sayı belirtin. Üst menu için C 'ye basın.");
                    if (Console.ReadLine().ToLower() == "c")
                    {
                        return;
                    }

                }
                Console.ReadLine();
            }
        }

        private static void CalisanEkle()
        {
            int calisanSayisi = 0;
            while (true)
            {
                for (int a = 0; a < FakeData.NumberData.GetNumber(1, 5); a++)
                {
                    AsciEkle();
                    calisanSayisi++;
                    if (calisanSayisi == 209)
                    {
                        return;
                    }
                }
                for (int g = 0; g < FakeData.NumberData.GetNumber(10, 15); g++)
                {
                    GarsonEkle();
                    calisanSayisi++;
                    if (calisanSayisi == 209)
                    {
                        return;
                    }
                }
                for (int b = 0; b < FakeData.NumberData.GetNumber(7, 13); b++)
                {
                    BulasikciEkle();
                    calisanSayisi++;
                    if (calisanSayisi == 209)
                    {
                        return;
                    }
                }
            }

            Console.ReadLine();
        }

        private static void ToplamGarsonBahsisleri()
        {
            Console.Clear();
            Console.WriteLine(DataManager.ToplamBahsisler());
            Console.ReadLine();
        }

        private static void GarsonListele()
        {
            Console.Clear();

            Console.Write("İsim".PadRight(30));
            Console.Write("İşe Giriş Tarihi".PadRight(30));
            Console.WriteLine("Bahşiş".PadRight(5));

            Console.WriteLine("".PadRight(80, '='));

            List<Garson> garsonlar = DataManager.GarsonListele();

            foreach (var garson in garsonlar)
            {
                Console.WriteLine($"{garson.Isim.PadRight(30)}{garson.IseGirisTarihi.ToString("dd.MM.yyyy").PadRight(30)}{garson.Bahsis}");
            }

            int garsonSayisi = DataManager.GarsonSayisi();
            double bahsis = DataManager.GarsonBahsisToplami();
            Console.WriteLine();
            Console.WriteLine($"Garson Sayısı: {garsonSayisi}");
            Console.WriteLine($"Toplam Bahşiş: {bahsis}");

            Console.ReadLine();
        }

        private static void CalisanSayisiniGetir()
        {
            Console.Clear();
            var calisanSayisi = DataManager.CalisanSayisiniGetir();

            Console.WriteLine($"Toplam {calisanSayisi} çalışan var.");
            Console.ReadLine();
        }

        private static void BulasikciEkle()
        {
            Console.Clear();

            //Console.Write("Isim: ");
            string isim = FakeData.NameData.GetFirstName();

            var bulasikci = new Bulasikci(isim, FakeData.DateTimeData.GetDatetime(), DataManager.AktifKafeyiGetir());
            bulasikci.HijyenPuani = 0;

            int id = DataManager.BulasikciEkle(bulasikci);

            Console.WriteLine($"{id} id'si ile bulaşıkçı eklendi.");

            //Console.ReadLine();
        }

        private static void CalisanListesiniGetir()
        {
            List<Calisan> liste = DataManager.CalisanListesiniGetir();
            int toplamSayfaSayisi = DataManager.CalisanSayfaSayisiniGetir();
            int sayfaNumarasi = 1;

            while (true)
            {
                CalisanListesiniEkranaYazdir(liste);

                Console.WriteLine($"Sayfa: {sayfaNumarasi}/{toplamSayfaSayisi}");

                sayfaNumarasi = SayfaNumarasiniOku(toplamSayfaSayisi);

                if (sayfaNumarasi == -5484)
                {
                    return;
                }

                liste = DataManager.CalisanListesiniGetir(sayfaNumarasi);
            }
        }

        private static int SayfaNumarasiniOku(int toplamSayfaSayisi)
        {
            do
            {
                Console.Write("\bSayfa numarası giriniz (çıkmak için h/H harfine basınız): ");
                
                var girdi = Console.ReadLine().ToUpper();

                if (girdi == "H")
                {
                    return -5484;
                }

                int sayfaNumarasi;



                if (!int.TryParse(girdi, out sayfaNumarasi))
                {
                    Console.WriteLine("Lütfen geçerli bir sayı giriniz. ");
                    continue;
                }

                if (sayfaNumarasi < 1 || sayfaNumarasi > toplamSayfaSayisi)
                {
                    Console.WriteLine($"Lütfen 1 - {toplamSayfaSayisi} arasında bir sayıgirin.");
                    continue;

                }

                return sayfaNumarasi;
            } while (true);


        }

        private static void CalisanListesiniEkranaYazdir(List<Calisan> liste)
        {
            Console.Clear();

            Console.Write("Id".PadRight(5));
            Console.Write("İsim".PadRight(30));
            Console.Write("İşe Giriş Tarihi".PadRight(20));
            Console.WriteLine("Görev");
            Console.WriteLine("".PadRight(60, '='));

            foreach (var calisan in liste)
            {
                Console.WriteLine($"{calisan.Id.ToString().PadRight(5)}{calisan.Isim.PadRight(30)}{calisan.IseGirisTarihi.ToString("yyyy.MMMM.dddd").PadRight(20)}{calisan.Gorev.GorevAdi}");
            }
        }

        private static void AsciEkle()
        {
            Console.Clear();

            //Console.Write("Isim: ");
            string isim = FakeData.NameData.GetFirstName();

            var asci = new Asci(isim, FakeData.DateTimeData.GetDatetime(), DataManager.AktifKafeyiGetir());
            asci.Puan = 0;

            int id = DataManager.AsciEkle(asci);

            Console.WriteLine($"{id} id'si ile aşçı eklendi.");

            //Console.ReadLine();
        }

        private static void GarsonEkle()
        {
            Console.Clear();

            //Console.Write("Isim: ");
            string isim = FakeData.NameData.GetFirstName();

            var garson = new Garson(isim, FakeData.DateTimeData.GetDatetime(), DataManager.AktifKafeyiGetir());

            int id = DataManager.GarsonEkle(garson);

            Console.WriteLine($"{id} id'si ile aşçı eklendi.");

            //Console.ReadLine();

        }

        private static void MasaSayisi()
        {
            Console.Clear();

            var result = DataManager.MasaSayisi();

            Console.WriteLine($"{result.Item1} adet masada {result.Item2} kişilik kapasiteniz var.");
            Console.ReadLine();
        }

        private static void UrunSil()
        {
            ButunUrunlerListesiniYazdir();
            Console.WriteLine("\n\nSilmek istediğiniz ürünlern ID'lerini yazınız: ");

            var idLer = Console.ReadLine();

            int result = DataManager.SecilenUrunleriSil(idLer);

            ButunUrunlerListesiniYazdir();

            Console.WriteLine($"\n\nToplam {result} adet ürün silindi...");

            Console.ReadLine();
        }

        private static void StoktaOlmayanUrunleriListele()
        {
            var urunler = DataManager.StoktaOlmayanUrunlerinListesiniGetir();
            UrunListesiYazdir(urunler, "Stokta Olmayan Ürünler", true);
            Console.ReadLine();
        }

        private static void UrunListesiYazdir(List<Urun> urunler, string baslik, bool ekranTemizlensinMi)
        {

            if (ekranTemizlensinMi)
            {
                Console.Clear();
            }

            if (!string.IsNullOrWhiteSpace(baslik))
            {
                Console.WriteLine(baslik);
            }

            Console.WriteLine($"{"ID".PadRight(4)} {"Isim".PadRight(19)} {"Fiyat".PadRight(19)} Stok Durumu");
            Console.WriteLine("".PadRight(60, '='));

            foreach (var urun in urunler)
            {
                Console.WriteLine();
                Console.Write($"{urun.Id.ToString().PadRight(5)}");
                Console.Write($"{urun.Ad.PadRight(20)}");
                Console.Write($"{urun.Fiyat.ToString().PadRight(20)}");
                Console.Write($"{urun.StoktaVarmi}");
            }
        }

        private static void ButunUrunlerListesiniYazdir()
        {
            var urunler = DataManager.UrunListesiniGetir();
            UrunListesiYazdir(urunler, "Tüm Ürünler", true);
        }

        private static void DegerdenYuksekFiyatliUrunleriGetir()
        {
            Console.Clear();
            Console.Write("Eşik Değeri giriniz: ");

            var doubleEsikDeger = double.Parse(Console.ReadLine());
            var liste = DataManager.DegerdenYuksekFiyatliUrunleriGetir(doubleEsikDeger);
            string baslik = $"Fiyatı {doubleEsikDeger} TL'den Yüksek Ürünler";

            UrunListesiYazdir(liste, baslik, true);
            Console.ReadLine();
        }

        private static void UrunGir()
        {
            Console.Clear();

            Console.Write("Ürün Adı:");
            string urunAdi = Console.ReadLine();

            Console.Write("Fiyat:");
            double fiyat = double.Parse(Console.ReadLine());

            Console.Write("Stokta Var mı (E/H):");
            bool stokDurumu = Console.ReadLine().ToUpper() == "E";

            var yeniUrun = new Urun(59, urunAdi, fiyat, stokDurumu);

            if (DataManager.UrunGir(yeniUrun))
            {
                Console.WriteLine("Ürün başarıyla eklendi.");
            }
            else
            {
                Console.WriteLine("Ürün eklenirken bir hata oluştu...");
            }

            Console.ReadLine();
        }

        public static void MasaEkle()
        {
            Console.Clear();
            Console.WriteLine("MASA EKLEME");

            Console.Write("Masa No: ");
            string masaNo = Console.ReadLine();
            var yeniMasa = new Masa(masaNo, DataManager.AktifKafeyiGetir());
            yeniMasa.Durum = MasaDurum.Bos;
            Console.Write("Kişi Sayısı: ");
            yeniMasa.KisiSayisi = byte.Parse(Console.ReadLine());

            int id = DataManager.MasaEkle(yeniMasa);

            Console.WriteLine($"{id} ID'li masa eklendi");

            Console.ReadLine();
        }
    }
}
