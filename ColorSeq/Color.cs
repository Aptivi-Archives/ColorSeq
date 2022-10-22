/*
 * MIT License
 * 
 * Copyright (c) 2022 Aptivi
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Extensification.StringExts;
using System;

namespace ColorSeq
{
    public class Color
    {
        /// <summary>
        /// Either 0-255, or &lt;R&gt;;&lt;G&gt;;&lt;B&gt;
        /// </summary>
        public string PlainSequence { get; private set; }
        /// <summary>
        /// Either 0-255, or &lt;R&gt;;&lt;G&gt;;&lt;B&gt; enclosed in quotes if necessary
        /// </summary>
        public string PlainSequenceEnclosed { get; private set; }
        /// <summary>
        /// Parsable VT sequence (Foreground)
        /// </summary>
        public string VTSequenceForeground { get; private set; }
        /// <summary>
        /// Parsable VT sequence (Background)
        /// </summary>
        public string VTSequenceBackground { get; private set; }
        /// <summary>
        /// The red color value
        /// </summary>
        public int R { get; private set; }
        /// <summary>
        /// The green color value
        /// </summary>
        public int G { get; private set; }
        /// <summary>
        /// The blue color value
        /// </summary>
        public int B { get; private set; }
        /// <summary>
        /// Hexadecimal representation of the color
        /// </summary>
        public string Hex { get; private set; }
        /// <summary>
        /// Color type
        /// </summary>
        public ColorType Type { get; private set; }
        /// <summary>
        /// Is the color bright?
        /// </summary>
        public bool IsBright { get; private set; }
        /// <summary>
        /// Is the color dark?
        /// </summary>
        public bool IsDark { get; private set; }
        /// <summary>
        /// Empty color singleton
        /// </summary>
        public static Color Empty { get; private set; } = new Color(0, 0, 0);

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorSpecifier">A color specifier. It must be a valid number from 0-255 if using 255-colors, a VT sequence if using true color as follows: &lt;R&gt;;&lt;G&gt;;&lt;B&gt;, or a hexadecimal representation of a number (#AABBCC for example)</param>
        /// <exception cref="Exceptions.ColorException"></exception>
        public Color(string ColorSpecifier)
        {
            // Remove stray double quotes
            ColorSpecifier = ColorSpecifier.Replace("\"", "");

            // Now, parse the output
            if (ColorSpecifier.Contains(";"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = ColorSpecifier.Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    // We got the RGB values! Form the sequences
                    PlainSequence = $"{ColorSpecifierArray[0]};{ColorSpecifierArray[1]};{ColorSpecifierArray[2]}";
                    PlainSequenceEnclosed = $"{ColorSpecifierArray[0]};{ColorSpecifierArray[1]};{ColorSpecifierArray[2]}".EncloseByDoubleQuotes();
                    VTSequenceForeground = Color255.GetEsc() + $"[38;2;{PlainSequence}m";
                    VTSequenceBackground = Color255.GetEsc() + $"[48;2;{PlainSequence}m";

                    // Populate color properties
                    Type = ColorType.TrueColor;
                    IsBright = Convert.ToDouble(ColorSpecifierArray[0]) + 0.2126d + Convert.ToDouble(ColorSpecifierArray[1]) + 0.7152d + Convert.ToDouble(ColorSpecifierArray[2]) + 0.0722d > 255d / 2d;
                    IsDark = Convert.ToDouble(ColorSpecifierArray[0]) + 0.2126d + Convert.ToDouble(ColorSpecifierArray[1]) + 0.7152d + Convert.ToDouble(ColorSpecifierArray[2]) + 0.0722d < 255d / 2d;
                    R = Convert.ToInt32(ColorSpecifierArray[0]);
                    G = Convert.ToInt32(ColorSpecifierArray[1]);
                    B = Convert.ToInt32(ColorSpecifierArray[2]);
                }
                else
                {
                    throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");
                }
            }
            else if (StringTools.IsStringNumeric(ColorSpecifier) || Enum.IsDefined(typeof(ConsoleColors), ColorSpecifier))
            {
                // Form the sequences using the information from the color details
                var ColorsInfo = new ConsoleColorsInfo((ConsoleColors)Enum.Parse(typeof(ConsoleColors), ColorSpecifier));
                PlainSequence = $"{ColorsInfo.ColorID}";
                PlainSequenceEnclosed = $"{ColorsInfo.ColorID}";
                VTSequenceForeground = Color255.GetEsc() + $"[38;5;{PlainSequence}m";
                VTSequenceBackground = Color255.GetEsc() + $"[48;5;{PlainSequence}m";

                // Populate color properties
                Type = ColorsInfo.ColorID >= 16 ? ColorType._255Color : ColorType._16Color;
                IsBright = ColorsInfo.IsBright;
                IsDark = ColorsInfo.IsDark;
                R = ColorsInfo.R;
                G = ColorsInfo.G;
                B = ColorsInfo.B;
            }
            else if (ColorSpecifier.StartsWith("#"))
            {
                int ColorDecimal = Convert.ToInt32(ColorSpecifier.RemoveLetter(0), 16);

                // Convert the RGB values to numbers
                R = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
                G = (byte)((ColorDecimal & 0xFF00) >> 8);
                B = (byte)(ColorDecimal & 0xFF);

                // We got the RGB values! Form the sequences
                PlainSequence = $"{R};{G};{B}";
                PlainSequenceEnclosed = $"{R};{G};{B}".EncloseByDoubleQuotes();
                VTSequenceForeground = Color255.GetEsc() + $"[38;2;{PlainSequence}m";
                VTSequenceBackground = Color255.GetEsc() + $"[48;2;{PlainSequence}m";

                // Populate color properties
                Type = ColorType.TrueColor;
                IsBright = R + 0.2126d + G + 0.7152d + B + 0.0722d > 255d / 2d;
                IsDark = R + 0.2126d + G + 0.7152d + B + 0.0722d < 255d / 2d;
            }
            else
            {
                throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");
            }

            // Populate the hexadecimal representation of the color
            Hex = $"#{R:X2}{G:X2}{B:X2}";
        }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <exception cref="Exceptions.ColorException"></exception>
        public Color(int R, int G, int B)
        {
            if (R < 0 | R > 255)
                throw new ColorSeqException("Invalid red color specifier.");
            if (G < 0 | G > 255)
                throw new ColorSeqException("Invalid green color specifier.");
            if (B < 0 | B > 255)
                throw new ColorSeqException("Invalid blue color specifier.");

            // Populate sequences
            PlainSequence = $"{R};{G};{B}";
            PlainSequenceEnclosed = $"{R};{G};{B}".EncloseByDoubleQuotes();
            VTSequenceForeground = Color255.GetEsc() + $"[38;2;{PlainSequence}m";
            VTSequenceBackground = Color255.GetEsc() + $"[48;2;{PlainSequence}m";

            // Populate color properties
            Type = ColorType.TrueColor;
            IsBright = R + 0.2126d + G + 0.7152d + B + 0.0722d > 255d / 2d;
            IsDark = R + 0.2126d + G + 0.7152d + B + 0.0722d < 255d / 2d;
            this.R = R;
            this.G = G;
            this.B = B;

            // Populate the hexadecimal representation of the color
            Hex = $"#{R:X2}{G:X2}{B:X2}";
        }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorNum">The color number</param>
        /// <exception cref="Exceptions.ColorException"></exception>
        public Color(int ColorNum)
        {
            // Form the sequences using the information from the color details
            var ColorsInfo = new ConsoleColorsInfo((ConsoleColors)Enum.Parse(typeof(ConsoleColors), ColorNum.ToString()));
            PlainSequence = Convert.ToString(ColorNum);
            PlainSequenceEnclosed = Convert.ToString(ColorNum);
            VTSequenceForeground = Color255.GetEsc() + $"[38;5;{PlainSequence}m";
            VTSequenceBackground = Color255.GetEsc() + $"[48;5;{PlainSequence}m";

            // Populate color properties
            Type = ColorNum >= 16 ? ColorType._255Color : ColorType._16Color;
            IsBright = ColorsInfo.IsBright;
            IsDark = ColorsInfo.IsDark;
            R = ColorsInfo.R;
            G = ColorsInfo.G;
            B = ColorsInfo.B;

            // Populate the hexadecimal representation of the color
            Hex = $"#{R:X2}{G:X2}{B:X2}";
        }
    }
}
