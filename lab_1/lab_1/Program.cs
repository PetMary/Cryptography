using System;
using System.Collections.Generic;

namespace lab_1
{
    class Program
    {
        static void Main(string[] args)
        {

            /*
            byte[] array_byte = {24, 91, 17, 5 };
            int[,] p_block ={
                { 16,  7, 20, 21, 29, 12, 28, 17 },
                { 1, 15, 23, 26,  5, 18, 31, 10 },
                { 2,  8, 24, 14, 32, 27,  3,  9 },
                { 19, 13, 30,  6, 22, 11,  4, 25 }};
            byte[] array_byte2 = { 102, 91, 217};
            int[] s_block_4 = { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 };
            int[] s_block_3 = { 7, 4, 1, 0, 5, 3, 2, 6 };
            int k = 3;


            try
            {
                var res1 = Algorithms_1.P_Block(array_byte, p_block);
                var res2 = Algorithms_1.S_Block_2(array_byte2, s_block_3, k);

                Console.WriteLine("P-перестановка:");
                for (int i = 0; i < res1.Length; i++)
                {
                    Console.WriteLine("({0}){1} = {2}", i + 1, res1[i], Convert.ToString(res1[i], 2));
                }

                Console.WriteLine("S-перестановка:");
                for (int i = 0; i < res2.Length; i++)
                {
                    Console.WriteLine("({0}){1} = {2}", i + 1, res2[i], Convert.ToString(res2[i], 2));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            */
            byte[] block = { 115, 91, 176, 215, 54, 213, 187, 38 };
            byte[] key = { 83, 16, 197, 20, 209, 244, 46, 162 };
            DES d = new DES();

            var res = d.Encryption(block, key);
            for (int i = 0; i < res.Length; i++)
            {
                Console.WriteLine("({0}){1} = {2}", i + 1, res[i], Convert.ToString(res[i], 2));
            }
            
        }
    }
}
