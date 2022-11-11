// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CryptographyExtensions.cs" company="None">
//      This project is licensed under the Apache License 2.0.
// </copyright>
// <author> Piotr Knycpel </author>
// <creationDate> 2022-11-06 09:14 </creationDate>
// <summary>
//      Defines the CryptographyExtensions type to provides a implementation of method to encrypt or decrypt data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Func;

using System;
using System.Security.Cryptography;
using System.Text;
using JetBrains.Annotations;

/// <summary>
///     The CryptographyExtensions class to provides a implementation of method to encrypt or decrypt data.
/// </summary>
[PublicAPI]
public static class CryptographyExtensions
{
    /// <summary>
    ///     Decrypts a byte array using the supplied key. Decoding is done using TripleDES encryption.
    /// </summary>
    /// <param name="encrypted"> The array of unsigned bytes for decrypt. </param>
    /// <param name="key"> The secret key to use for the symmetric algorithm. </param>
    /// <param name="vector"> The initialization vector to use for the symmetric algorithm. </param>
    /// <returns> The decrypted string or null if decryption failed. </returns>
    /// <exception cref="ArgumentNullException"> The exception that is thrown when an <paramref name="encrypted" /> array is null or empty. </exception>
    /// <exception cref="CryptographicException"> The exception that is thrown when an error occurs during a cryptographic operation. </exception>
    public static string TripleDesDecryptToText(this byte[] encrypted, byte[] key, byte[] vector)
    {
        encrypted.ThrowIfNullOrEmpty(nameof(encrypted));
        key.ThrowIfNullOrEmpty(nameof(key));
        vector.ThrowIfNullOrEmpty(nameof(vector));

        var decrypted = new byte[encrypted.Length];
        var offset = 0;

        try
        {
            using var memoryStream = new MemoryStream(encrypted);
            using var tripleDes = TripleDES.Create();
            using var cryptoTransform = tripleDes.CreateDecryptor(key, vector);
            using var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);
            var read = 1;
            while (read > 0)
            {
                read = cryptoStream.Read(decrypted, offset, decrypted.Length - offset);
                offset += read;
            }

            return Encoding.UTF8.GetString(decrypted, 0, offset);
        }
        catch (CryptographicException e)
        {
            Console.WriteLine("The cryptographic error occurred: {0}", e.Message);
            throw;
        }
    }

    /// <summary>
    ///     Decrypts a byte array using the supplied key. Decoding is done using TripleDES encryption.
    /// </summary>
    /// <param name="textToDecrypt"> Text that must be decrypted. </param>
    /// <param name="key"> The secret key to use for the symmetric algorithm. </param>
    /// <param name="vector"> The initialization vector to use for the symmetric algorithm. </param>
    /// <returns> The decrypted string or null if decryption failed. </returns>
    /// <exception cref="ArgumentNullException"> The exception that is thrown when an <paramref name="textToDecrypt" /> is null or empty. </exception>
    /// <exception cref="CryptographicException"> The exception that is thrown when an error occurs during a cryptographic operation. </exception>
    public static string TripleDesDecryptToText(this string textToDecrypt, byte[] key, byte[] vector)
    {
        textToDecrypt.ThrowIfNullOrEmpty(nameof(textToDecrypt));
        var toDecrypt = Encoding.UTF8.GetBytes(textToDecrypt);
        return TripleDesDecryptToText(toDecrypt, key, vector);
    }

    /// <summary>
    ///     Encrypt a string using the supplied key. Encoding is done using the TripleDES algorithm.
    /// </summary>
    /// <param name="textToEncrypt"> Text that must be encrypted. </param>
    /// <param name="key"> The secret key to use for the symmetric algorithm. </param>
    /// <param name="vector"> The initialization vector to use for the symmetric algorithm. </param>
    /// <returns> A string representing a byte array of encrypted value. </returns>
    /// <exception cref="ArgumentNullException"> The exception that is thrown when an <paramref name="textToEncrypt" /> is null or empty. </exception>
    /// <exception cref="CryptographicException"> The exception that is thrown when an error occurs during a cryptographic operation. </exception>
    public static byte[] TripleDesEncryptToText(this string textToEncrypt, byte[] key, byte[] vector)
    {
        textToEncrypt.ThrowIfNullOrEmpty(nameof(textToEncrypt));
        key.ThrowIfNullOrEmpty(nameof(key));
        vector.ThrowIfNullOrEmpty(nameof(vector));

        try
        {
            using var stream = new MemoryStream();
            using var tripleDes = TripleDES.Create();
            using var cryptoTransform = tripleDes.CreateEncryptor(key, vector);
            using var cryptoStream = new CryptoStream(stream, cryptoTransform, CryptoStreamMode.Write);
            {
                var toEncrypt = Encoding.UTF8.GetBytes(textToEncrypt);
                cryptoStream.Write(toEncrypt, 0, toEncrypt.Length);
            }

            return stream.ToArray();
        }
        catch (CryptographicException e)
        {
            Console.WriteLine("The cryptographic error occurred: {0}", e.Message);
            throw;
        }
    }

    /// <summary>
    ///     Encrypt a string using the supplied key. Encoding is done using the AES (Advanced Encryption Standard) algorithm.
    /// </summary>
    /// <param name="textToEncrypt"> Text that must be encrypted. </param>
    /// <param name="key"> The secret key to use for the symmetric algorithm. </param>
    /// <param name="vector"> The initialization vector to use for the symmetric algorithm. </param>
    /// <returns> A string representing a byte array of encrypted value. </returns>
    /// <exception cref="ArgumentNullException"> The exception that is thrown when an <paramref name="textToEncrypt" /> is null or empty. </exception>
    /// <exception cref="CryptographicException"> The exception that is thrown when an error occurs during a cryptographic operation. </exception>
    public static byte[] AesEncryptToText(this string textToEncrypt, byte[] key, byte[] vector)
    {
        textToEncrypt.ThrowIfNullOrEmpty(nameof(textToEncrypt));
        key.ThrowIfNullOrEmpty(nameof(key));
        vector.ThrowIfNullOrEmpty(nameof(vector));

        try
        {
            using var aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.IV = vector;

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(textToEncrypt);
            }

            return msEncrypt.ToArray();
        }
        catch (CryptographicException e)
        {
            Console.WriteLine("The cryptographic error occurred: {0}", e.Message);
            throw;
        }
    }

    private static void ThrowIfNullOrEmpty(this byte[] value, string? paramName)
    {
        if (value is not { Length: > 0 })
        {
            throw new ArgumentNullException(paramName, $"The array value can't be null or empty. (Parameter name: '{paramName ?? string.Empty}').");
        }
    }

    private static void ThrowIfNullOrEmpty(this string? text, string? paramName)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentNullException(paramName, $"The string value can't be null or empty. (Parameter name: '{paramName ?? string.Empty}').");
        }
    }

    private static void ThrowIfNull(this object? obj, string? paramName)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(paramName, $"The value can't be null. (Parameter name: '{paramName ?? string.Empty}').");
        }
    }
}