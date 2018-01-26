using System.Net.Sockets;

namespace TelegramClient.Core.Helpers
{
    public static class TcpClientHelper
    {
        public static bool IsConnected(this TcpClient client)
        {
            if (client == null || !client.Connected || client.Client == null || !client.Client.Connected)
            {
                return false;
            }

            if (client.Client.Poll(0, SelectMode.SelectRead))
            {
                byte[] buff = new byte[1];
                if (client.Client.Receive(buff, SocketFlags.Peek) == 0)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
