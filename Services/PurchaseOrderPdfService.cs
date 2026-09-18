using Microsoft.Extensions.Localization;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WarehouseManagement.Localization;
using WarehouseManagement.Models;

namespace WarehouseManagement.Services;

public class PurchaseOrderPdfService
{
    private readonly IStringLocalizer<SharedResource> L;

    public PurchaseOrderPdfService(
        IStringLocalizer<SharedResource> localizer)
    {
        L = localizer;
    }

    public byte[] Generate(PurchaseOrder order)
    {
        var supplierName =
            order.Supplier?.CompanyName ?? "—";

        var supplierNumber =
            order.Supplier?.SupplierNumber ?? "—";

        var statusText =
            GetStatusText(order.Status);

        var document =
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x =>
                        x.FontSize(9));

                    page.Header()
                        .Element(ComposeHeader);

                    page.Content()
                        .PaddingVertical(20)
                        .Element(content =>
                            ComposeContent(
                                content,
                                order,
                                supplierName,
                                supplierNumber,
                                statusText));

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("  •  ");
                            text.CurrentPageNumber();
                            text.Span(" / ");
                            text.TotalPages();
                        });
                });
            });

        return document.GeneratePdf();
    }

    private void ComposeHeader(
        IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem()
                .Column(column =>
                {
                    column.Item()
                        .Text(L["PurchaseOrder"].Value)
                        .FontSize(22)
                        .Bold();

                    column.Item()
                        .PaddingTop(3)
                        .Text("Purchase Order")
                        .FontSize(9)
                        .FontColor(Colors.Grey.Darken1);
                });
        });
    }

    private void ComposeContent(
        IContainer container,
        PurchaseOrder order,
        string supplierName,
        string supplierNumber,
        string statusText)
    {
        container.Column(column =>
        {
            column.Item()
                .Element(c =>
                    ComposeOrderInfo(
                        c,
                        order,
                        supplierName,
                        supplierNumber,
                        statusText));

            column.Item()
                .PaddingTop(20)
                .Element(c =>
                    ComposeItemsTable(c, order));

            column.Item()
                .PaddingTop(18)
                .AlignRight()
                .Element(c =>
                {
                    c.Width(220)
                        .Column(total =>
                        {
                            total.Item()
                                .Row(row =>
                                {
                                    row.RelativeItem()
                                        .Text(L["GrandTotal"].Value)
                                        .Bold();

                                    row.ConstantItem(90)
                                        .AlignRight()
                                        .Text(
                                            $"{order.Total:N2} €")
                                        .Bold()
                                        .FontSize(14);
                                });
                        });
                });

            if (!string.IsNullOrWhiteSpace(order.Note))
            {
                column.Item()
                    .PaddingTop(22)
                    .Element(c =>
                    {
                        c.Border(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .Padding(10)
                            .Column(note =>
                            {
                                note.Item()
                                    .Text(L["Note"].Value)
                                    .Bold();

                                note.Item()
                                    .PaddingTop(5)
                                    .Text(order.Note);
                            });
                    });
            }
        });
    }

    private void ComposeOrderInfo(
        IContainer container,
        PurchaseOrder order,
        string supplierName,
        string supplierNumber,
        string statusText)
    {
        container
            .Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(12)
            .Row(row =>
            {
                row.RelativeItem()
                    .Column(left =>
                    {
                        InfoLine(
                            left,
                            L["OrderNumber"].Value,
                            order.OrderNumber);

                        InfoLine(
                            left,
                            L["OrderDate"].Value,
                            order.OrderDate.ToString("dd.MM.yyyy"));

                        InfoLine(
                            left,
                            L["Reference"].Value,
                            string.IsNullOrWhiteSpace(
                                order.ReferenceNumber)
                                ? "—"
                                : order.ReferenceNumber!);
                    });

                row.RelativeItem()
                    .Column(right =>
                    {
                        InfoLine(
                            right,
                            L["Supplier"].Value,
                            supplierName);

                        InfoLine(
                            right,
                            L["SupplierNumber"].Value,
                            supplierNumber);

                        InfoLine(
                            right,
                            L["Status"].Value,
                            statusText);
                    });
            });
    }

    private static void InfoLine(
        ColumnDescriptor column,
        string label,
        string value)
    {
        column.Item()
            .PaddingBottom(5)
            .Row(row =>
            {
                row.ConstantItem(90)
                    .Text(label)
                    .FontColor(Colors.Grey.Darken1);

                row.RelativeItem()
                    .Text(value)
                    .Bold();
            });
    }

    private void ComposeItemsTable(
        IContainer container,
        PurchaseOrder order)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(28);
                columns.RelativeColumn(4);
                columns.RelativeColumn(1.3f);
                columns.RelativeColumn(1.4f);
                columns.RelativeColumn(1.6f);
            });

            table.Header(header =>
            {
                HeaderCell(header.Cell(), "#");
                HeaderCell(header.Cell(), L["Product"].Value);
                HeaderCell(header.Cell(), L["Quantity"].Value);
                HeaderCell(header.Cell(), L["UnitPrice"].Value);
                HeaderCell(header.Cell(), L["Total"].Value);
            });

            var index = 1;

            foreach (var item in order.Items)
            {
                var productName =
                    item.Product?.Name ?? "—";

                var sku =
                    item.Product?.SKU;

                var productText =
                    string.IsNullOrWhiteSpace(sku)
                        ? productName
                        : $"{sku} - {productName}";

                var lineTotal =
                    item.Quantity * item.UnitPrice;

                BodyCell(table.Cell(), index.ToString());

                BodyCell(
                    table.Cell(),
                    productText);

                BodyCell(
                    table.Cell(),
                    item.Quantity.ToString("N2"));

                BodyCell(
                    table.Cell(),
                    $"{item.UnitPrice:N2} €");

                BodyCell(
                    table.Cell(),
                    $"{lineTotal:N2} €");

                index++;
            }
        });
    }

    private static void HeaderCell(
        IContainer container,
        string text)
    {
        container
            .Background(Colors.Grey.Lighten3)
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten1)
            .Padding(7)
            .Text(text)
            .Bold();
    }

    private static void BodyCell(
        IContainer container,
        string text)
    {
        container
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten3)
            .Padding(7)
            .Text(text);
    }

    private string GetStatusText(
        PurchaseOrderStatus status)
    {
        return status switch
        {
            PurchaseOrderStatus.Entwurf =>
                L["PurchaseOrderStatusDraft"].Value,

            PurchaseOrderStatus.Bestellt =>
                L["PurchaseOrderStatusOrdered"].Value,

            PurchaseOrderStatus.TeilweiseGeliefert =>
                L["PurchaseOrderStatusPartiallyDelivered"].Value,

            PurchaseOrderStatus.Geliefert =>
                L["PurchaseOrderStatusDelivered"].Value,

            PurchaseOrderStatus.Storniert =>
                L["PurchaseOrderStatusCancelled"].Value,

            _ => status.ToString()
        };
    }
}
