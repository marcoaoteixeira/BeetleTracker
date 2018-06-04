using Nameless.BeetleTracker.Resources;
using NUnit.Framework;

namespace Nameless.BeetleTracker {

    public class DescriptionAttributeTest {

        [Test]
        public void Get_Description_From_Resource() {
            // arrange
            var descriptionAttr = new DescriptionAttribute {
                ResourceKey = "MyDescriptionTest",
                ResourceType = typeof(InternalStrings)
            };
            var expected = InternalStrings.MyDescriptionTest;

            // act
            var actual = descriptionAttr.Description;

            // assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Get_Description_From_Fallback() {
            // arrange
            var expected = "ERROR";
            var descriptionAttr = new DescriptionAttribute(expected) {
                ResourceKey = "MyDescriptionTest_Error",
                ResourceType = typeof(InternalStrings)
            };

            // act
            var actual = descriptionAttr.Description;

            // assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Get_Description_From_NULL_Fallback() {
            // arrange
            var descriptionAttr = new DescriptionAttribute {
                ResourceKey = "MyDescriptionTest_Error",
                ResourceType = typeof(InternalStrings)
            };

            // act
            var actual = descriptionAttr.Description;

            // assert
            Assert.AreEqual(null, actual);
        }
    }
}