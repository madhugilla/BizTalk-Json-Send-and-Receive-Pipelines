using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using Newtonsoft.Json;

namespace JsonPipelineComponents
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    [Guid("26F3E4F7-4FB4-449F-96DE-6E6CD0056227")]
    public class JsonToXmlConverter : IBaseComponent, IComponentUI, IComponent, IPersistPropertyBag
    {
        public string Rootnode { get; set; }

        #region IBaseComponent

        string IBaseComponent.Description
        {
            get { return "Pipeline component used to convert Json to Xml string"; }
        }

        string IBaseComponent.Name
        {
            get { return "JsonToXmlConverter"; }
        }

        string IBaseComponent.Version
        {
            get { return "1.0.0.0"; }
        }

        #endregion

        #region IComponentUI

        IntPtr IComponentUI.Icon
        {
            get { return new IntPtr(); }
        }

        IEnumerator IComponentUI.Validate(object projectSystem)
        {
            return null;
        }

        #endregion

        #region IPersistPropertyBag

        void IPersistPropertyBag.GetClassID(out Guid classID)
        {
            classID = new Guid("8984FF1C-3172-4E82-B82B-018E531F44DB");
        }

        void IPersistPropertyBag.InitNew()
        {
        }

        void IPersistPropertyBag.Load(IPropertyBag propertyBag, int errorLog)
        {
            object obj1 = PcHelper.ReadPropertyBag(propertyBag, "RootNode");
            if (obj1 != null)
                Rootnode = (string)obj1;
        }

        void IPersistPropertyBag.Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            PcHelper.WritePropertyBag(propertyBag, "RootNode", Rootnode);
        }

        #endregion

        #region IComponent

        IBaseMessage IComponent.Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            string json;

            Trace.WriteLine("JsonToXmlConverter Pipeline - Entered Execute()");
            Trace.WriteLine(String.Format("JsonToXmlConverter Pipeline - RootNode: {0}", Rootnode));

            var originalStream = pInMsg.BodyPart.GetOriginalDataStream();
            using (TextReader reader = new StreamReader(originalStream))
            {
                json = reader.ReadToEnd();
            }

            Trace.WriteLine(String.Format("JsonToXmlConverter Pipeline - Read JSON Data: {0}", json));
            Trace.WriteLine(String.Format("JsonToXmlConverter Pipeline - Deserializing JSON to Xml..."));

            try
            {
                // Append deserialized Xml data to master root node.
                XmlDocument xmlDoc = !string.IsNullOrWhiteSpace(Rootnode) ? JsonConvert.DeserializeXmlNode(json, Rootnode) : JsonConvert.DeserializeXmlNode(json);

                Trace.WriteLine(String.Format("JsonToXmlConverter Pipeline - Xml: {0}", xmlDoc.InnerXml));

                var output = Encoding.ASCII.GetBytes(xmlDoc.InnerXml);
                var memoryStream = new MemoryStream();
                memoryStream.Write(output, 0, output.Length);
                memoryStream.Position = 0;
                pInMsg.BodyPart.Data = memoryStream;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(String.Format("JsonToXmlConverter Pipeline - Exception: {0}", ex.Message));
            }

            pInMsg.BodyPart.Data.Position = 0;

            return pInMsg;
        }

        #endregion
    }
}