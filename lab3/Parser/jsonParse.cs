
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
    public class jsonParse : IParser
    {
        private Type classs;
        private string path;

        public jsonParser(string p, Type tipe)
        {
            path = p;
            classs = tipe;
        }

        public T GetOptions<T>()
        {
            object result = Activator.CreateInstance(typeof(T));

            try
            {
                PropertyInfo[] properties = typeof(T).GetProperties();


                foreach (PropertyInfo pi in properties)
                {


                    DeserializeRecursive(pi, result, FindElement<T>());
                }
            }
            catch (Exception)
            {
                result = null;

            }



            return (T)result;
        }

        private JsonElement FindElement<T>()
        {
            string json;
            using (StreamReader file = File.OpenText(path))
            {
                json = file.ReadToEnd();
            }

            JsonDocument doc = JsonDocument.Parse(json);

            if (typeof(T) == classs)
            {
                return doc.RootElement;
            }

            PropertyInfo[] properties = classs.GetProperties();

            JsonElement result = default;

            foreach (PropertyInfo pi in properties)
            {
                FindElementnotRecursive<T>(pi, doc.RootElement, ref result);
            }

            JsonElement d = default;
            if (result.Equals(d))
            {
                throw new ArgumentNullException($"{nameof(result)} is null");
            }

            return result;
        }

        private void FindElementnotRecursive<T>(PropertyInfo pi, JsonElement parentNode, ref JsonElement result)
        {
            foreach (var node in parentNode.EnumerateObject())
            {
                JsonElement doc = default;
                if (node.Name == pi.Name && pi.PropertyType == typeof(T) && result.Equals(doc))
                {

                    result = node.Value;


                }
            }
        }

        private void DeserializeRecursive(PropertyInfo pi, object parent, JsonElement parentNode)
        {
            foreach (JsonProperty node in parentNode.EnumerateObject())
            {
                if (node.Name == pi.Name)
                {
                    if (pi.PropertyType == typeof(string))
                    {


                        pi.SetValue(parent, Convert.ChangeType(node.Value.ToString(), pi.PropertyType));
                    }
                    else if (pi.PropertyType.IsPrimitive)
                    {
                        
                        pi.SetValue(parent, Convert.ChangeType(node.Value.ToString(), pi.PropertyType));
                    }
                    else if (pi.PropertyType.IsEnum)
                    {

                        pi.SetValue(parent, Enum.Parse(pi.PropertyType, node.ToString()));
                    }
                    else
                    {
                        Type subType = pi.PropertyType;
                        object subObj = Activator.CreateInstance(subType);

                        pi.SetValue(parent, subObj);

                        PropertyInfo[] props = subType.GetProperties();
                        foreach (PropertyInfo ppi in props)
                        {
                            DeserializeRecursive(ppi, subObj, node.Value);
                        }
                    }
                }
            }
        }

    }
}
