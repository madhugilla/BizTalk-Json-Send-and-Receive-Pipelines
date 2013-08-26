using System.Diagnostics;
using System.IO;
using System.Text;
using JsonPipelineComponents;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTL = Winterdom.BizTalk.PipelineTesting;

namespace JsonPipelineComponentsTests
{
    [TestClass]
    public class DotNetTypesToJsonConverterTests
    {
        [TestMethod]
        public void DotNetTypesToJsonConverter()
        {
            //Arrange
            IBaseMessage inputMessage =
                PTL.MessageHelper.CreateFromStream(MessageHelper.LoadMessage("PurchaseOrder.xml"));
            var dotNetTypesToJsonConverter = new DotNetTypesToJsonConverter { TypeName = "PO" };
            PTL.SendPipelineWrapper testPipelineWrapper = PTL.PipelineFactory.CreateEmptySendPipeline();
            testPipelineWrapper.AddComponent(dotNetTypesToJsonConverter, PTL.PipelineStage.Encode);

            //Act
            IBaseMessage outputMessages = testPipelineWrapper.Execute(inputMessage);

            //Assert
            Assert.IsTrue(outputMessages.BodyPart.Data.Position == 0, "The stream position should be zero");
            var rdr = new StreamReader(outputMessages.BodyPart.Data);
            string outMsg = rdr.ReadToEnd();
            Debug.WriteLine("Output Message: " + outMsg);
            Assert.IsFalse(outMsg.IndexOfAny(new char['@'], 0) == 0);
        }
    }
}