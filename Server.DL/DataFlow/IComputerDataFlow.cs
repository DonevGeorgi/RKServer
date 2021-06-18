namespace Server.DL.DataFlow
{
    public interface IComputerDataFlow
    {
        void ProcessMessage(byte[] message);
    }
}
