using Moq;
using Nameless.BeetleTracker.Environment;
using Nameless.BeetleTracker.Localization.Json;
using NUnit.Framework;

namespace Nameless.BeetleTracker.Localization {

    public class StringLocalizerTest {

        [Test]
        public void CreateStringLocalizerFactory() {
            var options = new LocalizationOptions {
                ResourcesRelativePath = "\\Localization\\Resources"
            };
            var hostingEnvironment = new Mock<IHostingEnvironment>();
            hostingEnvironment
                .Setup(_ => _.ApplicationBasePath)
                .Returns(typeof(StringLocalizerTest).Assembly.GetDirectoryPath());
            var factory = new FileSystemStringLocalizerFactory(hostingEnvironment.Object, options);

            var localizer = factory.Create(typeof(StringLocalizerTest));

            var value = localizer["It's a-me, Test!"];

            Assert.NotNull(value);
            Assert.AreEqual("Sou eu, Teste!", (string)value);

            System.Console.WriteLine($"SearchedLocation : {value.SearchedLocation}");
        }
    }
}