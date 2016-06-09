using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SaferString
{
    /// <summary>
    /// Provides extension methods to <see cref="SecureString"/> objects.
    /// </summary>
    [SecuritySafeCritical]
    public static class SecureStringExtensions
    {
        /// <summary>
        /// Converts a <see cref="SecureString"/> to a <see cref="String"/>.
        /// </summary>
        /// <param name="secureValue">The value to decrypt.</param>
        /// <returns>The unencrypted value.</returns>
        [SecuritySafeCritical]
        public static String ToUnsecureString(this SecureString secureValue)
        {
            if (secureValue == null)
                throw new ArgumentNullException(nameof(secureValue));

            var unmanagedStringPtr = IntPtr.Zero;
            try
            {
                unmanagedStringPtr = Marshal.SecureStringToGlobalAllocUnicode(secureValue);
                return Marshal.PtrToStringUni(unmanagedStringPtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedStringPtr);
            }
        }

        /// <summary>
        /// Allows executing code against a <see cref="SecurityElement"/> in
        /// a controlled mannor and return a derived value.
        /// </summary>
        /// <param name="secureValue">The value to act apon.</param>
        /// <param name="lambda">The delegate to execute.</param>
        /// <returns>The <seealso cref="T"/> returned from the delegate.</returns>
        /// <remarks>
        /// This does not prevent you from extracting the unecrypted string.  This is 
        /// ment to allow executing code based on the unencrypted string but not copy
        /// the unencrypted string.
        /// 
        /// Thing such as string format, string concatenation, and string buildars
        /// are a few things that can and will leak the unencrypted string.
        /// 
        /// <b>The original unencrypted string will be erased after the delegate has
        /// been executed.  Any direct reference to it will not carry over outside
        /// of this method.</b>
        /// </remarks>
        [SecuritySafeCritical]
        public static T Lambda<T>(this SecureString secureValue, Func<String, T> lambda)
        {
            var handle = default(GCHandle);
            try
            {
                handle = GCHandle.Alloc(secureValue.ToUnsecureString(), GCHandleType.Pinned);
                var results = lambda(handle.Target as String);
                return results;
            }
            finally
            {
                if (handle != default(GCHandle))
                {
                    (handle.Target as String)?.EraseString();
                    handle.Free();
                }
            }
        }

        /// <summary>
        /// Allows executing code against a <see cref="SecurityElement"/> in
        /// a controlled mannor and return a derived value.
        /// </summary>
        /// <param name="secureValue">The value to act apon.</param>
        /// <param name="lambda">The delegate to execute.</param>
        /// <remarks>
        /// This does not prevent you from extracting the unecrypted string.  This is 
        /// ment to allow executing code based on the unencrypted string but not copy
        /// the unencrypted string.
        /// 
        /// Thing such as string format, string concatenation, and string buildars
        /// are a few things that can and will leak the unencrypted string.
        /// 
        /// <b>The original unencrypted string will be erased after the delegate has
        /// been executed.  Any direct reference to it will not carry over outside
        /// of this method.</b>
        /// </remarks>
        [SecuritySafeCritical]
        public static void Lambda(this SecureString secureValue, Action<String> lambda)
        {
            var handle = default(GCHandle);
            try
            {
                handle = GCHandle.Alloc(secureValue.ToUnsecureString(), GCHandleType.Pinned);
                lambda(handle.Target as String);
            }
            finally
            {
                if (handle != default(GCHandle))
                {
                    (handle.Target as String)?.EraseString();
                    handle.Free();
                }
            }
        }
    }
}