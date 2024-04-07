using System;

namespace Iot.Device.Lufft.Shm31.ModbusRtu
{
    /// <summary>
    /// The input register address.
    /// </summary>
    public enum ModbusInputRegisterAddress : byte
    {
        // Status Information registers
        SI_DeviceIdentification = 0, // value = High byte: device subtype, Low byte: software version
        SI_DeviceStatusLower = 1,
        SI_DeviceStatusUpper = 2,
        SI_BlockHeatingState = 3, // value = HeatingModeState
        SI_WindowHeatingState = 4, // value = HeatingModeState
        SI_BlockTemperatureStatus = 5, // value = StatusCode
        SI_AmbientTemperatureStatus = 6, // value = StatusCode
        SI_LaserTemperatureStatus = 7, // value = StatusCode
        SI_TiltAngleStatus = 8, // value = StatusCode
        SI_SnowHeightStatus = 9, // value = StatusCode
        SI_DistanceStatus = 10, // value = StatusCode
        SI_NormalizedSignalStatus = 11, // value = StatusCode
        SI_Reserved_12 = 12,
        SI_Reserved_13 = 13,
        SI_ErrorCode = 14, // value = DeviceErrorCode
        SI_ErrorCode_Current = 15, // value = DeviceErrorCode
        SI_AccumulatedOpperatingTimeLower = 16,
        SI_AccumulatedOpperatingTimeUpper = 17,
        SI_SystemTimeLower = 18,
        SI_SystemTimeUpper = 19,
        // Standard data registers (metric units)
        SDM_SnowHeightMillimeter_Current = 20, // value = signed short
        SDM_BlockTemperatureDegC_Current, // value = signed short scaled by 10
        SDM_AmbientTemperatureDegC_Current, // value = signed short scaled by 10
        SDM_LaserTemperatureDegC_Current, // value = signed short scaled by 10
        SDM_NormalizedSignal_Metric,
        SDM_TiltAngle_Metric_Current, // value = signed short scaled by 10
        SDM_ErrorCode_Metric,
        SDM_Reserved_27,
        SDM_Reserved_28,
        SDM_Reserved_29,
        // Standard data set registers (imperial units)
        SDI_SnowHeightInches_Current = 30, // value = signed short scaled by 20
        SDI_BlockTemperatureDegF_Current, // value = signed short scaled by 10
        SDI_AmbientTemperatureDegF_Current, // value = signed short scaled by 10
        SDI_LaserTemperatureDegF_Current, // value = signed short scaled by 10
        SDI_NormalizedSignal_Imperial,
        SDI_TiltAngle_Imperial_Current, // value = signed short scaled by 10
        SDI_ErrorCode_Imperial,
        SDI_Reserved_37,
        SDI_Reserved_38,
        SDI_Reserved_39,
        // Distance Registers
        D_SnowHeight_Millimeter_Current,
        D_SnowHeight_Millimeter_Minimum,
        D_SnowHeight_Millimeter_Maximum,
        D_SnowHeight_Millimeter_Average,
        D_Calibrated_Millimeter_Current,
        D_Raw_Millimeter_Current,
        D_SnowHeight_Inches_Current, // value = signed short scaled by 20
        D_SnowHeight_Inches_Minimum, // value = signed short scaled by 20
        D_SnowHeight_Inches_Maximum, // value = signed short scaled by 20
        D_SnowHeight_Inches_Average, // value = signed short scaled by 20
        D_Calibrated_Inches_Current, // value = signed short scaled by 20
        D_Raw_Inches_Current, // value = signed short scaled by 20
        D_ReferenceHeight_millimeter = 52,
        D_SnowHeight_millimeter_HighRes, //value = unsigned short scaled by 10
        D_Reserved_54,
        // Temperature Registers (metric units)
        TM_BlockTemperatureDegC_Current = 55, // value = signed short scaled by 10
        TM_BlockTemperatureDegC_Minimum, // value = signed short scaled by 10
        TM_BlockTemperatureDegC_Maximum, // value = signed short scaled by 10
        TM_BlockTemperatureDegC_Average, // value = signed short scaled by 10
        TM_AmbientTemperatureDegC_Current, // value = signed short scaled by 10
        TM_AmbientTemperatureDegC_Minimum, // value = signed short scaled by 10
        TM_AmbientTemperatureDegC_Maximum, // value = signed short scaled by 10
        TM_AmbientTemperatureDegC_Average, // value = signed short scaled by 10
        TM_LaserTemperatureDegC_Current, // value = signed short scaled by 10
        TM_LaserTemperatureDegC_Minimum, // value = signed short scaled by 10
        TM_LaserTemperatureDegC_Maximum, // value = signed short scaled by 10
        TM_LaserTemperatureDegC_Average, // value = signed short scaled by 10
        TM_Reserved_67,
        TM_Reserved_68,
        TM_Reserved_69,
        // Temperature Registers (imperial units)
        TI_BlockTemperatureDegF_Current = 70, // value = signed short scaled by 10
        TI_BlockTemperatureDegF_Minimum, // value = signed short scaled by 10
        TI_BlockTemperatureDegF_Maximum, // value = signed short scaled by 10
        TI_BlockTemperatureDegF_Average, // value = signed short scaled by 10
        TI_AmbientTemperatureDegF_Current, // value = signed short scaled by 10
        TI_AmbientTemperatureDegF_Minimum, // value = signed short scaled by 10
        TI_AmbientTemperatureDegF_Maximum, // value = signed short scaled by 10
        TI_AmbientTemperatureDegF_Average, // value = signed short scaled by 10
        TI_LaserTemperatureDegF_Current, // value = signed short scaled by 10
        TI_LaserTemperatureDegF_Minimum, // value = signed short scaled by 10
        TI_LaserTemperatureDegF_Maximum, // value = signed short scaled by 10
        TI_LaserTemperatureDegF_Average, // value = signed short scaled by 10
        TI_Reserved_82,
        TI_Reserved_83,
        TI_Reserved_84,
        // Angles registers
        A_AngleTilt_Current = 85, // value = signed short scaled by 10
        A_AngleTilt_Minimum, // value = signed short scaled by 10
        A_AngleTilt_Maximum, // value = signed short scaled by 10
        A_AngleTilt_Average, // value = signed short scaled by 10
        A_AngleX_Current = 89, // value = signed short scaled by 10
        A_AngleY_Current, // value = signed short scaled by 10
        A_AngleZ_Current, // value = signed short scaled by 10
        A_TiltAngleReference, // value = signed short scaled by 10
        A_Reserved_93,
        A_Reserved_94,
        // Logic and normalized values registers
        LNV_SnowFlag = 95,
        LNV_Reserved_96, // TODO: Note: this is missing in the manual!
        LNV_NormalizedSignal_Current = 97,
        LNV_NormalizedSignal_Minimum,
        LNV_NormalizedSignal_Maximum,
        LNV_NormalizedSignal_Average,
        LNV_Reserved_101,
        LNV_Reserved_102,
        LNV_Reserved_103,
        LNV_Reserved_104,
        // Service Channel Registers
        SC_BlockHeatingState = 105, // value = HeatingModeState
        SC_InternalTemperatureDegC_NTC = 106, // value = signed short scaled by 10
        SC_Reserved_107 = 107,
        SC_BlockHeatingDefrostTime_Seconds = 108,
        SC_WindowHeatingState = 109,
        SC_ExternalTemperatureDegC_NTC = 110, // value = signed short scaled by 10
        SC_Reserved_111 = 111,
        SC_WindowHeatingDefrostTime_Seconds = 112,
        SC_LaserGainCode = 113,
        SC_LaserSignalIntensity_uV = 114, // value = signed short scaled by 0.1
        SC_LaserDistance_Millimeter = 115,
        SC_LaserTemperatureDegC = 116, // value = signed short scaled by 10
        SC_OperatingVoltage = 117, // value = signed short scaled by 10
        SC_Reserved_118 = 118,
        SC_Reserved_119 = 119,
        /// <summary>
        /// The register is unknown or above the allowed value.
        /// </summary>
        Unknown = 0xFF
    }
}
