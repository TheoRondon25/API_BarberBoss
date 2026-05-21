using System.Reflection;
using BarberBoss.Application.UseCases.Billings.Reports.Pdf.Colors;
using BarberBoss.Application.UseCases.Billings.Reports.Pdf.Fonts;
using BarberBoss.Domain.Extensions;
using BarberBoss.Domain.Reports;
using BarberBoss.Domain.Repositories.Billings;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Fonts;

namespace BarberBoss.Application.UseCases.Billings.Reports.Pdf;
public class GenerateBillingsReportPdfUseCase : IGenerateBillingsReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private const int HEIGHT_ROW_BILLING_TABLE = 25;

    private readonly IBillingsReadOnlyRepository _repository;

    public GenerateBillingsReportPdfUseCase(IBillingsReadOnlyRepository respository)
    {        
        _repository = respository;

        GlobalFontSettings.FontResolver = new BillingsReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly datePdf)
    {
        var billings = await _repository.FilterByWeek(datePdf);
        if (billings.Count == 0)
        {
            return [];
        }

        var document = CreateDocument(datePdf);
        var page = CreatePage(document);

        CreateHeaderWithLogoAndName(page);

        var totalBillings = billings.Sum(billing => billing.Amount);
        CreateTotalBillingSection(page, datePdf, totalBillings);

        foreach(var billing in billings)
        {
            var table = CreateBillingTable(page);

            var row = table.AddRow();
            row.Height = HEIGHT_ROW_BILLING_TABLE;

            AddBillingTitle(row.Cells[0], billing.ServiceName);
            AddHeaderForAmount(row.Cells[3]);

            row = table.AddRow();
            row.Height = HEIGHT_ROW_BILLING_TABLE;

            row.Cells[0].AddParagraph(billing.Date.ToString("D"));
            SetStyleBaseForBillingInformation(row.Cells[0]);
            row.Cells[0].Format.LeftIndent = 10;

            row.Cells[1].AddParagraph(billing.Date.ToString("t"));
            SetStyleBaseForBillingInformation(row.Cells[1]);

            row.Cells[2].AddParagraph(billing.PaymentMethod.PaymentMethodToString());
            SetStyleBaseForBillingInformation(row.Cells[2]);

            AddAmountForBilling(row.Cells[3], billing.Amount);

            if (string.IsNullOrWhiteSpace(billing.Notes) == false)
            {
                var descriptionRow = table.AddRow();
                descriptionRow.Height = HEIGHT_ROW_BILLING_TABLE;

                descriptionRow.Cells[0].AddParagraph(billing.Notes);
                descriptionRow.Cells[0].Format.Font = new Font { Name = FontHelper.ROBOTO_VARIABLE, Size = 9, Color = ColorsHelper.BLACK };
                descriptionRow.Cells[0].Shading.Color = ColorsHelper.GRAY_LIGHT;
                descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                descriptionRow.Cells[0].MergeRight = 2;
                descriptionRow.Cells[0].Format.LeftIndent = 10;

                row.Cells[3].MergeDown = 1;
            }

            AddWhiteSpace(table);
        }

        return RenderDocument(document);
    }

    private Document CreateDocument(DateOnly date)
    {
        var startOfWeek = date.AddDays(-(int)date.DayOfWeek);
        var endOfWeek = startOfWeek.AddDays(6);

        var document = new Document();

        document.Info.Title = $"{ResourceReportGenerationMessages.BILLINGS_TO} {startOfWeek:dd/MM/yyyy} - {endOfWeek:dd/MM/yyyy}";
        document.Info.Author = "Theo Rondon";

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.BEBAS_NEUE_REGULAR;

        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;
    }

    private void CreateHeaderWithLogoAndName(Section page)
    {
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn(300);

        var row = table.AddRow();

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, "Logo", "logo_BarberBoss.png");

        row.Cells[0].AddImage(pathFile);

        row.Cells[1].AddParagraph("BARBEARIA DO THEO");
        row.Cells[1].Format.Font = new Font { Name = FontHelper.BEBAS_NEUE_REGULAR, Size = 25 };
        row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
    }

    private void CreateTotalBillingSection(Section page, DateOnly date, decimal totalBillings)
    {
        var startOfWeek = date.AddDays(-(int)date.DayOfWeek);

        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";


        var title = string.Format(ResourceReportGenerationMessages.TOTAL_BILLING_IN, startOfWeek);

        paragraph.AddFormattedText(title, new Font { Name = FontHelper.ROBOTO_VARIABLE, Size = 15 });

        paragraph.AddLineBreak();

        paragraph.AddFormattedText($"{CURRENCY_SYMBOL} {totalBillings}", new Font { Name = FontHelper.BEBAS_NEUE_REGULAR, Size = 50 });
    }

    private Table CreateBillingTable(Section page)
    {
        var table = page.AddTable();

        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;
        return table;
    }

    private void AddBillingTitle(Cell cell, string billingTitle)
    {
        cell.AddParagraph(billingTitle);
        cell.Format.Font = new Font { Name = FontHelper.BEBAS_NEUE_REGULAR, Size = 15, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.BLUE_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 10;
    }

    private void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
        cell.Format.Font = new Font { Name = FontHelper.BEBAS_NEUE_REGULAR, Size = 14, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.ACQUA_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void SetStyleBaseForBillingInformation(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontHelper.ROBOTO_VARIABLE, Size = 10, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.GRAY;
        cell.VerticalAlignment = VerticalAlignment.Center;        
    }

    private void AddAmountForBilling(Cell cell, decimal amount)
    {
        cell.AddParagraph($"{CURRENCY_SYMBOL} {amount}");
        cell.Format.Font = new Font { Name = FontHelper.ROBOTO_VARIABLE, Size = 10, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;        
    }

    private void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }

    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document,
        };

        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }
}
