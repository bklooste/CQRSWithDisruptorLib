using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L6.Infrastructure.Util;
using L6.Infrastructure.Domain;


namespace L6.Infrastructure.Events
{
    // fixme does this need to be serializable
    [Serializable]
    public class TimeoutMessage :DomainEvent
    {
        DateTime expires;
      //  Id<Saga> id; 

        public TimeoutMessage(DateTime expiration )// ,Id<Saga> sagaId ) 
        {
            expires = DateTime.SpecifyKind(expiration, DateTimeKind.Utc);
          //  id = sagaId;

        }

        ///// <summary>
        ///// Indicate a timeout within the given time for the given saga maintaing the given state.
        ///// </summary>
        ///// <param name="expireIn"></param>
        ///// <param name="saga"></param>
        ///// <param name="state"></param>
        //public TimeoutMessage(TimeSpan expireIn, ISagaEntity saga, object state) :
        //    this(DateTime.UtcNow + expireIn, saga, state)
        //{

        //}
    }
}
