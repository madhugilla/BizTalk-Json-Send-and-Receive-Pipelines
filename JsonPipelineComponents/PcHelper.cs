using System;
using Microsoft.BizTalk.Component.Interop;
using System.Xml.Serialization;
using System.IO;

namespace JsonPipelineComponents
{
    public class PcHelper
    {
        public static object ReadPropertyBag(IPropertyBag propertyBag, string propName)
        {
            object ptrVar = (object)null;
            try
            {
                propertyBag.Read(propName, out ptrVar, 0);
            }
            catch (ArgumentException ex)
            {
                return ptrVar;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while reading the propery bag", ex);
            }
            return ptrVar;
        }

        public static void WritePropertyBag(IPropertyBag propertyBag, string propName, object val)
        {
            try
            {
                propertyBag.Write(propName, ref val);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while writing to the propery bag", ex);
            }
        }

        //Creates an object from an XML string.
        public static object FromXml(Stream Xml, System.Type ObjType)
        {
            var ser = new XmlSerializer(ObjType);
            return ser.Deserialize(Xml);
        }
    }
}
