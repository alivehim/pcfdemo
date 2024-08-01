using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Utilites
{
    public class DeviceHelper
    {
        public static (int lineCountOfPage, int countOfLine) GetCounts(int width, int height, int lineHeight, int fontSize)
        {
            int[] numArray = new int[2];
            int num = 0;
            int num2 = 0;
            int num3 = (fontSize * 0x20) / 100;
            num2 = width / (fontSize - num3);//? 460为宽度
            num = height / (fontSize + lineHeight);
            return (num, num2);
        }
    }
}
