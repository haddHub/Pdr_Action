using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    /// <summary>
    /// Frame encoding class. Contains static methods for frame encoding and decoding.
    /// </summary>
    internal class FrameEncoding
    {
        // TODO : mise en forme d'une trame
        /// <summary>
        /// Encodes packet with consecutive right shifts so that the msb of each encoded byte is clear. The msb of the final byte is set to indicate the end of the packet.
        /// </summary>
        /// <param name="decodedPacket">
        /// The decoded packet contents to be encoded.
        /// </param>
        /// <returns>
        /// The encoded packet.
        /// </returns> 
        public static byte[] EncodeFrame(byte[] decodedFrame)
        {
            //byte[] encodedPacket = new byte[(int)(Math.Ceiling((((float)decodedPacket.Length * 1.125f)) + 0.125f))];
            //byte[] shiftRegister = new byte[encodedPacket.Length];
            //Array.Copy(decodedPacket, shiftRegister, decodedPacket.Length);     // copy encoded packet to shift register
            //for (int i = 0; i < encodedPacket.Length; i++)
            //{
            //    encodedPacket[i] = shiftRegister[i];                            // store encoded byte i
            //    shiftRegister[i] = 0;                                           // clear byte i in shift register
            //}
            //encodedPacket[encodedPacket.Length - 1] |= 0x80;                    // set msb of framing byte
            return decodedFrame;
        }

        /// <summary>
        /// Decodes a packet with consecutive left shifts so that the msb of each encoded byte is removed.
        /// </summary>
        /// <param name="encodedPacket">
        /// The endcoded packet to be decoded.
        /// </param>
        /// <returns>
        /// The decoded packet.
        /// </returns> 
        public static byte[] DecodeFrame(byte[] encodedPacket)
        {
            byte[] decodedPacket = new byte[(int)(Math.Floor(((float)encodedPacket.Length - 0.125f) / 1.125f))];
            byte[] shiftRegister = new byte[encodedPacket.Length];
            for (int i = shiftRegister.Length - 1; i >= 0; i--)
            {
                shiftRegister[i] = encodedPacket[i];
                //shiftRegister = LeftShiftByteArray(shiftRegister);
            }
            Array.Copy(shiftRegister, decodedPacket, decodedPacket.Length);
            return decodedPacket;
        }
    }
}
