using BL.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Encrypt
{
    public interface IEncryption
    {
        void EncryptData(object data);
        void DecryptData(object data);
        string Encrypt(string data);
        string Decrypt(string cipherText);
    }
}
