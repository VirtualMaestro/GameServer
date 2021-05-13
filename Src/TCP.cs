using System;
using System.Net.Sockets;

namespace GameServer
{
   public class TCP
   {
       public const int DataBufferSize = 4096;
       
       private TcpClient _socket;
       private readonly int _id;
       private NetworkStream _stream;
       private byte[] _receiveBuffer;

       public TcpClient Socket => _socket;
       public int Id => _id;

       public TCP(int id)
       {
           _id = id;
       }

       public void Connect(TcpClient socket)
       {
           _socket = socket;
           _socket.ReceiveBufferSize = DataBufferSize;
           _socket.SendBufferSize = DataBufferSize;

           _stream = socket.GetStream();
           _receiveBuffer = new byte[DataBufferSize];

           _stream.BeginRead(_receiveBuffer, 0, DataBufferSize, _OnReceiveCallback, null);
       }

       private void _OnReceiveCallback(IAsyncResult result)
       {
           try
           {
               var byteLength = _stream.EndRead(result);
               
               if (byteLength <= 0)
               {
                   // TODO: disconnect
                   return;
               }
               
               var data = new byte[byteLength];
               Array.Copy(_receiveBuffer, data, byteLength);
               
               _stream.BeginRead(_receiveBuffer, 0, DataBufferSize, _OnReceiveCallback, null);
           }
           catch (Exception exception)
           {
               Console.WriteLine($"Error receiving TCP data: {exception}");
           }
       }
   }
}