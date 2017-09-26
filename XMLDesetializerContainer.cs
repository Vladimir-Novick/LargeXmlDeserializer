using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

////////////////////////////////////////////////////////////////////////////
//	Copyright 2017 : Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/  
//
//         https://github.com/Vladimir-Novick/LargeXmlDeserializer
//
//    NO WARRANTIES ARE EXTENDED. USE AT YOUR OWN RISK. 
//
// To contact the author with suggestions or comments, use  :vlad.novick@gmail.com
//
////////////////////////////////////////////////////////////////////////////

namespace MemoryDB.StaticData.Utils
{

    /*
     * 
     *  Example :
     *  
   
     Multu-Part, Multi-Class deserialization:
     
            var listObjetct = new List<XMLDataObject>(); // Create Objects Containers 

            var package = new XMLDataObjectItem<Package>("Package"); // Create Store item. Section Package , Class Package
            listObjetct.Add(package);  // add element to conteiner

            var itineraryDetail = new XMLDataObjectItem<ItineraryDetail>("ItineraryDetail"); // Create additional item
            listObjetct.Add(itineraryDetail);

             // using custom regular expression:
			 // Create store item with specific regular expression
             
             var bc = new XMLDataObjectItem<BClient>("BClient","(?<data><(?<pair>BClient)[ ]*>.+?</BClient[ ]*></BClient[ ]*>)");
             listObjetct.Add(bc);

             XMLDesetializerContainer.Deserialize(listObjetct, fileName);  // Parse  
     * 
     * 
     */

    public interface XMLDataObject
    {
        string GetPeirName();
        
        void Add(String pearValue);

        string GetRegExExpression();
        
        Object GetDataObject();

    }

    public class XMLDataObjectItem<T> : XMLDataObject
    {
        public XMLDataObjectItem(string pairName)
        {
            PairName = pairName;
        }
        /// <summary>
        ///   Create data object with custom regular expression
            /// </summary>
        /// <param name="pairName"></param>
        /// <param name="RegExpression"></param>
        public XMLDataObjectItem(string pairName, string RegExpression)
        {
            PairName = pairName;
            customRegEx = RegExpression;
        }

        private String customRegEx = null;

        public List<T> DataObject = new List<T>();
        
        public Object GetDataObject(){
           return DataObject;
        }
        
        
        private String PairName { get; set; }
        private XmlSerializer ser = new XmlSerializer(typeof(T));

        public void Add(String peirValue)
        {
            using (StringReader sr2 = new StringReader(peirValue))
            {
                try
                {
                    T o = (T)ser.Deserialize(sr2);
                    DataObject.Add(o);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deserialization: {ex.Message} -> {peirValue}");
                }
            }
        }

        public string GetPeirName()
        {
            return PairName;
        }

        public string GetRegExExpression()
        {
            if (customRegEx == null)
            {
                String regEx = $"(?<data><(?<pair>{PairName})[ ]*>.+?</{PairName}[ ]*>)";
                return regEx;
            }
            return customRegEx;
        }
    }

    public class XMLDesetializerContainer
    {
        public static void Deserialize(List<XMLDataObject> dataObjects, string InputFile)
        {

            StringBuilder regExBuilder = new StringBuilder();
            Dictionary<String, XMLDataObject> outputData = new Dictionary<string, XMLDataObject>();

            bool first = true;
            foreach (XMLDataObject item in dataObjects)
            {
                String pairName = item.GetPeirName();

                outputData.Add(pairName, item);

                String regEx = item.GetRegExExpression();

                if (!first)
                {
                    regExBuilder.Append("|");

                }
                regExBuilder.Append(regEx);
                first = false;
            }

            String regExpression = regExBuilder.ToString();

            using (Stream stream = File.Open(InputFile, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    StringBuilder bilder = new StringBuilder();

                    Regex expression = new Regex(regExpression);

                    char[] c = null;
                    while (sr.Peek() >= 0)
                    {
                        c = new char[4028];
                        int count = sr.ReadBlock(c, 0, c.Length);
                        string charToString = new string(c, 0, count);

                        bilder.Append(charToString);
                        String line = bilder.ToString();

                        MatchCollection matchCollection = expression.Matches(line);

                        if (matchCollection.Count > 0)
                        {
                            int LastIndex = 0;

                            for (int i = 0; i < matchCollection.Count; i++)
                            {
                                Match pairItem = matchCollection[i];
                                string strValue = pairItem.Groups["data"].Value;
                                string key = pairItem.Groups["pair"].Value;

                                XMLDataObject outData = null;
                                outputData.TryGetValue(key, out outData);

                                outData.Add(strValue);

                                var lastElement = matchCollection[matchCollection.Count - 1];
                                int istartD = lastElement.Index + lastElement.Length - 1;

                                if (LastIndex < istartD) { LastIndex = istartD; }

                            }

                            String delta = line.Substring(LastIndex);
                            bilder.Clear();
                            bilder.Append(delta);
                        }
                    }

                }
            }
        }
    }
}

