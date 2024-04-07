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
            Debug.WriteLine("SHM31 Sample!");


            var shm31Sensor = new LufftShm31ModbusRTU("COM3");

            var regs = shm31Sensor.ReadNormalRegistersRaw();

            foreach (var reg in regs)
            {
                Debug.WriteLine($"{reg}");
            }

            Thread.Sleep(Timeout.Infinite);

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }
    }
}
