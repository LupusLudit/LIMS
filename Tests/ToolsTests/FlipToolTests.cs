using LIMS.Logic.Tools;

namespace LIMS.Tests
{
    [TestClass]
    public class FlipToolTests
    {
        [TestMethod]
        public void TestFlipPropertyValidation()
        {
            FlipTool tool = new FlipTool();

            Assert.IsFalse(tool.FlipHorizontal);
            Assert.IsFalse(tool.FlipVertical);

            tool.FlipHorizontal = true;
            Assert.IsTrue(tool.FlipHorizontal);

            tool.FlipVertical = true;
            Assert.IsTrue(tool.FlipVertical);

            tool.FlipHorizontal = false;
            Assert.IsFalse(tool.FlipHorizontal);

            tool.FlipVertical = false;
            Assert.IsFalse(tool.FlipVertical);
        }

        [TestMethod]
        public void TestInvalidStateNoDirection()
        {
            FlipTool tool = new FlipTool();
            tool.Enabled = true;

            bool valid = tool.IsInValidState(out string? error);
            Assert.IsFalse(valid);
            Assert.IsNotNull(error);

            tool.FlipHorizontal = true;
            valid = tool.IsInValidState(out error);
            Assert.IsTrue(valid);
            Assert.IsNull(error);

            tool.FlipHorizontal = false;
            valid = tool.IsInValidState(out error);
            Assert.IsFalse(valid);
            Assert.IsNotNull(error);
        }

        [TestMethod]
        public void TestValidStateDirections()
        {
            FlipTool tool = new FlipTool();
            tool.Enabled = true;

            tool.FlipHorizontal = true;
            bool valid = tool.IsInValidState(out string? error);
            Assert.IsTrue(valid);
            Assert.IsNull(error);

            tool.FlipHorizontal = false;
            tool.FlipVertical = true;
            valid = tool.IsInValidState(out error);
            Assert.IsTrue(valid);
            Assert.IsNull(error);

            tool.FlipHorizontal = true;
            valid = tool.IsInValidState(out error);
            Assert.IsTrue(valid);
            Assert.IsNull(error);
        }
    }
}