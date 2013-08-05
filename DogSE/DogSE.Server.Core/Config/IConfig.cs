using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DogSE.Server.Core.Config
{
    interface IConfig<T>
    {
        static T Instance { get; }
    }
}
