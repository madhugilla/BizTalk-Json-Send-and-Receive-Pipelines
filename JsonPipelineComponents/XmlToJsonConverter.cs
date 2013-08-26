using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace JsonPipelineComponents
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    [Guid("6303A9C3-C6ED-46B5-A1A5-E445470676EC")]
    public class XmlToJsonConverter : IBaseComponent, IComponentUI, IComponent, IPersistPropertyBag
    {
        public bool RemoveAtTheRateChar { get; set; }

        public bool OmitRootObject { get; set; }

        #region IBaseComponent

        string IBaseComponent.Description
        {
            get { return "Pipeline component used to convert Xml to Json string"; }
        }

        string IBaseComponent.Name
        {
            get { return "XmlToJsonConverter"; }
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
            classID = new Guid("9A25708D-DECE-4CFB-B1F8-D26E9CBCF63E");
        }

        void IPersistPropertyBag.InitNew()
        {
        }

        void IPersistPropertyBag.Load(IPropertyBag propertyBag, int errorLog)
        {
            object obj1 = PcHelper.ReadPropertyBag(propertyBag, "RemoveAtTheRateChar");
            if (obj1 != null)
                RemoveAtTheRateChar = (bool) obj1;

            obj1 = PcHelper.ReadPropertyBag(propertyBag, "OmitRootObject");
            if (obj1 != null)
                OmitRootObject = (bool) obj1;
        }

        void IPersistPropertyBag.Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            PcHelper.WritePropertyBag(propertyBag, "RemoveAtTheRateChar", RemoveAtTheRateChar);
            PcHelper.WritePropertyBag(propertyBag, "OmitRootObject", OmitRootObject);
        }

        #endregion

        #region IComponent

        IBaseMessage IComponent.Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            Trace.WriteLine("JsonToXmlConverter Pipeline - Entered Execute()");
            Trace.WriteLine("JsonToXmlConverter Pipeline - RemoveAtTheRateChar is set to: " + RemoveAtTheRateChar);
            Trace.WriteLine("JsonToXmlConverter Pipeline - OmitRootObject is set to: " + OmitRootObject);

            IBaseMessagePart bodyPart = pInMsg.BodyPart;
            if (bodyPart != null)
            {
                Stream originalStream = bodyPart.GetOriginalDataStream();
                if (originalStream != null)
                {
                    var xdoc = new XmlDocument();
                    xdoc.Load(originalStream);

                    if (xdoc.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                        xdoc.RemoveChild(xdoc.FirstChild);

                    string jsonText;
                    if (OmitRootObject)
                    {
                        jsonText = JsonConvert.SerializeXmlNode(xdoc, Formatting.Indented, true);
                    }
                    else
                    {
                        jsonText = JsonConvert.SerializeXmlNode(xdoc, Formatting.Indented, false);
                    }

                    if (RemoveAtTheRateChar)
                    {
                        jsonText = Regex.Replace(jsonText, "(?<=\")(@)(?!.*\":\\s )", String.Empty,
                                                 RegexOptions.IgnoreCase);
                        Trace.WriteLine("JsonToXmlConverter Pipeline - Removed the '@' Character");
                    }
                    Trace.WriteLine("JsonToXmlConverter output: " + jsonText);

                    byte[] outBytes = Encoding.ASCII.GetBytes(jsonText);

                    var memStream = new MemoryStream();
                    memStream.Write(outBytes, 0, outBytes.Length);
                    memStream.Position = 0;

                    bodyPart.Data = memStream;
                    pContext.ResourceTracker.AddResource(memStream);
                }
            }
            Trace.WriteLine("JsonToXmlConverter Pipeline - Exited Execute()");
            return pInMsg;
        }

        #endregion
    }
}