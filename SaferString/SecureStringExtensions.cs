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
        /// a controlled manor and return a derived value.
        /// </summary>
        /// <param name="secureValue">The value to act upon.</param>
        /// <param name="lambda">The delegate to execute.  The first argument must be a <see cref="String"/>/.</param>
        /// <param name="args">Arguments for the delegate.</param>
        /// <returns>The <see cref="Lambda{T}"/> returned from the delegate.</returns>
        /// <remarks>
        /// This does not prevent you from extracting the unencrypted string.  This is 
        /// meant to allow executing code based on the unencrypted string but not copy
        /// the unencrypted string.
        /// 
        /// Thing such as string format, string concatenation, and string builders
        /// are a few things that can and will leak the unencrypted string.
        /// 
        /// <b>The original unencrypted string will be erased after the delegate has
        /// been executed.  Any direct reference to it will not carry over outside
        /// of this method.</b>
        /// </remarks>
        [SecuritySafeCritical]
        public static T Lambda<T>(this SecureString secureValue, Delegate lambda, params object[] args)
        {
            var handle = default(GCHandle);
            try
            {
                handle = GCHandle.Alloc(secureValue.ToUnsecureString(), GCHandleType.Pinned);
                var results = (T) lambda.DynamicInvoke(BuildLambdaArgs(handle, args));
                return results;
            }
            finally
            {
                if (handle != default(GCHandle))
                {
                    (handle.Target as String)?.Zero();
                    handle.Free();
                }
            }
        }

        /// <summary>
        /// Allows executing code against a <see cref="SecurityElement"/> in
        /// a controlled manor and return a derived value.
        /// </summary>
        /// <param name="secureValue">The value to act apron.</param>
        /// <param name="lambda">The delegate to execute.  The first argument must be a <see cref="String"/>/.</param>
        /// <param name="args">Arguments for the delegate.</param>
        /// <remarks>
        /// This does not prevent you from extracting the unencrypted string.  This is 
        /// meant to allow executing code based on the unencrypted string but not copy
        /// the unencrypted string.
        /// 
        /// Thing such as string format, string concatenation, and string builders
        /// are a few things that can and will leak the unencrypted string.
        /// 
        /// <b>The original unencrypted string will be erased after the delegate has
        /// been executed.  Any direct reference to it will not carry over outside
        /// of this method.</b>
        /// </remarks>
        [SecuritySafeCritical]
        public static void Lambda(this SecureString secureValue, Delegate lambda, params object[] args)
        {
            var handle = default(GCHandle);
            try
            {
                handle = GCHandle.Alloc(secureValue.ToUnsecureString(), GCHandleType.Pinned);
                lambda.DynamicInvoke(BuildLambdaArgs(handle, args));
            }
            finally
            {
                if (handle != default(GCHandle))
                {
                    (handle.Target as String)?.Zero();
                    handle.Free();
                }
            }
        }

        [SecurityCritical]
        private static object[] BuildLambdaArgs(GCHandle handle, params object[] args)
        {
            var lambdaArgs = new object[args.Length + 1];
            lambdaArgs[0] = handle.Target as String;
            Array.Copy(args, 0, lambdaArgs, 1, args.Length);
            return lambdaArgs;
        }
    }
}