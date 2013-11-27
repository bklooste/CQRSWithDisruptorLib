using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L6.Infrastructure.Commands;
using L6.Infrastructure.Domain;
using L6.Infrastructure.Util;

namespace L6.Infrastructure.Domain
{

    /// <summary>
    ///  Looks like repositories call beyond the Disruptor boundary ??? ( look at the API ) .. 
    ///  
    /// Note repository is responsible of converting Domain Ids to Event store Guids and generating Ids
    /// </summary>
    public interface IRequiresData
    {
        void SetData(Object data); 
    };

}
