using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

using Q.Models;

namespace Q.Common
{
    public class Crypto
    {
        public static string ComputeHash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return GetHash(sha256Hash, input);
            }
        }

        //https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.computehash?view=net-5.0
        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static KeyPair GenerateKeyPair()
        {
            ECDsa curve = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            curve.GenerateKey(ECCurve.NamedCurves.nistP256);
            return new KeyPair() {
                PrivateKey = curve.ExportECPrivateKey(),
                PublicKey = curve.ExportSubjectPublicKeyInfo()
            };
        }

        public static string Sign(byte[] privateKey, string data)
        {
            ECDsa curve = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            curve.GenerateKey(ECCurve.NamedCurves.nistP256);
            int bytesRead = 0;
            curve.ImportECPrivateKey(privateKey, out bytesRead);

            return Convert.ToBase64String(curve.SignData(Encoding.UTF8.GetBytes(data), HashAlgorithmName.SHA256));
        }

        public static bool Verify(byte[] publicKey, string data, string signature)
        {
            ECDsa curve = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            curve.GenerateKey(ECCurve.NamedCurves.nistP256);
            int bytesRead = 0;
            curve.ImportSubjectPublicKeyInfo(publicKey, out bytesRead);

            return curve.VerifyData(Encoding.UTF8.GetBytes(data), Convert.FromBase64String(signature), HashAlgorithmName.SHA256);
        }
    }
}
