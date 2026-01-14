BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260114115517_AddListing'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Offers]') AND [c].[name] = N'ListingId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Offers] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Offers] ALTER COLUMN [ListingId] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260114115517_AddListing'
)
BEGIN
    CREATE TABLE [Listings] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ServiceType] nvarchar(max) NOT NULL,
        [Title] nvarchar(200) NOT NULL,
        [Description] nvarchar(2000) NULL,
        [Area] decimal(18,2) NOT NULL,
        [RoomCount] int NOT NULL,
        [CeilingHeight] decimal(5,2) NOT NULL,
        [Location] nvarchar(500) NOT NULL,
        [StartDate] datetime2 NULL,
        [Deadline] datetime2 NULL,
        [Budget] decimal(18,2) NULL,
        [Status] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [EstimatedMaterialMin] decimal(18,2) NOT NULL,
        [EstimatedMaterialMax] decimal(18,2) NOT NULL,
        [EstimatedLaborMin] decimal(18,2) NOT NULL,
        [EstimatedLaborMax] decimal(18,2) NOT NULL,
        [EstimatedDaysMin] int NOT NULL,
        [EstimatedDaysMax] int NOT NULL,
        CONSTRAINT [PK_Listings] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Listings_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260114115517_AddListing'
)
BEGIN
    CREATE INDEX [IX_Offers_ListingId] ON [Offers] ([ListingId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260114115517_AddListing'
)
BEGIN
    CREATE INDEX [IX_Listings_UserId] ON [Listings] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260114115517_AddListing'
)
BEGIN
    ALTER TABLE [Offers] ADD CONSTRAINT [FK_Offers_Listings_ListingId] FOREIGN KEY ([ListingId]) REFERENCES [Listings] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260114115517_AddListing'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260114115517_AddListing', N'9.0.0');
END;

COMMIT;
GO

