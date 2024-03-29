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

using Shouldly;

namespace ColorSeq.Tests
{
    [TestFixture]
    public partial class ColorInitializationTests
    {
        [SetUp]
        public void ResetColorDeficiency()
        {
            ColorTools.EnableColorTransformation = false;
            ColorTools.EnableSimpleColorTransformation = false;
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255Colors()
        {
            // Create instance
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("18");
            ColorInstance.Type.ShouldBe(ColorType._255Color);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;5;18m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;5;18m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(135);
            ColorInstance.IsBright.ShouldBeTrue();
            ColorInstance.IsDark.ShouldBeFalse();
            ColorInstance.Hex.ShouldBe("#000087");
            ColorInstance.ColorEnum255.ShouldBe(ConsoleColors.DarkBlue_000087);
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Protanopia)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsProtanopia()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorDeficiency = Accessibility.Deficiency.Protan;
            ColorTools.ColorDeficiencySeverity = 1.0;
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;24;135");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;2;0;24;135m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;2;0;24;135m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(24);
            ColorInstance.B.ShouldBe(135);
            ColorInstance.IsBright.ShouldBeTrue();
            ColorInstance.IsDark.ShouldBeFalse();
            ColorInstance.Hex.ShouldBe("#001887");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Protanomaly)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsProtanomaly()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorDeficiency = Accessibility.Deficiency.Protan;
            ColorTools.ColorDeficiencySeverity = 0.6;
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;17;135");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;2;0;17;135m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;2;0;17;135m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(17);
            ColorInstance.B.ShouldBe(135);
            ColorInstance.IsBright.ShouldBeTrue();
            ColorInstance.IsDark.ShouldBeFalse();
            ColorInstance.Hex.ShouldBe("#001187");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Deuteranopia)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsDeuteranopia()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorDeficiency = Accessibility.Deficiency.Deutan;
            ColorTools.ColorDeficiencySeverity = 1.0;
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;41;134");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;2;0;41;134m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;2;0;41;134m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(41);
            ColorInstance.B.ShouldBe(134);
            ColorInstance.IsBright.ShouldBeTrue();
            ColorInstance.IsDark.ShouldBeFalse();
            ColorInstance.Hex.ShouldBe("#002986");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Deuteranomaly)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsDeuteranomaly()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorDeficiency = Accessibility.Deficiency.Deutan;
            ColorTools.ColorDeficiencySeverity = 0.6;
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;31;134");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;2;0;31;134m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;2;0;31;134m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(31);
            ColorInstance.B.ShouldBe(134);
            ColorInstance.IsBright.ShouldBeTrue();
            ColorInstance.IsDark.ShouldBeFalse();
            ColorInstance.Hex.ShouldBe("#001F86");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Tritanopia)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsTritanopia()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorDeficiency = Accessibility.Deficiency.Tritan;
            ColorTools.ColorDeficiencySeverity = 1.0;
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;48;69");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;2;0;48;69m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;2;0;48;69m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(48);
            ColorInstance.B.ShouldBe(69);
            ColorInstance.IsBright.ShouldBeFalse();
            ColorInstance.IsDark.ShouldBeTrue();
            ColorInstance.Hex.ShouldBe("#003045");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Tritanomaly)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsTritanomaly()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorDeficiency = Accessibility.Deficiency.Tritan;
            ColorTools.ColorDeficiencySeverity = 0.6;
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;36;102");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;2;0;36;102m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;2;0;36;102m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(36);
            ColorInstance.B.ShouldBe(102);
            ColorInstance.IsBright.ShouldBeTrue();
            ColorInstance.IsDark.ShouldBeFalse();
            ColorInstance.Hex.ShouldBe("#002466");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Protanopia, Vienot)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsProtanopiaSimple()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.EnableSimpleColorTransformation = true;
            ColorTools.ColorDeficiency = Accessibility.Deficiency.Protan;
            ColorTools.ColorDeficiencySeverity = 1.0;
            var ColorInstance = new Color(10);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("241;241;0");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;2;241;241;0m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;2;241;241;0m");
            ColorInstance.R.ShouldBe(241);
            ColorInstance.G.ShouldBe(241);
            ColorInstance.B.ShouldBe(0);
            ColorInstance.IsBright.ShouldBeTrue();
            ColorInstance.IsDark.ShouldBeFalse();
            ColorInstance.Hex.ShouldBe("#F1F100");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Protanomaly, Vienot)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsProtanomalySimple()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.EnableSimpleColorTransformation = true;
            ColorTools.ColorDeficiency = Accessibility.Deficiency.Protan;
            ColorTools.ColorDeficiencySeverity = 0.6;
            var ColorInstance = new Color(10);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("192;247;0");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;2;192;247;0m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;2;192;247;0m");
            ColorInstance.R.ShouldBe(192);
            ColorInstance.G.ShouldBe(247);
            ColorInstance.B.ShouldBe(0);
            ColorInstance.IsBright.ShouldBeTrue();
            ColorInstance.IsDark.ShouldBeFalse();
            ColorInstance.Hex.ShouldBe("#C0F700");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Deuteranopia, Vienot)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsDeuteranopiaSimple()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.EnableSimpleColorTransformation = true;
            ColorTools.ColorDeficiency = Accessibility.Deficiency.Deutan;
            ColorTools.ColorDeficiencySeverity = 1.0;
            var ColorInstance = new Color(10);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("218;218;41");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;2;218;218;41m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;2;218;218;41m");
            ColorInstance.R.ShouldBe(218);
            ColorInstance.G.ShouldBe(218);
            ColorInstance.B.ShouldBe(41);
            ColorInstance.IsBright.ShouldBeTrue();
            ColorInstance.IsDark.ShouldBeFalse();
            ColorInstance.Hex.ShouldBe("#DADA29");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Deuteranomaly, Vienot)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsDeuteranomalySimple()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.EnableSimpleColorTransformation = true;
            ColorTools.ColorDeficiency = Accessibility.Deficiency.Deutan;
            ColorTools.ColorDeficiencySeverity = 0.6;
            var ColorInstance = new Color(10);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("174;234;30");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;2;174;234;30m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;2;174;234;30m");
            ColorInstance.R.ShouldBe(174);
            ColorInstance.G.ShouldBe(234);
            ColorInstance.B.ShouldBe(30);
            ColorInstance.IsBright.ShouldBeTrue();
            ColorInstance.IsDark.ShouldBeFalse();
            ColorInstance.Hex.ShouldBe("#AEEA1E");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Tritanopia, Vienot)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsTritanopiaSimple()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.EnableSimpleColorTransformation = true;
            ColorTools.ColorDeficiency = Accessibility.Deficiency.Tritan;
            ColorTools.ColorDeficiencySeverity = 1.0;
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;51;51");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;2;0;51;51m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;2;0;51;51m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(51);
            ColorInstance.B.ShouldBe(51);
            ColorInstance.IsBright.ShouldBeFalse();
            ColorInstance.IsDark.ShouldBeTrue();
            ColorInstance.Hex.ShouldBe("#003333");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 255 colors (Tritanomaly, Vienot)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom255ColorsTritanomalySimple()
        {
            // Create instance
            ColorTools.EnableColorTransformation = true;
            ColorTools.EnableSimpleColorTransformation = true;
            ColorTools.ColorDeficiency = Accessibility.Deficiency.Tritan;
            ColorTools.ColorDeficiencySeverity = 0.6;
            var ColorInstance = new Color(18);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0;39;96");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;2;0;39;96m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;2;0;39;96m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(39);
            ColorInstance.B.ShouldBe(96);
            ColorInstance.IsBright.ShouldBeTrue();
            ColorInstance.IsDark.ShouldBeFalse();
            ColorInstance.Hex.ShouldBe("#002760");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from 16 colors
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom16Colors()
        {
            // Create instance
            var ColorInstance = new Color(ConsoleColors.Magenta);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("13");
            ColorInstance.Type.ShouldBe(ColorType._16Color);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;5;13m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;5;13m");
            ColorInstance.R.ShouldBe(255);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(255);
            ColorInstance.IsBright.ShouldBeTrue();
            ColorInstance.IsDark.ShouldBeFalse();
            ColorInstance.Hex.ShouldBe("#FF00FF");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Magenta);
        }

        /// <summary>
        /// Tests initializing color instance from true color
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColor()
        {
            // Create instance
            var ColorInstance = new Color("94;0;63");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("94;0;63");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;2;94;0;63m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;2;94;0;63m");
            ColorInstance.R.ShouldBe(94);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(63);
            ColorInstance.IsBright.ShouldBeTrue();
            ColorInstance.IsDark.ShouldBeFalse();
            ColorInstance.Hex.ShouldBe("#5E003F");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color numbers
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromTrueColorNumbers()
        {
            // Create instance
            var ColorInstance = new Color(94, 0, 63);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("94;0;63");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;2;94;0;63m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;2;94;0;63m");
            ColorInstance.R.ShouldBe(94);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(63);
            ColorInstance.IsBright.ShouldBeTrue();
            ColorInstance.IsDark.ShouldBeFalse();
            ColorInstance.Hex.ShouldBe("#5E003F");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from true color
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromHex()
        {
            // Create instance
            var ColorInstance = new Color("#0F0F0F");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("15;15;15");
            ColorInstance.Type.ShouldBe(ColorType.TrueColor);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;2;15;15;15m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;2;15;15;15m");
            ColorInstance.R.ShouldBe(15);
            ColorInstance.G.ShouldBe(15);
            ColorInstance.B.ShouldBe(15);
            ColorInstance.IsBright.ShouldBeFalse();
            ColorInstance.IsDark.ShouldBeTrue();
            ColorInstance.Hex.ShouldBe("#0F0F0F");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe((ConsoleColor)(-1));
        }

        /// <summary>
        /// Tests initializing color instance from color name taken from <see cref="ConsoleColors"/>
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromName()
        {
            // Create instance
            var ColorInstance = new Color("Magenta");

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("13");
            ColorInstance.Type.ShouldBe(ColorType._16Color);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;5;13m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;5;13m");
            ColorInstance.R.ShouldBe(255);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(255);
            ColorInstance.IsBright.ShouldBeTrue();
            ColorInstance.IsDark.ShouldBeFalse();
            ColorInstance.Hex.ShouldBe("#FF00FF");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Magenta);
        }

        /// <summary>
        /// Tests initializing color instance from enum
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromEnum()
        {
            // Create instance
            var ColorInstance = new Color(ConsoleColors.Magenta);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("13");
            ColorInstance.Type.ShouldBe(ColorType._16Color);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;5;13m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;5;13m");
            ColorInstance.R.ShouldBe(255);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(255);
            ColorInstance.IsBright.ShouldBeTrue();
            ColorInstance.IsDark.ShouldBeFalse();
            ColorInstance.Hex.ShouldBe("#FF00FF");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Magenta);
        }

        /// <summary>
        /// Tests initializing color instance from enum (16 colors)
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFromEnum16()
        {
            // Create instance
            var ColorInstance = new Color(ConsoleColor.Magenta);

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("13");
            ColorInstance.Type.ShouldBe(ColorType._16Color);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;5;13m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;5;13m");
            ColorInstance.R.ShouldBe(255);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(255);
            ColorInstance.IsBright.ShouldBeTrue();
            ColorInstance.IsDark.ShouldBeFalse();
            ColorInstance.Hex.ShouldBe("#FF00FF");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Magenta);
        }

        /// <summary>
        /// Tests getting empty color
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestGetEmptyColor()
        {
            // Create instance
            var ColorInstance = Color.Empty;

            // Check for null
            ColorInstance.ShouldNotBeNull();
            ColorInstance.PlainSequence.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceBackground.ShouldNotBeNullOrEmpty();
            ColorInstance.VTSequenceForeground.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ColorInstance.PlainSequence.ShouldBe("0");
            ColorInstance.Type.ShouldBe(ColorType._16Color);
            ColorInstance.VTSequenceBackground.ShouldBe(Color255.GetEsc() + "[48;5;0m");
            ColorInstance.VTSequenceForeground.ShouldBe(Color255.GetEsc() + "[38;5;0m");
            ColorInstance.R.ShouldBe(0);
            ColorInstance.G.ShouldBe(0);
            ColorInstance.B.ShouldBe(0);
            ColorInstance.IsBright.ShouldBeFalse();
            ColorInstance.IsDark.ShouldBeTrue();
            ColorInstance.Hex.ShouldBe("#000000");
            ColorInstance.ColorEnum255.ShouldBe((ConsoleColors)(-1));
            ColorInstance.ColorEnum16.ShouldBe(ConsoleColor.Black);
        }
    }
}