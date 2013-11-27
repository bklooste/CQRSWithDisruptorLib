using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L6.Infrastructure.Util
{

    public class RarityHolder<T>
    {
        public float Rarity { get; set; }
        public T Value { get; set; }

        public RarityHolder() {}


      
    }

    public class RandomHelper
    {

            uint   m_w = 521288629;
             uint m_z = 362436069;


        //FIXME set seed 
         public RandomHelper()
        {


        }



         public T FindRandomInRarityList<T>(IEnumerable<RarityHolder<T>> items) 
        {
            if (items.Count() == 0)
                return default(T);

      

            float itemRarity = 0;
            foreach (var item in items)
                    itemRarity += item.Rarity;

            var getRandom = RandomNumberGenerator.Instance.NextDouble() * itemRarity;
            double count = 0;
            foreach (var item in items)
            {
                count += item.Rarity;
                if (getRandom <= count)
                    return item.Value ;

            }
            throw new InvalidOperationException("An error has occured and we didnt find an item in a non zero lenght list"); 
            //return default(T);
        }


        //Warnign this does not work with anoymous types
       public T FindRandomInListWithRarity<T>(IEnumerable<T> items) where T : class
       {
           if (items.Count() == 0)
               return null;



           float itemRarity = 0;
           foreach (var item in items)
           {
               if (((dynamic)item).Rarity == null)
                   throw new InvalidOperationException("item " + item.ToString() + " does not have a rarity field");
               itemRarity += ((dynamic)item).Rarity;
           }


           var getRandom = RandomNumberGenerator.Instance.NextDouble() * itemRarity;
           double count = 0;
           foreach (var item in items)
           {
               count += ((dynamic)item).Rarity;
               if (getRandom <= count)
                   return item as T;

           }
           return default(T);
       }

       public T FindRandomInList<T>(IEnumerable<T> items) where T : class
        {

            var list = items.ToList();
            var index = RandomNumberGenerator.Instance.Next(0, list.Count - 1);

            return list[index];
        }


       //public int GetRandomValueBaseOnSD(uint value, float sd)
       //{
       //    return GetRandomValueBaseOnSD(value, sd);
       //}

        // min 0 
       public int GetRandomValueBaseOnSD(int value, float sd)
       {
           double random_value = GetNormal(1, sd /2);
           var result = value * random_value;
           if (result < 0)
               result = 0; 
           return Convert.ToInt32(result); 
       }
        

          // Get exponential random sample with mean 1
        public  double GetExponential()
        {
            return -Math.Log( GetUniform() );
        }

        // Get exponential random sample with specified mean
        public  double GetExponential(double mean)
        {
            if (mean <= 0.0)
            {
                string msg = string.Format("Mean must be positive. Received {0}.", mean);
                throw new ArgumentOutOfRangeException(msg);
            }
            return mean*GetExponential();
        }

        //// code this then test our randome code .. Also remove references to random! need seed

        //Math.pow(Math.random(), skew);  // 1 skew = balanced , 0 to 1 gives  a root distribution  > 1 will make  small numbers more common 
        //// eg for skew calues    , randoms become. 
        ////  0.5 ,  0.5 = 0.71 , 1 = 1  , 2 = 0.25
        //// 1.5 ,  0.5  = 0.35 , 1 = 1 , 2 = 2.25
        ////  2 ,  0.5 = 0.25 , 1 = 1  , 2 = 4 

        ////For skewing your distribution I'd just use a regular normal distribution, 
        ////choosing μ and σ appropriately for one side of your curve and then determine on which side of your wanted mean a point fell, 
        ////stretching it appropriately to fit your desired distribution.

        // pass in say 1 and 0.66  then multiply.
          public  double GetNormal(double mean, double standardDeviation)
           {
               if (standardDeviation <= 0.0)
               {
                   string msg = string.Format("Shape must be positive. Received {0}.", standardDeviation);
                   throw new ArgumentOutOfRangeException(msg);
               }
               return mean + standardDeviation*GetNormal();
           }

           public  double GetNormal()
           {
               // Use Box-Muller algorithm
               double u1 = GetUniform();
               double u2 = GetUniform();
               double r = Math.Sqrt( -2.0*Math.Log(u1) );
               double theta = 2.0*Math.PI*u2;
               return r*Math.Sin(theta);
           }

           public  double GetUniform()
           {
               // 0 <= u < 2^32
               uint u = GetUint();
               // The magic number below is 1/(2^32 + 2).
               // The result is strictly between 0 and 1.
               return (u + 1.0) * 2.328306435454494e-10;
           }

        private  uint GetUint()
           {
               m_z = 36969 * (m_z & 65535) + (m_z >> 16);
               m_w = 18000 * (m_w & 65535) + (m_w >> 16);
               return (m_z << 16) + m_w;
           }


        public  double GetGamma(double shape, double scale)
        {
            // Implementation based on "A Simple Method for Generating Gamma Variables"
            // by George Marsaglia and Wai Wan Tsang.  ACM Transactions on Mathematical Software
            // Vol 26, No 3, September 2000, pages 363-372.

            double d, c, x, xsquared, v, u;

            if (shape >= 1.0)
            {
                d = shape - 1.0 / 3.0;
                c = 1.0 / Math.Sqrt(9.0 * d);
                for (; ; )
                {
                    do
                    {
                        x = GetNormal();
                        v = 1.0 + c * x;
                    }
                    while (v <= 0.0);
                    v = v * v * v;
                    u = GetUniform();
                    xsquared = x * x;
                    if (u < 1.0 - .0331 * xsquared * xsquared || Math.Log(u) < 0.5 * xsquared + d * (1.0 - v + Math.Log(v)))
                        return scale * d * v;
                }
            }
            else if (shape <= 0.0)
            {
                string msg = string.Format("Shape must be positive. Received {0}.", shape);
                throw new ArgumentOutOfRangeException(msg);
            }
            else
            {
                double g = GetGamma(shape + 1.0, 1.0);
                double w = GetUniform();
                return scale * g * Math.Pow(w, 1.0 / shape);
            }
        }


        // k =3 and k =4 are nice low stacked algorithms
        public  double GetChiSquare(double degreesOfFreedom)
        {
            // A chi squared distribution with n degrees of freedom
            // is a gamma distribution with shape n/2 and scale 2.
            return GetGamma(0.5 * degreesOfFreedom, 2.0);
        }



        public float Next(float min, float max )
        {
            return min + Convert.ToSingle(RandomNumberGenerator.Instance.NextDouble() * (max - min));
        }
    }
}
