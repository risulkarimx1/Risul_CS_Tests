using System;
using System.Collections.Generic;

namespace RisulGamigoTest.Problem2_SortArrayWithPattern
{
    public class LetterSorting
    {
        public void SortLetters(byte[] inputAndOutput, byte[] sortOrder)
        {
            if(inputAndOutput.Length == 0 && sortOrder.Length ==0) return;
            
            var occurrence = new Dictionary<byte, int>();
            foreach (var b in inputAndOutput)
            {
                if (occurrence.ContainsKey(b) == false)
                {
                    occurrence.Add(b,0);
                }

                occurrence[b]++;
            }

            var index = 0;
            foreach (var b in sortOrder)
            {
                if (occurrence.TryGetValue(b, out int lengthOfOccurrence) == false)
                {
                    throw new InvalidOperationException();
                } 
                
                for (int i = 0; i < lengthOfOccurrence; i++)
                {
                    inputAndOutput[index] = b;
                    index++;
                }
            }
        }

    }
}
