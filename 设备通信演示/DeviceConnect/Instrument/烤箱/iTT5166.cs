using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConnect
{
    public class iOven
    {
        string errorMsg { set; get; }
        bool isConnect { set; get; }
    }

    public enum Oven
    {
        ON, OFF
    }
    public enum Get
    {
        ConTemSet, ConHumSet, Temp1_PV, Temp_SV, Humid_PV, Humid_SV
    }
    public enum Mode
    {
        Constant, Program
    }
    public enum Section
    {
        one, two, three, four, five
    }
    public struct SectionData
    {
        public Int16 Temperature;
        public UInt16 Humid;
        public UInt16 Hour;
        public UInt16 Minute;
        public string StandByMode;
    }
}
