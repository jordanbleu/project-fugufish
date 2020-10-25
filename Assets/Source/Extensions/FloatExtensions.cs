using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Source.Extensions
{
    public static class FloatExtensions
    {

        /// <summary>
        /// Returns true if this is within '<paramref name="units"/>' of '<paramref name="ofValue"/>' 
        /// (plus or minus)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="units"></param>
        /// <param name="ofValue"></param>
        /// <returns></returns>
        public static bool IsWithin(this float value, float units, float ofValue)
        {
            var min = ofValue - units;
            var max = ofValue + units;
            return (value >= min) && (value <= max);
        }


        /// <summary>
        /// Transforms gradually transforms this number to the normalized value.  Useful for slowly
        /// decelerating a velocity value. 
        /// </summary>
        /// <param name="value">The current value</param>
        /// <param name="stabilizationRate">how quickly to move towards the normalized value</param>
        /// <param name="stabilzedValue">The value we are moving towards</param>
        /// <returns></returns>
        public static float Stabilize(this float value, float stabilizationRate, float stabilzedValue)
        {

            float newValue = value;

            if (value.IsWithin(stabilizationRate, stabilzedValue))
            {
                newValue = stabilzedValue;
            }
            else if (value > stabilzedValue)
            {
                newValue -= stabilizationRate;
            }
            else if (value < stabilzedValue)
            {
                newValue += stabilizationRate;
            }

            return newValue;
        }
    }
}
