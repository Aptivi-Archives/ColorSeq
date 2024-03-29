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

using System;

namespace ColorSeq.Accessibility
{
    // Refer to Hans Brettel, Françoise Viénot, and John D. Mollon, "Computerized simulation of color appearance for dichromats," J. Opt. Soc. Am. A 14, 2647-2655 (1997)
    // for more information.
    internal static class Brettel1997
    {
        static readonly BrettelParameters bp_protan = new()
        {
            TransPlane1 = new double[9]
            {
                0.14980, 1.19548, -0.34528,
                0.10764, 0.84864, 0.04372,
                0.00384, -0.00540, 1.00156
            },
            TransPlane2 = new double[9]
            {
                0.14570, 1.16172, -0.30742,
                0.10816, 0.85291, 0.03892,
                0.00386, -0.00524, 1.00139
            },
            SeparationPlaneNormalRGB = new double[3]
            {
                0.00048, 0.00393, -0.00441
            }
        };

        static readonly BrettelParameters bp_deutan = new()
        {
            TransPlane1 = new double[9]
            {
                0.36477, 0.86381, -0.22858,
                0.26294, 0.64245, 0.09462,
                -0.02006, 0.02728, 0.99278
            },
            TransPlane2 = new double[9]
            {
                0.37298, 0.88166, -0.25464,
                0.25954, 0.63506, 0.10540,
                -0.01980, 0.02784, 0.99196
            },
            SeparationPlaneNormalRGB = new double[3]
            {
                -0.00281, -0.00611, 0.00892
            }
        };

        static readonly BrettelParameters bp_tritan = new()
        {
            TransPlane1 = new double[9]
            {
                1.01277, 0.13548, -0.14826,
                -0.01243, 0.86812, 0.14431,
                0.07589, 0.80500, 0.11911
            },
            TransPlane2 = new double[9]
            {
                0.93678, 0.18979, -0.12657,
                0.06154, 0.81526, 0.12320,
                -0.37562, 1.12767, 0.24796
            },
            SeparationPlaneNormalRGB = new double[3]
            {
                0.03901, -0.02788, -0.01113
            }
        };

        public static (int, int, int) Transform(int r, int g, int b, Deficiency def, double severity)
        {
            // Check values
            if (r < 0 || r > 255)
                throw new ArgumentOutOfRangeException("r");
            if (g < 0 || g > 255)
                throw new ArgumentOutOfRangeException("g");
            if (b < 0 || b > 255)
                throw new ArgumentOutOfRangeException("b");
            if (severity < 0.0d || severity > 1.0d)
                throw new ArgumentOutOfRangeException("severity");

            // Select what Brettel deficiency profile to choose how to transform the three RGB values
            BrettelParameters bp = null;
            switch (def)
            {
                case Deficiency.Protan:
                    bp = bp_protan;
                    break;
                case Deficiency.Deutan:
                    bp = bp_deutan;
                    break;
                case Deficiency.Tritan:
                    bp = bp_tritan;
                    break;
            }

            // Get linear RGB from these three RGB values
            double[] linears = new double[3]
            {
                ColorTools.SRGBToLinearRGB(r),
                ColorTools.SRGBToLinearRGB(g),
                ColorTools.SRGBToLinearRGB(b)
            };

            // Select deficiency plane as appropriate
            var spn = bp.SeparationPlaneNormalRGB;
            double projection = linears[0] * spn[0] + linears[1] * spn[1] + linears[2] * spn[2];
            var defPlane = projection >= 0 ? bp.TransPlane1 : bp.TransPlane2;
            double[] rgbMatrix = new double[3]
            {
                defPlane[0]*linears[0] + defPlane[1]*linears[1] + defPlane[2]*linears[2],
                defPlane[3]*linears[0] + defPlane[4]*linears[1] + defPlane[5]*linears[2],
                defPlane[6]*linears[0] + defPlane[7]*linears[1] + defPlane[8]*linears[2],
            };

            // Transform the colors with the severity rate in a linear transform method
            rgbMatrix[0] = rgbMatrix[0] * severity + linears[0] * (1d - severity);
            rgbMatrix[1] = rgbMatrix[1] * severity + linears[1] * (1d - severity);
            rgbMatrix[2] = rgbMatrix[2] * severity + linears[2] * (1d - severity);

            // Convert these values back to sRGB
            int sRGB_R = ColorTools.LinearRGBTosRGB(rgbMatrix[0]);
            int sRGB_G = ColorTools.LinearRGBTosRGB(rgbMatrix[1]);
            int sRGB_B = ColorTools.LinearRGBTosRGB(rgbMatrix[2]);
            return (sRGB_R, sRGB_G, sRGB_B);
        }
    }
}
