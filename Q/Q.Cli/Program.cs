using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.Dynamic;

using Newtonsoft.Json;

using Q.Chain.Controllers;
using Q.Chain.Models;
using Q.Data.Common;
using Q.Data.Models;
using Q.Data.Models.Struct;

namespace Q.Cli
{
    class Program
    {
        public static Dictionary<string, string> keys = new Dictionary<string, string>();
        public static KeyPair keyPair;

        static void Main(string[] args)
        {
            WriteInfo();

            //Create new chain if does not exist
            if (BlockChain.LastBlock == null)
            {
                CommandController.NewChain();
            }

            do
            {
                Console.WriteLine("");
                string input = Console.ReadLine();
                if (input == "exit")
                {
                    break;
                }
                else if (input == "clear")
                {
                    WriteInfo();
                }
                else
                {
                    try
                    {
                        bool executionResult = ParseInput(input);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"-- ERROR: {ex.Message}");
                    }
                }
            } while (true);
        }

        static void WriteInfo()
        {
            Console.Clear();
            Console.WriteLine("Q");
            Console.WriteLine("Commands:");
            Console.WriteLine("");
            Console.WriteLine("  generatekeys");
            Console.WriteLine("  savekeys {NAME}");
            Console.WriteLine("  loadkeys");
            Console.WriteLine("  setkeys {PRIVATEKEY} {PUBLICKEY}");
            Console.WriteLine("");
            Console.WriteLine("  newchain");
            Console.WriteLine("  viewchain");
            Console.WriteLine("  viewstage");
            Console.WriteLine("");
            Console.WriteLine("  register {ALIAS}");
            Console.WriteLine("  transaction {AMOUNT} {FROM} {TO}");
            Console.WriteLine("  message {MESSAGE}");
            Console.WriteLine("");
            Console.WriteLine("  mine {SEED}");
            Console.WriteLine("");
            Console.WriteLine("  clear");
            Console.WriteLine("  exit");
            Console.WriteLine("");
        }

        static bool ParseInput(string input)
        {
            Regex commandRegex = new Regex("^(generatekeys|savekeys|loadkeys|setkeys|newchain|viewchain|viewstage|register|transaction|message|mine)");

            Regex generatekeysRegex = new Regex("^(generatekeys)$");
            Regex savekeysRegex = new Regex("^(savekeys)\\s(.+)$");
            Regex loadkeysRegex = new Regex("^(loadkeys)$");
            Regex setkeysRegex = new Regex("^(setkeys)\\s(.+)\\s(.+)$");

            Regex newchainRegex = new Regex("^(newchain)$");
            Regex viewchainRegex = new Regex("^(viewchain)$");
            Regex viewstageRegex = new Regex("^(viewstage)$");

            Regex registerRegex = new Regex("^(register)\\s(.+?)$");
            Regex transactionRegex = new Regex("^(transaction)\\s([\\d.]+?)\\s(.+?)\\s(.+?)$");
            Regex messageRegex = new Regex("^(message)\\s(.+?)$");

            Regex mineRegex = new Regex("^(mine)\\s(.+)$");

            string path = Paths.KeysPath;

            string command = null;
            if (commandRegex.IsMatch(input))
            {
                command = commandRegex.Matches(input)[0].ToString();
            }
            switch (command)
            {
                case "generatekeys":
                    if (!generatekeysRegex.IsMatch(input))
                    {
                        throw new Exception("Invalid command syntax");
                    }
                    keyPair = Crypto.GenerateKeyPair();
                    Console.WriteLine($"- Generated key pair");

                    return false;

                case "savekeys":
                    if (!savekeysRegex.IsMatch(input))
                    {
                        throw new Exception("Invalid command syntax");
                    }
                    if (keyPair == null)
                    {
                        throw new Exception("No keys found, please generate keys first");
                    }
                    MatchCollection savekeysMatches = savekeysRegex.Matches(input);
                    string keyName = savekeysMatches[0].Groups[2].ToString();
                    dynamic keyObj = JsonConvert.DeserializeObject(keyPair.ToString());

                    File.WriteAllLines($"{path}/{keyName}.private.pem", new string[] { "-----BEGIN PRIVATE KEY-----", keyObj.privateKey, "-----END PRIVATE KEY-----" });
                    Console.WriteLine($"- Wrote {path}/{keyName}.private.pem");
                    File.WriteAllLines($"{path}/{keyName}.public.pem", new string[] { "-----BEGIN PUBLIC KEY-----", keyObj.publicKey, "-----END PUBLIC KEY-----" });
                    Console.WriteLine($"- Wrote {path}/{keyName}.public.pem");

                    return false;

                case "loadkeys":
                    if (!loadkeysRegex.IsMatch(input))
                    {
                        throw new Exception("Invalid command syntax");
                    }
                    Regex fileNameRegex = new Regex("^.+[\\/\\\\](.+\\.pem)$");
                    List<string> files = Directory.EnumerateFiles(path).ToList();
                    foreach (string file in files)
                    {
                        if (fileNameRegex.IsMatch(file))
                        {
                            MatchCollection fileNameMatches = fileNameRegex.Matches(file);
                            string fileName = fileNameMatches[0].Groups[1].ToString();

                            string text = File.ReadAllText(file);
                            text = Regex.Replace(text, "(--.+--)|[\\s]", "").Trim();
                            keys.Add(fileName, text);

                            Console.WriteLine($"- Loaded {fileName}");
                        }
                    }
                    return false;

                case "setkeys":
                    if (!setkeysRegex.IsMatch(input))
                    {
                        throw new Exception("Invalid command syntax");
                    }
                    MatchCollection setkeysMatches = setkeysRegex.Matches(input);
                    string privateKey = setkeysMatches[0].Groups[2].ToString();
                    string privateKeyVal = keys[privateKey] ?? privateKey;
                    string publicKey = setkeysMatches[0].Groups[3].ToString();
                    string publicKeyVal = keys[publicKey] ?? publicKey;
                    keyPair = KeyPair.Parse(privateKeyVal, publicKeyVal);
                    Console.WriteLine($"- Key pair set");
                    return false;

                case "newchain":
                    if (!newchainRegex.IsMatch(input))
                    {
                        throw new Exception("Invalid command syntax");
                    }
                    bool newchainResult = CommandController.NewChain();
                    Console.WriteLine($"- Created new blockchain");
                    return newchainResult;

                case "viewchain":
                    if (!viewchainRegex.IsMatch(input))
                    {
                        throw new Exception("Invalid command syntax");
                    }
                    Console.Write($"- Chain: {BlockChain.ToString()}");
                    return false;

                case "viewstage":
                    if (!viewstageRegex.IsMatch(input))
                    {
                        throw new Exception("Invalid command syntax");
                    }
                    Console.Write($"- Stage: {BlockChain.Stage}");
                    return false;

                case "register":
                    if (!registerRegex.IsMatch(input))
                    {
                        throw new Exception("Invalid command syntax");
                    }
                    MatchCollection registerMatches = registerRegex.Matches(input);
                    string alias = registerMatches[0].Groups[2].ToString().ToLower();

                    //Create data object
                    BlockDataRegistration registrationData = new BlockDataRegistration()
                    {
                        Alias = alias,
                        PublicKey = keyPair.PublicKeyString
                    };
                    registrationData.Signature = Crypto.Sign(keyPair.PrivateKey, registrationData.Hash);
                    bool registrationResult = CommandController.Register(registrationData);
                    Console.WriteLine($"- Registered user {alias}");
                    return registrationResult;

                case "transaction":
                    if (!transactionRegex.IsMatch(input))
                    {
                        throw new Exception("Invalid command syntax");
                    }
                    MatchCollection transactionMatches = transactionRegex.Matches(input);
                    string amount = transactionMatches[0].Groups[2].ToString();
                    string payFrom = transactionMatches[0].Groups[3].ToString();
                    string payTo = transactionMatches[0].Groups[4].ToString();

                    dynamic transactionData = new ExpandoObject();
                    transactionData.amount = amount;
                    transactionData.from = payFrom;
                    transactionData.to = payTo;
                    //bool transactionResult = CommandController.Transaction();
                    Console.WriteLine($"- Created transaction");
                    return false;//transactionResult;

                case "message":
                    if (!messageRegex.IsMatch(input))
                    {
                        throw new Exception("Invalid command syntax");
                    }
                    MatchCollection messageMatches = messageRegex.Matches(input);
                    string messageMessage = messageMatches[0].Groups[2].ToString();

                    //Create data object
                    BlockDataMessage messageData = new BlockDataMessage()
                    {
                        PublicKey = keyPair.PublicKeyString,
                        Data = messageMessage
                    };
                    messageData.Signature = Crypto.Sign(keyPair.PrivateKey, messageData.Hash);
                    bool messageResult = CommandController.Message(messageData);
                    Console.WriteLine($"- Posted message");
                    return false;

                case "mine":
                    if (!mineRegex.IsMatch(input))
                    {
                        throw new Exception("Invalid command syntax");
                    }
                    MatchCollection mineMatches = mineRegex.Matches(input);
                    string seed = mineMatches[0].Groups[2].ToString();

                    //Create coinbase transaction
                    BlockDataTransaction coinbaseTx = new BlockDataTransaction()
                    {
                        TxOut = new List<TransactionOutput>()
                        {
                            new TransactionOutput() {
                                Address = keyPair.PublicKeyString,
                                Amount = CommandController.COINBASE_AMT
                            },
                        }
                    };
                    coinbaseTx.Signature = Crypto.Sign(keyPair.PrivateKey, coinbaseTx.Hash);

                    bool mineResult = CommandController.StartMining(seed, coinbaseTx);
                    Console.WriteLine($"- Mining has started");
                    return mineResult;

                default:
                    throw new Exception("Invalid command");
            }
        }
    }
}
