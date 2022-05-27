using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_1
{
    class Round_key : I_Key_expansion
    {
        private static int[,] P_1 = {{ 57, 49, 41, 33, 25, 17, 9, 1}, 
                                     { 58, 50, 42, 34, 26, 18, 10, 2 },
                                     { 59, 51, 43, 35, 27, 19, 11, 3},
                                     { 60, 52, 44, 36, 63, 55, 47, 39},
                                     { 31, 23, 15, 7, 62, 54, 46, 38},
                                     { 30, 22, 14, 6, 61, 53, 45, 37},
                                     { 29, 21, 13, 5, 28, 20, 12, 4 }};
        private static int[,] P_2 = {{14, 17, 11, 24, 1, 5, 3, 28 },
                                    {15, 6, 21, 10, 23, 19, 12, 4 },
                                    {26, 8, 16, 7, 27, 20, 13, 2},
                                    {41, 52, 31, 37, 47, 55, 30, 40 },
                                    {51, 45, 33, 48, 44, 49, 39, 56 },
                                    {34, 53, 46, 42, 50, 36, 29, 32}};
        private static int[] shift_array = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };

        public Round_key() { }

        public byte[][] Get_round_Keys(byte[] key)
        {
            byte[] key_56, C = { 0, 0, 0, 0 }, D = { 0, 0, 0, 0 };
            byte[] CD = new byte[7];
            byte[][] res = new byte[16][];
            for (int i = 0; i < 16; i++)
                res[i] = new byte[6];

            if (key.Length != 8)
                throw new ArgumentException("invalid key length");

            key_56 = Algorithms_1.P_Block(key, P_1);
            for (int i = 0; i < 4; i++)
            {
                C[i] = (byte)(C[i] | key_56[i] >> 4);
                if (i != 3)
                    C[i + 1] = (byte)(C[i + 1] | (key_56[i] << 4) & 255);
                if (i == 0)
                    D[i] = (byte)(key_56[i + 3] & 15);
                else
                    D[i] = key_56[i + 3];
            }

            for (int i = 0; i < 16; i++)
            {
                Shift_Left(C, shift_array[i]);
                Shift_Left(D, shift_array[i]);

                for (int j = 0; j < 4; j++)
                {
                    CD[j] = (byte)((C[j] << 4) & 255);
                    if (j != 3)
                        CD[j] = (byte)(CD[j] | C[j + 1] >> 4);
                    if (j != 0)
                        CD[j + 3] = D[j];
                }
                CD[3] = (byte)(CD[3] | D[0]);

                res[i] = Algorithms_1.P_Block(CD, P_2);
            }

            return res;
        }

        public void Shift_Left(byte[] array_byte, int n)
        {
            byte[] bit = { 0, 0 };

            for (int i = array_byte.Length - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    bit[i % 2] = (byte)(array_byte[i] >> (4 - n));
                    array_byte[i] = (byte)((array_byte[i] << n | bit[(i + 1) % 2]) & 15);
                }
                else
                {
                    bit[i % 2] = (byte)(array_byte[i] >> (8 - n));
                    array_byte[i] = (byte)((array_byte[i] << n | bit[(i + 1) % 2]) & 255);
                }
            }
            array_byte[array_byte.Length - 1] = (byte)(array_byte[array_byte.Length - 1] | bit[0]);
        }
    }

    class Func_transform : I_Encryption_transform
    {
        static int[,] E = {{32, 1, 2, 3, 4, 5, 4, 5 },
                   { 6, 7, 8, 9, 8, 9, 10, 11 },
                   { 12, 13, 12, 13, 14, 15, 16, 17 },
                   { 16, 17, 18, 19, 20, 21, 20, 21 },
                   { 22, 23, 24, 25, 24, 25, 26, 27 },
                   { 28, 29, 28, 29, 30, 31, 32, 1} };

        public static int[,,] S_box = {{
            { 14,  4, 13,  1,  2, 15, 11,  8,  3, 10,  6, 12,  5,  9,  0,  7},
            {  0, 15,  7,  4, 14,  2, 13,  1, 10,  6, 12, 11,  9,  5,  3,  8 },
            {  4,  1, 14,  8, 13,  6,  2, 11, 15, 12,  9,  7,  3, 10,  5,  0 },
            { 15, 12,  8,  2,  4,  9,  1,  7,  5, 11,  3, 14, 10,  0,  6, 13 } },
        {
            { 15,  1,  8, 14,  6, 11,  3,  4,  9,  7,  2, 13, 12,  0,  5, 10 },
            {  3, 13,  4,  7, 15,  2,  8, 14, 12,  0,  1, 10,  6,  9, 11,  5 },
            {  0, 14,  7, 11, 10,  4, 13,  1,  5,  8, 12,  6,  9,  3,  2, 15 },
            { 13,  8, 10,  1,  3, 15,  4,  2, 11,  6,  7, 12,  0,  5, 14,  9 } },
        {
            { 10,  0,  9, 14,  6,  3, 15,  5,  1, 13, 12,  7, 11,  4,  2,  8 },
            { 13,  7,  0,  9,  3,  4,  6, 10,  2,  8,  5, 14, 12, 11, 15,  1 },
            { 13,  6,  4,  9,  8, 15,  3,  0, 11,  1,  2, 12,  5, 10, 14,  7 },
            {  1, 10, 13,  0,  6,  9,  8,  7,  4, 15, 14,  3, 11,  5,  2, 12 } },
        {
            {  7, 13, 14,  3,  0,  6,  9, 10,  1,  2,  8,  5, 11, 12,  4, 15 },
            { 13,  8, 11,  5,  6, 15,  0,  3,  4,  7,  2, 12,  1, 10, 14,  9 },
            { 10,  6,  9,  0, 12, 11,  7, 13, 15,  1,  3, 14,  5,  2,  8,  4 },
            {  3, 15,  0,  6, 10,  1, 13,  8,  9,  4,  5, 11, 12,  7,  2, 14 } },
        {
            {  2, 12,  4,  1,  7, 10, 11,  6,  8,  5,  3, 15, 13,  0, 14,  9 },
            { 14, 11,  2, 12,  4,  7, 13,  1,  5,  0, 15, 10,  3,  9,  8,  6 },
            {  4,  2,  1, 11, 10, 13,  7,  8, 15,  9, 12,  5,  6,  3,  0, 14 },
            { 11,  8, 12,  7,  1, 14,  2, 13,  6, 15,  0,  9, 10 , 4,  5,  3 } },
        {
            { 12,  1, 10, 15,  9,  2,  6,  8,  0, 13,  3,  4, 14,  7,  5, 11 },
            { 10, 15,  4,  2,  7, 12,  9,  5,  6,  1, 13, 14,  0, 11,  3,  8 },
            {  9, 14, 15,  5,  2,  8, 12,  3,  7,  0,  4, 10,  1, 13, 11,  6 },
            {  4,  3,  2, 12,  9,  5, 15, 10, 11, 14,  1,  7,  6,  0,  8, 13 } },
        {
            {  4, 11,  2, 14, 15,  0,  8, 13,  3, 12,  9,  7,  5, 10,  6,  1 },
            { 13,  0, 11,  7,  4,  9,  1, 10, 14,  3,  5, 12,  2, 15,  8,  6 },
            {  1,  4, 11, 13, 12,  3,  7, 14, 10, 15,  6,  8,  0,  5,  9,  2 },
            {  6, 11, 13,  8,  1,  4, 10,  7,  9,  5,  0, 15, 14,  2,  3, 12 } },
        {
            { 13,  2,  8,  4,  6, 15, 11,  1, 10,  9,  3, 14,  5,  0, 12,  7 },
            {  1, 15, 13,  8, 10,  3,  7,  4, 12,  5,  6, 11,  0, 14,  9,  2 },
            {  7, 11,  4,  1,  9, 12, 14,  2,  0,  6, 10, 13, 15,  3,  5,  8 },
            {  2,  1, 14,  7,  4, 10,  8, 13, 15, 12,  9,  0,  3,  5,  6, 11 } } };

        static int[,] P = {{16, 7, 20, 21, 29, 12, 28, 17 },
                  {1, 15, 23, 26, 5, 18, 31, 10 },
                  { 2, 8, 24, 14, 32, 27, 3, 9},
                  { 19, 13, 30, 6, 22, 11, 4, 25 }};
        byte[] I_Encryption_transform.F_transform(byte[] array_byte, byte[] round_key_i)
        {

            byte[] EP_array = Algorithms_1.P_Block(array_byte, E);
            byte[] EP_XOR = Algorithms_1.XOR(EP_array, round_key_i);
            byte[] new_block = Algorithms_1.S_Block(EP_XOR, S_box);
            byte[] res = Algorithms_1.P_Block(new_block, P);

            return res;
        }

    }

    class DES
    {
        private I_Symmetric_algorithm f_DES;
        
        public DES()
        {
            f_DES = new Feistel(new Round_key(), new Func_transform());
        }

        public byte[] Encryption(byte[] array_byte, byte[] key)
        {

            byte[] res = f_DES.Encryption(array_byte, key);
            return res;
        }
        
        public byte[] Decryption(byte[] array_byte, byte[] key)
        {

            byte[] res = f_DES.Decryption(array_byte, key);
            return res;
        }
    }
}
