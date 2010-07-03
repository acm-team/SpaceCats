using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ConsoleApplication1
{
    [Serializable]
    public struct Test
    {
        public int width,height;
        public bool fullScreen;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Test test = new Test();
            test.width = 640;
            test.height = 480;
            test.fullScreen = true;

            XmlSerializer ser = new XmlSerializer(typeof(Test));
            TextWriter tw = new StreamWriter("test.xml");
            ser.Serialize(tw, test);
            tw.Close();

            //System.Xml.XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration  CreateNode("","width","89"));
            //xmlDoc.Save("test.xml");

        }
    }
}
