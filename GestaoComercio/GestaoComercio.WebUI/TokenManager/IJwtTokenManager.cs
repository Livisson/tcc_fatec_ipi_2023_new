using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoComercio.WebUI.TokenManager
{
    public interface IJwtTokenManager
    {
        string Authenticate(string userName, string password);
    }
}
