using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System.Net.Mail;
using System.Text;

namespace SportsStore.Domain.Concrete
{
	public class EmailSettings
	{
		public bool WriteAsFile = true;
		public string FileLocation = @"C:\";
		public string From = "from@from.from";
		public string To = "to@to.to";
	}

	public class EmailOrderProcessor : IOrderProcessor
	{
		EmailSettings emailSettings;

		public EmailOrderProcessor(EmailSettings settings)
		{
			emailSettings = settings;
		}

		public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
		{
			using (SmtpClient smtpClient = new SmtpClient())
			{
				smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
				smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;


				MailMessage mailMessage = new MailMessage(emailSettings.From, emailSettings.To, "New Order", "Body");
				mailMessage.BodyEncoding = Encoding.ASCII;

				smtpClient.Send(mailMessage);
			}
		}
	}
}
