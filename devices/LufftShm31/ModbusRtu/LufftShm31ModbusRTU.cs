using Iot.Device.Modbus.Client;
using Iot.Device.Modbus.Util;
using System;

namespace Iot.Device.Lufft.Shm31.ModbusRtu
{
    /// <summary>
    /// Lufft SHM31 Modbus RTU
    /// </summary>
    /// <remarks>
    /// Snow depth sensor.
    /// Manufacturer: OTT Hydromet (Lufft, Jenoptik).
    /// Manual: https://www.lufft.com/download/manual-lufft-shm31-en/
    /// </remarks>
    public class LufftShm31ModbusRTU
    {

        private const short ACTION_APPLY = 12871;

        public byte DeviceId { get; private set; }

        private readonly ModbusClient client; // FIXME: technically modbus is RS485 only, so this should be added to the modbus client.

        private readonly ModbusInputRegisters inputRegisters;

        public LufftShm31ModbusRTU(string port, byte deviceId = 1, int baudRate = 19200)
        {
            DeviceId = deviceId;
            client = new(port, baudRate);
            client.ReadTimeout = client.WriteTimeout = 500;
            inputRegisters = new();
            DebugHelper.DebugListAvailableInputRegisters(inputRegisters.InputRegisters);
        }

        // FIXME: should be able to close the connection!
        //public void Close()
        //{
        //    client.Dispose();
        //}

        public float ReadRegister(ModbusInputRegisterAddress address)
        {
            // FIXME: return a proper value.
            ModbusInputValue val = new ModbusInputValue((ushort)address, ReadRegisterRaw(address));
            return val.AdjustedValue;
        }

        public short ReadRegisterRaw(ModbusInputRegisterAddress address)
        {
            var val = client.ReadInputRegisters(DeviceId, (ushort)address, 1);
            return val[0];
        }

        public short[] ReadAllRegistersRaw()
        {
            // TODO: is there a way to check if the read has happened successfully!? probably returns null...
            // read and return all (0..119) registers on the device.
            return ReadSequentialRegisterRangeRaw(0, ModbusInputRegisters.REG_ADDRESS_MAX);
        }

        public short[] ReadRegisterTypeRaw(ModbusInputRegisterType regType)
        {
            // TODO: is there a way to check if the read has happened successfully!? probably returns null...

            return ReadSequentialRegisterRangeRaw(
                ModbusInputRegisterTypes.GetStartAddress(regType),
                ModbusInputRegisterTypes.GetEndAddress(regType)
                );
        }

        public short[] ReadSequentialRegisterRangeRaw(ushort fromAddress, ushort toAddress)
        {
            // TODO: is there a way to check if the read has happened successfully!? probably returns null...
            // read and return the info and standard measurements
            // TODO: some error checking is needed, like making sure the to reg is not larger than the from reg.
            return client.ReadInputRegisters( DeviceId, fromAddress, toAddress );
        }

        public bool PerformAction(ModbusSensorAction actionRegister)
        {
            if ((ushort)actionRegister < (ushort)ModbusSensorActionType.IsSettableValue)
            {
                return client.WriteSingleRegister(DeviceId, (ushort)actionRegister, ACTION_APPLY);
            }
            return false; // it involved a register that required a settable value.
        }

        public bool PerformAction(ModbusSensorAction actionRegister, ushort value)
        {
            if (value >= short.MaxValue)
            {
                // FIXME: certain values might need ushort (like height change acceptance time)
                // TODO: apply twos complement and write it raw?

                return false;
            }

            if ((ushort)actionRegister >= (ushort)ModbusSensorActionType.IsSettableValue) // make sure this is a settable register
            {
                var res = client.WriteSingleRegister(DeviceId, (ushort)actionRegister, (short)value);
                if (!res)
                {
                    res = PerformAction(ModbusSensorAction.InitiateReboot);
                }
                return res;
            }
            return false;
        }

        public static float AdjustValue_ScaleFactor(short regValue, short scaleFactor)
        {
            return regValue / scaleFactor;
        }

        // TODO: convert short to ushort for most regs.

        public static uint GetValueAsUInt32(ushort lowerValue, ushort upperValue)
        {
            // TODO: switch to Iot.Device.Modbus.Util
            uint value = upperValue;
            value <<= 16;
            value |= lowerValue;
            return value;
        }
    }
}
