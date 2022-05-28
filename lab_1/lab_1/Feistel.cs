using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_1
{
    class Feistel : I_Symmetric_algorithm
    {
        I_Key_expansion round_key;
        I_Encryption_transform func_transform;

        public Feistel(I_Key_expansion rk, I_Encryption_transform func)
        {
            round_key = rk;
            func_transform = func;
        }

        public byte[] Encryption(byte[] array_byte, byte[] key)
        {
            byte[][] rk = Round_Keys(key);
            byte[] L = new byte[4], R = new byte[4];
            byte[] res = new byte[array_byte.Length];

            for (int i = 0; i < 4; i++)
            {
                L[i] = array_byte[i];
                R[i] = array_byte[i + 4];
            }

            for (int i = 0; i < rk.GetUpperBound(0)+1; i++)
            {
                var tmp = Round_encryption(L, R, rk[i]);
                if (i == rk.GetUpperBound(0))
                {
                    L = tmp;
                    break;
                }
                L = R;
                R = tmp;
            }

            for (int i = 0; i < array_byte.Length / 2; i++)
            {
                res[i] = L[i];
                res[i + 4] = R[i];
            }
            return res;
        }
        
        public byte[] Decryption(byte[] array_byte, byte[] key)
        {
            byte[][] rk = Round_Keys(key);
            byte[] L = new byte[4], R = new byte[4];
            byte[] res = new byte[array_byte.Length];

            for (int i = 0; i < 4; i++)
            {
                L[i] = array_byte[i];
                R[i] = array_byte[i + 4];
            }

            for (int i = rk.GetUpperBound(0); i >= 0; i--)
            {
                var tmp = Round_encryption(R, L, rk[i]);
                if (i == 0)
                {
                    R = tmp;
                    break;
                }
                R = L;
                L = tmp;
            }

            for (int i = 0; i < array_byte.Length / 2; i++)
            {
                res[i] = L[i];
                res[i + 4] = R[i];
            }
            return res;
        }

        public byte[] Func_transform(byte[] array_byte, byte[] round_key_i) => func_transform.F_transform(array_byte, round_key_i);
        
        public byte[][] Round_Keys(byte[] key) => round_key.Get_round_Keys(key);

        public byte[] Round_encryption(byte[] Left, byte[] Right, byte[] round_key)
        {
            return Algorithms_1.XOR(Left, Func_transform(Right, round_key));
        }

    }
}
