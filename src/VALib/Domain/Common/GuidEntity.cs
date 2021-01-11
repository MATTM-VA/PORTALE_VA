using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Common
{
    public abstract class GuidEntity
    {
        public Guid ID { get; internal set; }
    }
}
