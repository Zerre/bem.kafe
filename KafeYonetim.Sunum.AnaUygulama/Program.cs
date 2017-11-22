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

            //DataManager.KafeBilgisiniYazdir();

            //UrunListesiniYazdir();

            //dataManager.KafeAdiniGetir();

            //DataManager.UrunFiyatiniGetir();

            //dataManager.DegerdenYuksekFiyatliUrunleriGetir();


            // dataManager.KapatilmamimsBaglanti();

            //dataManager.SecilenUrunleriSil();

            //UrunGir();

            //DegerdenYuksekFiyatliUrunleriGetir();

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
                    case "h": return;
                    default:
                        break;
                }

            } while (true);
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

            Console.Write("Isim: ");
            string isim = Console.ReadLine();

            var bulasikci = new Bulasikci(isim, DateTime.Now, DataManager.AktifKafeyiGetir());
            bulasikci.HijyenPuani = 0;

            int id = DataManager.BulasikciEkle(bulasikci);

            Console.WriteLine($"{id} id'si ile bulaşıkçı eklendi.");

            Console.ReadLine();
        }

        private static void CalisanListesiniGetir()
        {
            Console.Clear();

            List<Calisan> liste = DataManager.CalisanListesiniGetir();

            Console.Write("Id".PadRight(5));
            Console.Write("İsim".PadRight(30));
            Console.Write("İşe Giriş Tarihi".PadRight(20));
            Console.WriteLine("Görev");
            Console.WriteLine("".PadRight(60, '='));

            foreach (var calisan in liste)
            {
                Console.WriteLine($"{calisan.Id.ToString().PadRight(5)}{calisan.Isim.PadRight(30)}{calisan.IseGirisTarihi.ToString("yyyy.MMMM.dddd").PadRight(20)}{calisan.Gorev.GorevAdi}");
            }

            Console.ReadLine();
        }

        private static void AsciEkle()
        {
            Console.Clear();

            Console.Write("Isim: ");
            string isim = Console.ReadLine();

            var asci = new Asci(isim, DateTime.Now, DataManager.AktifKafeyiGetir());
            asci.Puan = 0;

            int id = DataManager.AsciEkle(asci);

            Console.WriteLine($"{id} id'si ile aşçı eklendi.");

            Console.ReadLine();
        }

        private static void GarsonEkle()
        {
            Console.Clear();

            Console.Write("Isim: ");
            string isim = Console.ReadLine();

            var garson = new Garson(isim, DateTime.Now, DataManager.AktifKafeyiGetir());

            DataManager.GarsonEkle(garson);

            Console.ReadLine();

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
