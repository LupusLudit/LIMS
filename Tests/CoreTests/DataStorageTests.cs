using LIMS.Logic.Core;
using LIMS.Logic.ImageLoading;

namespace LIMS.Tests
{
    [TestClass]
    public class DataStorageTests
    {
        [TestMethod]
        public void TestStoringImages()
        {
            DataStorage storage = new DataStorage();
            ImageDataContainer image = new ImageDataContainer("img1.png", new byte[] { 1, 2, 3 });

            storage.AddImage(image);
            storage.TryGetImage("img1.png", out var result);

            Assert.IsInstanceOfType(result, typeof(ImageDataContainer));
            Assert.IsNotNull(result);
            Assert.AreEqual(image, result);

            storage.Clear();
            Assert.AreEqual(0, storage.GetAllImages().Count());

            storage.AddImage(new ImageDataContainer("img2.jpg", new byte[] { 6 }));
            storage.AddImage(new ImageDataContainer("img3.jpeg", new byte[] { 6 }));
            storage.AddImage(new ImageDataContainer("img4.png", new byte[] { 6 }));

            storage.TryGetImage("img3.jpeg", out var result2);
            Assert.IsNotNull(result2);
            Assert.AreEqual(3, storage.GetAllImages().Count());
        }

        [TestMethod]
        public void TestIgnoreDuplicates()
        {
            DataStorage storage = new DataStorage();
            ImageDataContainer img1 = new ImageDataContainer("Duplicate.png", new byte[] { 1 });
            ImageDataContainer img2 = new ImageDataContainer("Duplicate.png", new byte[] { 9 });

            storage.AddImage(img1);
            storage.AddImage(img2);

            storage.TryGetImage("Duplicate.png", out var result);
            Assert.IsNotNull(result);
            Assert.AreEqual(img1.RawBytes, result.RawBytes);


            storage.Clear();

            ImageDataContainer img3 = new ImageDataContainer("NotDuplicates.png", new byte[] { 6, 7, 8 });
            ImageDataContainer img4 = new ImageDataContainer("NotDuplicates.jpg", new byte[] { 1, 2, 3 });

            storage.AddImage(img3);
            storage.AddImage(img4);

            var all = storage.GetAllImages().ToList();

            CollectionAssert.Contains(all, img3);
            CollectionAssert.Contains(all, img4);
            Assert.AreEqual(2, all.Count);
        }

        [TestMethod]
        public void TestRemoveAllImages()
        {
            DataStorage storage = new DataStorage();
            storage.AddImage(new ImageDataContainer("a.png", new byte[] { 1, 4, 9 }));
            storage.AddImage(new ImageDataContainer("b.jpg", new byte[] { 8 }));

            storage.Clear();
            Assert.AreEqual(0, storage.GetAllImages().Count());

            storage.AddImage(new ImageDataContainer("c.jpeg", new byte[] { 6 }));

            storage.Clear();
            Assert.AreEqual(0, storage.GetAllImages().Count());
        }
    }
}
