using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L6.Infrastructure.Commands;



namespace L6.Infrastructure.Domain
{
    //entry

    //FIXME duel function holds and dispatches to the domain 
    public static class MultipleDomainCommandDispatcher
    {
        //static IDomainDispatcher[] domains;
        static IDictionary<Guid,IDomain> domains = new Dictionary<Guid,IDomain> () ; // use an array as growing is not thread safe.
          static IDictionary<int,IDomain> domainsGameIndex = new Dictionary<int,IDomain> () ; // use an array as growing is not thread safe.
        // use disruptor with infinity 

        // N producers , 1 Consumer



        // may be a bit dodgy
        // it will swap dommains in an atomic as we are still using the same domains 
        //and Domain dispatcher has top handle many callers .
        // i think it will be ok 

          static public void AddDomain(IDomain newEntry)
          {
              AddDomain(newEntry, -1);
          }

        static public void AddDomain(IDomain newEntry ,int  gameID)
        {

            domains.Add(newEntry.Id, newEntry);

            if (gameID > 0)
                domainsGameIndex.Add(gameID, newEntry);

            //int size = Math.Max(newEntry.DomainId, domains.Length);
            //IDomainCommandDispatcher[] new_dommains = new DomainCommandDispatcher[size];

            //foreach (var dom in domains)
            //    new_dommains[dom.DomainId] = dom;

            //new_dommains[newEntry.DomainId] = newEntry;

            //domains = new_dommains; // SHould be atomic
        }



        //use ringbuffer with IDomaincommand
        static public void Publish(DomainCommand command)
        {
            Publish(command.ToDomain , command.Command);  // could unwrap here ...
        }


        static public void Publish(Guid id , Command command)
        {
            domains[id].Publisher.Publish(command);
        }



        public static void Publish(int gameId, Command command)
        {
            domainsGameIndex[gameId].Publisher.Publish(command);
        }
    }

    // Commands
}
