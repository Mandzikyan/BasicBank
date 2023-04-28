using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Configuration
{
    public interface IConfigurationKey
    {
        string  EncryptKey { get; }
        string  EncrpytByte { get; }
        string EmailLogin { get; }
        string EmailPassword { get; }

    }
}
