using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Q.Common;
using Q.Models;

namespace Q.Controllers
{
    public class CommandController
    {
        public enum COMMANDS {
            NEW_CHAIN,
            VIEW_CHAIN,
            REGISTER,
            POST_TRANSACTION,
            POST_REFERENCE,
            MINE
        }

        static CommandController(){}

        public static bool Execute(COMMANDS command)
        {
            return Execute(command, null);
        }
        public static bool Execute(COMMANDS command, string jsonData)
        {
            dynamic data = null;
            try
            {
                if (jsonData != null)
                {
                    data = JsonConvert.DeserializeObject(jsonData);
                }
            } catch (Exception ex)
            {
                throw new Exception("Invalid JSON");
            }

            switch (command)
            {
                case COMMANDS.NEW_CHAIN:
                    return DoNewChain();

                case COMMANDS.VIEW_CHAIN:
                    return DoViewChain();

                case COMMANDS.REGISTER:
                    return DoRegister(data);

                case COMMANDS.POST_TRANSACTION:
                    return DoTransaction(data);

                case COMMANDS.POST_REFERENCE:
                    return DoReference(data);

                case COMMANDS.MINE:
                    return DoMine(data);
            }

            return false;
        }

        private static bool DoNewChain()
        {
            BlockChain.Blocks = new List<Block>();

            //Genesis block
            Block genesis = new Block(){
                Height = 0
            };
            BlockChain.Add(genesis);

            Logger.Info("Created new BlockChain");

            return false;
        }

        private static bool DoViewChain()
        {
            Logger.Info(BlockChain.ToString());

            return false;
        }

        private static bool DoRegister(dynamic data)
        {
            BlockDataBase registrationData = new BlockDataRegistration()
            {
                Alias = data.alias,
                PublicKey = data.publicKey
            };

            Logger.Info($"Added data: {registrationData}");

            BlockChain.LastBlock.AddData(registrationData);

            return false;
        }

        private static bool DoTransaction(dynamic data)
        {
            return false;
        }

        private static bool DoReference(dynamic data)
        {
            return false;
        }

        private static bool DoMine(dynamic data)
        {
            Logger.Info($"Mining...");

            Block lastBlock = BlockChain.LastBlock;
            string desired = Enumerable.Repeat("0", lastBlock.Difficulty).Aggregate(string.Empty, (x, y) => x + y);
            string current = data.seed;
            string nonce;
            do
            {
                nonce = Utils.ComputeHash(current);
                string hash = lastBlock.ComputeHash(nonce);
                if (hash.Substring(0, lastBlock.Difficulty) == desired)
                {
                    //Found block!
                    break;
                }
                current = nonce;
            } while (true);
            lastBlock.Nonce = nonce;

            Logger.Info($"Mined block!");

            //Add new
            Block next = new Block()
            {
                Height = lastBlock.Height + 1
                //TODO: Adjust difficulty
            };
            BlockChain.Add(next);

            return false;
        }
    }
}
