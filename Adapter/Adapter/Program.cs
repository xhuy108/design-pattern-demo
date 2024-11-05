using System;
using System.Text.Json;
using System.Xml.Linq;

namespace AdapterPattern
{
    // The Client Interface that defines how clients should interact with the system
    public interface IDataProcessor
    {
        void ProcessData(string xmlData);
    }

    // The incompatible Service (3rd party) that we want to adapt
    // This service only works with JSON
    public class JsonProcessor
    {
        public void ProcessJsonData(string jsonData)
        {
            // Simulate processing JSON data
            Console.WriteLine("Processing JSON data:");
            Console.WriteLine(jsonData);
            Console.WriteLine("JSON processing completed\n");
        }
    }

    // The Adapter class that makes XML data work with JsonProcessor
    public class XmlToJsonAdapter : IDataProcessor
    {
        private readonly JsonProcessor _jsonProcessor;

        public XmlToJsonAdapter(JsonProcessor jsonProcessor)
        {
            _jsonProcessor = jsonProcessor;
        }

        public void ProcessData(string xmlData)
        {
            // Convert XML to JSON
            string jsonData = ConvertXmlToJson(xmlData);
            
            // Use the adapted service
            _jsonProcessor.ProcessJsonData(jsonData);
        }

        private string ConvertXmlToJson(string xmlData)
        {
            try
            {
                // Parse XML
                XDocument xmlDoc = XDocument.Parse(xmlData);

                // Create an anonymous object to match the XML structure
                var jsonObject = new
                {
                    root = new
                    {
                        xmlDoc.Root?.Name,
                        Value = xmlDoc.Root?.Value,
                        Elements = xmlDoc.Root?.Elements().Select(e => new
                        {
                            e.Name,
                            e.Value
                        })
                    }
                };

                // Convert to JSON
                return JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            }
            catch (Exception ex)
            {
                return $"{{\"error\": \"{ex.Message}\"}}";
            }
        }
    }

    // Client code that uses the adapter
    public class Client
    {
        private readonly IDataProcessor _dataProcessor;

        public Client(IDataProcessor dataProcessor)
        {
            _dataProcessor = dataProcessor;
        }

        public void ProcessXmlData(string xmlData)
        {
            _dataProcessor.ProcessData(xmlData);
        }
    }

    // Program to demonstrate the adapter pattern
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Adapter Pattern Demonstration\n");

            // Sample XML data
            string xmlData = @"
                <root>
                    <person>
                        <name>John Doe</name>
                        <age>30</age>
                    </person>
                </root>";

            // Create the original service
            JsonProcessor jsonProcessor = new JsonProcessor();

            // Create the adapter
            IDataProcessor adapter = new XmlToJsonAdapter(jsonProcessor);

            // Create the client
            Client client = new Client(adapter);

            // Process XML data through the adapter
            Console.WriteLine("Original XML Data:");
            Console.WriteLine(xmlData);
            Console.WriteLine("\nProcessing through adapter:");
            client.ProcessXmlData(xmlData);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
