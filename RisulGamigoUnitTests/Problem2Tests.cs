using System;
using NUnit.Framework;
using RisulGamigoTest.Problem2_SortArrayWithPattern;

namespace RisulGamigoUnitTests
{
    [TestFixture]
    public class Problem2Tests
    {
        private LetterSorting _letterSorting;
        [SetUp]
        public void Setup()
        {
            _letterSorting = new LetterSorting();
        }

        [Test]
        public void InputEmpty_OutputEmpty()
        {
            string inputStr = "";
            string sortStr = "";
            byte[] inputAndOutputBytes = System.Text.Encoding.ASCII.GetBytes(inputStr);
            byte[] sortBytes = System.Text.Encoding.ASCII.GetBytes(sortStr);
            var expectedOutput = "";
            _letterSorting.SortLetters(inputAndOutputBytes, sortBytes);

            var obtainedOutput = System.Text.Encoding.ASCII.GetString(inputAndOutputBytes);
            Assert.AreEqual(obtainedOutput, expectedOutput);
        }

        [Test]
        public void Input_mdrisulkarim_seq_silkmdrau_output_siilkmdrrau()
        {
            string inputStr = "mdrisulkarim";
            string sortStr = "silkmdrau";
            byte[] inputAndOutputBytes = System.Text.Encoding.ASCII.GetBytes(inputStr);
            byte[] sortBytes = System.Text.Encoding.ASCII.GetBytes(sortStr);
            var expectedOutput = "siilkmmdrrau";
            _letterSorting.SortLetters(inputAndOutputBytes, sortBytes);

            var obtainedOutput = System.Text.Encoding.ASCII.GetString(inputAndOutputBytes);
            Assert.AreEqual(obtainedOutput, expectedOutput);
        }

        [Test]
        public void Invalid_Sequence_Input_risul_Sequence_aeiou()
        {
            string inputStr = "risul";
            string sortStr = "aeiou";
            byte[] inputAndOutputBytes = System.Text.Encoding.ASCII.GetBytes(inputStr);
            byte[] sortBytes = System.Text.Encoding.ASCII.GetBytes(sortStr);
            Assert.Throws<InvalidOperationException>(()=> _letterSorting.SortLetters(inputAndOutputBytes, sortBytes));
        }
        
        [Test]
        public void Inp_trion_world_network_Seq_oinewkrtdl_Out_oooinnewwkrrrttdl()
        {
            string inputStr = "trion world network";
            string sortStr = " oinewkrtdl";
            byte[] inputAndOutputBytes = System.Text.Encoding.ASCII.GetBytes(inputStr);
            byte[] sortBytes = System.Text.Encoding.ASCII.GetBytes(sortStr);
            var expectedOutput = "  oooinnewwkrrrttdl";
            
           _letterSorting.SortLetters(inputAndOutputBytes, sortBytes);
           var obtainedOutput = System.Text.Encoding.ASCII.GetString(inputAndOutputBytes);
           Assert.AreEqual(obtainedOutput, expectedOutput);
        }
    }
}