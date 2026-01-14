INSERT INTO Listings (UserId, ServiceType, Title, Description, Area, Location, Status, IsActive, CreatedAt) 
VALUES (1, 'Boya', '120m² Daire Boyama', '3 oda tavan dahil', 120, 'Kadıköy', 'Open', 1, GETDATE());

INSERT INTO ListingCalculations (ListingId, RequiredPaint, RequiredPrimer, RequiredCeramic, EstimatedMaterialMin, EstimatedMaterialMax, EstimatedLaborMin, EstimatedLaborMax, EstimatedDaysMin, EstimatedDaysMax) 
VALUES (1, 35, 15, 0, 8500, 12000, 6000, 9000, 3, 4);
