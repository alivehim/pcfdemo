using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace UtilityTools.Core.Utilites
{
    public class ColorSerializeHelper
    {
        enum ColorFormat
        {
            NamedColor,
            ARGBColor
        }

        public static string SerializeColor(Color color)
        {

            return string.Format("{0}:{1}:{2}:{3}:{4}", ColorFormat.ARGBColor, color.A, color.R, color.G, color.B);
        }

        public static string[] SerializeColorList(Color[] colorList)
        {
            string[] colorStringArray = new string[colorList.Count<Color>()];
            for (int i = 0; i < colorList.Count<Color>(); i++)
            {
                colorStringArray[i] = SerializeColor(colorList[i]);
            }
            return colorStringArray;
        }

        public static Color DeserializeColor(string color)
        {
            if (string.IsNullOrEmpty(color))
                return Color.FromArgb(0x00, 0x00, 0x00, 0x00);
            byte a, r, g, b;
            string[] pieces = color.Split(new char[] { ':' });
            ColorFormat colorType = (ColorFormat)Enum.Parse(typeof(ColorFormat), pieces[0], true);


            switch (colorType)
            {
                case ColorFormat.ARGBColor:
                    a = byte.Parse(pieces[1]);
                    r = byte.Parse(pieces[2]);
                    g = byte.Parse(pieces[3]);
                    b = byte.Parse(pieces[4]);
                    return Color.FromArgb(a, r, g, b);
            }
            return Color.FromArgb(0x00, 0x00, 0x00, 0x00);
        }

        public static Color[] DeserializeColorList(string[] colorList)
        {
            Color[] resultColorArray = new Color[colorList.Count<string>()];
            for (int i = 0; i < colorList.Count<string>(); i++)
            {
                resultColorArray[i] = DeserializeColor(colorList[i]);
            }
            return resultColorArray;
        }
    }
}
