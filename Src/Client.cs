namespace GameServer
{
    public class Client
    {
        private readonly int _id;

        public TCP Tcp { get; }

        public Client(int clientId)
        {
            _id = clientId;
            Tcp = new TCP(_id);
        }
    }
}