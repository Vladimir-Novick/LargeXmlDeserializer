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

namespace SGCombo.Utils
{
    public class LargeXmlDeserializer<T>
    {

        public void Deserialize(String pairName, List<T> ObjectList, string InputFile)
        {

            Type type = typeof(T);
            String regEx = $"(<{pairName}[ ]*>.+?</{pairName}[ ]*>)";

            using (Stream stream = File.Open(InputFile, FileMode.Open,FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    StringBuilder bilder = new StringBuilder();

                    Regex expression = new Regex(regEx, RegexOptions.CultureInvariant & RegexOptions.Singleline & RegexOptions.Multiline & RegexOptions.IgnoreCase);

                    string lineR;
                    while ((lineR = sr.ReadLine()) != null)
                    {
						string charToString = lineR + " ";

                        bilder.Append(charToString);
                        String line = bilder.ToString();

                        MatchCollection matchCollection = expression.Matches(line);
                        if (matchCollection.Count > 0)
                        {
                            for (int i = 0; i < matchCollection.Count; i++)
                            {
                                var peir = matchCollection[i];
                                String peirValue = peir.Value;
                                XmlSerializer ser = new XmlSerializer(type);
                                using (StringReader sr2 = new StringReader(peirValue))
                                {
                                    T o = (T)ser.Deserialize(sr2);
                                    ObjectList.Add(o);
                                }
                            }
                            var lastElement = matchCollection[matchCollection.Count - 1];
                            int istartD = lastElement.Index + lastElement.Length - 1;
                            String delta = line.Substring(istartD);
                            bilder.Clear();
                            bilder.Append(delta);
                        }
                    }

                }
            }
        }
    }
}

