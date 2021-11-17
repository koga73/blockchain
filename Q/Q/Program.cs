using Q.Controllers;
using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Dynamic;
using System.Diagnostics;
using Q.Common;
using Q.Models;

namespace Q
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Commands");
            Console.WriteLine("");
            Console.WriteLine("  newchain");
            Console.WriteLine("  viewchain");
            Console.WriteLine("  register {ALIAS} {PUBLICKEY}");
            Console.WriteLine("  transaction {AMOUNT} {FROM} {TO}");
            Console.WriteLine("  post {FROM} {TO} {DESCRIPTION} {DATA}");
            Console.WriteLine("  mine {SEED}");
            Console.WriteLine("  exit");
            
            do
            {
                Console.WriteLine("");
                string input = Console.ReadLine();
                if (input == "exit")
                {
                    break;
                } else
                {
                    try
                    {
                        bool executionResult = parseInput(input);
                    } catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            } while (true);
        }

        static bool parseInput(string input)
        {
            Regex commandRegex = new Regex("^(newchain|viewchain|register|transaction|post|mine)");
            Regex newchainRegex = new Regex("^(newchain)$");
            Regex viewchainRegex = new Regex("^(viewchain)$");
            Regex registerRegex = new Regex("^(register)\\s(.+?)\\s(.+?)$");
            Regex transactionRegex = new Regex("^(transaction)\\s([\\d.]+?)\\s(.+?)\\s(.+?)$");
            Regex postRegex = new Regex("^(post)\\s(.+?)\\s(.+?)\\s(\".+\"|.+?)\\s(\".+\"|.+?)$");
            Regex mineRegex = new Regex("^(mine)\\s(.+)$");

            string command = null;
            if (commandRegex.IsMatch(input))
            {
                command = commandRegex.Matches(input)[0].ToString();
            }
            switch (command)
            {
                case "newchain":
                    if (!newchainRegex.IsMatch(input))
                    {
                        throw new Exception("Invalid command syntax");
                    }
                    return CommandController.Execute(CommandController.COMMANDS.NEW_CHAIN);

                case "viewchain":
                    if (!viewchainRegex.IsMatch(input))
                    {
                        throw new Exception("Invalid command syntax");
                    }
                    Console.Write(BlockChain.ToString());
                    return false;

                case "register":
                    if (!registerRegex.IsMatch(input))
                    {
                        throw new Exception("Invalid command syntax");
                    }
                    MatchCollection registerMatches = registerRegex.Matches(input);
                    string alias = registerMatches[0].Groups[2].ToString();
                    string publicKey = registerMatches[0].Groups[3].ToString();
                    
                    dynamic registerData = new ExpandoObject();
                    registerData.alias = alias;
                    registerData.publicKey = publicKey;
                    return CommandController.Execute(CommandController.COMMANDS.REGISTER, JsonConvert.SerializeObject(registerData));
                    
                case "transaction":
                    if (!transactionRegex.IsMatch(input))
                    {
                        throw new Exception("Invalid command syntax");
                    }
                    MatchCollection transactionMatches = transactionRegex.Matches(input);
                    string amount = transactionMatches[0].Groups[2].ToString();
                    string payFrom = transactionMatches[0].Groups[3].ToString();
                    string payTo = transactionMatches[0].Groups[4].ToString();

                    dynamic transactionData= new ExpandoObject();
                    transactionData.amount = amount;
                    transactionData.from = payFrom;
                    transactionData.to= payTo;
                    return CommandController.Execute(CommandController.COMMANDS.POST_TRANSACTION, JsonConvert.SerializeObject(transactionData));

                case "post":
                    if (!postRegex.IsMatch(input))
                    {
                        throw new Exception("Invalid command syntax");
                    }
                    MatchCollection postMatches = postRegex.Matches(input);
                    string postFrom = postMatches[0].Groups[2].ToString();
                    string postTo = postMatches[0].Groups[3].ToString();
                    string description = postMatches[0].Groups[4].ToString().Replace("\"", "");
                    string data = postMatches[0].Groups[5].ToString().Replace("\"", "");

                    dynamic postData = new ExpandoObject();
                    postData.from = postFrom;
                    postData.to = postTo;
                    postData.description = description;
                    postData.data = data;
                    return CommandController.Execute(CommandController.COMMANDS.POST_REFERENCE, JsonConvert.SerializeObject(postData));

                case "mine":
                    if (!mineRegex.IsMatch(input))
                    {
                        throw new Exception("Invalid command syntax");
                    }
                    MatchCollection mineMatches = mineRegex.Matches(input);
                    string seed = mineMatches[0].Groups[2].ToString();

                    dynamic mineData = new ExpandoObject();
                    mineData.seed = seed;
                    return CommandController.Execute(CommandController.COMMANDS.MINE, JsonConvert.SerializeObject(mineData));

                default:
                    throw new Exception("Invalid command");

            }
        }
    }
}
