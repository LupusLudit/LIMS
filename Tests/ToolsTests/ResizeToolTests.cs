using LIMS.Logic.Tools;

namespace LIMS.Tests
{
    [TestClass]
    public class ResizeToolTests
    {
        [TestMethod]
        public void TestDimensionsValidation()
        {
            ResizeTool tool = new ResizeTool();

            int[] invalidValues = { -100, 0, 10001 };

            foreach (int val in invalidValues)
            {
                tool.Width = val;
                tool.Height = val;
                Assert.AreEqual(0, tool.Width);
                Assert.AreEqual(0, tool.Height);
            }

            tool.Width = 100;
            tool.Height = 100;
            Assert.AreEqual(100, tool.Width);
            Assert.AreEqual(100, tool.Height);

            tool.Width = -50;
            Assert.AreEqual(100, tool.Width);

            tool.Width = 500;
            Assert.AreEqual(500, tool.Width);
        }

        [TestMethod]
        public void TestInvalidStateMissingDimensions()
        {
            ResizeTool tool = new ResizeTool();
            tool.Enabled = true;

            bool valid = tool.IsInValidState(out string? error);
            Assert.IsFalse(valid);
            Assert.IsNotNull(error);

            tool.Width = 100;
            valid = tool.IsInValidState(out error);
            Assert.IsFalse(valid);
            Assert.IsNotNull(error);

            tool.Height = 100;
            valid = tool.IsInValidState(out error);
            Assert.IsTrue(valid);
            Assert.IsNull(error);
        }

        [TestMethod]
        public void TestValidState()
        {
            ResizeTool tool = new ResizeTool();
            tool.Enabled = true;

            tool.Width = 800;
            tool.Height = 600;

            bool valid = tool.IsInValidState(out string? error);
            Assert.IsTrue(valid);
            Assert.IsNull(error);

            tool.Width = 1024;
            valid = tool.IsInValidState(out error);
            Assert.IsTrue(valid);
            Assert.IsNull(error);

            tool.Height = 768;
            valid = tool.IsInValidState(out error);
            Assert.IsTrue(valid);
            Assert.IsNull(error);
        }
    }
}