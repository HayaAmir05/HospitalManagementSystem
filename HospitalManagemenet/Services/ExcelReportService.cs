using ClosedXML.Excel;
using HospitalManagemenet.Models;

namespace HospitalManagemenet.Services
{
    public class ExcelReportService
    {
        public byte[] GenerateAppointmentsReport(List<Appointment> appointments)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Appointments");

            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Patient Name";
            worksheet.Cell(1, 3).Value = "Doctor Name";
            worksheet.Cell(1, 4).Value = "Appointment Date";
            worksheet.Cell(1, 5).Value = "Status";
            worksheet.Cell(1, 6).Value = "Created By";

            var headerRow = worksheet.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#2563eb");
            headerRow.Style.Font.FontColor = XLColor.White;

            int row = 2;
            foreach (var a in appointments)
            {
                worksheet.Cell(row, 1).Value = a.Id;
                worksheet.Cell(row, 2).Value = a.Patient?.Name ?? "";
                worksheet.Cell(row, 3).Value = a.Doctor?.Name ?? "";
                worksheet.Cell(row, 4).Value = a.AppointmentDate.ToString("yyyy-MM-dd");
                worksheet.Cell(row, 5).Value = a.Status;
                worksheet.Cell(row, 6).Value = a.createdBy;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}