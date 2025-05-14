using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDomain.Entities
{
    public class Class1
    {

        static void Main(string[] args)
        {


            //string name  = "John Doe";

            //Dictionary<char, int> charCount = new Dictionary<char, int>();
            //foreach(char c in name)
            //{
            //    if (charCount.ContainsKey(c))
            //    {
            //        charCount[c]++;
            //    }
            //    else
            //    {
            //        charCount[c] = 1;
            //    }
            //}

            //foreach (var kvp in charCount)
            //{
            //    Console.WriteLine($"Character: {kvp.Key}, Count: {kvp.Value}");
            //}



            int a = 10;
            int b = 20;
           
            a = a ^ b;
            b = a ^ b;
            a = a ^ b;
            Console.WriteLine($"a: {a}, b: {b}");
         



        }
    }
}
