using System.Diagnostics;
using System.IO;
using JsonPipelineComponents;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using PTL = Winterdom.BizTalk.PipelineTesting;

namespace JsonPipelineComponentsTests
{
    [TestClass]
    public class XmlToJsonConverterTests
    {
        [TestMethod]
        public void XmlToJsonConverterPcRemovesAtTheRateCharactersWhenRemoveAtTheRateCharIsSetToTrue()
        {
            //Arrange
            IBaseMessage inputMessage =
                PTL.MessageHelper.CreateFromStream(MessageHelper.LoadMessage("PurchaseOrder.xml"));
            var xmlToJsonConverter = new XmlToJsonConverter {RemoveAtTheRateChar = true};
            PTL.SendPipelineWrapper testPipelineWrapper = PTL.PipelineFactory.CreateEmptySendPipeline();
            testPipelineWrapper.AddComponent(xmlToJsonConverter, PTL.PipelineStage.Encode);

            //Act
            IBaseMessage outputMessages = testPipelineWrapper.Execute(inputMessage);

            //Assert
            Assert.IsTrue(outputMessages.BodyPart.Data.Position == 0, "The stream position should be zero");
            var rdr = new StreamReader(outputMessages.BodyPart.Data);
            string outMsg = rdr.ReadToEnd();
            Debug.WriteLine("Output Message: " + outMsg);
            Assert.IsFalse(outMsg.IndexOfAny(new char['@'], 0) == 0);
        }


        [TestMethod]
        public void XmlToJsonConverterPcDoesNotRemovesAtTheRateCharactersWhenRemoveAtTheRateCharIsSetToFalse()
        {
            //Arrange
            IBaseMessage inputMessage =
                PTL.MessageHelper.CreateFromStream(MessageHelper.LoadMessage("PurchaseOrder.xml"));
            var xmlToJsonConverter = new XmlToJsonConverter {RemoveAtTheRateChar = false};
            PTL.SendPipelineWrapper testPipelineWrapper = PTL.PipelineFactory.CreateEmptySendPipeline();
            testPipelineWrapper.AddComponent(xmlToJsonConverter, PTL.PipelineStage.Encode);
            //Act
            IBaseMessage outputMessages = testPipelineWrapper.Execute(inputMessage);

            //Assert
            Assert.IsTrue(outputMessages.BodyPart.Data.Position == 0, "The stream position should be zero");
            var rdr = new StreamReader(outputMessages.BodyPart.Data);
            string outMsg = rdr.ReadToEnd();
            Debug.WriteLine("Output Message: " + outMsg);
            Assert.IsTrue(outMsg.IndexOf('@', 0) > 0);
        }

        [TestMethod]
        public void XmlToJsonConverterRemovesTheRootNodeWhenOmitRootObjectIsSetToFalse()
        {
            //Arrange
            IBaseMessage inputMessage =
                PTL.MessageHelper.CreateFromStream(MessageHelper.LoadMessage("PurchaseOrderWithOutNamespace.xml"));
            var xmlToJsonConverter = new XmlToJsonConverter {OmitRootObject = true};
            PTL.SendPipelineWrapper testPipelineWrapper = PTL.PipelineFactory.CreateEmptySendPipeline();
            testPipelineWrapper.AddComponent(xmlToJsonConverter, PTL.PipelineStage.Encode);
            //Act
            IBaseMessage outputMessages = testPipelineWrapper.Execute(inputMessage);

            //Assert
            Assert.IsTrue(outputMessages.BodyPart.Data.Position == 0, "The stream position should be zero");
            var rdr = new StreamReader(outputMessages.BodyPart.Data);
            string outMsg = rdr.ReadToEnd();
            JObject json = JObject.Parse(outMsg);
            Assert.IsNotNull(json);
            Assert.IsNull(json.SelectToken("PO"));
            Assert.IsTrue(System.String.CompareOrdinal(json.Property("poNum").Value.ToString(), "poNum_0") == 0);
        }

        [TestMethod]
        public void XmlToJsonConverterDoesNotRemovetheRootNodeWhenOmitRootObjectIsSetToTrue()
        {
            //Arrange
            IBaseMessage inputMessage =
                PTL.MessageHelper.CreateFromStream(MessageHelper.LoadMessage("PurchaseOrderWithOutNamespace.xml"));
            var xmlToJsonConverter = new XmlToJsonConverter {OmitRootObject = false};
            PTL.SendPipelineWrapper testPipelineWrapper = PTL.PipelineFactory.CreateEmptySendPipeline();
            testPipelineWrapper.AddComponent(xmlToJsonConverter, PTL.PipelineStage.Encode);
            //Act
            IBaseMessage outputMessages = testPipelineWrapper.Execute(inputMessage);

            //Assert
            Assert.IsTrue(outputMessages.BodyPart.Data.Position == 0, "The stream position should be zero");
            var rdr = new StreamReader(outputMessages.BodyPart.Data);
            string outMsg = rdr.ReadToEnd();
            JObject json = JObject.Parse(outMsg);
            Assert.IsNotNull(json);
            JToken po = json.SelectToken("PO");

            Assert.IsTrue(System.String.CompareOrdinal(po.SelectToken("poNum").Value<string>(), "poNum_0") == 0);
        }
    }
}