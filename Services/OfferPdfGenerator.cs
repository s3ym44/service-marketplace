using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ServiceMarketplace.Models;

namespace ServiceMarketplace.Services
{
    public class OfferPdfGenerator
    {
        public byte[] GeneratePdf(Offer offer)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Element(c => ComposeHeader(c, offer));
                    page.Content().Element(c => ComposeContent(c, offer));
                    page.Footer().Element(ComposeFooter);
                });
            });

            return document.GeneratePdf();
        }

        private void ComposeHeader(IContainer container, Offer offer)
        {
            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("ServiceMarketplace")
                            .FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                        col.Item().Text("Teklif Belgesi").FontSize(12).Italic();
                    });

                    row.ConstantItem(100).Column(col =>
                    {
                        col.Item().AlignRight().Text($"Teklif #{offer.Id}").Bold();
                        col.Item().AlignRight().Text(offer.CreatedAt.ToString("dd.MM.yyyy"));
                    });
                });

                column.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
            });
        }

        private void ComposeContent(IContainer container, Offer offer)
        {
            container.PaddingVertical(20).Column(column =>
            {
                // Status Badge
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Durum: ").Bold();
                    row.RelativeItem().Text(offer.Status)
                        .FontColor(offer.Status == "Accepted" ? Colors.Green.Darken2 : 
                                   offer.Status == "Pending" ? Colors.Orange.Darken2 : Colors.Red.Darken2);
                });

                column.Item().PaddingTop(15).Text("Teklif Detayları").FontSize(14).Bold();
                column.Item().PaddingTop(5).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2);

                // Info Grid
                column.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("İlan No:").Bold();
                        col.Item().Text($"#{offer.ListingId}");
                    });
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("Tahmini Süre:").Bold();
                        col.Item().Text($"{offer.EstimatedDays} gün");
                    });
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("Garanti:").Bold();
                        col.Item().Text($"{offer.WarrantyMonths} ay");
                    });
                });

                // Materials Table
                column.Item().PaddingTop(20).Text("Malzemeler").FontSize(14).Bold();
                column.Item().PaddingTop(5).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2);

                if (offer.Materials != null && offer.Materials.Any())
                {
                    column.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1.5f);
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Malzeme").Bold();
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Marka").Bold();
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Miktar").Bold();
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Birim Fiyat").Bold();
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Toplam").Bold();
                        });

                        // Rows
                        foreach (var mat in offer.Materials)
                        {
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(mat.MaterialName);
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(mat.Brand);
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{mat.Quantity} {mat.Unit}");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"₺{mat.UnitPrice:N2}");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"₺{mat.TotalPrice:N2}");
                        }
                    });
                }
                else
                {
                    column.Item().PaddingTop(10).Text("Malzeme bilgisi bulunmamaktadır.").Italic();
                }

                // Pricing Summary
                column.Item().PaddingTop(25).Text("Fiyat Özeti").FontSize(14).Bold();
                column.Item().PaddingTop(5).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2);

                column.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem();
                    row.ConstantItem(200).Column(col =>
                    {
                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Text("İşçilik:");
                            r.RelativeItem().AlignRight().Text($"₺{offer.LaborCost:N2} ({offer.LaborCostType})");
                        });
                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Malzeme:");
                            r.RelativeItem().AlignRight().Text($"₺{offer.MaterialCostTotal:N2}");
                        });
                        col.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Darken1);
                        col.Item().PaddingTop(5).Row(r =>
                        {
                            r.RelativeItem().Text("TOPLAM:").Bold().FontSize(13);
                            r.RelativeItem().AlignRight().Text($"₺{offer.TotalOfferPrice:N2}").Bold().FontSize(13);
                        });
                    });
                });
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                column.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().Text("ServiceMarketplace © 2026").FontSize(9).FontColor(Colors.Grey.Medium);
                    row.RelativeItem().AlignRight().Text(text =>
                    {
                        text.Span("Sayfa ").FontSize(9);
                        text.CurrentPageNumber().FontSize(9);
                        text.Span(" / ").FontSize(9);
                        text.TotalPages().FontSize(9);
                    });
                });
            });
        }
    }
}
