using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Q.Models
{
    public static class BlockChain
    {
        public static Block Stage = null;
        public static List<Block> Blocks = new List<Block>();
        public static Dictionary<string, User> Users = new Dictionary<string, User>();

        public static Block LastBlock {
            get {
                return Blocks[Blocks.Count - 1];
            }
        }

        public static void Commit(){
            //Store users and unspent transactions in memory
            foreach (BlockDataBase data in Stage.Data)
            {
                if (data is BlockDataRegistration)
                {
                    BlockDataRegistration registrationData = (BlockDataRegistration)data;
                    Users.Add(registrationData.Alias, new User()
                    {
                        Alias = registrationData.Alias,
                        PublicKey = Convert.FromBase64String(registrationData.PublicKey)
                    });
                }
            }
            Blocks.Add(Stage);
            Stage = null;
        }

        public static string ToString()
        {
            string output = "";
            foreach (Block block in Blocks)
            {
                output += $"\n{block},";
            }
            return $"[ {output}\n ]";
        }
    }
}
