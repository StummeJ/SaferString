using System;
using System.Security;

namespace SaferString
{
    [SecurityCritical]
    internal static class Buffer
    {
        internal const int Int32Size = sizeof(Int32) / 2;

        [SecurityCritical]
        internal static unsafe void ZeroString(char* ptr, int length)
        {
            ZeroMemory(ptr, length);
            ZeroMemory(ptr - Int32Size, Int32Size);
        }

        [SecurityCritical]
        internal static unsafe void ZeroMemory(char* dmem, int charCount)
        {
            ZeroMemory((byte*)dmem, charCount * 2);
        }

        [SecurityCritical]
        internal static unsafe void ZeroMemory(byte* src, long len)
        {
            while (len-- > 0)
                *(src + len) = 0;
        }
    }
}