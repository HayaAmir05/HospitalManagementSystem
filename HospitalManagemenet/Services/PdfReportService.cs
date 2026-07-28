using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using HospitalManagemenet.Models;

namespace HospitalManagemenet.Services
{
    public class PdfReportService
    {
        public byte[] GenerateAppointmentsReport(List<Appointment> appointments)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(11));


                    page.Header().PaddingBottom(20).Column(col =>
                    {
                        col.Item().Text("Appointments Report")
                            .FontSize(22).Bold().FontColor(Colors.Blue.Darken2).AlignCenter();
                        col.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                    });

                    page.Content().PaddingTop(15).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            void HeaderCell(string text) =>
                                header.Cell().Background(Colors.Blue.Darken2).Padding(6)
                                    .Text(text).FontColor(Colors.White).Bold();

                            HeaderCell("Id");
                            HeaderCell("Patient");
                            HeaderCell("Doctor");
                            HeaderCell("Date");
                            HeaderCell("Status");
                            HeaderCell("Created By");
                        });

                        bool alternate = false;
                        foreach (var a in appointments)
                        {
                            var bg = alternate ? Colors.Grey.Lighten4 : Colors.White;
                            alternate = !alternate;

                            void RowCell(string text) =>
                                table.Cell().Background(bg).Padding(6).Text(text);

                            RowCell(a.Id.ToString());
                            RowCell(a.Patient?.Name ?? "");
                            RowCell(a.Doctor?.Name ?? "");
                            RowCell(a.AppointmentDate.ToString("yyyy-MM-dd"));
                            RowCell(a.Status);
                            RowCell(a.createdBy);
                        }
                    });


                    page.Footer().PaddingTop(15).AlignCenter().Text(x =>
                    {
                        x.Span("Generated on ").FontColor(Colors.Grey.Darken1);
                        x.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm")).FontColor(Colors.Grey.Darken1);
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
