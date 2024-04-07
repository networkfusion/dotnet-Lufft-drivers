using System;
using System.Diagnostics;
using System.Threading;
using Iot.Device.Lufft.Shm31.ModbusRtu;

namespace Iot.Device.Lufft.Shm31.Sample
{
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("Lufft SHM31 Sample!");


            var shm31Sensor = new LufftShm31ModbusRTU("COM3");

            var regs = shm31Sensor.ReadAllRegistersRaw();

            for (ushort i = 0; i < regs.Length; i++)
            {
                ModbusInputValue vals = new(i, regs[i]);

                Debug.WriteLine($"Raw is: {vals.RawValue}, Adj-Val is: {vals.AdjustedValue}");
            }

            Thread.Sleep(Timeout.Infinite);

        }

        //public short[] ReadNormalRegistersRaw()
        //{
        //    // TODO: is there a way to check if the read has happened successfully!? probably returns null...
        //    // read and return the info and standard measurements
        //    return client.ReadInputRegisters(
        //        DeviceId,
        //        ModbusInputRegisterTypes.GetStartAddress(ModbusInputRegisterType.StatusInformation),
        //        ModbusInputRegisterTypes.GetEndAddress(ModbusInputRegisterType.StandardMetric)
        //        );
        //    // TODO: add the snow flag:
        //    //short[] regsRead = client.ReadInputRegisters(DeviceId, 95, 1);
        //}
    }
}
