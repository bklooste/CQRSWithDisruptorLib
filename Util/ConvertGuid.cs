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
    public static class ConvertGuid
    {
      
        public static int FromGuid(Guid value)
        {
            return BitConverter.ToInt32(value.ToByteArray(), 0);
        }

        public static Guid ToGuid(uint value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);


            //   BitConverter.GetBytes
            //return Long2Guid(_value);
            //      return new Guid(, (short)_value, (short)(_value >> 16), 0, 0, 0, 0, 0, 0, 0 , 0);
        }

        //public static int FromGuid(Guid value)
        //{
        //    return BitConverter.ToInt32(value.ToByteArray(), 0);
        //}

        public static Guid ToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);


            //   BitConverter.GetBytes
            //return Long2Guid(_value);
            //      return new Guid(, (short)_value, (short)(_value >> 16), 0, 0, 0, 0, 0, 0, 0 , 0);
        }

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
}
