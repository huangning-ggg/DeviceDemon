using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConnect
{
    public interface IOPS
    {
        string errorMsg { set; get; }
        bool isConnect { set; get; }
        bool Connect(string com, int baurdRate = 115200);
        bool Disconnect();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel">1-4</param>
        /// <returns></returns>
        bool OpenOPTSource(byte channel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel">1-4</param>
        /// <returns></returns>
        bool CloseOPTSource(byte channel);

        /// <summary>打开1-4通道光路
        /// 
        /// </summary>
        /// <returns></returns>
        bool OpenAll();

        /// <summary>关闭1-4通道光路
        /// 
        /// </summary>
        /// <returns></returns>
        bool CloseAll();

        /// <summary>单独开启或关闭单独某个通道
        /// channel:1-4
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        bool SetOutOnly(byte channel, bool sta);

        /// <summary>1-4
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="ATT"></param>
        /// <returns></returns>
        bool SetATT(byte channel, double ATT);

        /// <summary>channel:1-4
        /// 
        /// </summary>
        /// <param name="channel">1-4</param>
        /// <param name="power"></param>
        /// <returns></returns>
        bool GetATT(byte channel, ref double ATT);

        OPSSetAtt opsCheck{ set; get; }

        /// <summary>设置4个通道的衰减值
        /// 
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        bool SetATTFour(int station);

        /// <summary>设置四个通道指定衰减值
        /// 
        /// </summary>
        /// <param name="Atts"></param>
        /// <returns></returns>
        bool SetATTFour(double[] Atts);
    }
}
