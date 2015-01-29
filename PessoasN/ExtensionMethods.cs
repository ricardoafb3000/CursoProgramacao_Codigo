using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace PessoasN
{
    public static class ExtensionMethods
    {

        /// <summary>
        /// Get a string containing Xml from passed object
        /// </summary>
        /// <remarks>
        ///    This method uses DataContectSerializer to get the XML string. So the object to be serialized must have DataContract and DataMember Attributes implemented.
        /// 
        /// </remarks>
        /// <param name="obj">Object to serialize</param>
        /// <param name="FormatXml">If it is to format the xml response</param>
        /// <returns>Get a string containing Xml from passed object</returns>
        public static string GetXml(this object obj, bool FormatXml = false)
        {
            string strXml = "";

            // Assuming obj is an instance of an object
            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                serializer.WriteObject(memoryStream, obj);
                memoryStream.Position = 0;
                strXml = reader.ReadToEnd();
            }

            if (!FormatXml)
                return strXml;

            /*
             * Format the xml answer
             */
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(strXml);

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.IndentChars = "  ";
            settings.NewLineChars = "\r\n";
            settings.NewLineHandling = NewLineHandling.Replace;
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }

            return sb.ToString();

        }

        /// <summary>
        /// Get a object from a string in Xml Format
        /// </summary>
        /// <remarks>
        ///    This method uses DataContectSerializer to get the XML string. So the object to be deserialized must have DataContract and DataMember Attributes implemented.
        /// 
        /// </remarks>
        /// <returns>Object from a string containing Xml</returns>
        public static T GetObjectXml<T>(this string obj)
        {
            if (string.IsNullOrEmpty(obj))
                return default(T);

            using (Stream stream = new MemoryStream())
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(obj);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                DataContractSerializer deserializer = new DataContractSerializer(typeof(T));

                return (T)deserializer.ReadObject(stream);

            }

        }

    }
}
