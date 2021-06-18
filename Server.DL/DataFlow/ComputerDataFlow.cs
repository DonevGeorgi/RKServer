using MessagePack;
using Server.Models.Models;
using System.Threading.Tasks.Dataflow;

namespace Server.DL.DataFlow
{
    public class ComputerDataFlow : IComputerDataFlow
    {
        private TransformBlock<byte[], Computer> _deserializeBlock;
        private TransformBlock<Computer, Computer> _calculationBlock;

        public ComputerDataFlow()
        {
            _deserializeBlock = new TransformBlock<byte[], Computer>(msg => MessagePackSerializer.Deserialize<Computer>(msg));

            var linkOptions = new DataflowLinkOptions()
            {
                PropagateCompletion = true
            };
            _deserializeBlock.LinkTo(_calculationBlock, linkOptions);
        }
        public async void ProcessMessage(byte[] message)
        {
            await _deserializeBlock.SendAsync(message);
        }
    }
}
