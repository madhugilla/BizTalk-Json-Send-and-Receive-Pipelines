using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using Newtonsoft.Json;

namespace JsonPipelineComponents
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    [Guid("EB0241FF-B393-4D1A-B494-FC2D06169979")]
    public class DotNetTypesToJsonConverter
        : IBaseComponent, IComponentUI, IComponent, IPersistPropertyBag
    {
        public string TypeName { get; set; }

        #region IBaseComponent

        string IBaseComponent.Description
        {
            get { return "Pipeline component used to convert DotNetTypesToJsonConverter to Json string"; }
        }

        string IBaseComponent.Name
        {
            get { return "DotNetTypesToJsonConverter"; }
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
            classID = new Guid("5DF3163D-68E4-4CED-8271-79CE17324905");
        }

        void IPersistPropertyBag.InitNew()
        {
        }

        void IPersistPropertyBag.Load(IPropertyBag propertyBag, int errorLog)
        {
            object obj1 = PcHelper.ReadPropertyBag(propertyBag, "TypeName");
            if (obj1 != null)
                TypeName = (string) obj1;
        }

        void IPersistPropertyBag.Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            PcHelper.WritePropertyBag(propertyBag, "TypeName", TypeName);
        }

        #endregion

        #region IComponent

        IBaseMessage IComponent.Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            Trace.WriteLine("DotNetTypesToJsonConverter Pipeline - Entered Execute()");
            Trace.WriteLine("DotNetTypesToJsonConverter Pipeline - TypeName is set to: " + TypeName);

            IBaseMessagePart bodyPart = pInMsg.BodyPart;
            if (bodyPart != null)
            {
                Stream originalStream = bodyPart.GetOriginalDataStream();
                if (originalStream != null)
                {
                    Type myClassType = Type.GetType(TypeName);

                    object reqObj = PcHelper.FromXml(originalStream, myClassType);

                    string jsonText = JsonConvert.SerializeObject(reqObj, myClassType, Formatting.None,
                                                                  new JsonSerializerSettings());
                    Trace.WriteLine("DotNetTypesToJsonConverter output: " + jsonText);

                    byte[] outBytes = Encoding.ASCII.GetBytes(jsonText);

                    var memStream = new MemoryStream();
                    memStream.Write(outBytes, 0, outBytes.Length);
                    memStream.Position = 0;

                    bodyPart.Data = memStream;
                    pContext.ResourceTracker.AddResource(memStream);
                }
            }
            Trace.WriteLine("DotNetTypesToJsonConverter Pipeline - Exited Execute()");
            return pInMsg;
        }

        #endregion
    }
}