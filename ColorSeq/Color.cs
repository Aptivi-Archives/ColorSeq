﻿/*
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
using System;
using System.Collections.Generic;

namespace ColorSeq
{
    public class Color : IEquatable<Color>
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
        /// The color value converted to <see cref="ConsoleColors"/>. Not applicable [-1] to true color
        /// </summary>
        public ConsoleColors ColorEnum255 { get; private set; } = (ConsoleColors)(-1);
        /// <summary>
        /// The color value converted to <see cref="ConsoleColor"/>. Not applicable [-1] to true color and 256 colors
        /// </summary>
        public ConsoleColor ColorEnum16 { get; private set; } = (ConsoleColor)(-1);
        /// <summary>
        /// Empty color singleton
        /// </summary>
        public static Color Empty
        {
            get
            {
                // Get cached value if cached
                if (ColorTools._empty is not null)
                    return ColorTools._empty;

                // Else, cache the empty value and return it
                ColorTools._empty = new Color(0);
                return ColorTools._empty;
            }
        }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <exception cref="Exceptions.ColorException"></exception>
        public Color(int R, int G, int B)
            : this($"{R};{G};{B}") { }

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
        /// <param name="ColorDef">The color taken from <see cref="ConsoleColor"/></param>
        /// <exception cref="Exceptions.ColorException"></exception>
        public Color(ConsoleColor ColorDef)
            : this(Convert.ToInt32(ColorDef)) { }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorNum">The color number</param>
        /// <exception cref="Exceptions.ColorException"></exception>
        public Color(int ColorNum)
            : this($"{ColorNum}") { }

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
                    if (r < 0 || r > 255)
                        throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");
                    int g = Convert.ToInt32(ColorSpecifierArray[1]);
                    if (g < 0 || g > 255)
                        throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");
                    int b = Convert.ToInt32(ColorSpecifierArray[2]);
                    if (b < 0 || b > 255)
                        throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");
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
                    PlainSequenceEnclosed = $"\"{r};{g};{b}\"";
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
            else if (double.TryParse(ColorSpecifier, out double specifierNum) && specifierNum <= 255 || Enum.IsDefined(typeof(ConsoleColors), ColorSpecifier))
            {
                // Form the sequences using the information from the color details
                var parsedEnum = (ConsoleColors)Enum.Parse(typeof(ConsoleColors), ColorSpecifier);
                var parsedEnum16 = specifierNum <= 15 ? (ConsoleColor)Enum.Parse(typeof(ConsoleColor), ColorSpecifier) : default;
                var ColorsInfo = new ConsoleColorsInfo(parsedEnum);

                // Check to see if we need to transform color. Else, be sane.
                int r = Convert.ToInt32(ColorsInfo.R);
                if (r < 0 || r > 255)
                    throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");
                int g = Convert.ToInt32(ColorsInfo.G);
                if (g < 0 || g > 255)
                    throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");
                int b = Convert.ToInt32(ColorsInfo.B);
                if (b < 0 || b > 255)
                    throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");
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
                PlainSequenceEnclosed = ColorTools.EnableColorTransformation ? $"\"{r};{g};{b}\"" : $"{ColorsInfo.ColorID}";
                VTSequenceForeground = ColorTools.EnableColorTransformation ? Color255.GetEsc() + $"[38;2;{PlainSequence}m" : Color255.GetEsc() + $"[38;5;{PlainSequence}m";
                VTSequenceBackground = ColorTools.EnableColorTransformation ? Color255.GetEsc() + $"[48;2;{PlainSequence}m" : Color255.GetEsc() + $"[48;5;{PlainSequence}m";

                // Populate color properties
                Type = ColorTools.EnableColorTransformation ? ColorType.TrueColor : ColorsInfo.ColorID >= 16 ? ColorType._255Color : ColorType._16Color;
                IsBright = ColorTools.EnableColorTransformation ? Convert.ToDouble(r) + 0.2126d + Convert.ToDouble(g) + 0.7152d + Convert.ToDouble(b) + 0.0722d > 255d / 2d : ColorsInfo.IsBright;
                IsDark = ColorTools.EnableColorTransformation ? Convert.ToDouble(r) + 0.2126d + Convert.ToDouble(g) + 0.7152d + Convert.ToDouble(b) + 0.0722d < 255d / 2d : ColorsInfo.IsDark;
                R = r;
                G = g;
                B = b;
                ColorEnum255 = Type == ColorType._255Color ? parsedEnum : (ConsoleColors)(-1);
                ColorEnum16 = Type == ColorType._16Color ? parsedEnum16 : (ConsoleColor)(-1);
            }
            else if (ColorSpecifier.StartsWith("#"))
            {
                string finalSpecifier = ColorSpecifier.Substring(1);
                int ColorDecimal = Convert.ToInt32(finalSpecifier, 16);

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
                PlainSequenceEnclosed = $"\"{R};{G};{B}\"";
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

        public override bool Equals(object obj) =>
            base.Equals(obj);

        /// <summary>
        /// Checks to see if this instance of the color is equal to another instance of the color
        /// </summary>
        /// <param name="other">Another instance of the color to compare with this color</param>
        /// <returns>True if both the colors match; otherwise, false.</returns>
        public bool Equals(Color other)
            => Equals(this, other);

        /// <summary>
        /// Checks to see if the first instance of the color is equal to another instance of the color
        /// </summary>
        /// <param name="other">Another instance of the color to compare with another</param>
        /// <param name="other2">Another instance of the color to compare with another</param>
        /// <returns>True if both the colors match; otherwise, false.</returns>
        public bool Equals(Color other, Color other2)
        {
            // We can't perform this operation on null.
            if (other is null)
                return false;

            // Check all the properties
            return
                other.PlainSequence == other2.PlainSequence &&
                other.PlainSequenceEnclosed == other2.PlainSequenceEnclosed &&
                other.VTSequenceForeground == other2.VTSequenceForeground &&
                other.VTSequenceBackground == other2.VTSequenceBackground &&
                other.R == other2.R &&
                other.G == other2.G &&
                other.B == other2.B &&
                other.Hex == other2.Hex &&
                other.Type == other2.Type &&
                other.IsBright == other2.IsBright &&
                other.IsDark == other2.IsDark &&
                other.ColorEnum255 == other2.ColorEnum255 &&
                other.ColorEnum16 == other2.ColorEnum16
            ;
        }

        public static bool operator ==(Color a, Color b)
            => a.Equals(b);

        public static bool operator !=(Color a, Color b)
            => !a.Equals(b);

        public override int GetHashCode()
        {
            int hashCode = 746924978;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PlainSequence);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PlainSequenceEnclosed);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(VTSequenceForeground);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(VTSequenceBackground);
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Hex);
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + IsBright.GetHashCode();
            hashCode = hashCode * -1521134295 + IsDark.GetHashCode();
            hashCode = hashCode * -1521134295 + ColorEnum255.GetHashCode();
            hashCode = hashCode * -1521134295 + ColorEnum16.GetHashCode();
            return hashCode;
        }
    }
}
