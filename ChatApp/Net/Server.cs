using System;
using ChatClient.Net.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChatClient.Net
{
    class Server
    {
        TcpClient _client;
        //Чтение пакетов
        public PacketReader PacketReader;
        //События (подлючеения, отправления сообщения и отключения)
        public event Action connectedEvent;
        public event Action msgReceievedEvent;
        public event Action userDiscinnectEvent;

        public Server()
        {
            _client = new TcpClient();
        }
        //не подключен ли к серверу и подключение
        public void ConnectToServer(string username)
        {
            if (!_client.Connected)
            {


                _client.Connect("127.0.0.1", 7891);
                PacketReader = new PacketReader(_client.GetStream());

                if (!string.IsNullOrEmpty(username))
                {   
                    //отправляем пакет данных на сервак
                    var connectPacket = new PacketBuilder();

                    connectPacket.WriteOpCode(0);
                    connectPacket.WriteMessage(username);

                    _client.Client.Send(connectPacket.GetPacketBytes());
                }
                ReadPackets();

            }

        }

        //Загружаем данные в разные потоки
        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var opcode = PacketReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;
                        case 5:
                            msgReceievedEvent?.Invoke();
                            break;
                        case 10:
                            userDiscinnectEvent?.Invoke();
                            break;

                        default:
                            Console.WriteLine("YESSS");
                            break;
                    }
                }
            });
        }

        //показывает введенное сообщение 
        public void SendMessageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(message);
            _client?.Client.Send(messagePacket.GetPacketBytes());
        }
    }
}
