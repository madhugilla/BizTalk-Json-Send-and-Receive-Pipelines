namespace JsonSendAndReceivePipelines
{
    using System;
    using System.Collections.Generic;
    using Microsoft.BizTalk.PipelineOM;
    using Microsoft.BizTalk.Component;
    using Microsoft.BizTalk.Component.Interop;
    
    
    public sealed class XmlToSendPipeline : Microsoft.BizTalk.PipelineOM.SendPipeline
    {
        
        private const string _strPipeline = "<?xml version=\"1.0\" encoding=\"utf-16\"?><Document xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instanc"+
"e\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" MajorVersion=\"1\" MinorVersion=\"0\">  <Description /> "+
" <CategoryId>8c6b051c-0ff5-4fc2-9ae5-5016cb726282</CategoryId>  <FriendlyName>Transmit</FriendlyName"+
">  <Stages>    <Stage>      <PolicyFileStage _locAttrData=\"Name\" _locID=\"1\" Name=\"Pre-Assemble\" minO"+
"ccurs=\"0\" maxOccurs=\"-1\" execMethod=\"All\" stageId=\"9d0e4101-4cce-4536-83fa-4a5040674ad6\" />      <Co"+
"mponents />    </Stage>    <Stage>      <PolicyFileStage _locAttrData=\"Name\" _locID=\"2\" Name=\"Assemb"+
"le\" minOccurs=\"0\" maxOccurs=\"1\" execMethod=\"All\" stageId=\"9d0e4107-4cce-4536-83fa-4a5040674ad6\" />  "+
"    <Components />    </Stage>    <Stage>      <PolicyFileStage _locAttrData=\"Name\" _locID=\"3\" Name="+
"\"Encode\" minOccurs=\"0\" maxOccurs=\"-1\" execMethod=\"All\" stageId=\"9d0e4108-4cce-4536-83fa-4a5040674ad6"+
"\" />      <Components>        <Component>          <Name>Microsoft.Practices.ESB.Namespace.PipelineC"+
"omponents.RemoveNamespace,Microsoft.Practices.ESB.Namespace.PipelineComponents, Version=2.1.0.0, Cul"+
"ture=neutral, PublicKeyToken=31bf3856ad364e35</Name>          <ComponentName>ESB Remove Namespace</C"+
"omponentName>          <Description>Removes all namespaces from the root node.</Description>        "+
"  <Version>2.1</Version>          <Properties>            <Property Name=\"Encoding\">              <V"+
"alue xsi:type=\"xsd:string\">UTF8</Value>            </Property>            <Property Name=\"RemoveBOM\""+
">              <Value xsi:type=\"xsd:boolean\">false</Value>            </Property>          </Propert"+
"ies>          <CachedDisplayName>ESB Remove Namespace</CachedDisplayName>          <CachedIsManaged>"+
"true</CachedIsManaged>        </Component>        <Component>          <Name>JsonPipelineComponents."+
"DotNetTypesToJsonConverter,JsonPipelineComponents, Version=1.0.0.0, Culture=neutral, PublicKeyToken="+
"3573ec9fb1123352</Name>          <ComponentName>DotNetTypesToJsonConverter</ComponentName>          "+
"<Description>Pipeline component used to convert DotNetTypesToJsonConverter to Json string</Descripti"+
"on>          <Version>1.0.0.0</Version>          <Properties>            <Property Name=\"TypeName\" /"+
">          </Properties>          <CachedDisplayName>DotNetTypesToJsonConverter</CachedDisplayName> "+
"         <CachedIsManaged>true</CachedIsManaged>        </Component>      </Components>    </Stage> "+
" </Stages></Document>";
        
        private const string _versionDependentGuid = "3804e8fe-238a-46bb-bcd6-57491f11f61b";
        
        public XmlToSendPipeline()
        {
            Microsoft.BizTalk.PipelineOM.Stage stage = this.AddStage(new System.Guid("9d0e4108-4cce-4536-83fa-4a5040674ad6"), Microsoft.BizTalk.PipelineOM.ExecutionMode.all);
            IBaseComponent comp0 = Microsoft.BizTalk.PipelineOM.PipelineManager.CreateComponent("Microsoft.Practices.ESB.Namespace.PipelineComponents.RemoveNamespace,Microsoft.Practices.ESB.Namespace.PipelineComponents, Version=2.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");;
            if (comp0 is IPersistPropertyBag)
            {
                string comp0XmlProperties = "<?xml version=\"1.0\" encoding=\"utf-16\"?><PropertyBag xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-inst"+
"ance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Properties>    <Property Name=\"Encoding\">     "+
" <Value xsi:type=\"xsd:string\">UTF8</Value>    </Property>    <Property Name=\"RemoveBOM\">      <Value"+
" xsi:type=\"xsd:boolean\">false</Value>    </Property>  </Properties></PropertyBag>";
                PropertyBag pb = PropertyBag.DeserializeFromXml(comp0XmlProperties);;
                ((IPersistPropertyBag)(comp0)).Load(pb, 0);
            }
            this.AddComponent(stage, comp0);
            IBaseComponent comp1 = Microsoft.BizTalk.PipelineOM.PipelineManager.CreateComponent("JsonPipelineComponents.DotNetTypesToJsonConverter,JsonPipelineComponents, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3573ec9fb1123352");;
            if (comp1 is IPersistPropertyBag)
            {
                string comp1XmlProperties = "<?xml version=\"1.0\" encoding=\"utf-16\"?><PropertyBag xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-inst"+
"ance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Properties>    <Property Name=\"TypeName\" />  <"+
"/Properties></PropertyBag>";
                PropertyBag pb = PropertyBag.DeserializeFromXml(comp1XmlProperties);;
                ((IPersistPropertyBag)(comp1)).Load(pb, 0);
            }
            this.AddComponent(stage, comp1);
        }
        
        public override string XmlContent
        {
            get
            {
                return _strPipeline;
            }
        }
        
        public override System.Guid VersionDependentGuid
        {
            get
            {
                return new System.Guid(_versionDependentGuid);
            }
        }
    }
}
