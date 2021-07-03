using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class GenerationUtils
    {
        public static int ChoseItmGivenProbs(float RandomValue, float[] Probs)
        {
            var sum = Probs.Sum();
            var randomPoint = RandomValue * sum;
            for (var i = 0; i < Probs.Length; i++)
            {
                if(randomPoint < Probs[i])
                {
                    return i;

                }
                else
                {
                    randomPoint -= Probs[i];
                }
            }

            return Probs.Length - 1;
        }
        
    }
}