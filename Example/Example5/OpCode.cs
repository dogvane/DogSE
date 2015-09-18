using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example2
{
    public enum OpCode:ushort
    {
        Login = 0,

        SendMessage = 1,

        SendPriviteMessage= 2,


        LoginResult = 10,

        RecvMessage = 11,

        RecvPrivateMessage = 12,
    }
}
