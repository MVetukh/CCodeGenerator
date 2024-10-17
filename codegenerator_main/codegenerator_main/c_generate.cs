using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codegenerator_main
{
    internal class c_generate
    {
        public string GenerateCCode(List<Block> blocks)
        {
            string code = "#include \"nwocg_run.h\"\n";
            code += "#include <math.h>\n\n";
            code += GenerateStructDeclaration(blocks);
            code += GenerateInitFunction(blocks);
            code += GenerateStepFunction(blocks);
            code += GenerateExternalPorts(blocks);
            return code;
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

        private string GenerateInitFunction(List<Block> blocks)
        {
            string code = "void nwocg_generated_init()\n{\n";
            foreach (var block in blocks)
            {
                if (block.BlockType == "UnitDelay")
                {
                    code += $"    nwocg.{block.Name} = 0;\n";
                }
            }
            code += "}\n\n";
            return code;
        }

        private string GenerateStepFunction(List<Block> blocks)
        {
            string code = "void nwocg_generated_step()\n{\n";
            foreach (var block in blocks)
            {
                switch (block.BlockType)
                {
                    case "Sum":
                        string input1 = "nwocg." + GetInputVariable(block, 1);
                        string input2 = "nwocg." + GetInputVariable(block, 2);
                        code += $"    nwocg.{block.Name} = {input1} + {input2};\n";
                        break;

                    case "Gain":
                        string input = "nwocg." + GetInputVariable(block, 1);
                        string gain = block.Parameters["Gain"];
                        code += $"    nwocg.{block.Name} = {input} * {gain};\n";
                        break;

                        // другие типы блоков
                }
            }
            code += "}\n\n";
            return code;
        }

        private string GenerateExternalPorts(List<Block> blocks)
        {
            string code = "static const nwocg_ExtPort\n    ext_ports[] =\n{\n";
            foreach (var block in blocks)
            {
                if (block.BlockType == "Inport" || block.BlockType == "Outport")
                {
                    string direction = block.BlockType == "Inport" ? "1" : "0";
                    code += $"    {{ \"{block.Name}\", &nwocg.{block.Name}, {direction} }},\n";
                }
            }
            code += "    { 0, 0, 0 },\n};\n\n";
            code += "const nwocg_ExtPort * const\n    nwocg_generated_ext_ports = ext_ports;\n";
            code += "const size_t\n    nwocg_generated_ext_ports_size = sizeof(ext_ports);\n\n";
            return code;
        }

        private string GetInputVariable(Block block, int inputIndex)
        {
            // Функция для получения имени переменной на входе
            return "входная_переменная";
        }

    }
}
