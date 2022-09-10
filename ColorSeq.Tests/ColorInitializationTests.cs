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
        }

        /// <summary>
        /// Tests initializing color instance from 16 colors
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeColorInstanceFrom16Colors()
        {
            // Create instance
            var ColorInstance = new Color(13);

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
        }
    }
}