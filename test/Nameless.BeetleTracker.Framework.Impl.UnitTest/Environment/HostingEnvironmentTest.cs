using NUnit.Framework;

namespace Nameless.BeetleTracker.Environment {

    public class HostingEnvironmentTest {

        [Test]
        public void Get_ApplicationBasePath() {
            // arrange
            var environment = new HostingEnvironment();

            // act
            var appBasePath = environment.ApplicationBasePath;

            // assert
            Assert.AreEqual(typeof(HostingEnvironmentTest).Assembly.GetDirectoryPath(), appBasePath);
        }

        [Test]
        public void Set_And_Get_Data() {
            // arrange
            var environment = new HostingEnvironment();
            var key = "TestKey";
            var data = 1234;

            // act
            environment.SetData(key, data);
            var retrieved = environment.GetData(key);

            // assert
            Assert.AreEqual(data, retrieved);
        }
    }
}