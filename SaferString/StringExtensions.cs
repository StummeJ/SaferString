using System;
using System.Security;

namespace SaferString
{
    /// <summary>
    /// Provides extension methods to <see cref="string"/> objects.
    /// </summary>
    [SecuritySafeCritical]
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a <see cref="String"/> to a <see cref="SecureString"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A <see cref="SecureString"/> with the value encrypted.</returns>
        /// <remarks>
        /// This does not solve all issues with having sensative in a string
        /// in the first place.  This <b>ONLY</b> reduces the attack service.
        /// As part of that the original string will be <b>ERASED</b> with
        /// Zeros.
        /// <br/><br/>
        /// <b>THIS HAS ONLY BEEN TESTED ON AnyCPU ARCHITECTURE.</b>
        /// </remarks>
        [SecuritySafeCritical]
        public static SecureString ToSecureString(this string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            unsafe
            {
                fixed (char* valuePointer = value)
                {
                    var securePassword = new SecureString(valuePointer, value.Length);
                    securePassword.MakeReadOnly();
                    Buffer.ZeroString(valuePointer, value.Length);

                    return securePassword;
                }
            }
        }

        /// <summary>
        /// Erases a <see cref="String"/>.
        /// </summary>
        /// <param name="value">The value to be erased.</param>
        [SecuritySafeCritical]
        public static void Zero(this string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            unsafe
            {
                fixed (char* valuePointer = value)
                {
                    Buffer.ZeroString(valuePointer, value.Length);
                }
            }
        }
    }
}