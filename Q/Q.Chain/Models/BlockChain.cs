using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Q.Data.Models;
using Q.Data.Models.Struct;
using Q.DB;

namespace Q.Chain.Models
{
    public static class BlockChain
    {
        public static Block Stage = null;

        public static Block LastBlock
        {
            get
            {
                return BlockRepository.GetLastBlock();
            }
        }

        public static void Commit()
        {
            BlockRepository.Add(Stage);
            Stage = null;
        }

        public static void Clear()
        {
            BlockRepository.Clear();
        }

        public static string ToString()
        {
            return LastBlock.ToString();
        }
    }
}
