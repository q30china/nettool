/*
 * Genesis Socket Server and Client
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 * 
 * Please see included license.txt file for information on redistribution and usage.
 */
#region Using directives

using System;
using System.Globalization;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;
using System.Security;
using SocketTool;

#endregion

namespace SocketTool
{
    internal class Util
    {
        /// <summary>
        /// Gets the method name of the method that called the method that called this one.
        /// </summary>
        /// <returns>Name of method as string</returns>
        public static string GetMethod(int skipframes)
        {
            StackFrame sf = new StackFrame(skipframes, true);
            return sf.GetMethod().Name + "()";
        }

        #region Byte/number conversion
        /// <summary>
        /// Converts a string to a byte array
        /// </summary>
        /// <param name="s">String to convert</param>
        /// <returns>Byte array made from the string</returns>
        public static byte[] StringToBytes(string s)
        {
            byte[] tmp = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
                tmp[i] = (byte)s[i];

            return tmp;
        }

        /// <summary>
        /// Converts a byte array to a string
        /// </summary>
        /// <param name="b">Byte array to convert</param>
        /// <returns>A string</returns>
        public static string BytesToString(byte[] b)
        {
            string retval = "";

            for (int i = 0; i < b.Length; i++)
                retval += (char)b[i];

            return retval;
        }

        /// <summary>
        /// Given an integer, returns an eight byte representation of that long
        /// i.e 65535 = "ÿÿ\0\0\0\0\0\0" (FF FF 00 00 00 00 00 00);
        /// </summary>
        public static string LongToBytes(long num)
        {
            byte[] ab = BitConverter.GetBytes(num);

            string retval = "";

            for (int i = 0; i < ab.Length; i++)
            {
                retval += (char)ab[i];
            }

            return retval;
        }

        /// <summary>
        /// Given a byte array uses the first 8 to make a long, and returns it.
        /// </summary>
        public static long BytesToLong(string input)
        {
            byte[] bytes = new byte[8];

            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)input[i];

            return BitConverter.ToInt64(bytes, 0);
        }

        /// <summary>
        /// Given a short, returns a two byte representation in a string
        /// i.e 65535 = "ÿÿ" (FF FF);
        /// </summary>
        public static string ShortToBytes(short ch)
        {
            byte[] ab = BitConverter.GetBytes(ch);

            string retval = "";

            for (int i = 0; i < ab.Length; i++)
            {
                retval += (char)ab[i];
            }
            return retval;
        }

        /// <summary>
        /// Given a byte array uses the first 2 to make a long, and returns it.
        /// </summary>
        public static short BytesToShort(string input)
        {
            byte[] bytes = new byte[2];

            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)input[i];

            return BitConverter.ToInt16(bytes, 0);
        }


        /// <summary>
        /// Given an integer, returns a four byte representation of that integer
        /// i.e 65535 = "ÿÿ\0\0" (FF FF 00 00);
        /// </summary>
        public static string IntToBytes(int num)
        {
            byte[] ab = BitConverter.GetBytes(num);

            string retval = "";

            for (int i = 0; i < ab.Length; i++)
                retval += (char)ab[i];

            return retval;
        }

        /// <summary>
        /// Given a byte array uses the first 4 to make an integer, and returns it.
        /// </summary>
        public static int BytesToInt(string input)
        {
            byte[] bytes = new byte[4];

            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)input[i];

            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Converts an unsigned integer into its byte components
        /// </summary>
        /// <param name="num">Number to read</param>
        /// <returns>Bytes that make up the number</returns>
        public static string UintToBytes(uint num)
        {
            byte[] ab = BitConverter.GetBytes(num);

            StringBuilder sb = new StringBuilder(4);
            string retval = "";

            for (int i = 0; i < ab.Length; i++)
            {
                retval += (char)ab[i];
            }

            /*FileStream fst = new FileStream("c:\\test.log", FileMode.Append);
            BinaryWriter wrt = new BinaryWriter(fst);
            wrt.Write(tmp, 0, 4);
            wrt.Flush();
            wrt.Close();*/

            return retval;
        }

        /// <summary>
        /// Given a byte array uses the first 4 to make an unsigned integer, and returns it. 
        /// </summary>
        public static uint BytesToUint(string input)
        {
            byte[] bytes = new byte[4];

            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)input[i];

            return BitConverter.ToUInt32(bytes, 0);
        }
        #endregion

        /// <summary>
        /// Creates a command packet header
        /// </summary>
        /// <param name="seq_num">Sequence number of packet</param>
        /// <param name="flags">Command flags</param>
        /// <param name="opcode">Command OPCode</param>
        /// <param name="encrypt_key">Encryption key (use blank string for none)</param>
        /// <param name="fields">Command fields</param>
        /// <returns>The packet header</returns>
        public static string CreatePacketHeader(uint seq_num, byte flags, string opcode, string encrypt_key, string[] fields)
        {
            string retval = "";

            if (fields == null)
                fields = new string[0];

            retval += opcode;
            retval += UintToBytes(seq_num);
            retval += (char)flags;
            retval += ShortToBytes((short)fields.Length);
            if (fields.Length > 0)
                for (int i = 0; i < fields.Length; i++)
                    retval += ShortToBytes((short)fields[i].Length);

            //Add the constant string to check the encryption
            if (encrypt_key != "")
                retval += XORCrypt(Constants.ENCRYPT_CHECK_STRING, encrypt_key);
            else
                retval += Constants.ENCRYPT_CHECK_STRING;

            return retval;
        }

        /// <summary>
        /// Encrypts a string using XOR
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key">Encryption key</param>
        /// <returns>Encrypted or decrypted string</returns>
        public static string XORCrypt(string data, string key)
        {
            //Null key? Don't encrypt.
            if (key == "")
                return data;

            string retValue = "";

            int i = 0;
            int x = 0;

            int[] cipher = new int[data.Length];
            x = 0;

            for (i = 0; i < data.Length; i++)
            {
                retValue = retValue + (char)((data[i] ^ key[x]));
                cipher[i] = (data[i] ^ key[x]);
                x++;

                if (x >= key.Length)
                    x = 0;
            }
            return retValue;
        }

        /// <summary>
        /// Generates a random encryption key
        /// </summary>
        /// <returns>Randomly generated encryption key</returns>
        public static string GenerateEncryptionKey()
        {
            int size = 40; //320 bit
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(26 * random.NextDouble() + 65));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Returns a string array containing all local IP addresses
        /// </summary>
        public static string[] GetLocalAddresses()
        {
            // Get host name
            string strHostName = Dns.GetHostName();

            // Find host by name
            IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);

            string[] retval = new string[iphostentry.AddressList.Length];

            int i = 0;
            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                retval[i] = ipaddress.ToString();
                i++;
            }
            return retval;
        }

        public static string PrintFrame(byte[] frame)
        {

            int index = 0;
            StringBuilder stringBuilder = new StringBuilder();
            while (index < frame.Length)
            {

                stringBuilder.Append(frame[index].ToString("X02") + " ");
                if (index % 16 == 15)
                    stringBuilder.AppendLine();
                index++;
            }

            stringBuilder.AppendLine();

            return stringBuilder.ToString();
        }

        public static int AsciiToInt(char ch)
        {
            if (ch >= '0' && ch <= '9')
                return ch - '0';
            else if (ch >= 'A' && ch <= 'F')
                return ch - 'A' + 10;
            else
                throw new ArgumentException("ch must be in [0..9,A..F]");

        }

        public static int AsciiToInt(byte ch)
        {
            if (ch >= '0' && ch <= '9')
                return ch - '0';

            if (ch >= 'A' && ch <= 'F')
                return ch - 'A' + 10;
            if (ch >= 'a' && ch <= 'f')
                return ch - 'a' + 10;
            throw new ArgumentException("ch must be in [0..9,A..F]");
        }
        public static byte ConverStringToByte(string str)
        {
            str = str.Trim();
            if (str == null || str.Length <= 0) return 0x00;
            if (str.Length < 2) str = "0" + str;
            if (str.Length > 2) str = str.Substring(0, 2);
            byte bResult = 0;
            for (int i = 0; i < 2; i++)
            {
                int iValue = 0;
                string strOne = str.Substring(i, 1);
                switch (strOne)
                {
                    case "A":
                        iValue = 10;
                        break;
                    case "B":
                        iValue = 11;
                        break;
                    case "C":
                        iValue = 12;
                        break;
                    case "D":
                        iValue = 13;
                        break;
                    case "E":
                        iValue = 14;
                        break;
                    case "F":
                        iValue = 15;
                        break;
                    default:
                        iValue = int.Parse(strOne);
                        break;
                }
                bResult = (byte)(bResult * 16 + iValue);
            }

            return bResult;
        }
        public static string ConverByteToString(byte bData)
        {
            int result = (int)bData;
            int iFirst = result / 16;
            string strFirst = iFirst.ToString();
            if (iFirst >= 10)
            {
                switch (iFirst)
                {
                    case 10:
                        strFirst = "A";
                        break;
                    case 11:
                        strFirst = "B";
                        break;
                    case 12:
                        strFirst = "C";
                        break;
                    case 13:
                        strFirst = "D";
                        break;
                    case 14:
                        strFirst = "E";
                        break;
                    case 15:
                        strFirst = "F";
                        break;
                }
            }
            int iSecond = result % 16;
            string strSecond = iSecond.ToString();
            if (iSecond >= 10)
            {
                switch (iSecond)
                {
                    case 10:
                        strSecond = "A";
                        break;
                    case 11:
                        strSecond = "B";
                        break;
                    case 12:
                        strSecond = "C";
                        break;
                    case 13:
                        strSecond = "D";
                        break;
                    case 14:
                        strSecond = "E";
                        break;
                    case 15:
                        strSecond = "F";
                        break;
                }
            }

            return strFirst + strSecond;
        }
        public static string ConverByteToString(byte[] bData)
        {
            string strResult = "";
            for (int i = 0; i < bData.Length; i++)
            {
                strResult += ConverByteToString(bData[i]) + " ";

                //if ((i + 1) % 16 == 0)
                //{
                //    strResult += "\n";
                //}
            }
            return strResult;
        }

        //组建基本帧
        public static byte[] AssemblyFrameBase(string rtuAddr,
            byte msta, byte seq, byte control,
            byte[] data)
        {

            byte[] frame;
            int frameLength;

            long nrtuAddr;

            nrtuAddr = long.Parse(rtuAddr, NumberStyles.HexNumber);

            frameLength = (data == null) ? 16 : 16 + data.Length;
            frame = new byte[frameLength];
            int index = 0;

            frame[index++] = 0x68;

            //数据长度
            //for (int i = 0; i < 2; i++)
            //{
            //    if (data == null)
            //    {
            //        frame[index++] = 0x00;
            //        frame[index++] = 0x00;
            //    }
            //    else
            //    {
            //        frame[index++] = (byte)(frameLength % 256);
            //        frame[index++] = (byte)(frameLength / 256);
            //    }
            //}

            frame[index++] = 0x32;
            frame[index++] = 0x00;
            frame[index++] = 0x32;
            frame[index++] = 0x00;

            frame[index++] = 0x68;

            // control area
            frame[index++] = msta;

            //集中器地址
            frame[index++] = (byte)((nrtuAddr >> 16) & 0xFF);
            frame[index++] = (byte)((nrtuAddr >> 24) & 0xFF);
            frame[index++] = (byte)((nrtuAddr) & 0xFF);//0x40:低压抄表集中器            
            frame[index++] = (byte)((nrtuAddr >> 8) & 0xFF);

            //控制码
            frame[index++] = 0x00;
            frame[index++] = control;

            frame[index++] = seq;

            for (int j = 0; j < data.Length; j++)
            {
                frame[index++] = data[j];

            }

            //校验码
            frame[index++] = CheckSum(frame, 6, index - 6);

            frame[index++] = 0x16;

            return frame;
        }

        /// <summary>
        /// make alarm frame
        /// </summary>
        /// <param name="rtuAddr"></param>
        /// <param name="msta"></param>
        /// <param name="seq"></param>
        /// <param name="control"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] AssemblyFrameAlarm(string rtuAddr,
          byte pointid, byte alarmid, byte alarmtype)
        {

            byte[] frame;
            int frameLength;

            long nrtuAddr;

            nrtuAddr = long.Parse(rtuAddr, NumberStyles.HexNumber);

            //frameLength = (data == null) ? 16 : 16 + data.Length;
            frameLength = 40;

            frame = new byte[frameLength];
            int index = 0;

            frame[index++] = 0x68;

            frame[index++] = 0x82;
            frame[index++] = 0x00;
            frame[index++] = 0x82;
            frame[index++] = 0x00;

            frame[index++] = 0x68;
            frame[index++] = 0xC4;


            //集中器地址
            frame[index++] = (byte)((nrtuAddr >> 16) & 0xFF);
            frame[index++] = (byte)((nrtuAddr >> 24) & 0xFF);
            frame[index++] = (byte)((nrtuAddr) & 0xFF);
            frame[index++] = (byte)((nrtuAddr >> 8) & 0xFF);

            //控制码
            frame[index++] = 0x00;
            frame[index++] = 0x0E;
            frame[index++] = 0x6D;

            //
            frame[index++] = 0x00;
            frame[index++] = 0x00;
            frame[index++] = 0x01;
            frame[index++] = 0x00;

            frame[index++] = 0x63;
            frame[index++] = 0x00;
            frame[index++] = 0x62;
            frame[index++] = 0x63;

            // 28 标准事件记录  2B 电网事件记录
            frame[index++] = alarmtype;

            //事件数据长度
            frame[index++] = 0x0E;

            DateTime dt = DateTime.Now;
            byte[] datebytes = DateTimeToByteArray(dt);

            for (int j = 5; j > 0; j--)
                frame[index++] = datebytes[j - 1];
            //采集器采集时间  分 时 日 月 年

            //frame[index++] = 0x14;
            //frame[index++] = 0x17;
            //frame[index++] = 0x17;
            //frame[index++] = 0x06;
            //frame[index++] = 0x14;

            //测量点号码
            frame[index++] = pointid;
            frame[index++] = 0x00;

            //表事件
            frame[index++] = alarmid;


            //表计事项实际产生时间 分 时 日 月 年 秒

            for (int j = 5; j > 0; j--)
                frame[index++] = datebytes[j - 1];

            frame[index++] = datebytes[5];
            //frame[index++] = 0x37;
            //frame[index++] = 0x14;
            //frame[index++] = 0x17;
            //frame[index++] = 0x06;
            //frame[index++] = 0x14;
            //frame[index++] = 0x17;

            //校验码
            frame[index++] = CheckSum(frame, 6, index - 6);

            frame[index++] = 0x16;

            return frame;

        }

        /// <summary>
        /// Make frame of daily frozen data
        /// </summary>
        /// <param name="rtuAddr"></param>
        /// <param name="pointid"></param>
        /// <param name="alarmid"></param>
        /// <param name="alarmtype"></param>
        /// <param name="dtnow"></param>
        /// <returns></returns>
        public static byte[] AssemblyFrameDailyFrozen(string rtuAddr,
          byte pointid, byte alarmid, byte alarmtype, DateTime dtnow)
        {

            byte[] frame;
            int frameLength;

            long nrtuAddr;

            nrtuAddr = long.Parse(rtuAddr, NumberStyles.HexNumber);

            //frameLength = (data == null) ? 16 : 16 + data.Length;
            frameLength = 40;

            frame = new byte[frameLength];
            int index = 0;

            frame[index++] = 0x68;

            frame[index++] = 0x82;
            frame[index++] = 0x00;
            frame[index++] = 0x82;
            frame[index++] = 0x00;

            frame[index++] = 0x68;
            frame[index++] = 0xC4;


            //集中器地址
            frame[index++] = (byte)((nrtuAddr >> 16) & 0xFF);
            frame[index++] = (byte)((nrtuAddr >> 24) & 0xFF);
            frame[index++] = (byte)((nrtuAddr) & 0xFF);
            frame[index++] = (byte)((nrtuAddr >> 8) & 0xFF);

            //控制码
            frame[index++] = 0x00;
            frame[index++] = 0x0E;
            frame[index++] = 0x6D;

            //
            frame[index++] = 0x00;
            frame[index++] = 0x00;
            frame[index++] = 0x01;
            frame[index++] = 0x00;

            frame[index++] = 0x63;
            frame[index++] = 0x00;
            frame[index++] = 0x62;
            frame[index++] = 0x63;

            // 28 标准事件记录  2B 电网事件记录
            frame[index++] = alarmtype;

            //事件数据长度
            frame[index++] = 0x0E;

            DateTime dt = DateTime.Now;
            byte[] datebytes = DateTimeToByteArray(dt);

            for (int j = 5; j > 0; j--)
                frame[index++] = datebytes[j - 1];
            //采集器采集时间  分 时 日 月 年

            //frame[index++] = 0x14;
            //frame[index++] = 0x17;
            //frame[index++] = 0x17;
            //frame[index++] = 0x06;
            //frame[index++] = 0x14;

            //测量点号码
            frame[index++] = pointid;
            frame[index++] = 0x00;

            //表事件
            frame[index++] = alarmid;


            //表计事项实际产生时间 分 时 日 月 年 秒

            for (int j = 5; j > 0; j--)
                frame[index++] = datebytes[j - 1];

            frame[index++] = datebytes[5];
            //frame[index++] = 0x37;
            //frame[index++] = 0x14;
            //frame[index++] = 0x17;
            //frame[index++] = 0x06;
            //frame[index++] = 0x14;
            //frame[index++] = 0x17;

            //校验码
            frame[index++] = CheckSum(frame, 6, index - 6);

            frame[index++] = 0x16;

            return frame;

        }

        /// <summary>
        /// Make frame of daily frozen data
        /// </summary>
        /// <param name="rtuAddr"></param>
        /// <param name="pointid"></param>
        /// <param name="alarmid"></param>
        /// <param name="alarmtype"></param>
        /// <param name="dtnow"></param>
        /// <returns></returns>
        public static byte[] AssemblyFrameLoadProfile(string rtuAddr,
          byte pointid, byte alarmid, byte alarmtype, DateTime dtnow)
        {

            byte[] frame;
            int frameLength;

            long nrtuAddr;

            nrtuAddr = long.Parse(rtuAddr, NumberStyles.HexNumber);

            //frameLength = (data == null) ? 16 : 16 + data.Length;
            frameLength = 40;

            frame = new byte[frameLength];
            int index = 0;

            frame[index++] = 0x68;

            frame[index++] = 0x82;
            frame[index++] = 0x00;
            frame[index++] = 0x82;
            frame[index++] = 0x00;

            frame[index++] = 0x68;
            frame[index++] = 0xC4;


            //集中器地址
            frame[index++] = (byte)((nrtuAddr >> 16) & 0xFF);
            frame[index++] = (byte)((nrtuAddr >> 24) & 0xFF);
            frame[index++] = (byte)((nrtuAddr) & 0xFF);
            frame[index++] = (byte)((nrtuAddr >> 8) & 0xFF);

            //控制码
            frame[index++] = 0x00;
            frame[index++] = 0x0E;
            frame[index++] = 0x6D;

            //
            frame[index++] = 0x00;
            frame[index++] = 0x00;
            frame[index++] = 0x01;
            frame[index++] = 0x00;

            frame[index++] = 0x63;
            frame[index++] = 0x00;
            frame[index++] = 0x62;
            frame[index++] = 0x63;

            // 28 标准事件记录  2B 电网事件记录
            frame[index++] = alarmtype;

            //事件数据长度
            frame[index++] = 0x0E;

            DateTime dt = DateTime.Now;
            byte[] datebytes = DateTimeToByteArray(dt);

            for (int j = 5; j > 0; j--)
                frame[index++] = datebytes[j - 1];
            //采集器采集时间  分 时 日 月 年

            //frame[index++] = 0x14;
            //frame[index++] = 0x17;
            //frame[index++] = 0x17;
            //frame[index++] = 0x06;
            //frame[index++] = 0x14;

            //测量点号码
            frame[index++] = pointid;
            frame[index++] = 0x00;

            //表事件
            frame[index++] = alarmid;


            //表计事项实际产生时间 分 时 日 月 年 秒

            for (int j = 5; j > 0; j--)
                frame[index++] = datebytes[j - 1];

            frame[index++] = datebytes[5];
            //frame[index++] = 0x37;
            //frame[index++] = 0x14;
            //frame[index++] = 0x17;
            //frame[index++] = 0x06;
            //frame[index++] = 0x14;
            //frame[index++] = 0x17;

            //校验码
            frame[index++] = CheckSum(frame, 6, index - 6);

            frame[index++] = 0x16;

            return frame;

        }
        /// <summary>
        /// Make frame of daily frozen data
        /// </summary>
        /// <param name="rtuAddr"></param>
        /// <param name="pointid"></param>
        /// <param name="alarmid"></param>
        /// <param name="alarmtype"></param>
        /// <param name="dtnow"></param>
        /// <returns></returns>
        public static byte[] AssemblyFrameSendCurrentTime(string rtuAddr, DateTime dtnow)
        {

            byte[] frame;
            int frameLength;
            long nrtuAddr;
            nrtuAddr = long.Parse(rtuAddr, NumberStyles.HexNumber);

            frameLength = 26; //all data length for send replay datetime frame

            frame = new byte[frameLength];
            int index = 0;

            frame[index++] = 0x68;

            frame[index++] = 0x4A;
            frame[index++] = 0x00;
            frame[index++] = 0x4A;
            frame[index++] = 0x00;

            frame[index++] = 0x68;
            frame[index++] = 0x88;


            //集中器地址
            frame[index++] = (byte)((nrtuAddr >> 16) & 0xFF);
            frame[index++] = (byte)((nrtuAddr >> 24) & 0xFF);
            frame[index++] = (byte)((nrtuAddr) & 0xFF);
            frame[index++] = (byte)((nrtuAddr >> 8) & 0xFF);

            //控制码
            frame[index++] = 0x02;
            frame[index++] = 0x0C;
            frame[index++] = 0x6F;

            //
            frame[index++] = 0x00;
            frame[index++] = 0x00;
            frame[index++] = 0x02;
            frame[index++] = 0x00;

            byte[] datebytes = DateTimeToByteArray(dtnow);

            for (int j = 5; j >= 0; j--)
                frame[index++] = datebytes[j];
            //采集器采集时间  秒 分 时 日 月 年

            //校验码
            frame[index++] = CheckSum(frame, 6, index - 6);

            frame[index++] = 0x16;

            return frame;

        }

        public static Boolean CheckCallTimeFrame(string rtuAddr, byte[] data)
        {

            byte[] frame;
            int frameLength;
            long nrtuAddr;
            nrtuAddr = long.Parse(rtuAddr, NumberStyles.HexNumber);

            frameLength = 26; //all data length for send replay datetime frame

            frame = new byte[frameLength];
            int index = 0;

            frame[index++] = 0x68;

            frame[index++] = 0x4A;
            frame[index++] = 0x00;
            frame[index++] = 0x4A;
            frame[index++] = 0x00;

            frame[index++] = 0x68;
            frame[index++] = 0x88;


            //集中器地址
            frame[index++] = (byte)((nrtuAddr >> 16) & 0xFF);
            frame[index++] = (byte)((nrtuAddr >> 24) & 0xFF);
            frame[index++] = (byte)((nrtuAddr) & 0xFF);
            frame[index++] = (byte)((nrtuAddr >> 8) & 0xFF);

            //控制码
            frame[index++] = 0x02;
            frame[index++] = 0x0C;
            frame[index++] = 0x6F;

            //
            frame[index++] = 0x00;
            frame[index++] = 0x00;
            frame[index++] = 0x02;
            frame[index++] = 0x00;

            DateTime dt = DateTime.Now;
            byte[] datebytes = DateTimeToByteArray(dt);

            for (int j = 5; j >= 0; j--)
                frame[index++] = datebytes[j];
            //采集器采集时间  秒 分 时 日 月 年

            //校验码
            frame[index++] = CheckSum(frame, 6, index - 6);

            frame[index++] = 0x16;

            return true;

        }

        /// <summary>
        /// 求和校验 
        /// </summary>
        /// <param name="Buf"></param>
        /// <param name="begin"></param>
        /// <param name="Len"></param>
        /// <returns></returns>
        public static byte CheckSum(byte[] Buf, int begin, int Len)
        {
            byte Sum = 0;
            for (int i = begin; i < begin + Len; i++) Sum += Buf[i];
            return Sum;
        }

        public static byte[] GetByteDataByType(int len, byte type)
        {
            byte[] data = new Byte[len];

            switch (type)
            {
                case 0x01: //登录

                    data = new byte[] { 0x00, 0x00, 0x01, 0x00 };
                    break;

                case 0x04: //心跳

                    break;
            }

            return data;
        }

        public static string AddSpaceToFrame(string str)
        {
            int index = 0;
            StringBuilder stringBuilder = new StringBuilder();
            while (index < str.Length / 2)
            {

                stringBuilder.Append(str.Substring(index * 2, 2) + " ");
                index++;
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// INT转化为单字节HEX
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte IntToHEX(int data)
        {
            return (byte)data;
        }
        public static byte[] IntToBytes(int data, char c, int len)
        {
            int num = len - 1;
            byte[] thisbyte = new byte[len];
            int enters;
            switch (c)
            {
                case 'H':
                case 'h':
                    enters = 16;
                    break;
                case 'B':
                case 'b':
                    enters = 10;
                    break;
                default:
                    enters = 10;
                    break;
            }
            while (data > 0)
            {
                if (num < 0)
                    throw new ArgumentException("Parameter is outside field ");
                thisbyte[num] = (byte)((data % (enters * enters)) / enters * 16 + (data % (enters * enters)) % enters);
                data /= (enters * enters);
                num--;
            }
            while (num >= 0)
            {
                thisbyte[num] = 0;
                num--;
            }
            return thisbyte;
        }
        public static byte IntToBCDByte(int n)
        {

            return (byte)((n % 10) + (n / 10 % 10) * 0x10);

        }

        public static byte[] DateTimeToByteArray(DateTime time)
        {

            return new byte[]{ IntToBCDByte(time.Year),
                        IntToBCDByte(time.Month),
                        IntToBCDByte(time.Day),
                        IntToBCDByte(time.Hour),
                        IntToBCDByte(time.Minute),
                        IntToBCDByte(time.Second)
                };
        }

        public byte[] PickFrame(byte[] recvbuf, ref int beginIndex, int endIndex)
        {
            byte[] frame = new byte[20];
            int recvLen;
            int frameLen;

            while (true)
            {
                recvLen = (endIndex - beginIndex + recvbuf.Length) % recvbuf.Length;
                if (recvLen < 14)       //1+2+2+1+1+5+(?)+1+1
                    return null;

                // 1 校验启动位
                if (recvbuf[beginIndex] != 0x68)
                {
                    beginIndex = (++beginIndex) % recvbuf.Length;
                    continue;
                }

                if (recvbuf[(beginIndex + 5) % recvbuf.Length] != 0x68)
                {
                    beginIndex = (++beginIndex) % recvbuf.Length;
                    continue;
                }


                // 2 校验2个长度是否一致
                //if (recvbuf[(beginIndex + 1) % recvbuf.Length] != recvbuf[(beginIndex + 3) % recvbuf.Length])
                //{
                //    beginIndex = (++beginIndex) % recvbuf.Length;
                //    continue;
                //}
                //if (recvbuf[(beginIndex + 2) % recvbuf.Length] != recvbuf[(beginIndex + 4) % recvbuf.Length])
                //{
                //    beginIndex = (++beginIndex) % recvbuf.Length;
                //    continue;
                //}
                if (recvbuf.Length != 20)
                    return null;


                // 3 校验规约标识位
                if ((recvbuf[(beginIndex + 1) % recvbuf.Length] & 0x4B) != 0x01)
                {
                    beginIndex = (++beginIndex) % recvbuf.Length;
                    continue;
                }

                // 4 检查是否接收到完整的数据帧
                //frameLen = recvbuf[(beginIndex + 1) % recvbuf.Length] >> 2
                //    + recvbuf[(beginIndex + 2) % recvbuf.Length] * (1 << 6);

                //if (recvLen < frameLen + 8)
                //{
                //    return null;
                //}


                // 5 检查帧结束符 
                if (recvbuf[(beginIndex + 13) % recvbuf.Length] != 0x16)
                {
                    beginIndex = (++beginIndex) % recvbuf.Length;
                    continue;
                }

                //  复制完整的报文区
                for (int i = 0; i < 20; i++)
                {
                    frame[i] = recvbuf[(beginIndex + i) % recvbuf.Length];
                }

                // 6 校验（检出差错时，两帧之间的线路空闲间隔最少需33位）
                //if (Pr.DAQSvr.Commons.DAQSdk.CheckSum(frame, 6, frameLen) != frame[frame.Length - 2])
                //{

                //    beginIndex = (beginIndex + 4) % recvbuf.Length;
                //    continue;
                //}
                //beginIndex = (beginIndex + frame.Length) % recvbuf.Length;
                return frame;
            }
        }
    }
}
