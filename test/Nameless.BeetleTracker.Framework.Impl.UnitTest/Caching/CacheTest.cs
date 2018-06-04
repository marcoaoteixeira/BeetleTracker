using System;
using System.IO;
using System.Threading;
using NUnit.Framework;

namespace Nameless.BeetleTracker.Caching {

    public class CacheTest {

        // InMemoryCache uses the MemoryCache infrastructure for cache allocation.
        [Test]
        public void Set_Object_To_Cache() {
            var cache = new InMemoryCache();
            cache.Set("test_key", 123);
            var value = cache.Get("test_key");
            Assert.AreEqual(123, value);
        }

        [Test]
        public void Try_Get_Object_Not_In_Cache() {
            var cache = new InMemoryCache();
            cache.Set("test_key", 123);
            var value = cache.Get("test_key1");
            Assert.IsNull(value);
        }

        [Test]
        public void Evict_Object_From_Cache() {
            var cache = new InMemoryCache();
            string itemRemoved = null;
            Action<string> evictionCallback = (s) => {
                itemRemoved = s;
            };
            cache.Set("test_key", 123, evictionCallback);
            cache.Remove("test_key");
            var value = cache.Get("test_key");
            Assert.IsNull(value);
            Assert.IsNotNull(itemRemoved);
            Assert.AreEqual("test_key", itemRemoved);
        }

        [Test]
        public void TimeCacheDependency_For_Object() {
            var cache = new InMemoryCache();
            string itemRemoved = null;
            Action<string> evictionCallback = (s) => {
                itemRemoved = s;
            };
            cache.Set("test_key", 123, evictionCallback, TimeCacheDependency.Create(TimeSpan.FromMilliseconds(50)));
            var valueBefore = cache.Get("test_key");
            Assert.NotNull(valueBefore);

            Thread.Sleep(100);

            var valueAfter = cache.Get("test_key");
            Assert.IsNull(valueAfter);
            Assert.IsNotNull(itemRemoved);
            Assert.AreEqual("test_key", itemRemoved);
        }

        [Test]
        public void FileCacheDependency_For_Object() {
            // arrange
            var fileContent = "This is a Test!";
            var filePath = Path.Combine(typeof(CacheTest).Assembly.GetDirectoryPath(), "FileDependencyCache.cache");
            File.Delete(filePath);
            File.WriteAllText(filePath, fileContent);

            var cache = new InMemoryCache();
            string itemRemoved = null;
            Action<string> evictionCallback = (s) => {
                itemRemoved = s;
            };
            cache.Set("test_key", 123, evictionCallback, FileCacheDependency.Create(filePath));
            var valueBefore = cache.Get("test_key");
            Assert.NotNull(valueBefore);

            File.WriteAllText(filePath, string.Empty);

            var valueAfter = cache.Get("test_key");
            Assert.IsNull(valueAfter);
            Assert.IsNotNull(itemRemoved);
            Assert.AreEqual("test_key", itemRemoved);
        }
    }
}