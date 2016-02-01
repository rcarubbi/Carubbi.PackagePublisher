using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Collections.Generic;

namespace Carubbi.CaptchaBreaker.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    public enum PictureType
    {
        /// <summary>
        /// 
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// 
        /// </summary>
        Asirra = 86,
        /// <summary>
        /// 
        /// </summary>
        Unknown
    }

    /// <summary>
    /// Constantes da interface de comunicação decaptcher utilizada pelo GSA captchaBreaker
    /// </summary>
    public class APIConstants
    {
        /// <summary>
        /// everything went OK
        /// </summary>
        public const int ccERR_OK = 0; 

        /// <summary>
        /// general internal error
        /// </summary>
        public const int ccERR_GENERAL = -1; 
        /// <summary>
        /// status is not correct
        /// </summary>
        public const int ccERR_STATUS = -2;  

        /// <summary>
        /// network data transfer error
        /// </summary>
        public const int ccERR_NET_ERROR = -3; 
        /// <summary>
        /// text is not of an appropriate size
        /// </summary>
        public const int ccERR_TEXT_SIZE = -4; 
        /// <summary>
        /// server's overloaded
        /// </summary>
        public const int ccERR_OVERLOAD = -5;
        /// <summary>
        ///  not enough funds to complete the request
        /// </summary>
        public const int ccERR_BALANCE = -6;
        /// <summary>
        /// requiest timed out
        /// </summary>
        public const int ccERR_TIMEOUT = -7; 

        /// <summary>
        /// unknown error
        /// </summary>
        public const int ccERR_UNKNOWN = -200; 

        // picture processing TIMEOUTS
        /// <summary>
        /// default timeout, server-specific
        /// </summary>
        public const int ptoDEFAULT = 0; 
        /// <summary>
        /// long timeout for picture, server-specfic
        /// </summary>
        public const int ptoLONG = 1;
        /// <summary>
        /// 30 seconds timeout for picture
        /// </summary>
        public const int pto30SEC = 2; 
        /// <summary>
        ///60 seconds timeout for picture
        /// </summary>
        public const int pto60SEC = 3;  
        /// <summary>
        ///  90 seconds timeout for picture
        /// </summary>
        public const int pto90SEC = 4; 

        // picture processing TYPES
        /// <summary>
        /// picture type unspecified
        /// </summary>
        public const int ptUNSPECIFIED = 0; 
    }

    /// <summary>
    /// Proxy de comunicação do Decaptcher utilizada pelo GSA captchaBreaker
    /// </summary>
    class CCProtoPacket
    {
        /// <summary>
        /// 
        /// </summary>
        public const int CC_PROTO_VER = 1;  // protocol version

        /// <summary>
        /// 
        /// </summary>
        public const int CC_RAND_SIZE = 256; // size of the random sequence for authentication procedure

        /// <summary>
        /// 
        /// </summary>
        public const int CC_MAX_TEXT_SIZE = 100; // maximum characters in returned text for picture

        /// <summary>
        /// 
        /// </summary>
        public const int CC_MAX_LOGIN_SIZE = 100; // maximum characters in login string
        /// <summary>
        /// 
        /// </summary>
        public const int CC_MAX_PICTURE_SIZE = 200000; // 200 K bytes for picture seems sufficient for all purposes

        /// <summary>
        /// 
        /// </summary>
        public const int CC_HASH_SIZE = 32;  //

        /// <summary>
        /// 
        /// </summary>
        public const int cmdCC_UNUSED = 0;
        
        /// <summary>
        /// 
        /// </summary>
        public const int cmdCC_LOGIN = 1;  // login

        /// <summary>
        /// 
        /// </summary>
        public const int cmdCC_BYE = 2;  // end of session
        
        /// <summary>
        /// 
        /// </summary>
        public const int cmdCC_RAND = 3;  // random data for making hash with login+password
        
        /// <summary>
        /// 
        /// </summary>
        public const int cmdCC_HASH = 4;  // hash data
        /// <summary>
        /// 
        /// </summary>
        public const int cmdCC_PICTURE = 5;  // picture data, deprecated
        /// <summary>
        /// 
        /// </summary>
        public const int cmdCC_TEXT = 6;  // text data, deprecated
        /// <summary>
        /// 
        /// </summary>
        public const int cmdCC_OK = 7;  //
        /// <summary>
        /// 
        /// </summary>
        public const int cmdCC_FAILED = 8;  //
        /// <summary>
        /// 
        /// </summary>
        public const int cmdCC_OVERLOAD = 9;  //
        /// <summary>
        /// 
        /// </summary>
        public const int cmdCC_BALANCE = 10;  // zero balance
        /// <summary>
        /// 
        /// </summary>
        public const int cmdCC_TIMEOUT = 11;  // time out occured
        /// <summary>
        /// 
        /// </summary>
        public const int cmdCC_PICTURE2 = 12;  // picture data
        /// <summary>
        /// 
        /// </summary>
        public const int cmdCC_PICTUREFL = 13;  // picture failure
        /// <summary>
        /// 
        /// </summary>
        public const int cmdCC_TEXT2 = 14;  // text data
        /// <summary>
        /// 
        /// </summary>
        public const int SIZEOF_CC_PACKET = 6;
        /// <summary>
        /// 
        /// </summary>
        public const int SIZEOF_CC_PICT_DESCR = 20;
        /// <summary>
        /// 
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Command { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Size { get; set; }

        private byte[] _data = null;   // packet payload
        /// <summary>
        /// 
        /// </summary>
        public CCProtoPacket()
        {
            Version = CC_PROTO_VER;
            Command = cmdCC_BYE;
        }

        private bool CheckHeader(int command, int size)
        {
            if (Version != CC_PROTO_VER)
                return false;
            if ((command != -1) && (Command != command))
                return false;
            if ((size != -1) && (Size != size))
                return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oos"></param>
        /// <returns></returns>
        public bool PackTo(Stream oos)
        {
            try
            {
                var writer = new BinaryWriter(oos);
                writer.Write((byte)Version);
                writer.Write((byte)Command);
                writer.Write(Size);
                if (_data != null)
                {
                    if (_data.Length > 0)
                    {
                        oos.Write(_data, 0, _data.Length);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool UnpackHeader(Stream ios)
        {
            try
            {
                var reader = new BinaryReader(ios);
                Version = (int)reader.ReadByte();
                Command = (int)reader.ReadByte();
                Size = reader.ReadInt32();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="bytesToRead"></param>
        /// <returns></returns>
        public static byte[] ReadFully(Stream input, int bytesToRead)
        {
            byte[] data = new byte[bytesToRead];
            int offset = 0;
            int remaining = data.Length;
            while (remaining > 0)
            {
                int read = input.Read(data, offset, remaining);
                remaining -= read;
                offset += read;
            }
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dis"></param>
        /// <param name="cmd"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public bool UnpackFrom(Stream dis, int cmd, int size)
        {
            UnpackHeader(dis);

            if (CheckHeader(cmd, size) == false)
                return false;

            try
            {
                if (Size > 0)
                {
                    // check error
                    _data = new byte[Size];
                    _data = ReadFully(dis, Size);
                }
                else
                {
                    _data = null;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int calcSize()
        {
            if (_data != null)
            {
                Size = _data.Length;
            }
            else
            {
                Size = 0;
            }
            return Size;
        }

        int getFullSize()
        {
            return SIZEOF_CC_PACKET + Size;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void setData(byte[] data)
        {
            _data = data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] getData()
        {
            return _data;
        }
    }

    /// <summary>
    /// Proxy de comunicação do Decaptcher utilizada pelo GSA captchaBreaker
    /// </summary>
    class CCPictDescr
    {
        private int _timeout = APIConstants.ptoDEFAULT;
        private int _type = APIConstants.ptUNSPECIFIED;
        private int _size = 0;
        private int _major_id = 0;
        private int _minor_id = 0;
        private byte[] _data = null;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] pack()
        {

            int data_length = _data == null ? 0 : _data.Length;
            byte[] res = new byte[4 * 5 + data_length];
            int i = 0;
            int j = 0;
            int value = 0;

            value = _timeout;
            res[i++] = (byte)((value >> 0) & 0xff);
            res[i++] = (byte)((value >> 8) & 0xff);
            res[i++] = (byte)((value >> 16) & 0xff);
            res[i++] = (byte)((value >> 24) & 0xff);

            value = _type;
            res[i++] = (byte)((value >> 0) & 0xff);
            res[i++] = (byte)((value >> 8) & 0xff);
            res[i++] = (byte)((value >> 16) & 0xff);
            res[i++] = (byte)((value >> 24) & 0xff);

            value = _size;
            res[i++] = (byte)((value >> 0) & 0xff);
            res[i++] = (byte)((value >> 8) & 0xff);
            res[i++] = (byte)((value >> 16) & 0xff);
            res[i++] = (byte)((value >> 24) & 0xff);

            value = _major_id;
            res[i++] = (byte)((value >> 0) & 0xff);
            res[i++] = (byte)((value >> 8) & 0xff);
            res[i++] = (byte)((value >> 16) & 0xff);
            res[i++] = (byte)((value >> 24) & 0xff);

            value = _minor_id;
            res[i++] = (byte)((value >> 0) & 0xff);
            res[i++] = (byte)((value >> 8) & 0xff);
            res[i++] = (byte)((value >> 16) & 0xff);
            res[i++] = (byte)((value >> 24) & 0xff);

            if (_data != null)
            {
                for (j = 0; j < _data.Length; j++)
                {
                    res[i++] = _data[j];
                }
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bin"></param>
        public void unpack(byte[] bin)
        {
            int i = 0;
            int j = 0;

            if (bin.Length < CCProtoPacket.SIZEOF_CC_PICT_DESCR)
            {
                return;
            }
            _timeout = (bin[0] << 0) | (bin[1] << 8) | (bin[2] << 16) | (bin[3] << 24);
            _type = (bin[4] << 0) | (bin[5] << 8) | (bin[6] << 16) | (bin[7] << 24);
            _size = (bin[8] << 0) | (bin[9] << 8) | (bin[10] << 16) | (bin[11] << 24);
            _major_id = (bin[12] << 0) | (bin[13] << 8) | (bin[14] << 16) | (bin[15] << 24);
            _minor_id = (bin[16] << 0) | (bin[17] << 8) | (bin[18] << 16) | (bin[19] << 24);

            _data = null;

            // we have some additional data
            if (bin.Length > CCProtoPacket.SIZEOF_CC_PICT_DESCR)
            {
                _data = new byte[bin.Length - CCProtoPacket.SIZEOF_CC_PICT_DESCR];
                for (i = CCProtoPacket.SIZEOF_CC_PICT_DESCR, j = 0; i < bin.Length; i++, j++)
                {
                    _data[j] = bin[i];
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        public void setTimeout(int to)
        {
            _timeout = to;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getTimeout()
        {
            return _timeout;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public void setType(int type)
        {
            _type = type;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getType()
        {
            return _type;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public void setSize(int size)
        {
            _size = size;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getSize()
        {
            return _size;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int calcSize()
        {
            if (_data == null)
            {
                _size = 0;
            }
            else
            {
                _size = _data.Length;
            }
            return _size;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getFullSize()
        {
            return CCProtoPacket.SIZEOF_CC_PICT_DESCR + _size;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="major_id"></param>
        public void setMajorID(int major_id)
        {
            _major_id = major_id;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getMajorID()
        {
            return _major_id;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="minor_id"></param>
        public void setMinorID(int minor_id)
        {
            _minor_id = minor_id;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getMinorID()
        {
            return _minor_id;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void setData(byte[] data)
        {
            _data = data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] getData()
        {
            return _data;
        }
    }

    /// <summary>
    /// Proxy de comunicação do Decaptcher utilizada pelo GSA captchaBreaker
    /// </summary>
    public class BalanceResult
    {
        /// <summary>
        /// 
        /// </summary>
        public int ReturnCode { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Balance { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnCode"></param>
        public BalanceResult(int returnCode)
            : this(returnCode, string.Empty)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="balance"></param>
        public BalanceResult(int returnCode, string balance)
        {
            ReturnCode = returnCode;
            Balance = balance;
        }
    }

    /// <summary>
    /// Proxy de comunicação do Decaptcher utilizada pelo GSA captchaBreaker
    /// </summary>
    public class CCProto
    {
        /// <summary>
        /// 
        /// </summary>
        public const int sCCC_INIT = 1;  // initial status, ready to issue LOGIN on client

        /// <summary>
        /// 
        /// </summary>
        public const int sCCC_LOGIN = 2;  // LOGIN is sent, waiting for RAND (login accepted) or CLOSE CONNECTION (login is unknown) 
        /// <summary>
        /// 
        /// </summary>
        public const int sCCC_HASH = 3;  // HASH is sent, server may CLOSE CONNECTION (hash is not recognized)
        /// <summary>
        /// 
        /// </summary>
        public const int sCCC_PICTURE = 4;

        private int _status = sCCC_INIT;
        private TcpClient _client;
        private NetworkStream _stream;

        private List<int> portas = new List<int>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int Login(string hostname, int port, string username, string password)
        {
            CCProtoPacket pack = null;
            var md5 = MD5.Create();
            var sha = SHA256.Create();

            int i = 0;
            int j = 0;

            _status = sCCC_INIT;

            try
            {
                _client = new TcpClient(hostname, port);
                _stream = _client.GetStream();
            }
            catch (Exception)
            {
                return APIConstants.ccERR_NET_ERROR;
            }
                
            pack = new CCProtoPacket();

            pack.Command = CCProtoPacket.cmdCC_LOGIN;
            pack.Size = username.Length;
            pack.setData(Encoding.ASCII.GetBytes(username));

            if (pack.PackTo(_stream) == false)
            {
                return APIConstants.ccERR_NET_ERROR;
            }

            if (pack.UnpackFrom(_stream, CCProtoPacket.cmdCC_RAND, CCProtoPacket.CC_RAND_SIZE) == false)
            {
                return APIConstants.ccERR_NET_ERROR;
            }

            byte[] md5bin = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
            String md5str = "";
            char[] cvt = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
            for (i = 0; i < md5bin.Length; i++)
            {
                md5str += cvt[(md5bin[i] & 0xF0) >> 4];
                md5str += cvt[md5bin[i] & 0x0F];
            }

            byte[] shabuf = new byte[pack.getData().Length + md5str.Length + username.Length];
            j = 0;
            for (i = 0; i < pack.getData().Length; i++, j++)
            {
                shabuf[j] = pack.getData()[i];
            }
            for (i = 0; i < Encoding.ASCII.GetBytes(md5str).Length; i++, j++)
            {
                shabuf[j] = Encoding.ASCII.GetBytes(md5str)[i];
            }
            for (i = 0; i < Encoding.ASCII.GetBytes(username).Length; i++, j++)
            {
                shabuf[j] = Encoding.ASCII.GetBytes(username)[i];
            }

            pack = new CCProtoPacket();
            pack.Command = CCProtoPacket.cmdCC_HASH;
            pack.Size = CCProtoPacket.CC_HASH_SIZE;
            pack.setData(sha.ComputeHash(shabuf));

            if (pack.PackTo(_stream) == false)
            {
                return APIConstants.ccERR_NET_ERROR;
            }

            if (pack.UnpackFrom(_stream, CCProtoPacket.cmdCC_OK, 0) == false)
            {
                return APIConstants.ccERR_NET_ERROR;
            }

            _status = sCCC_PICTURE;

            return APIConstants.ccERR_OK;
        } // login()
        /// <summary>
        /// 
        /// </summary>
        public class PictureResult
        {
            /// <summary>
            /// 
            /// </summary>
            public readonly int timeReallyUsed;
            /// <summary>
            /// 
            /// </summary>
            public readonly int typeReallyUsed;
            /// <summary>
            /// 
            /// </summary>
            public readonly String text;
            /// <summary>
            /// 
            /// </summary>
            public readonly int majorId;
            /// <summary>
            /// 
            /// </summary>
            public readonly int minorId;
            /// <summary>
            /// 
            /// </summary>
            public readonly int returnCode;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="time"></param>
            /// <param name="type"></param>
            /// <param name="text"></param>
            /// <param name="major"></param>
            /// <param name="minor"></param>
            /// <param name="returnCode"></param>
            public PictureResult(int time, int type, String text, int? major, int? minor, int returnCode)
            {
                this.timeReallyUsed = time;
                this.typeReallyUsed = type;
                this.text = text;
                this.majorId = major ?? 0;
                this.minorId = minor ?? 0;
                this.returnCode = returnCode;
            }
        }

       
        
        /// <summary> Receive back a result object that includes all the details in a format trivial to use in other Java code, while passing only and exactly what's needed. This is a simple wrapper to picture2 and does no logic of its own.
        /// </summary>
        /// <param name="pict">The bytes of the picture to solve</param>
        /// <param name="timeout">How long the solution should take at most</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public PictureResult picture2(byte[] pict, int timeout, int type)
        {
            int[] to_wrapper = new int[] { timeout };
            int[] type_wrapper = new int[] { type };
            int[] major_wrapper = new int[1];
            int[] minor_wrapper = new int[1];
            String[] text_wrapper = new String[] { "" };

            int result = picture2(pict, to_wrapper, type_wrapper, text_wrapper, major_wrapper, minor_wrapper);
            return new PictureResult(to_wrapper[0], type_wrapper[0], text_wrapper[0], major_wrapper[0], minor_wrapper[0], result);
        }


        /// <summary>  say "thanks" to Java incapability to pass values by reference in order to use them as multiple returns all arrays[] are used as workarond to get values out of the function, really </summary>
        /// <param name="pict"></param>
        /// <param name="pict_to"></param>
        /// <param name="pict_type"></param>
        /// <param name="text"></param>
        /// <param name="major_id"></param>
        /// <param name="minor_id"></param>
        /// <returns></returns>
        public int picture2(
         byte[] pict,   // IN  picture binary data
         int[] pict_to,   // IN/OUT timeout specifier to be used, on return - really used specifier, see ptoXXX constants, ptoDEFAULT in case of unrecognizable
         int[] pict_type,   // IN/OUT type specifier to be used, on return - really used specifier, see ptXXX constants, ptUNSPECIFIED in case of unrecognizable
         String[] text,   // OUT text
         int[] major_id,  // OUT OPTIONAL major part of the picture ID
         int[] minor_id  // OUT OPTIONAL minor part of the picture ID
        )
        {

            if (_status != sCCC_PICTURE)
                return APIConstants.ccERR_STATUS;

            CCProtoPacket pack = new CCProtoPacket();
            pack.Command = CCProtoPacket.cmdCC_PICTURE2;


            CCPictDescr desc = new CCPictDescr();
            desc.setTimeout(pict_to[0]);
            desc.setType(pict_type[0]);
            desc.setMajorID(0);
            desc.setMinorID(0);
            desc.setData(pict);
            desc.calcSize();

            pack.setData(desc.pack());
            pack.calcSize();

            if (pack.PackTo(_stream) == false)
            {
                return APIConstants.ccERR_NET_ERROR;
            }

            if (pack.UnpackFrom(_stream, -1, -1) == false)
            {
                return APIConstants.ccERR_NET_ERROR;
            }

            switch (pack.Command)
            {
                case CCProtoPacket.cmdCC_TEXT2:
                    desc.unpack(pack.getData());
                    pict_to[0] = desc.getTimeout();
                    pict_type[0] = desc.getType();
                    text[0] = desc.getData() == null ? "" : Encoding.ASCII.GetString(desc.getData());

                    if (major_id != null)
                        major_id[0] = desc.getMajorID();
                    if (minor_id != null)
                        minor_id[0] = desc.getMinorID();

                    return APIConstants.ccERR_OK;

                case CCProtoPacket.cmdCC_BALANCE:
                    // balance depleted
                    return APIConstants.ccERR_BALANCE;

                case CCProtoPacket.cmdCC_OVERLOAD:
                    // server's busy
                    return APIConstants.ccERR_OVERLOAD;

                case CCProtoPacket.cmdCC_TIMEOUT:
                    // picture timed out
                    return APIConstants.ccERR_TIMEOUT;

                case CCProtoPacket.cmdCC_FAILED:
                    // server's error
                    return APIConstants.ccERR_GENERAL;

                default:
                    // unknown error
                    return APIConstants.ccERR_UNKNOWN;
            }
        } // picture2()

        /// <summary>
        /// 
        /// </summary>
        /// <param name="major_id"></param>
        /// <param name="minor_id"></param>
        /// <returns></returns>
        public int picture_bad2(int major_id, int minor_id)
        {
            CCProtoPacket pack = new CCProtoPacket();

            pack.Command = CCProtoPacket.cmdCC_PICTUREFL;

            CCPictDescr desc = new CCPictDescr();
            desc.setTimeout(APIConstants.ptoDEFAULT);
            desc.setType(APIConstants.ptUNSPECIFIED);
            desc.setMajorID(major_id);
            desc.setMinorID(minor_id);
            desc.calcSize();

            pack.setData(desc.pack());
            pack.calcSize();

            if (pack.PackTo(_stream) == false)
            {
                return APIConstants.ccERR_NET_ERROR;
            }

            return APIConstants.ccERR_NET_ERROR;
        } // picture_bad2()


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public BalanceResult GetBalance()
        {
            CCProtoPacket pack = null;

            if (_status != sCCC_PICTURE)
            {
                return new BalanceResult(APIConstants.ccERR_STATUS);
            }

            pack = new CCProtoPacket();
            pack.Command = CCProtoPacket.cmdCC_BALANCE;
            pack.Size = 0;

            if (pack.PackTo(_stream) == false)
            {
                return new BalanceResult(APIConstants.ccERR_NET_ERROR);
            }

            if (pack.UnpackFrom(_stream, -1, -1) == false)
            {
                return new BalanceResult(APIConstants.ccERR_NET_ERROR);
            }

            switch (pack.Command)
            {
                case CCProtoPacket.cmdCC_BALANCE:
                    return new BalanceResult(APIConstants.ccERR_OK, Encoding.ASCII.GetString(pack.getData()));
                default:
                    return new BalanceResult(APIConstants.ccERR_UNKNOWN);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Close()
        {
            CCProtoPacket pack = new CCProtoPacket();

            pack.Command = CCProtoPacket.cmdCC_BYE;
            pack.Size = 0;

            if (pack.PackTo(_stream) == false)
            {
                return APIConstants.ccERR_NET_ERROR;
            }

            try
            {
                _client.Close();
            }
            catch (Exception) { }
            _status = sCCC_INIT;

            return APIConstants.ccERR_OK;
        } // close()
    }

}
