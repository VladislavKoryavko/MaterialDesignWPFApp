using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace DidaktikaApp.Helpers.Utils.Serelization
{
    public static class UtilSerialized
    {
        public static void Serializable<TObjectSerializable>(TObjectSerializable @object, string pathAndName)
        {
            using (FileStream fileStream = new FileStream(pathAndName, FileMode.Create))
            {
                XmlWriter writer = XmlWriter.Create(fileStream,
                    new XmlWriterSettings()
                    {
                        OmitXmlDeclaration = false,
                        Indent = true,
                        Encoding = Encoding.UTF8
                    });
                new XmlSerializer(typeof(TObjectSerializable)).Serialize(writer, @object);
            }
        }

        public static TObjectDeserialized Deserialized<TObjectDeserialized>(string pathAndName)
        {
            if (!File.Exists(pathAndName))
            {
                // MessageBox.Show("Неа " + pathAndName);
                return default(TObjectDeserialized);
            }

            using (FileStream fileStream = new FileStream(pathAndName, FileMode.Open))
            {
                TObjectDeserialized f = default(TObjectDeserialized);
                try
                {
                    f = (TObjectDeserialized)new XmlSerializer(typeof(TObjectDeserialized))
                        .Deserialize(fileStream);
                }
                catch (InvalidOperationException e)
                {
                    MessageBox.Show(e.Message +
                                    '\n'
                                    + e.InnerException
                    );
                }

                return f;
            }
        }

        public static TObjectDeserialized DeserializedUri<TObjectDeserialized>(string pathAndName)
        {
            using (Stream fileStream = Application.GetResourceStream(new Uri(pathAndName, UriKind.Relative))?.Stream)
            {
                return (TObjectDeserialized)new XmlSerializer(typeof(TObjectDeserialized))
                    .Deserialize(fileStream);
            }
        }
    }
}
