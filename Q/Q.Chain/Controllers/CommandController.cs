using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

using Q.Chain.Models;
using Q.Data.Common;
using Q.Data.Models;
using Q.Data.Models.Struct;

using Newtonsoft.Json;

namespace Q.Chain.Controllers
{
    public class CommandController
    {
        public static int COINBASE_AMT = 256; //TODO: Make dynamic with halfing

        public static Thread? Miner = null;

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
            bool isValid = Crypto.Verify(Convert.FromBase64String(registrationData.PublicKey), registrationData.Hash, registrationData.Signature);
            if (!isValid)
            {
                throw new Exception("Invalid registration signature");
            }
            BlockChain.Stage.Data.Add(registrationData);

            Logger.Info($"Added data: {registrationData}");

            return false;
        }

        public static bool Transaction(BlockDataTransaction transactionData)
        {
            //TODO

            return false;
        }

        public static bool Message(BlockDataMessage messageData)
        {
            bool isValid = Crypto.Verify(Convert.FromBase64String(messageData.PublicKey), messageData.Hash, messageData.Signature);
            if (!isValid)
            {
                throw new Exception("Invalid message signature");
            }
            BlockChain.Stage.Data.Add(messageData);

            Logger.Info($"Added data: {messageData}");

            return false;
        }

        public static bool StartMining(string seed, BlockDataTransaction coinbaseTx)
        {
            //Verify coinbase transaction
            if (coinbaseTx.TxIn.Count > 0 || coinbaseTx.TxOut.Count > 1)
            {
                throw new Exception("Invalid coinbase transaction");
            }
            if (coinbaseTx.TxOut[0].Amount != COINBASE_AMT)
            {
                throw new Exception("Invalid coinbase amount");
            }
            bool isValid = Crypto.Verify(Convert.FromBase64String(coinbaseTx.TxOut[0].Address), coinbaseTx.Hash, coinbaseTx.Signature);
            if (!isValid)
            {
                throw new Exception("Invalid coinbase signature");
            }

            //Initialize mining
            Block stage = BlockChain.Stage;
            stage.Data.Insert(0, coinbaseTx);
            string desired = Enumerable.Repeat("0", stage.Difficulty).Aggregate(string.Empty, (x, y) => x + y);
            string current = seed;
            string nonce;

            //Start miner in new thread
            if (IsMining())
            {
                throw new Exception("Miner is already running");
            }
            Miner = new Thread(() => {
                //Begin!
                Logger.Info($"Mining...");
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

                //Complete!
                Logger.Info($"Mined block!");

                //Add new
                Block next = new Block()
                {
                    PreviousBlockHash = stage.Hash,
                    Height = stage.Height + 1
                    //TODO: Adjust difficulty
                };
                BlockChain.Stage = next;

                //Kill thread
                Miner = null;
            });
            Miner.Start();

            return false;
        }

        public static bool IsMining()
        {
            return Miner != null;
        }

        public static bool StopMining()
        {
            if (!IsMining())
            {
                throw new Exception("Miner is not running");
            }
            if (Miner != null)
            {
                Miner.Interrupt();
            }
            Miner = null;

            return false;
        }
    }
}
