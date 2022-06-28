using System;
using System.IO;
using System.Text;

namespace ChatClient.Net.IO
{
    //Добавляем данные в поток памяти 
    class PacketBuilder
    {
        MemoryStream _ms;
        public PacketBuilder()
        {
            _ms = new MemoryStream();
        }

        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }
        //Получаем данные в байтах и отпраем их на сервер
        public void WriteMessage(string msg)
        {
            var msgLenght = msg.Length;
            _ms.Write(BitConverter.GetBytes(msgLenght));
            _ms.Write(Encoding.ASCII.GetBytes(msg));
        }

        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        }

        internal void WriteString()
        {
            throw new NotImplementedException();
        }
    }
}
