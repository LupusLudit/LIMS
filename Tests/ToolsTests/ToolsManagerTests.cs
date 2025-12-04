using LIMS.Logic.ImageLoading;
using LIMS.Logic.Tools;

namespace LIMS.Tests
{

    [TestClass]
    public class ToolsManagerTests
    {
        private class DummyTool : ToolBase
        {
            public override void Apply(ImageDataContainer image) { }
            public override bool IsInValidState(out string? errorMessage)
            {
                errorMessage = null;
                return true;
            }
        }

        [TestMethod]
        public void TestToolRegistration()
        {
            ToolsManager manager = new ToolsManager();

            DummyTool t1 = new DummyTool();
            DummyTool t2 = new DummyTool();
            DummyTool t3 = new DummyTool();

            manager.RegisterTool(t1);
            manager.RegisterTool(t2);
            manager.RegisterTool(t3);

            Assert.AreEqual(3, manager.Tools.Count());
            Assert.IsTrue(manager.Tools.Contains(t1));
            Assert.IsTrue(manager.Tools.Contains(t2));
            Assert.IsTrue(manager.Tools.Contains(t3));
        }

        [TestMethod]
        public void TestIgnoreDuplicateTools()
        {
            ToolsManager manager = new ToolsManager();

            DummyTool tool = new DummyTool();

            manager.RegisterTool(tool);
            manager.RegisterTool(tool);

            Assert.AreEqual(1, manager.Tools.Count());
            Assert.IsTrue(manager.Tools.Contains(tool));

            DummyTool secondTool = new DummyTool();
            manager.RegisterTool(secondTool);

            Assert.AreEqual(2, manager.Tools.Count());
        }

        [TestMethod]
        public void TestRegisterNullTool()
        {
            ToolsManager manager = new ToolsManager();

            manager.RegisterTool<DummyTool>(null!);

            Assert.AreEqual(0, manager.Tools.Count());
        }
    }
}
