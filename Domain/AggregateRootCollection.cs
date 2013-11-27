//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using L6.Infrastructure.Domain;
//using L6.Infrastructure.Util;

//namespace L6.Infrastructure.Events
//{
//    public class AggregateRootList<T> :IList<T> where T: AggregateRoot , new()
//    {
//        IList<T> list = new List<T>();

//        public IEnumerator<T> GetEnumerator()
//        {
//            return list.GetEnumerator(); 
//        }

//        public void Add(T item)
//        {
//            if (item == null || item.Id != 0)
//                throw new ArgumentNullException("item is null or contains a non zero key");

//            var maxId = list.Max(x => x.Id)++;

//            item.Id = (Id<int>)maxId;

//            while (list.Count <= maxId)
//                list.Add(new T());

//            list[maxId] = item;

//        }

//        IEnumerator GetEnumerator()
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

//        public int IndexOf(T item)
//        {
//            throw new NotImplementedException();
//        }

//        public void Insert(int index, T item)
//        {
//            throw new NotImplementedException();
//        }

//        public void RemoveAt(int index)
//        {
//            throw new NotImplementedException();
//        }

//        public T this[int index]
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//            set
//            {
//                throw new NotImplementedException();
//            }
//        }


//        public void Clear()
//        {
//            throw new NotImplementedException();
//        }

//        public bool Contains(T item)
//        {
//            throw new NotImplementedException();
//        }

//        public void CopyTo(T[] array, int arrayIndex)
//        {
//            throw new NotImplementedException();
//        }

//        public int Count
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public bool IsReadOnly
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public bool Remove(T item)
//        {
//            throw new NotImplementedException();
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
