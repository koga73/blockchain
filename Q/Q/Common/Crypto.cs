using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

using Q.Models;

namespace Q.Common
{
    public class Crypto
    {
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
