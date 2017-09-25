using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    /// <summary>
    /// Frame construction class. Contains static methods for frame construction and deconstruction.
    /// </summary>
    public class FrameConstruction
    {
        #region Frame construction

        /// <summary>
        /// Constructs an encoded command frame.
        /// </summary>
        /// <param name="commandCode">
        /// Command code. See <see cref="CommandCodes"/>.
        /// </param> 
        /// <returns>
        /// Constructed and encoded command frame.
        /// </returns> 
        public static byte[] ConstructCommandFrame(CommandCodes commandCode)
        {
            byte[] decodedFrame = new byte[12] { (byte)FrameStart.Sart1, (byte)FrameStart.Start2, (byte)commandCode, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, (byte)FrameStop.Stop1, (byte)FrameStop.Stop2 };
            decodedFrame = InsertChecksum(decodedFrame);
            return FrameEncoding.EncodeFrame(decodedFrame);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exerciceData"></param>
        /// <returns></returns>
        public static byte[] ConstructWriteExerciceDataFrame(FrameExerciceDataModel exerciceData)
        {
            byte[] decodedFrame = new byte[12] { (byte)FrameStart.Sart1, (byte)FrameStart.Start2, (byte)exerciceData.Address, 0x04, exerciceData.Data1, exerciceData.Data2, exerciceData.Data3, exerciceData.Data4, 0x00, 0x00, (byte)FrameStop.Stop1, (byte)FrameStop.Stop2 };
            // TODO : ajouter dans la trame l'id et les datas
            decodedFrame = InsertChecksum(decodedFrame);
            return FrameEncoding.EncodeFrame(decodedFrame);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configData"></param>
        /// <returns></returns>
        public static byte[] ConstructWriteConfigDataFrame(FrameConfigDataModel configData)
        {
            byte[] decodedFrame = new byte[12] { (byte)FrameStart.Sart1, (byte)FrameStart.Start2, (byte)configData.Address, 0x04, (byte)(configData.Data1_2 >> 8), (byte)(configData.Data1_2 & 0x00FF), (byte)(configData.Data3_4 >> 8), (byte)(configData.Data3_4 & 0x00FF), 0x00, 0x00, (byte)FrameStop.Stop1, (byte)FrameStop.Stop2 };
            // TODO : ajouter dans la trame l'id et les datas
            decodedFrame = InsertChecksum(decodedFrame);
            return FrameEncoding.EncodeFrame(decodedFrame);
        }

        // TODO : mettre un Consctruct Exo data frame pour la config des exercices.

        /// <summary>
        /// Inserts check sum at last position in array equal to the sum all of bytes preceding it.
        /// </summary>
        /// <param name="decodedPacket">
        /// Decoded frame byte array.
        /// </param>
        /// <returns>
        /// Decoded frame with checksum inserted at last position in array.
        /// </returns> 
        private static byte[] InsertChecksum(byte[] decodedFrame)
        {
            ushort POLY = 0x8408;
            ushort length = 12;
            byte i, j = 0;
            uint data;
            uint crc = 0xffff;
            decodedFrame[8] = 0x00;
            decodedFrame[9] = 0x00;

            if (length == 0)
                throw new Exception("InsertChecksum error : Invalid frame.");
            do
            {
                for (i = 0, data = (uint)0xff & decodedFrame[j++]; i < 8; i++, data >>= 1)
                {
                    if (((crc & 0x0001) ^ (data & 0x0001)) != 0)
                    {
                        crc = (crc >> 1) ^ POLY;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            } while ((--length) != 0);

            crc = ~crc;
            data = crc;
            crc = (crc << 8) | (data >> 8 & 0xff);
            decodedFrame[8] = (byte)(crc >> 8);
            decodedFrame[9] = (byte)(crc & 0xFF);
            return decodedFrame;
        }

        /// <summary>
        /// Deconstructs frame from and encoded byte array and return data object.
        /// </summary>
        /// <param name="encodedFrame">
        /// Byte array containing the encoded frame.
        /// </param>
        /// <returns>
        /// <see cref="xIMUdata"/> object deconstructed from frame.
        /// </returns>  
        /// <exception cref="System.Exception">
        /// Thrown when deconstruction of an invalid frame is attempted.
        /// </exception>
        public static aXdataModel DeconstructFrame(byte[] encodedFrame)
        {
            // Decode packet
            //if (encodedFrame.Length < 12)
            //{
            //    throw new Exception("Too few bytes in frame.");
            //}
            //if (encodedFrame.Length > 12)
            //{
            //    throw new Exception("Too many bytes in frame.");
            //}
            //byte[] decodedFrame = FrameEncoding.DecodeFrame(encodedFrame);
            byte[] decodedFrame = encodedFrame;

            // TODO : check ckecksum :)
            // Confirm checksum
            //byte checksum = 0;
            //for (int i = 0; i < (decodedPacket.Length - 1); i++) checksum += decodedPacket[i];
            //if (checksum != decodedPacket[decodedPacket.Length - 1])
            //{
            //    throw new Exception("Invalid checksum.");
            //}

            // Interpret frame according to header
            switch (decodedFrame[2])
            {
                case ((byte)FrameHeaders.Error):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new ErrorDataModel((int)Concat((byte)decodedFrame[6], (byte)decodedFrame[7]));
                case ((byte)FrameHeaders.Force):   // Force de référence
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new ForceDataModel((int)ConcatS((byte)decodedFrame[4], (byte)decodedFrame[5]), (int)ConcatS((byte)decodedFrame[6], (byte)decodedFrame[7]));
                case ((byte)FrameHeaders.Force2):   // Couple de référence
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new Force2DataModel((int)ConcatS((byte)decodedFrame[4], (byte)decodedFrame[5]), (int)ConcatS((byte)decodedFrame[6], (byte)decodedFrame[7]));
                case ((byte)FrameHeaders.Position):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new PositionDataModel((int)ConcatS(decodedFrame[4], decodedFrame[5]), (int)ConcatS(decodedFrame[6], decodedFrame[7])); // TODO : changer la forceREf ????
                case ((byte)FrameHeaders.Position2):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new Position2DataModel((int)ConcatS(decodedFrame[4], decodedFrame[5]), (int)ConcatS(decodedFrame[6], decodedFrame[7])); // TODO : changer la forceREf ????
                case ((byte)FrameHeaders.Ppr):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new PprDataModel((int)ConcatS(decodedFrame[4], decodedFrame[5]), (int)ConcatS(decodedFrame[6], decodedFrame[7]));
                case ((byte)FrameHeaders.Vitesse):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new VitesseModel((int)ConcatS(decodedFrame[4], decodedFrame[5]), (int)ConcatS(decodedFrame[6], decodedFrame[7])); // TODO : changer la forceREf ????
                case ((byte)FrameHeaders.Vitesse2):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new Vitesse2Model((int)ConcatS(decodedFrame[4], decodedFrame[5]), (int)ConcatS(decodedFrame[6], decodedFrame[7])); // TODO : changer la forceREf ????
                case ((byte)FrameHeaders.ACK_Xdent):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new ACKDataModel(decodedFrame);
                case ((byte)FrameHeaders.ACK_Stop):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new ACKDataModel(decodedFrame);
                case ((byte)FrameHeaders.ACK_mod_suiv_traj):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new ACKDataModel(decodedFrame);
                case ((byte)FrameHeaders.ACK_mode_libre):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new ACKDataModel(decodedFrame);
                case ((byte)FrameHeaders.ACK_mod_traj):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new ACKDataModel(decodedFrame);
                case ((byte)FrameHeaders.ACK_mod_homing):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new ACKDataModel(decodedFrame);
                case ((byte)FrameHeaders.ACK_mod_init_traj):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new ACKDataModel(decodedFrame);
                case ((byte)FrameHeaders.ACK_mod_home):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new ACKDataModel(decodedFrame);
                case ((byte)FrameHeaders.ACK_mod_game):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new ACKDataModel(decodedFrame);
                case ((byte)FrameHeaders.ACK_mod_hemi_pdr):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new ACKDataModel(decodedFrame);
                case ((byte)FrameHeaders.ACK_Cibles):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new ACKDataModel(decodedFrame);
                case ((byte)FrameHeaders.ACK_Formes):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new ACKDataModel(decodedFrame);
                case ((byte)FrameHeaders.ACK_Mouvements):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new ACKDataModel(decodedFrame);
                case ((byte)FrameHeaders.ACK_MasseVisco):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new FrameConfigDataModel(ConfigAddresses.MasseVisco, Concat(decodedFrame[4], decodedFrame[5]), Concat(decodedFrame[6], decodedFrame[7]));
                case ((byte)FrameHeaders.ACK_KlatClong):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new FrameConfigDataModel(ConfigAddresses.KlatClong, Concat(decodedFrame[4], decodedFrame[5]), Concat(decodedFrame[6], decodedFrame[7]));
                case ((byte)FrameHeaders.ACK_VitesseNbrsrep):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new FrameConfigDataModel(ConfigAddresses.VitesseNbrsrep, Concat(decodedFrame[4], decodedFrame[5]), Concat(decodedFrame[6], decodedFrame[7]));
                case ((byte)FrameHeaders.ACK_ActifPassif):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new FrameConfigDataModel(ConfigAddresses.ActifPassif, Concat(decodedFrame[4], decodedFrame[5]), Concat(decodedFrame[6], decodedFrame[7]));
                case ((byte)FrameHeaders.ACK_Borne):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new BorneDataModel((char)decodedFrame[4], (char)decodedFrame[5], (char)decodedFrame[6], (char)decodedFrame[7]);
                case ((byte)FrameHeaders.ACK_StreamingError):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new StreamAckDataModel(FrameHeaders.ACK_StreamingError, decodedFrame[4], decodedFrame[5], decodedFrame[6], decodedFrame[7]);
                case ((byte)FrameHeaders.ACK_StreamingMod):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new StreamAckDataModel(FrameHeaders.ACK_StreamingMod, decodedFrame[4], decodedFrame[5], decodedFrame[6], decodedFrame[7]);
                case ((byte)FrameHeaders.ACK_StreamingPoint):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new StreamAckDataModel(FrameHeaders.ACK_StreamingPoint, decodedFrame[4], decodedFrame[5], decodedFrame[6], decodedFrame[7]);
                case ((byte)FrameHeaders.ACK_StreamingSeg):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new StreamAckDataModel(FrameHeaders.ACK_StreamingSeg, decodedFrame[4], decodedFrame[5], decodedFrame[6], decodedFrame[7]);
                case ((byte)FrameHeaders.ACK_StreamingInit):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new StreamAckDataModel(FrameHeaders.ACK_StreamingInit, decodedFrame[4], decodedFrame[5], decodedFrame[6], decodedFrame[7]);
                case ((byte)FrameHeaders.ACK_StreamingNext):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new StreamAckDataModel(FrameHeaders.ACK_StreamingNext, decodedFrame[4], decodedFrame[5], decodedFrame[6], decodedFrame[7]);
                case ((byte)FrameHeaders.ACK_modMazeCalib):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new ACKDataModel(decodedFrame);
                case ((byte)FrameHeaders.ACK_modMazeGame):
                    if (decodedFrame.Length != 12)
                    {
                        throw new Exception("Invalid number of bytes for frame header.");
                    }
                    return new ACKDataModel(decodedFrame);
                //case ((byte)FrameHeaders.ForceRef): // Force de référence
                //    if (decodedFrame.Length != 12)
                //    {
                //        throw new Exception("Invalid number of bytes for frame header.");
                //    }
                //    return new ForceDataModel((int)ConcatS((byte)decodedFrame[4], (byte)decodedFrame[5]), (int)ConcatS((byte)decodedFrame[6], (byte)decodedFrame[7]));
                default:
                    throw new Exception("Unknown frame header.");//FrameConfigDataModel
            }
        }

        /// <summary>
        /// Concatenates 2 bytes to return a short.
        /// </summary>
        /// <param name="MSB">
        /// Most Significant Byte.
        /// </param>
        /// <param name="LSB">
        /// Least Significant Byte.
        /// </param> 
        /// <returns>
        /// MSB and LSB concatenated to create a short.
        /// </returns>
        private static int ConcatS(byte MSB, byte LSB)
        {
            return (short)((short)((short)MSB << 8) | (short)LSB);
        }

        /// <summary>
        /// Concatenates 2 bytes to return a short.
        /// </summary>
        /// <param name="MSB">
        /// Most Significant Byte.
        /// </param>
        /// <param name="LSB">
        /// Least Significant Byte.
        /// </param> 
        /// <returns>
        /// MSB and LSB concatenated to create a short.
        /// </returns>
        private static short Concat(byte MSB, byte LSB)
        {
            return (short)((short)((short)MSB << 8) | (short)LSB);
        }
        #endregion
    }
}
