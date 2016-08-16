using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace TryParseXML
{
    class Program
    {
        static void Main(string[] args)
        {
            var doc = XDocument.Load("F:\\out.xml");
            XmlSchema schema = new XmlSchema();
            foreach (XElement xe in doc.Descendants("item"))
            {
                Console.WriteLine(xe.Name);
                Debug.WriteLine(xe.Name + " - " + xe.Attribute("name") + " - Picture: " + xe.Attribute("picture") +
                                " - Level " + xe.Attribute("level"));
                //There is no picture element for RECORD or PARENT lines.
                if (xe.Attributes().Any(n => n.Name == "picture"))
                {
                    var dataType = CobolUtilities.ConvertPicture(xe.Attribute("picture").Value);
                    Debug.WriteLine("Will be type: " + dataType);
                    // <xs:element name="cat" type="string"/>
                    XmlSchemaElement element = new XmlSchemaElement();
                    schema.Items.Add(element);
                    element.Name = xe.Attribute("name").Value;
                    element.SchemaTypeName = CobolUtilities.ConvertPicture(xe.Attribute("picture").Value);
                }
                else
                {
                    // <xs:element name="pets">
                    XmlSchemaElement element = new XmlSchemaElement();
                    schema.Items.Add(element);
                    element.Name = xe.Attribute("name").Value;

                    // <xs:complexType>
                    XmlSchemaComplexType complexType = new XmlSchemaComplexType();
                    element.SchemaType = complexType;

                    // <xs:choice minOccurs="0" maxOccurs="unbounded">
                    XmlSchemaSequence seq = new XmlSchemaSequence();
                    complexType.Particle = seq;
                    seq.MinOccurs = 0;
                    seq.MaxOccursString = "unbounded";
                    foreach (
                        var descs in xe.Descendants().Where(n => n.Attribute("name") != null && n.Name != "condition"))
                    {
                        XmlSchemaElement Ref = new XmlSchemaElement();
                        seq.Items.Add(Ref);
                        Ref.RefName = new XmlQualifiedName(descs.Attribute("name").Value);
                    }
                }
            }

            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add(schema);
            schemaSet.Compile();
            XmlSchema compiledSchema = null;

            foreach (XmlSchema schema1 in schemaSet.Schemas())
            {
                compiledSchema = schema1;
            }

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
            nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            var writer = new System.IO.StreamWriter(@"e:\output.xsd");
            compiledSchema.Write(writer, nsmgr);
        }
    }
}