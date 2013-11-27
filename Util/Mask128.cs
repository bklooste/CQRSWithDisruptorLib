using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L6.Infrastructure.Util
{
    /// <summary>
    /// Entity Framework style Ids which cant be Zero !  
    /// For Nullable Foreign Keys use uint?
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// 
    // public static class MyExtensions
    //{
    //    public static int WordCount(this String str)
    //    {
    //        return str.Split(new char[] { ' ', '.', '?' }, 
    //                         StringSplitOptions.RemoveEmptyEntries).Length;
    //    }
    //}

    //FIXME duplicated
    //public struct Mask128
    //{
    //    long val1;
    //    long val2;

    //    public Mask128(BitArray mask)
    //    {

    //    }

    //    public bool this[int index]
    //    {
    //        get { /* return the specified index here */ }
    //        set { /* set the specified index to value here */ }
    //    }

    //}

      //  public static uint FromGuid(Guid value)
      //  {
      //      return ( uint) BitConverter.ToInt32( value.ToByteArray() , 0);
      //  }

      //  internal Guid ToGuid()
      //  { 
      //      long val = typeof(T).Name.GetHashCode() << 32 + _value; 
      //   //   BitConverter.GetBytes
      //      return Long2Guid( val); 
      ////      return new Guid(, (short)_value, (short)(_value >> 16), 0, 0, 0, 0, 0, 0, 0 , 0);
      //  }


        //static Guid Long2Guid(long value)
        //{
        //    byte[] bytes = new byte[16];
        //    BitConverter.GetBytes(value).CopyTo(bytes, 0);
        //    return new Guid(bytes);
        //}

        //static long Guid2Long(Guid value)
        //{
        //    byte[] b = value.ToByteArray();
        //    long bint = BitConverter.ToInt64(b, 0);
        //    return bint;
        //}


        //static Guid Int2Guid(int value)
        //{
        //    byte[] bytes = new byte[16];
        //    BitConverter.GetBytes(value).CopyTo(bytes, 0);
        //    return new Guid(bytes);
        //}

        //static int Guid2Int(Guid value)
        //{
        //    byte[] b = value.ToByteArray();
        //    int bint = BitConverter.ToInt32(b, 0);
        //    return bint;
        //}
    
}
