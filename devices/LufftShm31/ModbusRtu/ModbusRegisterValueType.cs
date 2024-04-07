using System;

namespace Iot.Device.Lufft.Shm31.ModbusRtu
{
    /// <summary>
    /// The value type of the <see cref="ModbusInputRegisterAddress"/>.
    /// </summary>
    public enum ModbusRegisterValueType : byte
    {
        /// <summary>
        /// The value is an unsigned short (uint16)
        /// </summary>
        UnsignedShort,
        /// <summary>
        /// The value is a signed short (int16)
        /// </summary>
        /// <remarks>
        /// Might need conversion.
        /// </remarks>
        SignedShort,
        /// <summary>
        /// The value is the upper half of a Unt32
        /// </summary>
        /// <remarks> received as an unsigned short (uint16) </remarks>
        PartalUIntUpper16,
        /// <summary>
        /// The value is the lower half of a Unt32
        /// </summary>
        /// <remarks> received as an unsigned short (uint16) </remarks>
        PartialUIntLower16,
        /// <summary>
        /// The value type was unknown or above the allowed value.
        /// </summary>
        Unknown = 0xFF
    }
}
