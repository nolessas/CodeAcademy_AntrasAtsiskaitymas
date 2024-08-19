using System.Text;
using System.Net.Mail;
using System.Net;
using newConsoleApp2.Models;

namespace newConsoleApp2.Services
{
    public static class ReceiptGenerator
    {
        private const string SmtpServer = "smtp.gmail.com";
        private const int SmtpPort = 587;
        private const string SenderEmail = "egi.petreikis@gmail.com";
        private const string SenderPassword = "dcwt xftz wqqv bmkv";

        public static void PrintReceipt(Order order, bool isForRestaurant)
        {
            string receipt = GenerateReceiptText(order, isForRestaurant);
            Console.WriteLine();
            Console.WriteLine(receipt);
            Console.WriteLine();
            Console.WriteLine(isForRestaurant ? "Restaurant receipt printed successfully." : "Customer receipt printed successfully.");
            Console.WriteLine();
        }

        public static void EmailReceipt(Order order, bool isForRestaurant, string recipientEmail)
        {
            string receipt = GenerateReceiptText(order, isForRestaurant);
            string subject = isForRestaurant ? "Restaurant Receipt" : "Customer Receipt";

            try
            {
                using (SmtpClient smtpClient = new SmtpClient(SmtpServer, SmtpPort))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(SenderEmail, SenderPassword);

                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(SenderEmail),
                        Subject = subject,
                        Body = receipt,
                        IsBodyHtml = false,
                    };
                    mailMessage.To.Add(recipientEmail);

                    smtpClient.Send(mailMessage);
                    Console.WriteLine($"Receipt emailed successfully to {recipientEmail}");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                Console.WriteLine();
            }
        }

        private static string GenerateReceiptText(Order order, bool isForRestaurant)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(isForRestaurant ? "===== Restaurant Receipt =====" : "════════ Customer Receipt ════════");
            sb.AppendLine();
            sb.AppendLine($"Date: {order.OrderTime}");
            sb.AppendLine($"Table: {order.Table.Number}");
            sb.AppendLine();
            sb.AppendLine("-----------------------------");
            sb.AppendLine("Items:");
            foreach (var item in order.Items)
            {
                sb.AppendLine($"{item.Name} x{item.Quantity} - ${item.Price * item.Quantity:F2}");
            }
            sb.AppendLine("-----------------------------");
            sb.AppendLine();
            sb.AppendLine($"Total: ${order.CalculateTotal():F2}");
            sb.AppendLine();
            sb.AppendLine(isForRestaurant ? "Restaurant copy" : "Thank you for dining with us!");
            sb.AppendLine("=============================");
            return sb.ToString();
        }
    }
}