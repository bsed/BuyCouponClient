using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace CouponClient
{
    public static class MonitorNetworkSpeed
    {
        private static NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();

        private static float sent, received;

        static MonitorNetworkSpeed()
        {
            var task = new Task(() => { Run(); });
            task.Start();
        }

        public static void Run()
        {
            try
            {
                PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory("Network Interface");
                string instance = performanceCounterCategory.GetInstanceNames()[0]; // 1st NIC !
                PerformanceCounter performanceCounterSent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
                PerformanceCounter performanceCounterReceived = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);
                sent = performanceCounterSent.NextValue();
                received = performanceCounterReceived.NextValue();
                Thread.Sleep(1000);
                sent = performanceCounterSent.NextValue();
                received = performanceCounterReceived.NextValue();
                NetworkSpeedChanged?.Invoke(sent, received);
                Task task = new Task(()=> { Run(); });
                task.Start();
            }
            catch (Exception ex)
            {
                sent = 0;
                received = 0;
            }
          
        }

        public delegate void NetworkSpeedChangedHander(float sent, float received);

        public static event NetworkSpeedChangedHander NetworkSpeedChanged;

    }
}
