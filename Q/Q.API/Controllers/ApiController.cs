using System;
using System.Dynamic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using Q.API.Models;
using Q.API.Models.Requests;
using Q.API.Models.Responses;
using Q.Data.Common;
using Q.Data.Models;
using Q.Data.Models.Struct;
using Q.Chain.Controllers;

namespace Q.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private static Dictionary<string, string> keys = new Dictionary<string, string>();

        private readonly ILogger<ApiController> _logger;

        static ApiController()
        {
            //Load keys
            string path = Paths.KeysPath;
            Regex fileNameRegex = new Regex("^.+[\\/\\\\](.+\\.pem)$");
            List<string> files = Directory.EnumerateFiles(path).ToList();
            foreach (string file in files)
            {
                if (fileNameRegex.IsMatch(file))
                {
                    MatchCollection fileNameMatches = fileNameRegex.Matches(file);
                    string fileName = fileNameMatches[0].Groups[1].ToString();

                    string text = System.IO.File.ReadAllText(file);
                    text = Regex.Replace(text, "(--.+--)|[\\s]", "").Trim();
                    keys.Add(fileName, text);
                }
            }
        }

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet("[action]")]
        public ApiResponse Heartbeat()
        {
            return new ApiResponse()
            {
                Success = true,
                Data = "OK"
            };
        }

        [HttpGet("[action]")]
        public ApiResponse Users()
        {
            return new ApiResponse()
            {
                Success = true,
                Data = keys.Where(k => k.Key.EndsWith(".public.pem")).Select(k =>
                    new UserResponse()
                    {
                        Alias = k.Key.Replace(".public.pem", ""),
                        PublicKey = k.Value
                    }
                ).ToArray()
            };
        }

        [HttpPost("[action]")]
        public ApiResponse RegisterUser(RegistrationRequest request)
        {
            try
            {
                //Generate and save keys
                string path = Paths.KeysPath;
                KeyPair keyPair = Crypto.GenerateKeyPair();
                dynamic keyObj = JsonConvert.DeserializeObject(keyPair.ToString());
                System.IO.File.WriteAllLines($"{path}/{request.Alias}.private.pem", new string[] { "-----BEGIN PRIVATE KEY-----", keyObj.privateKey, "-----END PRIVATE KEY-----" });
                System.IO.File.WriteAllLines($"{path}/{request.Alias}.public.pem", new string[] { "-----BEGIN PUBLIC KEY-----", keyObj.publicKey, "-----END PUBLIC KEY-----" });
                keys.Add($"{request.Alias}.private.pem", (string)keyObj.privateKey);
                keys.Add($"{request.Alias}.public.pem", (string)keyObj.publicKey);

                //Create data object
                BlockDataRegistration registrationData = new BlockDataRegistration()
                {
                    Alias = request.Alias,
                    PublicKey = keyPair.PublicKeyString
                };
                registrationData.Signature = Crypto.Sign(keyPair.PrivateKey, registrationData.Hash);
                bool registrationResult = CommandController.Register(registrationData);
            
                //Return response
                return new ApiResponse()
                {
                    Success = true,
                    Data = new UserResponse()
                    {
                        Alias = request.Alias,
                        PublicKey = keyPair.PublicKeyString
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Success = false,
                    Data = ex.Message
                };
            }
        }

        [HttpPost("[action]")]
        public ApiResponse Mine(MineRequest request)
        {
            try
            {
                KeyPair keyPair = KeyPair.Parse(keys[$"{request.Alias}.private.pem"], keys[$"{request.Alias}.public.pem"]);

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

                CommandController.Mine(request.Seed, coinbaseTx);

                return new ApiResponse()
                {
                    Success = true,
                    Data = "STARTED MINING!"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Success = false,
                    Data = ex.Message
                };
            }

        }
    }
}