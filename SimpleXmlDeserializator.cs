using System;
using System.IO;
using System.Xml.Serialization;

namespace XmlUtils
{
    /// <summary>
    ///  Use:
    ///  
    ///       SimpleXmlDeserializator sertalizator = new SimpleXmlDeserializator();
    ///
    ///       rootAreas = sertalizator.DeserializeFile<RootAreas>(CrawlerConfig.GetConfigData.TargetFolder + "AREAS.xml");
    /// 
    /// </summary>
    public class SimpleXmlDeserializator
    {
        public T Deserialize<T>(String strXMLAreas)
        {
            T ret;

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader rdr = new StringReader(strXMLAreas))
            {
                ret = (T)serializer.Deserialize(rdr);
            }
            serializer = null;
            return ret;
        }


        public T DeserializeFile<T>(String strXMLFileName)
        {
            string strXMLAreas = File.ReadAllText(strXMLFileName, System.Text.Encoding.UTF8);
            T ret =  Deserialize<T>(strXMLAreas);
            strXMLAreas = null;
            return ret;
        }


    }
}
