using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MyUnityGameServer
{
    public class ByteBuffer : IDisposable
    {
        private List<byte> bufferList;
        private byte[] readBuffer;
        private int readPosition;
        private bool isBufferUpdated = false;

        public int Count => bufferList.Count;
        public int Length => Count - readPosition;

        public ByteBuffer()
        {
            bufferList = new List<byte>();
            readPosition = 0;
        }

        public int GetReadPosition()
        {
            return readPosition;
        }
        public byte[] ToArray()
        {
            return bufferList.ToArray();
        }
        public void Clear()
        {
            bufferList.Clear();
            readPosition = 0;
        }
        
        public void Write(byte input)
        {
            bufferList.Add(input);
            isBufferUpdated = true;
        }
        public void Write(byte[] input)
        {
            bufferList.AddRange(input);
            isBufferUpdated = true;
        }
        public void Write(short input)
        {
            bufferList.AddRange(BitConverter.GetBytes(input));
            isBufferUpdated = true;
        }
        public void Write(int input)
        {
            bufferList.AddRange(BitConverter.GetBytes(input));
            isBufferUpdated = true;
        }
        public void Write(long input)
        {
            bufferList.AddRange(BitConverter.GetBytes(input));
            isBufferUpdated = true;
        }
        public void Write(float input)
        {
            bufferList.AddRange(BitConverter.GetBytes(input));
            isBufferUpdated = true;
        }
        public void Write(bool input)
        {
            bufferList.AddRange(BitConverter.GetBytes(input));
            isBufferUpdated = true;
        }
        public void Write(string input)
        {
            bufferList.AddRange(BitConverter.GetBytes(input.Length));
            bufferList.AddRange(Encoding.ASCII.GetBytes(input));
            isBufferUpdated = true;
        }

        public byte ReadByte(bool peek = true)
        {
            if (bufferList.Count > readPosition)
            {
                if (isBufferUpdated)
                {
                    readBuffer = bufferList.ToArray();
                    isBufferUpdated = false;
                }

                byte value = readBuffer[readPosition];
                if (peek)
                {
                    readPosition += 1;
                }

                return value;
            }
            
            throw new Exception("You are not trying to read out a 'BYTE'");
        }
        public byte[] ReadBytes(int length, bool peek = true)
        {
            if (bufferList.Count > readPosition)
            {
                if (isBufferUpdated)
                {
                    readBuffer = bufferList.ToArray();
                    isBufferUpdated = false;
                }

                byte[] value = bufferList.GetRange(readPosition, length).ToArray();
                if (peek)
                {
                    readPosition += length;
                }

                return value;
            }
            
            throw new Exception("You are not trying to read out a 'BYTE[]'");
        }
        public short ReadShort(bool peek = true)
        {
            if (bufferList.Count > readPosition)
            {
                if (isBufferUpdated)
                {
                    readBuffer = bufferList.ToArray();
                    isBufferUpdated = false;
                }

                short value = BitConverter.ToInt16(readBuffer, readPosition);
                if (peek)
                {
                    readPosition += 2;
                }

                return value;
            }
            
            throw new Exception("You are not trying to read out a 'SHORT'");
        }
        public int ReadInt(bool peek = true)
        {
            if (bufferList.Count > readPosition)
            {
                if (isBufferUpdated)
                {
                    readBuffer = bufferList.ToArray();
                    isBufferUpdated = false;
                }

                int value = BitConverter.ToInt32(readBuffer, readPosition);
                if (peek)
                {
                    readPosition += 4;
                }

                return value;
            }
            
            throw new Exception("You are not trying to read out a 'INT'");
        }
        public long ReadLong(bool peek = true)
        {
            if (bufferList.Count > readPosition)
            {
                if (isBufferUpdated)
                {
                    readBuffer = bufferList.ToArray();
                    isBufferUpdated = false;
                }

                long value = BitConverter.ToInt64(readBuffer, readPosition);
                if (peek)
                {
                    readPosition += 8;
                }

                return value;
            }
            
            throw new Exception("You are not trying to read out a 'LONG'");
        }
        public float ReadFloat(bool peek = true)
        {
            if (bufferList.Count > readPosition)
            {
                if (isBufferUpdated)
                {
                    readBuffer = bufferList.ToArray();
                    isBufferUpdated = false;
                }

                float value = BitConverter.ToSingle(readBuffer, readPosition);
                if (peek)
                {
                    readPosition += 4;
                }

                return value;
            }
            
            throw new Exception("You are not trying to read out a 'FLOAT'");
        }
        public bool ReadBool(bool peek = true)
        {
            if (bufferList.Count > readPosition)
            {
                if (isBufferUpdated)
                {
                    readBuffer = bufferList.ToArray();
                    isBufferUpdated = false;
                }

                bool value = BitConverter.ToBoolean(readBuffer, readPosition);
                if (peek)
                {
                    readPosition += 1;
                }

                return value;
            }
            
            throw new Exception("You are not trying to read out a 'BOOL'");
        }
        public string ReadString(bool peek = true)
        {
            try
            {
                int length = ReadInt();
            
                if (isBufferUpdated)
                {
                    readBuffer = bufferList.ToArray();
                    isBufferUpdated = false;
                }

                string value = Encoding.ASCII.GetString(readBuffer, readPosition, length);
            
                if (peek)
                {
                    if (value.Length > 0) readPosition += length;
                }
            
                return value;
            }
            catch
            {
                throw new Exception("You are not trying to read out a 'STRING'");
            }
        }

        private bool isValuesDisposed = false;
        public void Dispose()
        {
            if (!isValuesDisposed)
            {
                bufferList.Clear();
                readPosition = 0;
                isValuesDisposed = true;
                
                GC.SuppressFinalize(this);
            }
        }
    }
}