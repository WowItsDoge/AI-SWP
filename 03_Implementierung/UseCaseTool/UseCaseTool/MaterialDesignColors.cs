using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace UseCaseTool
{
    public static class MaterialDesignColors
    {
        // colors by google material design
        // https://material.io/design/color/the-color-system.html#tools-for-picking-colors

        public static int CompletePaletteSize { get; private set; }
        public static int StandardPaletteSize { get; private set; }
        public static int SmallPaletteSize { get; private set; }

        public static string[] Red { get; private set; }

        public static string[] Green { get; private set; }

        static MaterialDesignColors()
        {
            // initialize palette size
            CompletePaletteSize = 14;
            StandardPaletteSize = 10;
            SmallPaletteSize = 2;

            // initialize colors
            Red = new string[]
            {
                "#FFEBEE",
                "#FFCDD2",
                "#EF9A9A",
                "#E57373",
                "#EF5350",
                "#F44336",
                "#E53935",
                "#D32F2F",
                "#C62828",
                "#B71C1C",
                "#FF8A80",
                "#FF5252",
                "#FF1744",
                "#D50000"
            };

            Green = new string[]
            {
                "#E8F5E9",
                "#C8E6C9",
                "#A5D6A7",
                "#81C784",
                "#66BB6A",
                "#4CAF50",
                "#43A047",
                "#388E3C",
                "#2E7D32",
                "#1B5E20",
                "#B9F6CA",
                "#69F0AE",
                "#00E676",
                "#00C853"
            };
        }

        public static Color ColorFromHex(string hexColor)
        {
            return (Color)ColorConverter.ConvertFromString(hexColor);
        }

        public static Microsoft.Msagl.Drawing.Color MsaglColorFromHex(string hexColor)
        {
            Color color = ColorFromHex(hexColor);

            return new Microsoft.Msagl.Drawing.Color(color.R, color.G, color.B);
        }
    }
}
