using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Q.Cli.Common;
using Q.Cli.Models;
using Q.Data.Common;
using Q.Data.Models;
using Q.Data.Models.Struct;

using Newtonsoft.Json;

namespace Q.Cli.Controllers
{
    public class CommandController
    {
        public enum COMMANDS
        {
            NEW_CHAIN,
            VIEW_CHAIN,
            REGISTER,
            POST_TRANSACTION,
            POST_REFERENCE,
            MINE
        }

        static CommandController() {
            Block lastBlock = BlockChain.LastBlock;
            if (lastBlock != null)
            {
                //Add new
                Block next = new Block()
                {
                    //TODO: Fix bug with previous block hash not lineing up
                    PreviousBlockHash = lastBlock.Hash,
                    Height = lastBlock.Height + 1
                    //TODO: Adjust difficulty
                };
                BlockChain.Stage = next;
            }
        }

        /*public static bool Execute(COMMANDS command)
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

                case COMMANDS.POST_TRANSACTION:
                    return DoTransaction(data);

                case COMMANDS.POST_REFERENCE:
                    return DoReference(data);

                case COMMANDS.MINE:
                    return DoMine(data);
            }

            return false;
        }*/

        public static bool NewChain()
        {
            BlockChain.Clear();

            //Genesis block
            Block genesis = new Block()
            {
                PreviousBlockHash = Crypto.ComputeHash("11/17/2021 - AJ Savino - Freedom of Speech"),
                Height = 0
            };
            BlockChain.Stage = genesis;

            Logger.Info("Created new BlockChain");

            return false;
        }

        public static bool Register(BlockDataRegistration registrationData)
        {
            Logger.Info($"Added data: {registrationData}");

            bool isValid = Crypto.Verify(Convert.FromBase64String(registrationData.PublicKey), registrationData.Hash, registrationData.Signature);
            if (!isValid)
            {
                throw new Exception("Invalid registration signature");
            }
            BlockChain.Stage.AddData(registrationData);

            return false;
        }

        public static bool Transaction(BlockDataTransaction transactionData)
        {
            //TODO

            return false;
        }

        public static bool Reference(BlockDataReference referenceData)
        {
            return false;
        }

        public static bool Mine(string seed)
        {
            Logger.Info($"Mining...");

            Block stage = BlockChain.Stage;
            string desired = Enumerable.Repeat("0", stage.Difficulty).Aggregate(string.Empty, (x, y) => x + y);
            string current = seed;
            string nonce;
            do
            {
                nonce = Crypto.ComputeHash(current);
                string hash = stage.ComputeHash(nonce);
                if (hash.Substring(0, stage.Difficulty) == desired)
                {
                    //Found block!
                    break;
                }
                current = nonce;
            } while (true);
            stage.Nonce = nonce;
            BlockChain.Commit();

            Logger.Info($"Mined block!");

            //Add new
            Block next = new Block()
            {
                PreviousBlockHash = stage.Hash,
                Height = stage.Height + 1
                //TODO: Adjust difficulty
            };
            BlockChain.Stage = next;

            return false;
        }
    }
}
