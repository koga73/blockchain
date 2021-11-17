using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Q.Models
{
    public static class BlockChain
    {
        public static List<Block> Blocks = new List<Block>();

        public static Block LastBlock {
            get {
                return Blocks[Blocks.Count - 1];
            }
        }

        public static void Add(Block block){
            Blocks.Add(block);
        }

        public static string ToString()
        {
            string output = "";
            foreach (Block block in Blocks)
            {
                output += "\n" + block;
            }
            return $"[ {output}\n ]";
        }
    }
}
