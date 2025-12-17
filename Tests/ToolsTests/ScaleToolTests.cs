using LIMS.Logic.Tools;

namespace LIMS.Tests
{
    [TestClass]
    public class ScaleToolTests
    {
        [TestMethod]
        public void TestScaleValueValidation()
        {
            ScaleTool tool = new ScaleTool();

            float[] invalidValues = { -1f, 0f, 2.1f, 5f };

            foreach (float val in invalidValues)
            {
                tool.ScaleValue = val;
                Assert.AreEqual(1f, tool.ScaleValue);
            }

            tool.ScaleValue = 1.5f;
            Assert.AreEqual(1.5f, tool.ScaleValue);

            tool.ScaleValue = 3.0f;
            Assert.AreEqual(1.5f, tool.ScaleValue);

            tool.ScaleValue = 0.5f;
            Assert.AreEqual(0.5f, tool.ScaleValue);
        }

        [TestMethod]
        public void TestDefaultValues()
        {
            ScaleTool tool = new ScaleTool();
            Assert.AreEqual(1f, tool.ScaleValue);
            Assert.IsFalse(tool.Enabled);
        }

        [TestMethod]
        public void TestStateValidation()
        {
            ScaleTool tool = new ScaleTool();
            tool.Enabled = true;

            bool valid = tool.IsInValidState(out string? error);
            Assert.IsTrue(valid);
            Assert.IsNull(error);

            tool.ScaleValue = 0.5f;
            valid = tool.IsInValidState(out error);
            Assert.IsTrue(valid);
            Assert.IsNull(error);

            tool.ScaleValue = 1.8f;
            valid = tool.IsInValidState(out error);
            Assert.IsTrue(valid);
            Assert.IsNull(error);
        }
    }
}