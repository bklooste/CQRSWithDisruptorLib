using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L6.Infrastructure.Util;

namespace L6.Infrastructure.Domain
{

    // an Entity root
    public  interface IAggregateRoot<T>
    {

       int AggregateRootId { get; }

        /// <summary>
        /// really should be private but we have repositories generating them . 
        /// Cant be internal as we may have new repositories.
        /// </summary>
        /// <param name="id"></param>
        void SetId(int id);
        //{
        //    AggregateRootId = id;
        //}
     //   void SetId(int id);
    }
}
