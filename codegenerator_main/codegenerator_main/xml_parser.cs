using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace codegenerator_main
{

    public class Block
    {
        public string BlockType { get; set; }
        public string Name { get; set; }
        public string SID { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
    }

    public class Connection
    {
        public string Src { get; set; }
        public string Dst { get; set; }
    }

    internal class xml_parser
    {
        public List<Block> Blocks { get; set; } = new List<Block>();
        public List<Connection> Connections { get; set; } = new List<Connection>();

        public void Parse(string xmlFilePath)
        {
            XDocument doc = XDocument.Load(xmlFilePath);

            // Парсинг блоков
            foreach (var blockElement in doc.Descendants("Block"))
            {
                var block = new Block
                {
                    BlockType = blockElement.Attribute("BlockType").Value,
                    Name = blockElement.Attribute("Name").Value,
                    SID = blockElement.Attribute("SID").Value
                };

                foreach (var param in blockElement.Elements("P"))
                {
                    block.Parameters[param.Attribute("Name").Value] = param.Value;
                }

                Blocks.Add(block);
            }

            // Парсинг соединений
            foreach (var lineElement in doc.Descendants("Line"))
            {
                var connection = new Connection
                {
                    Src = lineElement.Element("P").Value.Split('#')[0], // Получаем только ID блока
                    Dst = lineElement.Element("Dst").Value.Split('#')[0]
                };

                Connections.Add(connection);
            }
        }

        private string GenerateStructDeclaration(List<Block> blocks)
        {
            string code = "static struct\n{\n";
            foreach (var block in blocks)
            {
                code += $"    double {block.Name};\n";
            }
            code += "} nwocg;\n\n";
            return code;
        }
    }

   
}
