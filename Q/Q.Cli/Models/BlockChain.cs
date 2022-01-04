using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Q.Data.Models;
using Q.Data.Models.Struct;
using Q.DB;

namespace Q.Cli.Models
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
            //Store users and unspent transactions in memory
            foreach (BlockDataBase data in Stage.Data)
            {
                if (data is BlockDataRegistration)
                {
                    BlockDataRegistration registrationData = (BlockDataRegistration)data;
                    UserRepository.Add(new User(registrationData), Stage);
                }
            }
            BlockRepository.Add(Stage);
            Stage = null;
        }

        public static void Clear()
        {
            BlockRepository.Clear();
            UserRepository.Clear();
        }

        public static string ToString()
        {
            return LastBlock.ToString();
        }
    }
}
