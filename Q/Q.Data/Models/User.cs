using System;
using System.Collections.Generic;
using System.Text;

using Q.Data.Models.Struct;

namespace Q.Data.Models
{
    public class User : KeyPair
    {
        public string Alias;

        public User(BlockDataRegistration registrationData)
        {
            Alias = registrationData.Alias;
            PublicKey = Convert.FromBase64String(registrationData.PublicKey);
        }
    }
}
