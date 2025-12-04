using LIMS.Logic.Tools;

namespace LIMS.Tests
{

    [TestClass]
    public class WatermarkToolTests
    {

        [TestMethod]
        public void TestOpacityValidation()
        {
            WatermarkTool tool = new WatermarkTool();

            float[] invalidValues = { -2f, -1f, 1.5f, 3f };

            foreach (float val in invalidValues)
            {
                tool.Opacity = val;
                Assert.IsTrue(tool.Opacity >= 0 && tool.Opacity <= 1);
            }

            float[] validValues = { 0f, 0.5f, 1f };
            foreach (float val in validValues)
            {
                tool.Opacity = val;
                Assert.AreEqual(val, tool.Opacity);
            }
        }

        [TestMethod]
        public void TestInvalidWatermarkPath()
        {
            WatermarkTool tool = new WatermarkTool();
            Assert.IsNull(tool.WatermarkPath); // Null by default

            tool.WatermarkPath = "C:\\image.png";
            Assert.IsNotNull(tool.WatermarkPath);
            Assert.AreEqual("C:\\image.png", tool.WatermarkPath);

            tool.WatermarkPath = null;
            Assert.IsNotNull(tool.WatermarkPath); // Will not allow to set the watermark path to null again
        }

        [TestMethod]
        public void TestMissingWatermarkInvalidState()
        {
            WatermarkTool tool = new WatermarkTool();

            tool.Enabled = true;
            bool valid = tool.IsInValidState(out string? error);

            Assert.IsFalse(valid);
            Assert.IsNotNull(error);

            tool.WatermarkPath = "C:\\watermark.png";
            valid = tool.IsInValidState(out error);

            Assert.IsTrue(valid);
            Assert.IsNull(error);

            tool.WatermarkPath = "C:\\anotherWatermark.jpeg";
            valid = tool.IsInValidState(out error);

            Assert.IsTrue(valid);
            Assert.IsNull(error);
        }
    }
}
