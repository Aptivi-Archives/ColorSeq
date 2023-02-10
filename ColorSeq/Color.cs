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

using ColorSeq.Accessibility;
using Extensification.StringExts;
using System;
using System.Linq.Expressions;

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
        public static Color Empty
        {
            get
            {
                // Get cached value if cached
                if (ColorTools._empty != null)
                    return ColorTools._empty;

                // Else, cache the empty value and return it
                ColorTools._empty = new Color(0);
                return ColorTools._empty;
            }
        }
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
                    // We got the RGB values! First, check to see if we need to filter the color for the color-blind
                    int r = Convert.ToInt32(ColorSpecifierArray[0]);
                    int g = Convert.ToInt32(ColorSpecifierArray[1]);
                    int b = Convert.ToInt32(ColorSpecifierArray[2]);
                    if (ColorTools.EnableColorTransformation)
                    {
                        // We'll transform.
                        (int, int, int) transformed;
                        if (ColorTools.EnableSimpleColorTransformation)
                            transformed = Vienot1999.Transform(r, g, b, ColorTools.ColorDeficiency, ColorTools.ColorDeficiencySeverity);
                        else
                            transformed = Brettel1997.Transform(r, g, b, ColorTools.ColorDeficiency, ColorTools.ColorDeficiencySeverity);
                        r = transformed.Item1;
                        g = transformed.Item2;
                        b = transformed.Item3;
                    }

                    // Form the sequences
                    PlainSequence = $"{r};{g};{b}";
                    PlainSequenceEnclosed = $"{r};{g};{b}".EncloseByDoubleQuotes();
                    VTSequenceForeground = Color255.GetEsc() + $"[38;2;{PlainSequence}m";
                    VTSequenceBackground = Color255.GetEsc() + $"[48;2;{PlainSequence}m";

                    // Populate color properties
                    Type = ColorType.TrueColor;
                    IsBright = Convert.ToDouble(r) + 0.2126d + Convert.ToDouble(g) + 0.7152d + Convert.ToDouble(b) + 0.0722d > 255d / 2d;
                    IsDark = Convert.ToDouble(r) + 0.2126d + Convert.ToDouble(g) + 0.7152d + Convert.ToDouble(b) + 0.0722d < 255d / 2d;
                    R = r;
                    G = g;
                    B = b;
                }
                else
                {
                    throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");
                }
            }
            else if (double.TryParse(ColorSpecifier, out _) || Enum.IsDefined(typeof(ConsoleColors), ColorSpecifier))
            {
                // Form the sequences using the information from the color details
                var ColorsInfo = new ConsoleColorsInfo((ConsoleColors)Enum.Parse(typeof(ConsoleColors), ColorSpecifier));

                // Check to see if we need to transform color. Else, be sane.
                int r = Convert.ToInt32(ColorsInfo.R);
                int g = Convert.ToInt32(ColorsInfo.G);
                int b = Convert.ToInt32(ColorsInfo.B);
                if (ColorTools.EnableColorTransformation)
                {
                    // We'll transform.
                    (int, int, int) transformed;
                    if (ColorTools.EnableSimpleColorTransformation)
                        transformed = Vienot1999.Transform(r, g, b, ColorTools.ColorDeficiency, ColorTools.ColorDeficiencySeverity);
                    else
                        transformed = Brettel1997.Transform(r, g, b, ColorTools.ColorDeficiency, ColorTools.ColorDeficiencySeverity);
                    r = transformed.Item1;
                    g = transformed.Item2;
                    b = transformed.Item3;
                }
                PlainSequence = ColorTools.EnableColorTransformation ? $"{r};{g};{b}" : $"{ColorsInfo.ColorID}";
                PlainSequenceEnclosed = ColorTools.EnableColorTransformation ? $"{r};{g};{b}".EncloseByDoubleQuotes() : $"{ColorsInfo.ColorID}";
                VTSequenceForeground = ColorTools.EnableColorTransformation ? Color255.GetEsc() + $"[38;2;{PlainSequence}m" : Color255.GetEsc() + $"[38;5;{PlainSequence}m";
                VTSequenceBackground = ColorTools.EnableColorTransformation ? Color255.GetEsc() + $"[48;2;{PlainSequence}m" : Color255.GetEsc() + $"[48;5;{PlainSequence}m";

                // Populate color properties
                Type = ColorTools.EnableColorTransformation ? ColorType.TrueColor : ColorsInfo.ColorID >= 16 ? ColorType._255Color : ColorType._16Color;
                IsBright = ColorTools.EnableColorTransformation ? Convert.ToDouble(r) + 0.2126d + Convert.ToDouble(g) + 0.7152d + Convert.ToDouble(b) + 0.0722d > 255d / 2d : ColorsInfo.IsBright;
                IsDark = ColorTools.EnableColorTransformation ? Convert.ToDouble(r) + 0.2126d + Convert.ToDouble(g) + 0.7152d + Convert.ToDouble(b) + 0.0722d < 255d / 2d : ColorsInfo.IsDark;
                R = r;
                G = g;
                B = b;
            }
            else if (ColorSpecifier.StartsWith("#"))
            {
                int ColorDecimal = Convert.ToInt32(ColorSpecifier.RemoveLetter(0), 16);

                // Convert the RGB values to numbers
                R = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
                G = (byte)((ColorDecimal & 0xFF00) >> 8);
                B = (byte)(ColorDecimal & 0xFF);
                // First, check to see if we need to filter the color for the color-blind
                if (ColorTools.EnableColorTransformation)
                {
                    // We'll transform.
                    (int, int, int) transformed;
                    if (ColorTools.EnableSimpleColorTransformation)
                        transformed = Vienot1999.Transform(R, G, B, ColorTools.ColorDeficiency, ColorTools.ColorDeficiencySeverity);
                    else
                        transformed = Brettel1997.Transform(R, G, B, ColorTools.ColorDeficiency, ColorTools.ColorDeficiencySeverity);
                    R = transformed.Item1;
                    G = transformed.Item2;
                    B = transformed.Item3;
                }

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

            // We got the RGB values! First, check to see if we need to filter the color for the color-blind
            int r = Convert.ToInt32(R);
            int g = Convert.ToInt32(G);
            int b = Convert.ToInt32(B);
            if (ColorTools.EnableColorTransformation)
            {
                // We'll transform.
                (int, int, int) transformed;
                if (ColorTools.EnableSimpleColorTransformation)
                    transformed = Vienot1999.Transform(r, g, b, ColorTools.ColorDeficiency, ColorTools.ColorDeficiencySeverity);
                else
                    transformed = Brettel1997.Transform(r, g, b, ColorTools.ColorDeficiency, ColorTools.ColorDeficiencySeverity);
                r = transformed.Item1;
                g = transformed.Item2;
                b = transformed.Item3;
            }
            PlainSequence = $"{r};{g};{b}";
            PlainSequenceEnclosed = $"{r};{g};{b}".EncloseByDoubleQuotes();
            VTSequenceForeground = Color255.GetEsc() + $"[38;2;{PlainSequence}m";
            VTSequenceBackground = Color255.GetEsc() + $"[48;2;{PlainSequence}m";

            // Populate color properties
            Type = ColorType.TrueColor;
            IsBright = r + 0.2126d + g + 0.7152d + b + 0.0722d > 255d / 2d;
            IsDark = r + 0.2126d + g + 0.7152d + b + 0.0722d < 255d / 2d;
            this.R = r;
            this.G = g;
            this.B = b;

            // Populate the hexadecimal representation of the color
            Hex = $"#{r:X2}{g:X2}{b:X2}";
        }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorDef">The color taken from <see cref="ConsoleColors"/></param>
        /// <exception cref="Exceptions.ColorException"></exception>
        public Color(ConsoleColors ColorDef)
            : this(Convert.ToInt32(ColorDef)) { }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorNum">The color number</param>
        /// <exception cref="Exceptions.ColorException"></exception>
        public Color(int ColorNum)
        {
            // Form the sequences using the information from the color details
            var ColorsInfo = new ConsoleColorsInfo((ConsoleColors)Enum.Parse(typeof(ConsoleColors), ColorNum.ToString()));

            // Check to see if we need to transform color. Else, be sane.
            int r = Convert.ToInt32(ColorsInfo.R);
            int g = Convert.ToInt32(ColorsInfo.G);
            int b = Convert.ToInt32(ColorsInfo.B);
            if (ColorTools.EnableColorTransformation)
            {
                // We'll transform.
                (int, int, int) transformed;
                if (ColorTools.EnableSimpleColorTransformation)
                    transformed = Vienot1999.Transform(r, g, b, ColorTools.ColorDeficiency, ColorTools.ColorDeficiencySeverity);
                else
                    transformed = Brettel1997.Transform(r, g, b, ColorTools.ColorDeficiency, ColorTools.ColorDeficiencySeverity);
                r = transformed.Item1;
                g = transformed.Item2;
                b = transformed.Item3;
            }
            PlainSequence = ColorTools.EnableColorTransformation ? $"{r};{g};{b}" : $"{ColorsInfo.ColorID}";
            PlainSequenceEnclosed = ColorTools.EnableColorTransformation ? $"{r};{g};{b}".EncloseByDoubleQuotes() : $"{ColorsInfo.ColorID}";
            VTSequenceForeground = ColorTools.EnableColorTransformation ? Color255.GetEsc() + $"[38;2;{PlainSequence}m" : Color255.GetEsc() + $"[38;5;{PlainSequence}m";
            VTSequenceBackground = ColorTools.EnableColorTransformation ? Color255.GetEsc() + $"[48;2;{PlainSequence}m" : Color255.GetEsc() + $"[48;5;{PlainSequence}m";

            // Populate color properties
            Type = ColorTools.EnableColorTransformation ? ColorType.TrueColor : ColorsInfo.ColorID >= 16 ? ColorType._255Color : ColorType._16Color;
            IsBright = ColorTools.EnableColorTransformation ? Convert.ToDouble(r) + 0.2126d + Convert.ToDouble(g) + 0.7152d + Convert.ToDouble(b) + 0.0722d > 255d / 2d : ColorsInfo.IsBright;
            IsDark = ColorTools.EnableColorTransformation ? Convert.ToDouble(r) + 0.2126d + Convert.ToDouble(g) + 0.7152d + Convert.ToDouble(b) + 0.0722d < 255d / 2d : ColorsInfo.IsDark;
            R = r;
            G = g;
            B = b;

            // Populate the hexadecimal representation of the color
            Hex = $"#{R:X2}{G:X2}{B:X2}";
        }
    }
}
