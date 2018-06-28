// <copyright file="BitShifter.cs" company="Team B">
//      Team B. All rights reserved.
// </copyright>

namespace UseCaseCore.UcIntern
{
    using System;

    /// <summary>
    /// Used to shift the bits of integers.
    /// </summary>
    public static class BitShifter
    {
        /// <summary>
        /// Performs a left shift of the bits and preserves the bits shifted out to the left by inserting them into the right.
        /// Source: <see href="https://msdn.microsoft.com/de-de/library/system.object.gethashcode(v=vs.110).aspx"/> (Date: 05.06.2018 16:35)
        /// </summary>
        /// <param name="value">The value to be shifted.</param>
        /// <param name="positions">The number positions to shift.</param>
        /// <returns>The shifted value.</returns>
        public static int ShiftAndWrap(int value, int positions)
        {
            positions = positions & 0x1F;

            // Save the existing bit pattern, but interpret it as an unsigned integer.
            uint number = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);

            // Preserve the bits to be discarded.
            uint wrapped = number >> (32 - positions);

            // Shift and wrap the discarded bits.
            return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
        }
    }
}
