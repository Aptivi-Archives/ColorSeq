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

using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace ColorSeq
{
    public static class Color255
    {
        /// <summary>
        /// The 255 console colors data JSON token to get information about these colors
        /// </summary>
        public static readonly JToken ColorDataJson = JToken.Parse(Properties.Resources.ConsoleColorsData);

        /// <summary>
        /// [Windows] Sets console mode
        /// </summary>
        /// <param name="hConsoleHandle">Console Handle</param>
        /// <param name="mode">Mode</param>
        /// <returns>True if succeeded, false if failed</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);

        /// <summary>
        /// [Windows] Gets console mode
        /// </summary>
        /// <param name="handle">Console handle</param>
        /// <param name="mode">Mode</param>
        /// <returns>True if succeeded, false if failed</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleMode(IntPtr handle, out int mode);

        /// <summary>
        /// [Windows] Gets console handle
        /// </summary>
        /// <param name="handle">Handle number</param>
        /// <returns>True if succeeded, false if failed</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int handle);

        /// <summary>
        /// [Windows] Initializes 255 color support
        /// </summary>
        public static void Initialize255()
        {
            var handle = GetStdHandle(-11);
            GetConsoleMode(handle, out int mode);
            if (!(mode == 7))
                SetConsoleMode(handle, mode | 0x4);
        }

        /// <summary>
        /// A simplification for <see cref="Convert.ToChar(Integer)"/> function to return the ESC character
        /// </summary>
        /// <returns>ESC</returns>
        internal static char GetEsc() => Convert.ToChar(0x1B);
    }
}
