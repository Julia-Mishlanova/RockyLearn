using Braintree;
using Microsoft.Extensions.Options;

namespace Rocky.Utility.BrainTree
{
    public class BrainTreeGate : IBrainTreeGate
    {
        public BrainTreeSettings _options { get; set; }
        private IBraintreeGateway _brainTreeGateway { get; set; }
        public BrainTreeGate(IOptions<BrainTreeSettings> options) 
        {
            _options = options.Value;
        }
        public IBraintreeGateway CreateGateway()
        {
            return new BraintreeGateway(_options.Enviroment, _options.MerchantId, _options.PublicKey, _options.PrivateKey);
        }

        public IBraintreeGateway GetGateway()
        {
            if (_brainTreeGateway == null) 
            {
                _brainTreeGateway = CreateGateway();
            }
            return _brainTreeGateway;
        }
    }
}
