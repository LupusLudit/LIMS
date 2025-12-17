using LIMS.Logic.Tools;

namespace LIMS.Tests
{
    [TestClass]
    public class BrightnessToolTests
    {
        [TestMethod]
        public void TestBrightnessValidation()
        {
            BrightnessTool tool = new BrightnessTool();

            float[] invalidValues = { -1f, 0f, 3.1f, 5f };

            foreach (float val in invalidValues)
            {
                tool.Brightness = val;
                Assert.AreEqual(1.0f, tool.Brightness);
            }

            tool.Brightness = 0.5f;
            Assert.AreEqual(0.5f, tool.Brightness);

            tool.Brightness = 4f;
            Assert.AreEqual(0.5f, tool.Brightness);

            tool.Brightness = 2.0f;
            Assert.AreEqual(2.0f, tool.Brightness);
        }

        [TestMethod]
        public void TestDefaultValues()
        {
            BrightnessTool tool = new BrightnessTool();
            Assert.AreEqual(1.0f, tool.Brightness);
            Assert.IsFalse(tool.Enabled);
        }

        [TestMethod]
        public void TestStateValidation()
        {
            BrightnessTool tool = new BrightnessTool();
            tool.Enabled = true;

            bool valid = tool.IsInValidState(out string? error);
            Assert.IsTrue(valid);
            Assert.IsNull(error);

            tool.Brightness = 0.5f;
            valid = tool.IsInValidState(out error);
            Assert.IsTrue(valid);
            Assert.IsNull(error);

            tool.Brightness = 2.5f;
            valid = tool.IsInValidState(out error);
            Assert.IsTrue(valid);
            Assert.IsNull(error);
        }
    }
}