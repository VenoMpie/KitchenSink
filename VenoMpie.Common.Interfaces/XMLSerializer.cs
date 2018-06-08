using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace VenoMpie.Common.Interfaces
{
    public class XMLSerializer
    {
        public T ReadXML<T>(string path) where T : class
        {
            T retValue;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            XmlReader xr = XmlReader.Create(path);
            retValue = (T)xmlSerializer.Deserialize(xr);
            xr.Close();
            return retValue;
        }
        public void WriteXML<T>(string path, T classToWrite) where T : class
        {
            // Remove the XSI/XSD namespaces from serialization, we don't want namespaces for standard class serialization
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            XmlWriter xw = XmlWriter.Create(path, new XmlWriterSettings() { Indent = true, NewLineOnAttributes = true });
            xmlSerializer.Serialize(xw, classToWrite, ns);
            xw.Flush();
            xw.Close();
        }
    }
}
