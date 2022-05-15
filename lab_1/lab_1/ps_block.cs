using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_1
{
    class ps_block
    {
        public ps_block() { }

        public byte[] P_Block(byte[] array_byte, int[,] p_block)
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

        public byte[] S_Block(byte[] array_byte, int[] s_block, int k)
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
    }
}
