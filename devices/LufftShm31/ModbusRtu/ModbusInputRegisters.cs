using System;
using System.Collections;

namespace Iot.Device.Lufft.Shm31.ModbusRtu
{

    public class ModbusInputRegisterTypes
    {
        /// <summary>
        /// The input register type <see cref="ModbusInputRegisterType"/>.
        /// </summary>
        public ModbusInputRegisterType RegisterType { get; set; } = ModbusInputRegisterType.Unknown;

        /// <summary>
        /// The start address of the register type.
        /// </summary>
        public static ushort GetStartAddress(ModbusInputRegisterType registerType)
        {
            // FIXME: could probably be simplified to `return (ushort)registerType;`
            switch (registerType)
            {
                case ModbusInputRegisterType.StatusInformation:
                    return 0;
                case ModbusInputRegisterType.StandardMetric:
                    return 20;
                case ModbusInputRegisterType.StandardImperial:
                    return 30;
                case ModbusInputRegisterType.Distance:
                    return 40;
                case ModbusInputRegisterType.TemperaturesMetric:
                    return 55;
                case ModbusInputRegisterType.TemperaturesImperial:
                    return 70;
                case ModbusInputRegisterType.Angles:
                    return 85;
                case ModbusInputRegisterType.LogicAndNormalizedValues:
                    return 95;
                case ModbusInputRegisterType.ServiceChannels:
                    return 105;
                case ModbusInputRegisterType.Unknown:
                    break;
                default:
                    break;
            }
            return ushort.MaxValue;
        }

        /// <summary>
        /// The end address of the register type.
        /// </summary>
        public static ushort GetEndAddress(ModbusInputRegisterType registerType)
        {
            switch (registerType)
            {
                case ModbusInputRegisterType.StatusInformation:
                    return 19;
                case ModbusInputRegisterType.StandardMetric:
                    return 29;
                case ModbusInputRegisterType.StandardImperial:
                    return 39;
                case ModbusInputRegisterType.Distance:
                    return 54;
                case ModbusInputRegisterType.TemperaturesMetric:
                    return 69;
                case ModbusInputRegisterType.TemperaturesImperial:
                    return 84;
                case ModbusInputRegisterType.Angles:
                    return 94;
                case ModbusInputRegisterType.LogicAndNormalizedValues:
                    return 104;
                case ModbusInputRegisterType.ServiceChannels:
                    return 119;
                case ModbusInputRegisterType.Unknown:
                    break;
                default:
                    break;
            }
            return ushort.MaxValue;
        }
    }

    /// <summary>
    /// The valid range of a register value.
    /// </summary>
    public class ModbusRegisterValueRange
    {
        // requires int as the value might be short or ushort, but by default should use ushort.
        public int MinimumValue { get; set; } = ushort.MinValue;
        public int MaximumValue { get; set; } = ushort.MaxValue;

    }

    public class ModbusInputRegister
    {
        /// <summary>
        /// The address of the register.
        /// </summary>
        public ModbusInputRegisterAddress RegisterAddress { get; set; } = 0;
        /// <summary>
        /// The Register Type.
        /// </summary>
        public ModbusInputRegisterTypes RegisterType { get; set; } = new() { RegisterType = ModbusInputRegisterType.Unknown};
        /// <summary>
        /// The Register Value Type.
        /// </summary>
        public ModbusRegisterValueType ValueType { get; set; } = ModbusRegisterValueType.UnsignedShort;
        /// <summary>
        /// The scale factor of the registers value.
        /// </summary>
        public float ValueScaleFactor { get; set; } = 0.0f;
        /// <summary>
        /// The allowed range of the registers value.
        /// </summary>
        public ModbusRegisterValueRange ValueRange { get; set; } = new ModbusRegisterValueRange();
        // public int ValueUnitsType { get; set; } // TODO: add units (mm, inch, etc.). (Units.Net)

    }

    public class ModbusInputValue
    {
        public short RawValue { get; private set; } = short.MaxValue;
        public float AdjustedValue { get; private set; } = float.NaN;

        public ModbusInputValue(ushort regAddress, short rawValue)
        {
            RawValue = rawValue;
            // TODO: convert the raw value to adjusted value using InputAddress hashtable.
            //  make sure the value range, scale factor and type are used during the conversion.
        }

        public static float AdjustValue_ScaleFactor(short regValue, short scaleFactor)
        {
            return regValue / scaleFactor;
        }

        // TODO: convert short to ushort for most regs.
    }

    public class ModbusInputRegisters
    {
        /// <summary>
        /// The maximum input register address available.
        /// </summary>
        public const ushort REG_ADDRESS_MAX = 119;

        public Hashtable InputRegisters = new();

        public ModbusInputRegisters()
        {
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SI_DeviceIdentification,
                new ModbusInputRegister {
                    RegisterAddress = ModbusInputRegisterAddress.SI_DeviceIdentification,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StatusInformation },
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SI_DeviceStatusLower,
                new ModbusInputRegister {
                    RegisterAddress = ModbusInputRegisterAddress.SI_DeviceStatusLower,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StatusInformation },
                    ValueType = ModbusRegisterValueType.PartialUIntLower16
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SI_DeviceStatusUpper,
                new ModbusInputRegister {
                    RegisterAddress = ModbusInputRegisterAddress.SI_DeviceStatusUpper,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StatusInformation },
                    ValueType = ModbusRegisterValueType.PartalUIntUpper16
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SI_BlockHeatingState,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SI_BlockHeatingState,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StatusInformation },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = (int)HeatingModeState.UnknownError - 1 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SI_WindowHeatingState,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SI_WindowHeatingState,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StatusInformation },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = (int)HeatingModeState.UnknownError - 1 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SI_BlockTemperatureStatus,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SI_BlockTemperatureStatus,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StatusInformation },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = (int)StatusCode.UnknownError - 1 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SI_AmbientTemperatureStatus,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SI_AmbientTemperatureStatus,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StatusInformation },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = (int)StatusCode.UnknownError - 1 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SI_LaserTemperatureStatus,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SI_LaserTemperatureStatus,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StatusInformation },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = (int)StatusCode.UnknownError - 1 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SI_TiltAngleStatus,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SI_TiltAngleStatus,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StatusInformation },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = (int)StatusCode.UnknownError - 1 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SI_SnowHeightStatus,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SI_SnowHeightStatus,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StatusInformation },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = (int)StatusCode.UnknownError - 1 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SI_DistanceStatus,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SI_DistanceStatus,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StatusInformation },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = (int)StatusCode.UnknownError - 1 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SI_NormalizedSignalStatus,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SI_NormalizedSignalStatus,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StatusInformation },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = (int)StatusCode.UnknownError - 1 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SI_ErrorCode,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SI_ErrorCode,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StatusInformation },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = (int)DeviceErrorCode.UnknownError - 1 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SI_ErrorCode_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SI_ErrorCode_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StatusInformation },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = (int)DeviceErrorCode.UnknownError - 1 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SI_AccumulatedOpperatingTimeLower,
                 new ModbusInputRegister
                 {
                     RegisterAddress = ModbusInputRegisterAddress.SI_AccumulatedOpperatingTimeLower,
                     RegisterType = new() { RegisterType = ModbusInputRegisterType.StatusInformation },
                     ValueType = ModbusRegisterValueType.PartialUIntLower16
                 });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SI_AccumulatedOpperatingTimeUpper,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SI_AccumulatedOpperatingTimeUpper,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StatusInformation },
                    ValueType = ModbusRegisterValueType.PartalUIntUpper16
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SI_SystemTimeLower,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SI_SystemTimeLower,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StatusInformation },
                    ValueType = ModbusRegisterValueType.PartialUIntLower16
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SI_SystemTimeUpper,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SI_SystemTimeUpper,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StatusInformation },
                    ValueType = ModbusRegisterValueType.PartalUIntUpper16
                });
            // Standard Data Metric
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SDM_SnowHeightMillimeter_Current,
                new ModbusInputRegister {
                    RegisterAddress = ModbusInputRegisterAddress.SDM_SnowHeightMillimeter_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StandardMetric },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueRange = new() { MinimumValue = -16000, MaximumValue = 16000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SDM_BlockTemperatureDegC_Current,
                new ModbusInputRegister {
                    RegisterAddress = ModbusInputRegisterAddress.SDM_BlockTemperatureDegC_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StandardMetric },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -400, MaximumValue = 1000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SDM_AmbientTemperatureDegC_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SDM_AmbientTemperatureDegC_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StandardMetric },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -500, MaximumValue = 1000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SDM_LaserTemperatureDegC_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SDM_LaserTemperatureDegC_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StandardMetric },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -600, MaximumValue = 800 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SDM_NormalizedSignal_Metric,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SDM_NormalizedSignal_Metric,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StandardMetric },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = 255 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SDM_TiltAngle_Metric_Current,
                new ModbusInputRegister {
                    RegisterAddress = ModbusInputRegisterAddress.SDM_TiltAngle_Metric_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StandardMetric },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -1800, MaximumValue = 1800 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SDM_ErrorCode_Metric,
                new ModbusInputRegister {
                    RegisterAddress = ModbusInputRegisterAddress.SDM_ErrorCode_Metric,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StandardMetric },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = 255 }
                });
            // Standard Data Imperial
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SDI_SnowHeightInches_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SDI_SnowHeightInches_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StandardImperial },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 20,
                    ValueRange = new() { MinimumValue = -12598, MaximumValue = 12598 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SDI_BlockTemperatureDegF_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SDI_BlockTemperatureDegF_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StandardImperial },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -400, MaximumValue = 2120 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SDI_AmbientTemperatureDegF_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SDI_AmbientTemperatureDegF_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StandardImperial },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -580, MaximumValue = 2120 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SDI_LaserTemperatureDegF_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SDI_LaserTemperatureDegF_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StandardImperial },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -760, MaximumValue = 1760 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SDI_NormalizedSignal_Imperial,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SDI_NormalizedSignal_Imperial,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StandardImperial },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = 255 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SDI_TiltAngle_Imperial_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SDI_TiltAngle_Imperial_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StandardImperial },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -1800, MaximumValue = 1800 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SDI_ErrorCode_Imperial,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SDI_ErrorCode_Imperial,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.StandardImperial },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = 255 }
                });
            // Distances
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.D_SnowHeight_Millimeter_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.D_SnowHeight_Millimeter_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Distance },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueRange = new() { MinimumValue = -16000, MaximumValue = 16000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.D_SnowHeight_Millimeter_Minimum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.D_SnowHeight_Millimeter_Minimum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Distance },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueRange = new() { MinimumValue = -16000, MaximumValue = 16000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.D_SnowHeight_Millimeter_Maximum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.D_SnowHeight_Millimeter_Maximum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Distance },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueRange = new() { MinimumValue = -16000, MaximumValue = 16000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.D_SnowHeight_Millimeter_Average,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.D_SnowHeight_Millimeter_Average,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Distance },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueRange = new() { MinimumValue = -16000, MaximumValue = 16000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.D_Calibrated_Millimeter_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.D_Calibrated_Millimeter_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Distance },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueRange = new() { MinimumValue = -500, MaximumValue = 21000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.D_Raw_Millimeter_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.D_Raw_Millimeter_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Distance },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueRange = new() { MinimumValue = -500, MaximumValue = 21000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.D_SnowHeight_Inches_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.D_SnowHeight_Inches_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Distance },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 20,
                    ValueRange = new() { MinimumValue = -12598, MaximumValue = 12598 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.D_SnowHeight_Inches_Minimum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.D_SnowHeight_Inches_Minimum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Distance },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 20,
                    ValueRange = new() { MinimumValue = -12598, MaximumValue = 12598 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.D_SnowHeight_Inches_Maximum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.D_SnowHeight_Inches_Maximum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Distance },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 20,
                    ValueRange = new() { MinimumValue = -12598, MaximumValue = 12598 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.D_SnowHeight_Inches_Average,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.D_SnowHeight_Inches_Average,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Distance },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 20,
                    ValueRange = new() { MinimumValue = -12598, MaximumValue = 12598 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.D_Calibrated_Inches_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.D_Calibrated_Inches_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Distance },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 20,
                    ValueRange = new() { MinimumValue = -394, MaximumValue = 16536 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.D_Raw_Inches_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.D_Raw_Inches_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Distance },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 20,
                    ValueRange = new() { MinimumValue = -394, MaximumValue = 16536 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.D_ReferenceHeight_millimeter,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.D_ReferenceHeight_millimeter,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Distance },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueRange = new() { MinimumValue = 0, MaximumValue = 16000 } // FIXME: min value seems strange given the type.
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.D_SnowHeight_millimeter_HighRes,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.D_SnowHeight_millimeter_HighRes,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Distance },
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = 0, MaximumValue = 64000 }
                });
            // Temperatures Metric
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TM_BlockTemperatureDegC_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TM_BlockTemperatureDegC_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesMetric },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -400, MaximumValue = 1000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TM_BlockTemperatureDegC_Minimum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TM_BlockTemperatureDegC_Minimum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesMetric },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -400, MaximumValue = 1000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TM_BlockTemperatureDegC_Maximum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TM_BlockTemperatureDegC_Maximum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesMetric },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -400, MaximumValue = 1000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TM_BlockTemperatureDegC_Average,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TM_BlockTemperatureDegC_Average,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesMetric },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -400, MaximumValue = 1000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TM_AmbientTemperatureDegC_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TM_AmbientTemperatureDegC_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesMetric },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -500, MaximumValue = 1000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TM_AmbientTemperatureDegC_Minimum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TM_AmbientTemperatureDegC_Minimum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesMetric },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -500, MaximumValue = 1000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TM_AmbientTemperatureDegC_Maximum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TM_AmbientTemperatureDegC_Maximum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesMetric },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -500, MaximumValue = 1000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TM_AmbientTemperatureDegC_Average,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TM_AmbientTemperatureDegC_Average,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesMetric },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -500, MaximumValue = 1000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TM_LaserTemperatureDegC_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TM_LaserTemperatureDegC_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesMetric },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -600, MaximumValue = 800 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TM_LaserTemperatureDegC_Minimum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TM_LaserTemperatureDegC_Minimum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesMetric },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -600, MaximumValue = 800 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TM_LaserTemperatureDegC_Maximum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TM_LaserTemperatureDegC_Maximum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesMetric },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -600, MaximumValue = 800 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TM_LaserTemperatureDegC_Average,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TM_LaserTemperatureDegC_Average,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesMetric },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -600, MaximumValue = 800 }
                });
            // Temperatures Imperial
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TI_BlockTemperatureDegF_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TI_BlockTemperatureDegF_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesImperial },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -400, MaximumValue = 2120 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TI_BlockTemperatureDegF_Minimum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TI_BlockTemperatureDegF_Minimum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesImperial },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -400, MaximumValue = 2120 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TI_BlockTemperatureDegF_Maximum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TI_BlockTemperatureDegF_Maximum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesImperial },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -400, MaximumValue = 2120 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TI_BlockTemperatureDegF_Average,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TI_BlockTemperatureDegF_Average,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesImperial },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -400, MaximumValue = 2120 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TI_AmbientTemperatureDegF_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TI_AmbientTemperatureDegF_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesImperial },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -580, MaximumValue = 2120 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TI_AmbientTemperatureDegF_Minimum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TI_AmbientTemperatureDegF_Minimum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesImperial },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -580, MaximumValue = 2120 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TI_AmbientTemperatureDegF_Maximum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TI_AmbientTemperatureDegF_Maximum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesImperial },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -580, MaximumValue = 2120 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TI_AmbientTemperatureDegF_Average,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TI_AmbientTemperatureDegF_Average,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesImperial },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -580, MaximumValue = 2120 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TI_LaserTemperatureDegF_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TI_LaserTemperatureDegF_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesImperial },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -760, MaximumValue = 1760 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TI_LaserTemperatureDegF_Minimum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TI_LaserTemperatureDegF_Minimum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesImperial },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -760, MaximumValue = 1760 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TI_LaserTemperatureDegF_Maximum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TM_LaserTemperatureDegC_Maximum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesImperial },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -760, MaximumValue = 1760 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.TI_LaserTemperatureDegF_Average,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.TI_LaserTemperatureDegF_Average,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.TemperaturesImperial },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -760, MaximumValue = 1760 }
                });
            // Angles
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.A_AngleTilt_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.A_AngleTilt_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Angles },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -1800, MaximumValue = 1800 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.A_AngleTilt_Minimum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.A_AngleTilt_Minimum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Angles },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -1800, MaximumValue = 1800 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.A_AngleTilt_Maximum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.A_AngleTilt_Maximum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Angles },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -1800, MaximumValue = 1800 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.A_AngleTilt_Average,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.A_AngleTilt_Average,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Angles },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -1800, MaximumValue = 1800 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.A_AngleX_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.A_AngleX_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Angles },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -1800, MaximumValue = 1800 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.A_AngleY_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.A_AngleY_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Angles },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -1800, MaximumValue = 1800 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.A_AngleZ_Current,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.A_AngleZ_Current,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Angles },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -1800, MaximumValue = 1800 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.A_TiltAngleReference,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.A_TiltAngleReference,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.Angles },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -1800, MaximumValue = 1800 }
                });
            // Logic and normalized Values
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.LNV_SnowFlag,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.LNV_SnowFlag,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.LogicAndNormalizedValues },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = 1 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.LNV_NormalizedSignal_Current,
                new ModbusInputRegister
                {
                RegisterAddress = ModbusInputRegisterAddress.LNV_NormalizedSignal_Current,
                RegisterType = new() { RegisterType = ModbusInputRegisterType.LogicAndNormalizedValues },
                ValueRange = new() { MinimumValue = 0, MaximumValue = 255 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.LNV_NormalizedSignal_Minimum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.LNV_NormalizedSignal_Minimum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.LogicAndNormalizedValues },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = 255 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.LNV_NormalizedSignal_Maximum,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.LNV_NormalizedSignal_Maximum,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.LogicAndNormalizedValues },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = 255 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.LNV_NormalizedSignal_Average,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.LNV_NormalizedSignal_Average,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.LogicAndNormalizedValues },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = 255 }
                });
            // Service channels
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SC_BlockHeatingState,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SC_BlockHeatingState,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.ServiceChannels },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = (int)Shm31.HeatingModeState.UnknownError - 1 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SC_InternalTemperatureDegC_NTC,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SC_InternalTemperatureDegC_NTC,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.ServiceChannels },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -400, MaximumValue = 1000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SC_BlockHeatingDefrostTime_Seconds,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SC_BlockHeatingDefrostTime_Seconds,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.ServiceChannels },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = 65535 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SC_WindowHeatingState,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SC_WindowHeatingState,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.ServiceChannels },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = (int)Shm31.HeatingModeState.UnknownError - 1 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SC_ExternalTemperatureDegC_NTC,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SC_ExternalTemperatureDegC_NTC,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.ServiceChannels },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -500, MaximumValue = 1000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SC_WindowHeatingDefrostTime_Seconds,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SC_WindowHeatingDefrostTime_Seconds,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.ServiceChannels },
                    ValueRange = new() { MinimumValue = 0, MaximumValue = 65535 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SC_LaserGainCode,
                new ModbusInputRegister
                {
                RegisterAddress = ModbusInputRegisterAddress.SC_LaserGainCode,
                RegisterType = new() { RegisterType = ModbusInputRegisterType.ServiceChannels },
                ValueRange = new() { MinimumValue = 0, MaximumValue = 255 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SC_LaserSignalIntensity_uV,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SC_LaserSignalIntensity_uV,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.ServiceChannels },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 0.1f
                    // FIXME: This register is not implicitly clear on values in the manual!
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SC_LaserDistance_Millimeter,
                new ModbusInputRegister
                {
                RegisterAddress = ModbusInputRegisterAddress.SC_LaserDistance_Millimeter,
                RegisterType = new() { RegisterType = ModbusInputRegisterType.ServiceChannels },
                ValueRange = new() { MinimumValue = 0, MaximumValue = 32000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SC_LaserTemperatureDegC,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SC_LaserTemperatureDegC,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.ServiceChannels },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -600, MaximumValue = 8000 }
                });
            InputRegisters.Add((ushort)ModbusInputRegisterAddress.SC_OperatingVoltage,
                new ModbusInputRegister
                {
                    RegisterAddress = ModbusInputRegisterAddress.SC_OperatingVoltage,
                    RegisterType = new() { RegisterType = ModbusInputRegisterType.ServiceChannels },
                    ValueType = ModbusRegisterValueType.SignedShort,
                    ValueScaleFactor = 10,
                    ValueRange = new() { MinimumValue = -400, MaximumValue = 400 }
                });
        }
    }
}
