using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;

namespace HospitalManagemenet.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendAppointmentConfirmationAsync(string patientEmail, string patientName, string doctorName, DateTime appointmentDate)
        {
            string senderEmail = _configuration["EmailSettings:SenderEmail"]!;
            string appPassword = _configuration["EmailSettings:AppPassword"]!;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Hospital Management System", senderEmail));
            message.To.Add(new MailboxAddress(patientName, patientEmail));
            message.Subject = "Appointment Confirmation";

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = $@"
    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; background-color: #f8fafc;'>
        <div style='background-color: #1e40af; padding: 20px; border-radius: 10px 10px 0 0; text-align: center;'>
            <h2 style='color: white; margin: 0;'>🏥 Hospital Management System</h2>
        </div>

        <div style='background-color: white; padding: 30px; border-radius: 0 0 10px 10px; box-shadow: 0 2px 8px rgba(0,0,0,0.08);'>
            <h3 style='color: #1e293b;'>Appointment Confirmed</h3>
            <p style='color: #475569; font-size: 15px;'>Dear {patientName},</p>
            <p style='color: #475569; font-size: 15px;'>Your appointment has been successfully booked. Here are the details:</p>

            <table style='width: 100%; border-collapse: collapse; margin: 20px 0;'>
                <tr>
                    <td style='padding: 10px; border-bottom: 1px solid #e2e8f0; color: #64748b; font-weight: bold;'>Doctor</td>
                    <td style='padding: 10px; border-bottom: 1px solid #e2e8f0; color: #1e293b;'>Dr. {doctorName}</td>
                </tr>
                <tr>
                    <td style='padding: 10px; border-bottom: 1px solid #e2e8f0; color: #64748b; font-weight: bold;'>Date</td>
                    <td style='padding: 10px; border-bottom: 1px solid #e2e8f0; color: #1e293b;'>{appointmentDate:dddd, dd MMMM yyyy}</td>
                </tr>
                <tr>
                    <td style='padding: 10px; color: #64748b; font-weight: bold;'>Status</td>
                    <td style='padding: 10px;'>
                        <span style='background-color: #fef3c7; color: #92400e; padding: 4px 10px; border-radius: 12px; font-size: 13px;'>Pending</span>
                    </td>
                </tr>
            </table>

            <p style='color: #475569; font-size: 14px;'>Please arrive 10 minutes early. If you need to reschedule or cancel, please contact the hospital reception.</p>

            <hr style='border: none; border-top: 1px solid #e2e8f0; margin: 25px 0;' />

            <p style='color: #94a3b8; font-size: 12px; text-align: center;'>
                This is an automated message from the Hospital Management System.<br/>
                Please do not reply directly to this email.
            </p>
        </div>
    </div>"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(senderEmail, appPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }                              
    }
}





                             
                                         







