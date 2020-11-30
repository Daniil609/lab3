
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
namespace lab3.Parser
{
    public class XmlParser : IParser
    {
        private string path;

        private Type ttype;

        public XmlParser(string path, Type ttype)
        {
            this.path = path;
            this.ttype = ttype;
        }

        public T GetOptions<T>()
        {
            object result = Activator.CreateInstance(typeof(T));


            try
            {
                PropertyInfo[] properties = typeof(T).GetProperties();

                foreach (PropertyInfo pi in properties)
                {
                    DeserializeRecursive(pi, result, FindNode<T>());
                }
            }
            catch (Exception)
            {

                result = null;
            }


            return (T)result;
        }

        private void DeserializeRecursive(PropertyInfo pi, object parent, XmlNode parentNode)
        {
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.Name == pi.Name)
                {
                    if (pi.PropertyType.IsPrimitive || pi.PropertyType == typeof(string))
                    {
                        pi.SetValue(parent, Convert.ChangeType(node.InnerText, pi.PropertyType));
                    }
                    else if (pi.PropertyType.IsEnum)
                    {
                        pi.SetValue(parent, Enum.Parse(pi.PropertyType, node.InnerText));
                    }
                    else
                    {
                        Type subType = pi.PropertyType;
                        object subObj = Activator.CreateInstance(subType);

                        pi.SetValue(parent, subObj);

                        PropertyInfo[] subPIs = subType.GetProperties();
                        foreach (PropertyInfo spi in subPIs)
                        {
                            DeserializeRecursive(spi, subObj, node);
                        }
                    }
                }
            }
        }

        private XmlNode FindNode<T>()
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(path);

            if (typeof(T) == ttype)
            {
                return doc.DocumentElement;
            }

            PropertyInfo[] properties = ttype.GetProperties();

            XmlNode result = null;

            foreach (PropertyInfo ppi in properties)
            {
                FindNodeRecursive<T>(ppi, doc.DocumentElement, ref result);
            }

            if (result is null)
            {
                throw new ArgumentNullException($"{nameof(result)} is null");
            }

            return result;
        }

        private void FindNodeRecursive<T>(PropertyInfo pi, XmlNode parentNode, ref XmlNode result)
        {
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.Name == pi.Name && pi.PropertyType == typeof(T) && result == null)
                {
                    result = node;

                    if (!pi.PropertyType.IsPrimitive && !(pi.PropertyType == typeof(string)))
                    {
                        Type subt = pi.PropertyType;

                        PropertyInfo[] props = subt.GetProperties();
                        foreach (PropertyInfo ppi in props)
                        {
                            FindNodeRecursive<T>(ppi, node, ref result);
                        }
                    }
                }
            }
        }
    }
}
