using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_1
{
    interface I_Key_expansion
    {
        public byte[][] Get_round_Keys(byte[] key);
        public void Shift_Left(byte[] array_byte, int n);
    }

    interface I_Encryption_transform
    {
        byte[] F_transform(byte[] array_byte, byte[] round_key_i);
    }

    interface I_Symmetric_algorithm
    {
        byte[] Encryption(byte[] array_byte, byte[] key);
        
        byte[] Decryption(byte[] array_byte, byte[] key);

        byte[] Round_encryption(byte[] Left, byte[] Right, byte[] round_key);

        byte[] Func_transform(byte[] array_byte, byte[] round_key);

        byte[][] Round_Keys(byte[] key);
        
    }
}
