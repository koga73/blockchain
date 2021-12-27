using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Text;

namespace Q.Data.Models
{
    public class KeyPair
    {
        public byte[] PrivateKey;
        public byte[] PublicKey;

        public string PublicKeyString {
            get
            {
                return Convert.ToBase64String(PublicKey);
            }
        }

        override public string ToString()
        {
            return $"{{ \"privateKey\":\"{Convert.ToBase64String(PrivateKey)}\", \"publicKey\":\"{Convert.ToBase64String(PublicKey)}\" }}";
        }

        public static KeyPair Parse(string privateKey, string publicKey)
        {
            return new KeyPair()
            {
                PrivateKey = Convert.FromBase64String(privateKey),
                PublicKey = Convert.FromBase64String(publicKey)
            };
        }
    }
}
