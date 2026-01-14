namespace ServiceMarketplace.Services
{
    public class ListingCalculationService
    {
        private readonly decimal PAINT_LITERS_PER_SQM = 0.15m; // Per coat
        private readonly int PAINT_COATS = 2;
        private readonly decimal PRIMER_LITERS_PER_SQM = 0.10m;
        private readonly decimal CERAMIC_WASTE_FACTOR = 1.10m; // 10% waste
        private readonly decimal DRYWALL_WASTE_FACTOR = 1.05m; // 5% waste

        // Price ranges per m² (min-max)
        private readonly Dictionary<string, (decimal min, decimal max)> MaterialPricesPerSqm = new()
        {
            { "Boya", (25, 60) },       // Paint materials per m²
            { "Seramik", (120, 350) },  // Ceramic per m²
            { "Alçıpan", (80, 180) }    // Drywall per m²
        };

        private readonly Dictionary<string, (decimal min, decimal max)> LaborPricesPerSqm = new()
        {
            { "Boya", (15, 35) },       // Paint labor per m²
            { "Seramik", (40, 80) },    // Ceramic installation per m²
            { "Alçıpan", (30, 60) }     // Drywall installation per m²
        };

        private readonly Dictionary<string, (double min, double max)> DaysPerSqm = new()
        {
            { "Boya", (0.02, 0.04) },    // ~25-50 m² per day
            { "Seramik", (0.08, 0.12) }, // ~8-12 m² per day  
            { "Alçıpan", (0.05, 0.08) }  // ~12-20 m² per day
        };

        public CalculationResult Calculate(string serviceType, decimal area, int roomCount, decimal ceilingHeight)
        {
            if (!MaterialPricesPerSqm.ContainsKey(serviceType))
            {
                return new CalculationResult { ErrorMessage = "Unknown service type" };
            }

            // Calculate effective area (for paint, include walls)
            decimal effectiveArea = serviceType == "Boya" 
                ? CalculatePaintableArea(area, roomCount, ceilingHeight)
                : area;

            // Material cost
            var matPrices = MaterialPricesPerSqm[serviceType];
            decimal materialMin = effectiveArea * matPrices.min;
            decimal materialMax = effectiveArea * matPrices.max;

            // Labor cost
            var laborPrices = LaborPricesPerSqm[serviceType];
            decimal laborMin = effectiveArea * laborPrices.min;
            decimal laborMax = effectiveArea * laborPrices.max;

            // Estimated days
            var daysPerSqm = DaysPerSqm[serviceType];
            int daysMin = Math.Max(1, (int)Math.Ceiling((double)effectiveArea * daysPerSqm.min));
            int daysMax = Math.Max(1, (int)Math.Ceiling((double)effectiveArea * daysPerSqm.max));

            // Material quantities
            var materials = CalculateMaterialQuantities(serviceType, effectiveArea);

            return new CalculationResult
            {
                ServiceType = serviceType,
                InputArea = area,
                EffectiveArea = effectiveArea,
                RoomCount = roomCount,
                CeilingHeight = ceilingHeight,
                
                MaterialCostMin = Math.Round(materialMin, 2),
                MaterialCostMax = Math.Round(materialMax, 2),
                LaborCostMin = Math.Round(laborMin, 2),
                LaborCostMax = Math.Round(laborMax, 2),
                TotalCostMin = Math.Round(materialMin + laborMin, 2),
                TotalCostMax = Math.Round(materialMax + laborMax, 2),
                
                EstimatedDaysMin = daysMin,
                EstimatedDaysMax = daysMax,
                
                Materials = materials
            };
        }

        private decimal CalculatePaintableArea(decimal floorArea, int roomCount, decimal ceilingHeight)
        {
            // Estimate wall area: perimeter × height
            // Approximate perimeter from floor area (assuming roughly square rooms)
            decimal avgRoomSize = floorArea / roomCount;
            decimal roomSide = (decimal)Math.Sqrt((double)avgRoomSize);
            decimal perimeterPerRoom = roomSide * 4;
            decimal wallArea = perimeterPerRoom * ceilingHeight * roomCount;
            
            // Subtract ~15% for windows/doors
            wallArea *= 0.85m;
            
            // Add ceiling area
            return wallArea + floorArea;
        }

        private List<MaterialEstimate> CalculateMaterialQuantities(string serviceType, decimal area)
        {
            var materials = new List<MaterialEstimate>();

            switch (serviceType)
            {
                case "Boya":
                    decimal paintLiters = area * PAINT_LITERS_PER_SQM * PAINT_COATS;
                    decimal primerLiters = area * PRIMER_LITERS_PER_SQM;
                    
                    materials.Add(new MaterialEstimate { Name = "İç Cephe Boyası", Quantity = Math.Ceiling(paintLiters), Unit = "Litre" });
                    materials.Add(new MaterialEstimate { Name = "Astar", Quantity = Math.Ceiling(primerLiters), Unit = "Litre" });
                    materials.Add(new MaterialEstimate { Name = "Macun", Quantity = Math.Ceiling(area * 0.3m), Unit = "Kg" });
                    materials.Add(new MaterialEstimate { Name = "Zımpara", Quantity = Math.Ceiling(area * 0.05m), Unit = "Adet" });
                    break;

                case "Seramik":
                    decimal ceramicSqm = area * CERAMIC_WASTE_FACTOR;
                    
                    materials.Add(new MaterialEstimate { Name = "Seramik", Quantity = Math.Ceiling(ceramicSqm), Unit = "m²" });
                    materials.Add(new MaterialEstimate { Name = "Yapıştırıcı", Quantity = Math.Ceiling(area * 5), Unit = "Kg" });
                    materials.Add(new MaterialEstimate { Name = "Derz Dolgu", Quantity = Math.Ceiling(area * 0.5m), Unit = "Kg" });
                    break;

                case "Alçıpan":
                    decimal drywallSqm = area * DRYWALL_WASTE_FACTOR;
                    
                    materials.Add(new MaterialEstimate { Name = "Alçıpan Levha", Quantity = Math.Ceiling(drywallSqm / 3), Unit = "Adet" }); // 3m² per panel
                    materials.Add(new MaterialEstimate { Name = "Profil (C/U)", Quantity = Math.Ceiling(area * 0.8m), Unit = "Metre" });
                    materials.Add(new MaterialEstimate { Name = "Alçı Bant", Quantity = Math.Ceiling(area * 0.3m), Unit = "Metre" });
                    materials.Add(new MaterialEstimate { Name = "Vida", Quantity = Math.Ceiling(area * 15), Unit = "Adet" });
                    break;
            }

            return materials;
        }
    }

    public class CalculationResult
    {
        public string ServiceType { get; set; } = string.Empty;
        public decimal InputArea { get; set; }
        public decimal EffectiveArea { get; set; }
        public int RoomCount { get; set; }
        public decimal CeilingHeight { get; set; }
        
        public decimal MaterialCostMin { get; set; }
        public decimal MaterialCostMax { get; set; }
        public decimal LaborCostMin { get; set; }
        public decimal LaborCostMax { get; set; }
        public decimal TotalCostMin { get; set; }
        public decimal TotalCostMax { get; set; }
        
        public int EstimatedDaysMin { get; set; }
        public int EstimatedDaysMax { get; set; }
        
        public List<MaterialEstimate> Materials { get; set; } = new();
        public string? ErrorMessage { get; set; }
    }

    public class MaterialEstimate
    {
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
}
