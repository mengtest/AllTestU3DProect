using System;
using System.IO;
using System.Text;

namespace Game
{
    public class ByteBuffer
    {
        MemoryStream stream = null;
        BinaryWriter writer = null;
        BinaryReader reader = null;

        #region 通用方法
        public ByteBuffer()
        {
            stream = new MemoryStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
        }

        public ByteBuffer(byte[] data)
        {
            if (data != null)
            {
                stream = new MemoryStream(data);
                reader = new BinaryReader(stream);
            }
            else
            {
                stream = new MemoryStream();
                writer = new BinaryWriter(stream);
            }
        }

        public int Length
        {
            get { return (int)stream.Length; }
        }

        public int Position
        {
            get { return (int)stream.Position; }
            set { stream.Position = value; }
        }

        public void Flush()
        {
            if (writer != null) writer.Flush();
            stream.Flush();
        }

        public void Clear()
        {
            Flush();
            stream.Seek(0, SeekOrigin.Begin);
            stream.SetLength(0);
        }

        public void SetPos(int l)
        {
            stream.Seek(l, SeekOrigin.Begin);
        }

        public void Close()
        {
            if (writer != null) writer.Close();
            if (reader != null) reader.Close();

            stream.Close();
            writer = null;
            reader = null;
            stream = null;
        }
        #endregion

        #region 写的方法
        public void WriteSByte(sbyte v)
        {
            writer.Write(v);
        }

        public void WriteByte(byte v)
        {
            writer.Write(v);
        }

        public void WriteInt16(Int16 v)
        {
            writer.Write(v);
        }
        public void WriteUInt16(UInt16 v)
        {
            writer.Write(v);
        }
        public void WriteInt32(Int32 v)
        {
            writer.Write(v);
        }

        public void WriteUInt32(UInt32 v)
        {
            writer.Write(v);
        }

        public void WriteInt64(string v)
        {
            Int64 tmp = Convert.ToInt64(v);
            writer.Write(tmp);
        }

        public void WriteUInt64(string v)
        {
            UInt64 tmp = Convert.ToUInt64(v);
            writer.Write(tmp);
        }
        public void WriteString(string v)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(v);
            writer.Write((UInt16)bytes.Length); // 带string长度
            writer.Write(bytes);
        }

        public void WriteBytes(byte[] v, bool isSeekBegin = false)
        {
            writer.Write(v);

            if (isSeekBegin)
                Position = 0;
        }
        public void WriteBuffer(ByteBuffer strBuffer)
        {
            WriteBytes(strBuffer.stream.ToArray());
        }

        #endregion

        #region 读的方法
        public sbyte ReadSByte()
        {
            return reader.ReadSByte();
        }

        public byte ReadByte()
        {
            return reader.ReadByte();
        }

        public Int16 ReadInt16()
        {
            return reader.ReadInt16();
        }

        public UInt16 ReadUInt16()
        {
            return reader.ReadUInt16();
        }

        public Int32 ReadInt32()
        {
            return reader.ReadInt32();
        }

        public UInt32 ReadUInt32()
        {
            return reader.ReadUInt32();
        }

        public string ReadInt64()
        {
            return reader.ReadInt64().ToString();
        }

        public string ReadUInt64()
        {
            return reader.ReadUInt64().ToString();
        }

        public string ReadString()
        {
            Int16 len = reader.ReadInt16();
            byte[] buffer = new byte[len];
            buffer = reader.ReadBytes(len);
            string ret = Encoding.UTF8.GetString(buffer);
            return ret;
        }

        public byte[] ReadBytes(int count)
        {
            return reader.ReadBytes(count);
        }
        public ByteBuffer ReadBuffer(int count)
        {
            byte[] bytes = reader.ReadBytes(count);
            return new ByteBuffer(bytes);
        }

        public byte[] ToBytes()
        {
            writer.Flush();
            return stream.ToArray();
        }

        public string ToUTF8String()
        {
            if (Length > 0)
            {
                byte[] buffer = stream.ToArray();
                return Encoding.UTF8.GetString(buffer);
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion
    }

}
