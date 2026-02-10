using ServiceMarketplace.Models;

namespace ServiceMarketplace.Data
{
    public static class TemplateSeeder
    {
        public static void SeedTemplates(ApplicationDbContext context)
        {
            // Check if templates already exist
            if (context.ServiceTemplates.Any())
            {
                return; // Templates already seeded
            }

            // Ensure we have some basic categories and products first
            // This assumes AdminPriceReference data exists

            // Create Kitchen Renovation Template
            var kitchenTemplate = new ServiceTemplate
            {
                Name = "Standart Mutfak Tadilatı",
                Description = "Kapsamlı mutfak tadilatı için hazır şablon. Alçıpan, elektrik, sıhhi tesisat, boya ve mobilya dahil 158 kalem.",
                RenovationType = "Mutfak",
                IsActive = true
            };

            context.ServiceTemplates.Add(kitchenTemplate);
            context.SaveChanges(); // Save to get the ID

            // Kitchen Template Items - 158 lines
            var kitchenItems = new List<ServiceTemplateItem>();
            int orderCounter = 1;

            // ALÇIPAN İŞLERİ (20 items)
            kitchenItems.AddRange(new[]
            {
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "Tavan Alçıpan Kaplaması", 25, "m2"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "Duvar Alçıpan Kaplaması", 30, "m2"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "Alçıpan Levha 12.5mm", 55, "adet"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "Metal Profil U50", 100, "metre"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "Metal Profil C50", 100, "metre"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "Alçıpan Vidası", 5, "kutu"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "Derz Bandı", 4, "rulo"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "Derz Dolgu Macunu", 10, "kg"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "Köşe Profili", 20, "metre"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "Asma Tavan Askı Çubuğu", 30, "adet"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "Spot Armatürü Yuvası", 12, "adet"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "LED Şerit Aydınlatma Kartonpiyer", 10, "metre"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "Asma Tavan İşçiliği", 25, "m2", "Labor"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "Duvar Alçıpan İşçiliği", 30, "m2", "Labor"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "Köşe Profili Montaj İşçiliği", 20, "metre", "Labor"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "Derz Dolgu ve Zımparalama İşçiliği", 55, "m2", "Labor"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "Niş Yapımı", 2, "adet"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "Dekoratif Kartonpiyer", 8, "metre"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "Nem Dayanımlı Alçıpan Levha", 10, "adet"),
                CreateItem(kitchenTemplate.Id, "Alçıpan", orderCounter++, "İzolasyon Camyünü", 25, "m2"),
            });

            // ELEKTRİK İŞLERİ (35 items)
            kitchenItems.AddRange(new[]
            {
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Elektrik Tesisatı Projesi", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Trifaze Elektrik Kablosu 3x2.5mm", 50, "metre"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "NYA Kablo 2.5mm", 100, "metre"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "NYA Kablo 4mm", 50, "metre"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Priz Dalgıç 16A", 15, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Anahtar Dalgıç Tekli", 8, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Anahtar Dalgıç İkili", 4, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Kapaklı Topraklı Priz", 6, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Ankastre Priz Kutusu", 4, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "LED Spot Armatür 3W", 12, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "LED Panel 60x60", 2, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "LED Şerit RGB 5M", 10, "metre"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "LED Şerit Trafo 60W", 2, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "LED Dimmer", 2, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Sigorta Panosu 12 Mod", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Otomatik Sigorta 16A", 8, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Otomatik Sigorta 25A", 2, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Kaçak Akım Rölesi 25A", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Kablo Kanalı 16x16", 30, "metre"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Boru Kanalı 16mm", 40, "metre"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Boru Kanalı 20mm", 30, "metre"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Elektrik Kutusu 6x6", 25, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Davlumbaz Prizi Tesisatı", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Fırın Prizi Tesisatı", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Bulaşık Makinesi Prizi", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Buzdolabı Prizi", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Su Pompası Prizi", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Elektrik Tesisatı İşçiliği", 1, "adet", "Labor"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Priz Montajı İşçiliği", 15, "adet", "Labor"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Anahtar Montajı İşçiliği", 12, "adet", "Labor"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Spot Montajı İşçiliği", 12, "adet", "Labor"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Panel Montajı İşçiliği", 2, "adet", "Labor"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Sigorta Panosu Montajı", 1, "adet", "Labor"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Kablo Döşeme İşçiliği", 120, "metre", "Labor"),
                CreateItem(kitchenTemplate.Id, "Elektrik", orderCounter++, "Test ve Devreye Alma", 1, "adet", "Labor"),
            });

            // SIHHİ TESİSAT (25 items)
            kitchenItems.AddRange(new[]
            {
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "Eviye Bataryası Premium", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "Ankastre Eviye Paslanmaz", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "PPR Boru 20mm", 15, "metre"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "PPR Boru 25mm", 10, "metre"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "PPR Boru 32mm", 8, "metre"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "PPR Dirsek 20mm", 12, "adet"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "PPR Te 20mm", 8, "adet"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "PPR Vana 20mm", 4, "adet"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "PPR Vana 25mm", 2, "adet"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "Sifon Eviye Tipi", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "Bulaşık Makinesi Bağlantı Seti", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "Çamaşır Makinesi Musluğu", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "Su Filtresi 3 Kademeli", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "Kombi Bağlantı Boruları", 1, "set"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "Atsu Borusu 40mm", 6, "metre"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "Atsu Borusu 50mm", 4, "metre"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "Atsu Dirsek 40mm", 6, "adet"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "Atsu Te 40mm", 2, "adet"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "Tesisat İşçiliği", 1, "set", "Labor"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "Kaynak İşçiliği", 35, "nokta", "Labor"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "Eviye Montajı", 1, "adet", "Labor"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "Batarya Montajı", 1, "adet", "Labor"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "Sifon Montajı", 1, "adet", "Labor"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "Test ve Sızdırmazlık Kontrolü", 1, "adet", "Labor"),
                CreateItem(kitchenTemplate.Id, "Sıhhi Tesisat", orderCounter++, "Hidrofor Montajı İşçiliği", 1, "adet", "Labor"),
            });

            // BOYA İŞLERİ (18 items)
            kitchenItems.AddRange(new[]
            {
                CreateItem(kitchenTemplate.Id, "Boya", orderCounter++, "Duvar Astarı", 20, "kg"),
                CreateItem(kitchenTemplate.Id, "Boya", orderCounter++, "Plastik İç Cephe Boyası Premium", 35, "kg"),
                CreateItem(kitchenTemplate.Id, "Boya", orderCounter++, "Tavan Boyası", 15, "kg"),
                CreateItem(kitchenTemplate.Id, "Boya", orderCounter++, "Silikone Boya", 10, "kg"),
                CreateItem(kitchenTemplate.Id, "Boya", orderCounter++, "Ahşap Verniği", 2, "litre"),
                CreateItem(kitchenTemplate.Id, "Boya", orderCounter++, "Macun Alçı Bazlı", 25, "kg"),
                CreateItem(kitchenTemplate.Id, "Boya", orderCounter++, "Macun Sentetik", 5, "kg"),
                CreateItem(kitchenTemplate.Id, "Boya", orderCounter++, "Zımpara Kağıdı", 20, "adet"),
                CreateItem(kitchenTemplate.Id, "Boya", orderCounter++, "Fırça Seti", 1, "set"),
                CreateItem(kitchenTemplate.Id, "Boya", orderCounter++, "Rulo Set", 2, "set"),
                CreateItem(kitchenTemplate.Id, "Boya", orderCounter++, "Maskeleme Bandı", 6, "rulo"),
                CreateItem(kitchenTemplate.Id, "Boya", orderCounter++, "Koruyucu Naylon", 50, "m2"),
                CreateItem(kitchenTemplate.Id, "Boya", orderCounter++, "Macun Yapımı ve Zımpara İşçiliği", 55, "m2", "Labor"),
                CreateItem(kitchenTemplate.Id, "Boya", orderCounter++, "Astar Atma İşçiliği", 55, "m2", "Labor"),
                CreateItem(kitchenTemplate.Id, "Boya", orderCounter++, "Birinci Kat Boya İşçiliği", 55, "m2", "Labor"),
                CreateItem(kitchenTemplate.Id, "Boya", orderCounter++, "İkinci Kat Boya İşçiliği", 55, "m2", "Labor"),
                CreateItem(kitchenTemplate.Id, "Boya", orderCounter++, "Detay İşleri İşçiliği", 10, "saat", "Labor"),
                CreateItem(kitchenTemplate.Id, "Boya", orderCounter++, "Temizlik İşçiliği", 1, "set", "Labor"),
            });

            // MUTFAK MOBİLYASI (30 items)
            kitchenItems.AddRange(new[]
            {
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Üst Dolap 60cm", 4, "adet"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Üst Dolap 80cm", 2, "adet"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Alt Dolap 60cm", 3, "adet"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Alt Dolap 80cm", 2, "adet"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Çekmeceli Alt Dolap", 2, "adet"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Köşe Dolap Alt", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Köşe Dolap Üst", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Evye Altı Dolap", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Davlumbaz Üstü Dolap", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Fırın Dolabı", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Ankastre Dolap", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Granit Tezgah", 4, "m2"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Tezgah Arası Cam Panel", 3, "m2"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Dolap Kulp Seti", 1, "set"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Menteşe Seti", 1, "set"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Ray Seti", 1, "set"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Çöp Kovası Mekanizması", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Baharat Rafı", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Tabak Kurutma Rafı", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Soft Closing Menteşe", 20, "adet"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Led Aydınlatma Dolap İçi", 4, "metre"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Mobilya Montaj İşçiliği", 1, "set", "Labor"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Tezgah Montajı", 4, "m2", "Labor"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Cam Panel Montajı", 3, "m2", "Labor"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Dolap Ayar İşçiliği", 15, "adet", "Labor"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Ölçü Alma Hizmeti", 1, "adet", "Labor"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Taşıma Hizmeti", 1, "adet", "Labor"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Hurda Taşıma", 1, "adet", "Labor"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Garanti Belgesi", 1, "adet"),
                CreateItem(kitchenTemplate.Id, "Mutfak Mobilyası", orderCounter++, "Bakım Kiti", 1, "adet"),
            });

            // ZEMİN KAPLAMA (15 items)
            kitchenItems.AddRange(new[]
            {
                CreateItem(kitchenTemplate.Id, "Zemin Kaplama", orderCounter++, "Seramik 60x60 Premium", 15, "m2"),
                CreateItem(kitchenTemplate.Id, "Zemin Kaplama", orderCounter++, "Seramik Yapıştırıcısı", 6, "torba"),
                CreateItem(kitchenTemplate.Id, "Zemin Kaplama", orderCounter++, "Derz Dolgusu Gri", 5, "kg"),
                CreateItem(kitchenTemplate.Id, "Zemin Kaplama", orderCounter++, "Silikon Şeffaf", 3, "adet"),
                CreateItem(kitchenTemplate.Id, "Zemin Kaplama", orderCounter++, "Su Yalıtımı", 15, "m2"),
                CreateItem(kitchenTemplate.Id, "Zemin Kaplama", orderCounter++, "Şap Betonu", 2, "m3"),
                CreateItem(kitchenTemplate.Id, "Zemin Kaplama", orderCounter++, "Köşe Fayansı", 10, "metre"),
                CreateItem(kitchenTemplate.Id, "Zemin Kaplama", orderCounter++, "Süpürgelik Fayans", 10, "metre"),
                CreateItem(kitchenTemplate.Id, "Zemin Kaplama", orderCounter++, "Şap Atma İşçiliği", 15, "m2", "Labor"),
                CreateItem(kitchenTemplate.Id, "Zemin Kaplama", orderCounter++, "Su Yalıtımı İşçiliği", 15, "m2", "Labor"),
                CreateItem(kitchenTemplate.Id, "Zemin Kaplama", orderCounter++, "Seramik Döşeme İşçiliği", 15, "m2", "Labor"),
                CreateItem(kitchenTemplate.Id, "Zemin Kaplama", orderCounter++, "Derz Dolgu İşçiliği", 15, "m2", "Labor"),
                CreateItem(kitchenTemplate.Id, "Zemin Kaplama", orderCounter++, "Köşe Montajı İşçiliği", 10, "metre", "Labor"),
                CreateItem(kitchenTemplate.Id, "Zemin Kaplama", orderCounter++, "Süpürgelik Montajı", 10, "metre", "Labor"),
                CreateItem(kitchenTemplate.Id, "Zemin Kaplama", orderCounter++, "Silikon Çekme İşçiliği", 10, "metre", "Labor"),
            });

            // DİĞER İŞLER (15 items)
            kitchenItems.AddRange(new[]
            {
                CreateItem(kitchenTemplate.Id, "Diğer İşler", orderCounter++, "Söküm İşleri", 1, "set", "Labor"),
                CreateItem(kitchenTemplate.Id, "Diğer İşler", orderCounter++, "Eski Mobilya Sökümü", 1, "set", "Labor"),
                CreateItem(kitchenTemplate.Id, "Diğer İşler", orderCounter++, "Moloz Toplama ve Paketleme", 1, "set", "Labor"),
                CreateItem(kitchenTemplate.Id, "Diğer İşler", orderCounter++, "Moloz Taşıma", 3, "ton", "Labor"),
                CreateItem(kitchenTemplate.Id, "Diğer İşler", orderCounter++, "Hurda Taşıma", 1, "ton", "Labor"),
                CreateItem(kitchenTemplate.Id, "Diğer İşler", orderCounter++, "Koruyucu Naylon Çekme", 30, "m2", "Labor"),
                CreateItem(kitchenTemplate.Id, "Diğer İşler", orderCounter++, "Koruma Bandı", 50, "metre"),
                CreateItem(kitchenTemplate.Id, "Diğer İşler", orderCounter++, "Genel Temizlik", 1, "set", "Labor"),
                CreateItem(kitchenTemplate.Id, "Diğer İşler", orderCounter++, "İnce Temizlik", 1, "set", "Labor"),
                CreateItem(kitchenTemplate.Id, "Diğer İşler", orderCounter++, "Cam Temizliği", 1, "set", "Labor"),
                CreateItem(kitchenTemplate.Id, "Diğer İşler", orderCounter++, "Nakliye Ücreti", 1, "adet", "Labor"),
                CreateItem(kitchenTemplate.Id, "Diğer İşler", orderCounter++, "İskele Kurulumu", 1, "adet", "Labor"),
                CreateItem(kitchenTemplate.Id, "Diğer İşler", orderCounter++, "Proje Çizimi", 1, "adet", "Labor"),
                CreateItem(kitchenTemplate.Id, "Diğer İşler", orderCounter++, "Keşif Raporu", 1, "adet", "Labor"),
                CreateItem(kitchenTemplate.Id, "Diğer İşler", orderCounter++, "Teknik Destek", 1, "set", "Labor"),
            });

            context.ServiceTemplateItems.AddRange(kitchenItems);
            context.SaveChanges();

            // Create other templates (simplified versions)
            CreateBathroomTemplate(context);
            CreateLivingRoomTemplate(context);
            CreateBedroomTemplate(context);
        }

        private static ServiceTemplateItem CreateItem(int templateId, string subCategory, int order, string itemName, decimal quantity, string unit, string itemType = "Material")
        {
            return new ServiceTemplateItem
            {
                ServiceTemplateId = templateId,
                SubCategory = subCategory,
                ItemType = itemType,
                DisplayOrder = order,
                Quantity = quantity,
                Notes = itemName,
                DefaultNote = $"{quantity} {unit} {itemName}",
                // AdminPriceReferenceId will need to be set separately or we create dummy references
                AdminPriceReferenceId = 0 // This needs to be updated based on actual product data
            };
        }

        private static void CreateBathroomTemplate(ApplicationDbContext context)
        {
            var template = new ServiceTemplate
            {
                Name = "Standart Banyo Tadilatı",
                Description = "Kapsamlı banyo tadilatı paketi",
                RenovationType = "Banyo",
                IsActive = true
            };
            context.ServiceTemplates.Add(template);
            context.SaveChanges();
        }

        private static void CreateLivingRoomTemplate(ApplicationDbContext context)
        {
            var template = new ServiceTemplate
            {
                Name = "Standart Salon Tadilatı",
                Description = "Kapsamlı salon tadilatı paketi",
                RenovationType = "Salon",
                IsActive = true
            };
            context.ServiceTemplates.Add(template);
            context.SaveChanges();
        }

        private static void CreateBedroomTemplate(ApplicationDbContext context)
        {
            var template = new ServiceTemplate
            {
                Name = "Standart Yatak Odası Tadilatı",
                Description = "Kapsamlı yatak odası tadilatı paketi",
                RenovationType = "Yatak Odası",
                IsActive = true
            };
            context.ServiceTemplates.Add(template);
            context.SaveChanges();
        }
    }
}
