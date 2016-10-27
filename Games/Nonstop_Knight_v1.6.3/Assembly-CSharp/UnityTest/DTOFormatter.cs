namespace UnityTest
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;

    public class DTOFormatter
    {
        public object Deserialize(Stream stream)
        {
            ResultDTO safeUninitializedObject = (ResultDTO) FormatterServices.GetSafeUninitializedObject(typeof(ResultDTO));
            this.Transfer(safeUninitializedObject, new Reader(stream));
            return safeUninitializedObject;
        }

        public void Serialize(Stream stream, ResultDTO dto)
        {
            this.Transfer(dto, new Writer(stream));
        }

        private void Transfer(ResultDTO dto, ITransferInterface transfer)
        {
            transfer.Transfer(ref dto.messageType);
            transfer.Transfer(ref dto.levelCount);
            transfer.Transfer(ref dto.loadedLevel);
            transfer.Transfer(ref dto.loadedLevelName);
            if (((dto.messageType != ResultDTO.MessageType.Ping) && (dto.messageType != ResultDTO.MessageType.RunStarted)) && ((dto.messageType != ResultDTO.MessageType.RunFinished) && (dto.messageType != ResultDTO.MessageType.RunInterrupted)))
            {
                transfer.Transfer(ref dto.testName);
                transfer.Transfer(ref dto.testTimeout);
                if (transfer is Reader)
                {
                    dto.testResult = new SerializableTestResult();
                }
                SerializableTestResult testResult = (SerializableTestResult) dto.testResult;
                transfer.Transfer(ref testResult.resultState);
                transfer.Transfer(ref testResult.message);
                transfer.Transfer(ref testResult.executed);
                transfer.Transfer(ref testResult.name);
                transfer.Transfer(ref testResult.fullName);
                transfer.Transfer(ref testResult.id);
                transfer.Transfer(ref testResult.isSuccess);
                transfer.Transfer(ref testResult.duration);
                transfer.Transfer(ref testResult.stackTrace);
            }
        }

        private interface ITransferInterface
        {
            void Transfer(ref bool val);
            void Transfer(ref byte val);
            void Transfer(ref double val);
            void Transfer(ref int val);
            void Transfer(ref float val);
            void Transfer(ref string val);
            void Transfer(ref ResultDTO.MessageType val);
            void Transfer(ref TestResultState val);
        }

        private class Reader : DTOFormatter.ITransferInterface
        {
            private readonly Stream _stream;

            public Reader(Stream stream)
            {
                this._stream = stream;
            }

            private byte[] ReadConvertedNumber(int size)
            {
                byte[] buffer = new byte[size];
                this._stream.Read(buffer, 0, buffer.Length);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(buffer);
                }
                return buffer;
            }

            public void Transfer(ref bool val)
            {
                val = this._stream.ReadByte() != 0;
            }

            public void Transfer(ref byte val)
            {
                val = (byte) this._stream.ReadByte();
            }

            public void Transfer(ref double val)
            {
                val = BitConverter.ToDouble(this.ReadConvertedNumber(8), 0);
            }

            public void Transfer(ref int val)
            {
                val = BitConverter.ToInt32(this.ReadConvertedNumber(4), 0);
            }

            public void Transfer(ref float val)
            {
                val = BitConverter.ToSingle(this.ReadConvertedNumber(4), 0);
            }

            public void Transfer(ref string val)
            {
                int num = 0;
                this.Transfer(ref num);
                byte[] buffer = new byte[num];
                this._stream.Read(buffer, 0, num);
                val = Encoding.BigEndianUnicode.GetString(buffer);
            }

            public void Transfer(ref ResultDTO.MessageType val)
            {
                val = (ResultDTO.MessageType) ((byte) this._stream.ReadByte());
            }

            public void Transfer(ref TestResultState val)
            {
                val = (TestResultState) ((byte) this._stream.ReadByte());
            }
        }

        private class Writer : DTOFormatter.ITransferInterface
        {
            private readonly Stream _stream;

            public Writer(Stream stream)
            {
                this._stream = stream;
            }

            public void Transfer(ref bool val)
            {
                this._stream.WriteByte(!val ? ((byte) 0) : ((byte) 1));
            }

            public void Transfer(ref byte val)
            {
                this._stream.WriteByte(val);
            }

            public void Transfer(ref double val)
            {
                this.WriteConvertedNumber(BitConverter.GetBytes(val));
            }

            public void Transfer(ref int val)
            {
                this.WriteConvertedNumber(BitConverter.GetBytes(val));
            }

            public void Transfer(ref float val)
            {
                this.WriteConvertedNumber(BitConverter.GetBytes(val));
            }

            public void Transfer(ref string val)
            {
                byte[] bytes = Encoding.BigEndianUnicode.GetBytes(val);
                int length = bytes.Length;
                this.Transfer(ref length);
                this._stream.Write(bytes, 0, bytes.Length);
            }

            public void Transfer(ref ResultDTO.MessageType val)
            {
                this._stream.WriteByte((byte) val);
            }

            public void Transfer(ref TestResultState val)
            {
                this._stream.WriteByte((byte) val);
            }

            private void WriteConvertedNumber(byte[] bytes)
            {
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes);
                }
                this._stream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}

