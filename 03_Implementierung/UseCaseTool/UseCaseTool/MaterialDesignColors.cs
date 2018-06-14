// <copyright file="MaterialDesignColors.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media;

    /// <summary>
    /// Material Design Colors
    /// https://material.io/design/color/the-color-system.html#tools-for-picking-colors
    /// </summary>
    public static class MaterialDesignColors
    {
        /// <summary>
        /// Initializes static members of the <see cref="MaterialDesignColors"/> class.
        /// </summary>
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

            RedStartWhite = 4;
            RedExceptionIds = new int[] { 10 };

            Pink = new string[]
            {
                "#FCE4EC",
                "#F8BBD0",
                "#F48FB1",
                "#F06292",
                "#EC407A",
                "#E91E63",
                "#D81B60",
                "#C2185B",
                "#AD1457",
                "#880E4F",
                "#FF80AB",
                "#FF4081",
                "#F50057",
                "#C51162"
            };

            PinkStartWhite = 4;
            PinkExceptionIds = new int[] { 10 };

            Purple = new string[]
            {
                "#F3E5F5",
                "#E1BEE7",
                "#CE93D8",
                "#BA68C8",
                "#AB47BC",
                "#9C27B0",
                "#8E24AA",
                "#7B1FA2",
                "#6A1B9A",
                "#4A148C",
                "#EA80FC",
                "#E040FB",
                "#D500F9",
                "#AA00FF"
            };

            PurpleStartWhite = 3;
            PurpleExceptionIds = new int[] { 10 };

            DeepPurple = new string[]
            {
                "#EDE7F6",
                "#D1C4E9",
                "#B39DDB",
                "#9575CD",
                "#7E57C2",
                "#673AB7",
                "#5E35B1",
                "#512DA8",
                "#4527A0",
                "#311B92",
                "#B388FF",
                "#7C4DFF",
                "#651FFF",
                "#6200EA"
            };

            DeepPurpleStartWhite = 3;
            DeepPurpleExceptionIds = new int[] { 10 };

            Indigo = new string[]
            {
                "#E8EAF6",
                "#C5CAE9",
                "#9FA8DA",
                "#7986CB",
                "#5C6BC0",
                "#3F51B5",
                "#3949AB",
                "#303F9F",
                "#283593",
                "#1A237E",
                "#8C9EFF",
                "#536DFE",
                "#3D5AFE",
                "#304FFE"
            };

            IndigoStartWhite = 3;
            IndigoExceptionIds = new int[] { 10 };

            Blue = new string[]
            {
                "#E3F2FD",
                "#BBDEFB",
                "#90CAF9",
                "#64B5F6",
                "#42A5F5",
                "#2196F3",
                "#1E88E5",
                "#1976D2",
                "#1565C0",
                "#0D47A1",
                "#82B1FF",
                "#448AFF",
                "#2979FF",
                "#2962FF"
            };

            BlueStartWhite = 6;
            BlueExceptionIds = new int[] { 10 };

            LightBlue = new string[]
            {
                "#E1F5FE",
                "#B3E5FC",
                "#81D4FA",
                "#4FC3F7",
                "#29B6F6",
                "#03A9F4",
                "#039BE5",
                "#0288D1",
                "#0277BD",
                "#01579B",
                "#80D8FF",
                "#40C4FF",
                "#00B0FF",
                "#0091EA"
            };

            LightBlueStartWhite = 7;
            LightBlueExceptionIds = new int[] { 10, 11, 12 };

            Cyan = new string[]
            {
                "#E0F7FA",
                "#B2EBF2",
                "#80DEEA",
                "#4DD0E1",
                "#26C6DA",
                "#00BCD4",
                "#00ACC1",
                "#0097A7",
                "#00838F",
                "#006064",
                "#84FFFF",
                "#18FFFF",
                "#00E5FF",
                "#00B8D4"
            };

            CyanStartWhite = 7;
            CyanExceptionIds = new int[] { 10, 11, 12, 13 };

            Teal = new string[]
            {
                "#E0F2F1",
                "#B2DFDB",
                "#80CBC4",
                "#4DB6AC",
                "#26A69A",
                "#009688",
                "#00897B",
                "#00796B",
                "#00695C",
                "#004D40",
                "#A7FFEB",
                "#64FFDA",
                "#1DE9B6",
                "#00BFA5"
            };

            TealStartWhite = 5;
            TealExceptionIds = new int[] { 10, 11, 12, 13 };

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

            GreenStartWhite = 6;
            GreenExceptionIds = new int[] { 10, 11, 12, 13 };

            LightGreen = new string[]
            {
                "#F1F8E9",
                "#DCEDC8",
                "#C5E1A5",
                "#AED581",
                "#9CCC65",
                "#8BC34A",
                "#7CB342",
                "#689F38",
                "#558B2F",
                "#33691E",
                "#CCFF90",
                "#B2FF59",
                "#76FF03",
                "#64DD17"
            };

            LightGreenStartWhite = 8;
            LightGreenExceptionIds = new int[] { 10, 11, 12, 13 };

            Lime = new string[]
            {
                "#F9FBE7",
                "#F0F4C3",
                "#E6EE9C",
                "#DCE775",
                "#D4E157",
                "#CDDC39",
                "#C0CA33",
                "#AFB42B",
                "#9E9D24",
                "#827717",
                "#F4FF81",
                "#EEFF41",
                "#C6FF00",
                "#AEEA00"
            };

            LimeStartWhite = 9;
            LimeExceptionIds = new int[] { 10, 11, 12, 13 };

            Yellow = new string[]
            {
                "#FFFDE7",
                "#FFF9C4",
                "#FFF59D",
                "#FFF176",
                "#FFEE58",
                "#FFEB3B",
                "#FDD835",
                "#FBC02D",
                "#F9A825",
                "#F57F17",
                "#FFFF8D",
                "#FFFF00",
                "#FFEA00",
                "#FFD600"
            };

            YellowStartWhite = 14;
            YellowExceptionIds = new int[] { };

            Amber = new string[]
            {
                "#FFF8E1",
                "#FFECB3",
                "#FFE082",
                "#FFD54F",
                "#FFCA28",
                "#FFC107",
                "#FFB300",
                "#FFA000",
                "#FF8F00",
                "#FF6F00",
                "#FFE57F",
                "#FFD740",
                "#FFC400",
                "#FFAB00"
            };

            AmberStartWhite = 14;
            AmberExceptionIds = new int[] { };

            Orange = new string[]
            {
                "#FFF3E0",
                "#FFE0B2",
                "#FFCC80",
                "#FFB74D",
                "#FFA726",
                "#FF9800",
                "#FB8C00",
                "#F57C00",
                "#EF6C00",
                "#E65100",
                "#FFD180",
                "#FFAB40",
                "#FF9100",
                "#FF6D00"
            };

            OrangeStartWhite = 9;
            OrangeExceptionIds = new int[] { 10, 11, 12, 13 };

            DeepOrange = new string[]
            {
                "#FBE9E7",
                "#FFCCBC",
                "#FFAB91",
                "#FF8A65",
                "#FF7043",
                "#FF5722",
                "#F4511E",
                "#E64A19",
                "#D84315",
                "#BF360C",
                "#FF9E80",
                "#FF6E40",
                "#FF3D00",
                "#DD2C00"
            };

            DeepOrangeStartWhite = 6;
            DeepOrangeExceptionIds = new int[] { 10, 11 };

            Brown = new string[]
            {
                "#EFEBE9",
                "#D7CCC8",
                "#BCAAA4",
                "#A1887F",
                "#8D6E63",
                "#795548",
                "#6D4C41",
                "#5D4037",
                "#4E342E",
                "#3E2723"
            };

            BrownStartWhite = 3;
            BrownExceptionIds = new int[] { };

            Gray = new string[]
            {
                "#FAFAFA",
                "#F5F5F5",
                "#EEEEEE",
                "#E0E0E0",
                "#BDBDBD",
                "#9E9E9E",
                "#757575",
                "#616161",
                "#424242",
                "#212121"
            };

            GrayStartWhite = 6;
            GrayExceptionIds = new int[] { };

            BlueGray = new string[]
            {
                "#ECEFF1",
                "#CFD8DC",
                "#B0BEC5",
                "#90A4AE",
                "#78909C",
                "#607D8B",
                "#546E7A",
                "#455A64",
                "#37474F",
                "#263238"
            };

            BlueGrayStartWhite = 4;
            BlueGrayExceptionIds = new int[] { };

            Black = "#000000";

            White = "#FFFFFF";
        }

        /// <summary>
        /// Gets CompletePaletteSize
        /// </summary>
        public static int CompletePaletteSize { get; private set; }

        /// <summary>
        /// Gets StandardPaletteSize
        /// </summary>
        public static int StandardPaletteSize { get; private set; }

        /// <summary>
        /// Gets SmallPaletteSize
        /// </summary>
        public static int SmallPaletteSize { get; private set; }

        private static Random random { get; set; }

        #region Red
        /// <summary>
        /// Gets Red colors
        /// </summary>
        public static string[] Red { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for red
        /// </summary>
        public static int RedStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for red
        /// </summary>
        public static int[] RedExceptionIds { get; private set; }
        #endregion

        #region Pink
        /// <summary>
        /// Gets Pink colors
        /// </summary>
        public static string[] Pink { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for pink
        /// </summary>
        public static int PinkStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for pink
        /// </summary>
        public static int[] PinkExceptionIds { get; private set; }
        #endregion

        #region Purple
        /// <summary>
        /// Gets Purple colors
        /// </summary>
        public static string[] Purple { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for purple
        /// </summary>
        public static int PurpleStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for purple
        /// </summary>
        public static int[] PurpleExceptionIds { get; private set; }
        #endregion

        #region Deep Purple
        /// <summary>
        /// Gets Deep Purple colors
        /// </summary>
        public static string[] DeepPurple { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for deep purple
        /// </summary>
        public static int DeepPurpleStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for deep purple
        /// </summary>
        public static int[] DeepPurpleExceptionIds { get; private set; }
        #endregion

        #region Indigo
        /// <summary>
        /// Gets Indigo colors
        /// </summary>
        public static string[] Indigo { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for indigo
        /// </summary>
        public static int IndigoStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for indigo
        /// </summary>
        public static int[] IndigoExceptionIds { get; private set; }
        #endregion

        #region Blue
        /// <summary>
        /// Gets Blue colors
        /// </summary>
        public static string[] Blue { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for blue
        /// </summary>
        public static int BlueStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for blue
        /// </summary>
        public static int[] BlueExceptionIds { get; private set; }
        #endregion

        #region Light Blue
        /// <summary>
        /// Gets Light Blue colors
        /// </summary>
        public static string[] LightBlue { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for light blue
        /// </summary>
        public static int LightBlueStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for light blue
        /// </summary>
        public static int[] LightBlueExceptionIds { get; private set; }
        #endregion

        #region Cyan
        /// <summary>
        /// Gets Cyan colors
        /// </summary>
        public static string[] Cyan { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for cyan
        /// </summary>
        public static int CyanStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for cyan
        /// </summary>
        public static int[] CyanExceptionIds { get; private set; }
        #endregion

        #region Teal
        /// <summary>
        /// Gets Teal colors
        /// </summary>
        public static string[] Teal { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for teal
        /// </summary>
        public static int TealStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for teal
        /// </summary>
        public static int[] TealExceptionIds { get; private set; }
        #endregion

        #region Green
        /// <summary>
        /// Gets Green colors
        /// </summary>
        public static string[] Green { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for green
        /// </summary>
        public static int GreenStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for green
        /// </summary>
        public static int[] GreenExceptionIds { get; private set; }
        #endregion

        #region Light Green
        /// <summary>
        /// Gets Light Green colors
        /// </summary>
        public static string[] LightGreen { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for light green
        /// </summary>
        public static int LightGreenStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for light green
        /// </summary>
        public static int[] LightGreenExceptionIds { get; private set; }
        #endregion

        #region Lime
        /// <summary>
        /// Gets Lime colors
        /// </summary>
        public static string[] Lime { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for lime
        /// </summary>
        public static int LimeStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for lime
        /// </summary>
        public static int[] LimeExceptionIds { get; private set; }
        #endregion

        #region Yellow
        /// <summary>
        /// Gets Yellow colors
        /// </summary>
        public static string[] Yellow { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for yellow
        /// </summary>
        public static int YellowStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for yellow
        /// </summary>
        public static int[] YellowExceptionIds { get; private set; }
        #endregion

        #region Amber
        /// <summary>
        /// Gets Amber colors
        /// </summary>
        public static string[] Amber { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for amber
        /// </summary>
        public static int AmberStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for amber
        /// </summary>
        public static int[] AmberExceptionIds { get; private set; }
        #endregion

        #region Orange
        /// <summary>
        /// Gets Orange colors
        /// </summary>
        public static string[] Orange { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for orange
        /// </summary>
        public static int OrangeStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for orange
        /// </summary>
        public static int[] OrangeExceptionIds { get; private set; }
        #endregion

        #region Deep Orange
        /// <summary>
        /// Gets Deep Orange colors
        /// </summary>
        public static string[] DeepOrange { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for deep orange
        /// </summary>
        public static int DeepOrangeStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for deep orange
        /// </summary>
        public static int[] DeepOrangeExceptionIds { get; private set; }
        #endregion

        #region Brown
        /// <summary>
        /// Gets Brown colors
        /// </summary>
        public static string[] Brown { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for brown
        /// </summary>
        public static int BrownStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for brown
        /// </summary>
        public static int[] BrownExceptionIds { get; private set; }
        #endregion Brown

        #region Gray
        /// <summary>
        /// Gets Gray colors
        /// </summary>
        public static string[] Gray { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for gray
        /// </summary>
        public static int GrayStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for gray
        /// </summary>
        public static int[] GrayExceptionIds { get; private set; }
        #endregion

        #region Blue Gray
        /// <summary>
        /// Gets Gray colors
        /// </summary>
        public static string[] BlueGray { get; private set; }

        /// <summary>
        /// Gets white foreground color start index for blue gray
        /// </summary>
        public static int BlueGrayStartWhite { get; private set; }

        /// <summary>
        /// Gets black foreground color ids in white index for blue gray
        /// </summary>
        public static int[] BlueGrayExceptionIds { get; private set; }
        #endregion

        /// <summary>
        /// Gets Black colors
        /// </summary>
        public static string Black { get; private set; }

        /// <summary>
        /// Gets White colors
        /// </summary>
        public static string White { get; private set; }

        /// <summary>
        /// Converts a hex color string to a Windows.Media.Color
        /// </summary>
        /// <param name="hexColor">Color in hex</param>
        /// <returns>Windows.Media.Color object</returns>
        public static Color ColorFromHex(string hexColor)
        {
            return (Color)ColorConverter.ConvertFromString(hexColor);
        }

        /// <summary>
        /// Converts a hex color to a Microsoft Auto Graph Layout Color
        /// </summary>
        /// <param name="hexColor">Color in hex</param>
        /// <returns>Microsoft Auto Graph Layout Color</returns>
        public static Microsoft.Msagl.Drawing.Color MsaglColorFromHex(string hexColor)
        {
            Color color = ColorFromHex(hexColor);

            return new Microsoft.Msagl.Drawing.Color(color.R, color.G, color.B);
        }

        /// <summary>
        /// Converts a Microsoft Auto Graph Layout Color to hex color
        /// </summary>
        /// <param name="hexColor">Microsoft Auto Graph Layout Color</param>
        /// <returns>Color in hex</returns>
        public static string MsaglColorToHex(Microsoft.Msagl.Drawing.Color color)
        {
            return "#" + BitConverter.ToString(new byte[] { color.R }) +
                BitConverter.ToString(new byte[] { color.G }) +
                BitConverter.ToString(new byte[] { color.B });
        }

        public static Microsoft.Msagl.Drawing.Color RandomMsaglColor()
        {
            if (MaterialDesignColors.random == null)
            {
                MaterialDesignColors.random = new Random();
            }

            var colorNr = random.Next(0, 18);

            var colorHex = White;
            var paletteSelected = Green;
            int paletteColor = 0;

            switch (colorNr)
            {
                case 0:
                    paletteSelected = Red;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                case 1:
                    paletteSelected = Pink;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                case 2:
                    paletteSelected = Purple;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                case 3:
                    paletteSelected = DeepPurple;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                case 4:
                    paletteSelected = Indigo;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                case 5:
                    paletteSelected = Blue;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                case 6:
                    paletteSelected = LightBlue;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                case 7:
                    paletteSelected = Cyan;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                case 8:
                    paletteSelected = Teal;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                case 9:
                    paletteSelected = Green;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                case 10:
                    paletteSelected = LightGreen;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                case 11:
                    paletteSelected = Lime;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                case 12:
                    paletteSelected = Yellow;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                case 13:
                    paletteSelected = Amber;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                case 14:
                    paletteSelected = Orange;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                case 15:
                    paletteSelected = DeepOrange;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                case 16:
                    paletteSelected = Brown;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                case 17:
                    paletteSelected = Gray;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                case 18:
                    paletteSelected = BlueGray;
                    paletteColor = random.Next(0, paletteSelected.Length - 1);
                    colorHex = paletteSelected[paletteColor];
                    break;
                default:
                    break;
            }

            return MsaglColorFromHex(colorHex);            
        }

        /// <summary>
        /// Returns the foreground color for a background color.
        /// </summary>
        /// <param name="colorHex"></param>
        /// <returns></returns>
        public static string GetForegroundColor(string colorHex)
        {
            var colorId = GetColorId(colorHex, Red);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, RedStartWhite, RedExceptionIds);
            }

            colorId = GetColorId(colorHex, Pink);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, PinkStartWhite, PinkExceptionIds);
            }

            colorId = GetColorId(colorHex, Purple);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, PurpleStartWhite, PurpleExceptionIds);
            }

            colorId = GetColorId(colorHex, DeepPurple);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, DeepPurpleStartWhite, DeepPurpleExceptionIds);
            }

            colorId = GetColorId(colorHex, Indigo);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, IndigoStartWhite, IndigoExceptionIds);
            }

            colorId = GetColorId(colorHex, Blue);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, BlueStartWhite, BlueExceptionIds);
            }

            colorId = GetColorId(colorHex, LightBlue);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, LightBlueStartWhite, LightBlueExceptionIds);
            }

            colorId = GetColorId(colorHex, Cyan);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, CyanStartWhite, CyanExceptionIds);
            }

            colorId = GetColorId(colorHex, Teal);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, TealStartWhite, TealExceptionIds);
            }

            colorId = GetColorId(colorHex, Green);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, GreenStartWhite, GreenExceptionIds);
            }

            colorId = GetColorId(colorHex, LightGreen);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, LightGreenStartWhite, LightGreenExceptionIds);
            }

            colorId = GetColorId(colorHex, Lime);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, LimeStartWhite, LimeExceptionIds);
            }

            colorId = GetColorId(colorHex, Yellow);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, YellowStartWhite, YellowExceptionIds);
            }

            colorId = GetColorId(colorHex, Amber);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, AmberStartWhite, AmberExceptionIds);
            }

            colorId = GetColorId(colorHex, Orange);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, OrangeStartWhite, OrangeExceptionIds);
            }

            colorId = GetColorId(colorHex, DeepOrange);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, DeepOrangeStartWhite, DeepOrangeExceptionIds);
            }

            colorId = GetColorId(colorHex, Brown);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, BrownStartWhite, BrownExceptionIds);
            }

            colorId = GetColorId(colorHex, Gray);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, GrayStartWhite, GrayExceptionIds);
            }

            colorId = GetColorId(colorHex, BlueGray);
            if (colorId >= 0)
            {
                return GetForegroundColor(colorId, BlueStartWhite, BlueExceptionIds);
            }

            return Black;
        }

        /// <summary>
        /// Gets the foreground color for a background color
        /// </summary>
        /// <param name="colorId"></param>
        /// <param name="colorStartWhite"></param>
        /// <param name="colorExceptionIds"></param>
        /// <returns></returns>
        private static string GetForegroundColor(int colorId, int colorStartWhite, int[] colorExceptionIds)
        {
            if (colorId < colorStartWhite || colorExceptionIds.Contains(colorId))
            {
                return Black;
            }
            else
            {
                return White;
            }
        }

        /// <summary>
        /// Returns the array index of a color
        /// </summary>
        /// <param name="color"></param>
        /// <param name="colors"></param>
        /// <returns></returns>
        private static int GetColorId(string color, string[] colors)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                if (color.Equals(colors[i]))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
