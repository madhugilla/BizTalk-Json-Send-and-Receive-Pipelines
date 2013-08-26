using System.Xml;
using JsonPipelineComponents;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTL = Winterdom.BizTalk.PipelineTesting;


namespace JsonPipelineComponentsTests
{
    [TestClass]
    public class JsonToXmlConverterTests
    {
        [TestMethod]
        public void JsonToXmlConvertCreatesAValidXmlDocAndAddsARootNodeForJsonMessagesWhichDoNotHaveAParentTag()
        {
            //Arrange
            IBaseMessage inputMessage =
                PTL.MessageHelper.CreateFromStream(MessageHelper.LoadMessage("JsonPurchaseOrder.txt"));
            var jsonToXmlConverter = new JsonToXmlConverter();
            jsonToXmlConverter.Rootnode = "PurchaseOrder";
            PTL.ReceivePipelineWrapper testPipelineWrapper = PTL.PipelineFactory.CreateEmptyReceivePipeline();
            testPipelineWrapper.AddComponent(jsonToXmlConverter, PTL.PipelineStage.Decode);

            //Act
            PTL.MessageCollection outputMessages = testPipelineWrapper.Execute(inputMessage);

            //Assert
            Assert.IsTrue(outputMessages.Count == 1);
            IBaseMessage outputMessage = outputMessages[0];
            Assert.IsTrue(outputMessage.BodyPart.Data.Position == 0, "The stream position should be zero");
            //The stream is at position zero

            //the following code is an Assert in itself, will throw an exception if an invalid xml is generated.
            var output = new XmlDocument();
            output.Load(outputMessage.BodyPart.Data);
            Assert.IsTrue(System.String.CompareOrdinal(output.FirstChild.Name, jsonToXmlConverter.Rootnode) == 0);
        }

        [TestMethod]
        public void
            JsonToXmlConvertCreatesAValidXmlDocWhenRootNodePropertyIsNotSetForJsonMessagesWhichAlreadyHaveAParentTag()
        {
            //Arrange
            IBaseMessage inputMessage =
                PTL.MessageHelper.CreateFromStream(MessageHelper.LoadMessage("JsonPurchaseOrder.txt"));
            var jsonToXmlConverter = new JsonToXmlConverter();
            PTL.ReceivePipelineWrapper testPipelineWrapper = PTL.PipelineFactory.CreateEmptyReceivePipeline();
            testPipelineWrapper.AddComponent(jsonToXmlConverter, PTL.PipelineStage.Decode);

            //Act
            PTL.MessageCollection outputMessages = testPipelineWrapper.Execute(inputMessage);

            //Assert
            Assert.IsTrue(outputMessages.Count == 1);
            IBaseMessage outputMessage = outputMessages[0];
            Assert.IsTrue(outputMessage.BodyPart.Data.Position == 0, "The stream position should be zero");
            //The stream is at position zero


            //the following code is an Assert in itself, will throw an exception if an invalid xml is generated.
            var output = new XmlDocument();
            output.Load(outputMessage.BodyPart.Data);
            Assert.IsTrue(System.String.CompareOrdinal(output.FirstChild.Name, "PO") == 0);
        }
    }
}