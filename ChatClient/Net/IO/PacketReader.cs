using System.IO;
using System.Net.Sockets;
using System.Text;


namespace ChatServer.Net.IO
{
    //Пакет чтения данных для сервера
    class PacketReader : BinaryReader
    {
        //Частный сетевой поток
        private NetworkStream _ns;
        public PacketReader(NetworkStream ns) : base(ns)
        {
            _ns = ns;

        }
        //Чтения сообщения от пользователя 
        public string ReadMessage()
        {
            //Буфферка сообщения массива байтов
            byte[] msgBuffer;
            var lenght = ReadInt32();
            msgBuffer = new byte[lenght];
            //Длинна фактического пакета данных
            _ns.Read(msgBuffer, 0, lenght);

            //Расшифровка данных
            var msg = Encoding.ASCII.GetString(msgBuffer);
            return msg;

        }
    }
}
