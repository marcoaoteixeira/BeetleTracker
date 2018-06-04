using System.IO;
using Moq;
using Nameless.BeetleTracker.Text;
using NUnit.Framework;

namespace Nameless.BeetleTracker.Email {

    public class EmailServiceTest {

        [Test]
        public void Send_Email() {
            // arrange
            var messageBody = "E-mail body message";
            var interpolator = new Mock<IInterpolator>();
            interpolator
                .Setup(_ => _.Interpolate(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(messageBody);

            var pickupDirectory = Path.Combine(typeof(EmailServiceTest).Assembly.GetDirectoryPath(), "EmailPickupDirectory");
            var service = new EmailService(interpolator.Object, new SmtpClientSettings {
                DeliveryMethod = SmtpClientSettings.DeliveryMethods.PickupDirectory,
                PickupDirectory = pickupDirectory
            });

            // Clean pickup directory
            if (!Directory.Exists(pickupDirectory)) {
                Directory.CreateDirectory(pickupDirectory);
            }
            Directory.GetFiles(pickupDirectory, searchPattern: "*.eml", searchOption: SearchOption.AllDirectories).Each(File.Delete);


            // act
            var message = new Message {
                Body = messageBody,
                From = "from@test.com",
                Sender = "sender@test.com",
                Subject = "Test"
            };
            message.To.Add("recipient@test.com");
            service.Send(message);

            // assert
            var files = Directory.GetFiles(pickupDirectory);
            Assert.AreEqual(1, files.Length);
            Assert.True(files[0].EndsWith(".eml"));
            var text = File.ReadAllText(files[0]);
            Assert.True(text.Contains(messageBody));
        }

        [Test]
        public void Send_Email_With_BodyData() {
            // arrange
            var messageBody = @"
                General Grievous: Kenobi!
                Obi-Wan: {ObiWanResponse}!
            ";
            var interpolator = new Interpolator(new DataBinder());
            var pickupDirectory = Path.Combine(typeof(EmailServiceTest).Assembly.GetDirectoryPath(), "EmailPickupDirectory");
            var service = new EmailService(interpolator, new SmtpClientSettings {
                DeliveryMethod = SmtpClientSettings.DeliveryMethods.PickupDirectory,
                PickupDirectory = pickupDirectory
            });

            // Clean pickup directory
            if (!Directory.Exists(pickupDirectory)) {
                Directory.CreateDirectory(pickupDirectory);
            }
            Directory.GetFiles(pickupDirectory, searchPattern: "*.eml", searchOption: SearchOption.AllDirectories).Each(File.Delete);


            // act
            var message = new Message {
                Body = messageBody,
                BodyData = new {
                    ObiWanResponse = "Hello there"
                },
                From = "from@test.com",
                Sender = "sender@test.com",
                Subject = "Test"
            };
            message.To.Add("recipient@test.com");
            service.Send(message);

            // assert
            var files = Directory.GetFiles(pickupDirectory);
            Assert.AreEqual(1, files.Length);
            Assert.True(files[0].EndsWith(".eml"));
            var text = File.ReadAllText(files[0]);
            Assert.True(text.Contains("Hello there"));
        }
    }
}