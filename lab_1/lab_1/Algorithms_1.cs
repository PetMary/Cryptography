using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_1
{
    static class Algorithms_1
    {
        public static byte[] P_Block(byte[] array_byte, int[,] p_block)
        {
            int size_i = p_block.GetUpperBound(0) + 1;
            int size_j = p_block.GetUpperBound(1) + 1;
            byte[] res = new byte[size_i];

            if (p_block is null)
                throw new ArgumentException("p-block is null");

            for (int i = 0; i < size_i; i++)
            {
                for (int j = 0; j < size_j; j++)
                {
                    int index = p_block[i, j] - 1;
                    byte byte_x = array_byte[index / size_j];
                    byte x = (byte)((byte_x >> (size_j - 1 - index % size_j)) & 1);
                    res[i] = (byte)(res[i] | (x << (size_j - 1 - j)));
                }
            }
            return res;
        }

        public static byte[] S_Block(byte[] array_byte, int[,,] s_block)
        {
            byte[] res = new byte[4];
            int n = 8;
            byte bit_6;

            if (s_block is null)
                throw new ArgumentException("s-block is null");

            for (int i = 0, j = 0; i < array_byte.Length; i++)
            {
                bit_6 = 0;
                for (int k = 5; k >=0; k--)
                {
                    bit_6 = (byte)(bit_6 | (((array_byte[j] >> --n) & 1) << k));
                    if (n==0)
                    {
                        j++;
                        n = 8;
                    }
                }
                int x = (bit_6 >> 5) << 1 | (bit_6 & 1);
                int y = (bit_6 >> 1) & (15);

                int new_x = s_block[i, x, y];
                if (i % 2 == 0)
                    res[i / 2] = (byte)(res[i / 2] | (new_x << 4)); 
                else
                    res[i / 2] = (byte)(res[i / 2] | (new_x)); 

            }
            return res;
        }

        public static byte[] S_Block_2(byte[] array_byte, int[] s_block, int k)
        {
            byte[] res = new byte[array_byte.Length];
            byte new_byte = 0, k_bit = 0;

            if (s_block is null)
                throw new ArgumentException("s-block is null");

            for (int i = array_byte.Length - 1, j = 0; i >= 0; i--)
            {
                for (; j < 8; j += k)
                {
                    k_bit = (byte)((array_byte[i] >> j) & ((1 << k) - 1));
                    if (j + k <= 8)
                        new_byte = (byte)(new_byte | (s_block[k_bit] << j));
                }

                if ((j != 8) && (i != 0))
                {
                    k_bit = (byte)(k_bit | ((array_byte[i - 1] & ((1 << (j - 8)) - 1)) << (k - j + 8)));
                    new_byte = (byte)((new_byte | (s_block[k_bit] << (j - k))) & 255);

                }
                else if ((i == 0) && (j != 8))
                {
                    new_byte = (byte)(new_byte | (k_bit << (j - k)));
                }

                res[i] = new_byte;
                new_byte = (byte)(s_block[k_bit] >> (k - j + 8));
                j = j % 8;
            }
            return res;
        }

        public static byte[] XOR(byte[] byte_1, byte[] byte_2)
        {
            byte[] res;
            int size;
            if (byte_1.Length > byte_2.Length)
            {
                res = (byte[])byte_1.Clone();
                size = byte_2.Length;
            }
            else
            {
                res = (byte[])byte_2.Clone();
                size = byte_1.Length;
            }
            for (int i = 0; i < size; i++)
            {
                res[i] = (byte)(byte_1[i] ^ byte_2[i]);
            }
            return res;
        }

    }
}
