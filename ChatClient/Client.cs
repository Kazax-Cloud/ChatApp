using ChatServer.Net.IO;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;


namespace ChatServer
{
    class Client
    {
        public string Username { get; set; }
        //глобальный индетификатор
        public Guid UID { get; set; }
        public TcpClient ClientSocket { get; set; }

        //Считыватель пакетов
        PacketReader _packetReader;
        //Клиентский сокет и новый экземляр при подкл нового пользователя
        public Client(TcpClient client)
        {
            ClientSocket = client;
            //новый идентификатор при каждом новом польхователе
            UID = Guid.NewGuid();
            _packetReader = new PacketReader(ClientSocket.GetStream());
            //Считываем пакеты
            var opcode = _packetReader.ReadByte();
            //Считываем строку
            Username = _packetReader.ReadMessage();

            //Ассинхронное программирование =( я больше сюда не полезу!!!
            Task.Run(() => Process());
        }

        //Обрабатываем пакеты сообщения
        void Process()
        {
            while (true)
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 5:
                            var msg = _packetReader.ReadMessage();
                            Console.WriteLine($"[{DateTime.Now}]: Message receid! {msg}");
                            Program.BroadcastMessage($"[{DateTime.Now}]: [{Username}] {msg}");
                            break;
                        default:
                            break;
                    }
                }
                //Отключения пользователя от сервера
                catch (Exception)
                {
                    Console.WriteLine($"[{UID.ToString()}]: Disconnected!");
                    Program.BroadcastDisconnect(UID.ToString());
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }

}
