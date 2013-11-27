//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace L6.Infrastructure.Events
//{

//    // we move persistance to commands
//    public class PersistedAggregateRootCollection<T> : IAggregateRootCollection<T> where T : IAggregateRoot, new()
//    {
//        IList<T> list = new List<T>();
//        IDataContext<T> context;
//        Action<T> sucess ;


//        PersistedAggregateRootCollection(IDataContext<T> context)
//        {
//            this.context = context;
//            sucess  = new Action<T>(AddInternal); 
//        }

//        public IEnumerator<T> GetEnumerator()
//        {
//            return list.GetEnumerator(); 
//        }

//        public void Add(T item)
//        {
//            context.Save(item, sucess, null); 


//        }

//        void AddInternal (T item)
//        {
            
//            if (item == null || item.Id != 0)
//                throw new ArgumentNullException("item is null or contains a non zero key");

//            var maxId = list.Max( x => x.Id);

//            item.SetId(++maxId) ;

//            while (list.Count <= maxId)
//                list.Add(new T());

//            list[maxId] = item; 
//        }

//        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
//        {
//            return list.GetEnumerator() as System.Collections.IEnumerator; 
//        }

//        public T this[int index]
//        {
//            get
//            {
//                return list[index];
//            }
          
//        }
//    }
//}
