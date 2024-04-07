using System;

namespace Iot.Device.Lufft.Shm31.ModbusRtu
{
    /// <summary>
    /// The input register type.
    /// </summary>
    /// <remarks>
    /// The value is the start address.
    /// </remarks>
    public enum ModbusInputRegisterType : byte
    {
        /// <summary>
        /// Status Information registers.
        /// </summary>
        /// <remarks> 0..19 </remarks>
        StatusInformation = 0,
        /// <summary>
        /// Standard data registers (metric units).
        /// </summary>
        /// <remarks> 20..29 </remarks>
        StandardMetric = 20,
        /// <summary>
        /// Standard data set registers (imperial units).
        /// </summary>
        /// <remarks> 30..39 </remarks>
        StandardImperial = 30,
        /// <summary>
        /// Distance registers.
        /// </summary>
        /// <remarks> 40..54 </remarks>
        Distance = 40,
        /// <summary>
        /// Temperature registers (metric units).
        /// </summary>
        /// <remarks> 55..69 </remarks>
        TemperaturesMetric = 55,
        /// <summary>
        /// Temperature registers (imperial units).
        /// </summary>
        /// <remarks> 70..84 </remarks>
        TemperaturesImperial = 70,
        /// <summary>
        /// Angles registers.
        /// </summary>
        /// <remarks> 85..94 </remarks>
        Angles = 85,
        /// <summary>
        /// Logic and normalized values registers.
        /// </summary>
        /// <remarks> 95..104 </remarks>
        LogicAndNormalizedValues = 90,
        /// <summary>
        /// Service Channel registers.
        /// </summary>
        /// <remarks> 105..119 </remarks>
        ServiceChannels = 105,
        /// <summary>
        /// The Register type was not recognised or above the allowed value.
        /// </summary>
        Unknown = 120
    }
}
