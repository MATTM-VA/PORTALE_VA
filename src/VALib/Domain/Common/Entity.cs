using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Common
{
    public abstract class Entity
    {
        public int ID { get; internal set; }
    }
}
