using System;
using System.Collections.Generic;
using System.Text;

namespace Sabatex.Extensions.Text
{
    /// <summary>
    /// Provides an encoding provider that supports the Windows-1251 (Cyrillic) code page for use with the .NET encoding
    /// infrastructure.
    /// </summary>
    /// <remarks>This provider enables applications to obtain an encoding for code page 1251 ("windows-1251"),
    /// which is commonly used for Cyrillic scripts. Register this provider with Encoding.RegisterProvider to make the
    /// encoding available through Encoding.GetEncoding methods. Only code page 1251 is supported; requests for other
    /// encodings will result in a NotImplementedException.</remarks>
    public class EncodingProvider1251 : EncodingProvider
    {
        /// <summary>
        /// Returns an encoding object for the specified code page.
        /// </summary>
        /// <param name="codepage">The code page identifier for which to obtain the encoding. Only code page 1251 is supported.</param>
        /// <returns>An <see cref="Encoding"/> object that corresponds to the specified code page.</returns>
        /// <exception cref="NotImplementedException">Thrown if <paramref name="codepage"/> is not 1251.</exception>
        public override Encoding GetEncoding(int codepage)
        {
            if (codepage == 1251)
                return new Encoding1251();
            throw new NotImplementedException();
        }
        /// <summary>
        /// Retrieves an encoding object for the specified encoding name.
        /// </summary>
        /// <remarks>Currently, only the encoding name "windows-1251" is supported. The comparison is
        /// case-insensitive.</remarks>
        /// <param name="name">The name of the encoding to retrieve. For example, "windows-1251".</param>
        /// <returns>An <see cref="Encoding"/> object that corresponds to the specified encoding name.</returns>
        /// <exception cref="NotImplementedException">Thrown if the specified encoding name is not supported.</exception>
        public override Encoding GetEncoding(string name)
        {
            if (name.ToLower() == "windows-1251")
                return new Encoding1251();

            throw new NotImplementedException();
        }
    }
}
