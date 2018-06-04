using System.Collections.Generic;
using NUnit.Framework;

namespace Nameless.BeetleTracker.Dynamic {

    public class HashtableDynamicObjectTest {

        [Test]
        public void Get_And_Set_KeyValue() {
            // arrange
            var expected = "Value";
            dynamic hash = new HashtableDynamicObject();

            // act
            hash.Value = expected;

            // assert
            Assert.AreEqual(expected, hash.Value);
        }

        [Test]
        public void Get_And_Set_KeyValue_Via_Indexer() {
            // arrange
            var expected = "Value";
            dynamic hash = new HashtableDynamicObject();

            // act
            hash["Value"] = expected;

            // assert
            Assert.AreEqual(expected, hash.Value);
        }

        [Test]
        public void Get_And_Set_Dictionary_Via_Indexer() {
            // arrange
            var expected = "Value";
            dynamic hash = new HashtableDynamicObject();

            // act
            hash["Value"] = new Dictionary<string, string> {
                { "Key", "Value" }
            };

            // assert
            Assert.AreEqual(expected, hash.Value["Key"]);
        }
    }
}