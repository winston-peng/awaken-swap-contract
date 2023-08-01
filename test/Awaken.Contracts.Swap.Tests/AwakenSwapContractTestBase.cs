using System.IO;
using System.Threading.Tasks;
using AElf.Cryptography.ECDSA;
using AElf.Kernel;
using AElf.Kernel.SmartContract.Application;
using AElf.Types;
using Google.Protobuf;
using System.Linq;
using AElf.Contracts.MultiToken;
using AElf.ContractTestBase.ContractTestKit;
using AElf.Kernel.Blockchain.Application;
using AElf.Kernel.Token;
using AElf.Standards.ACS0;
using Awaken.Contracts.Token;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Threading;
namespace Awaken.Contracts.Swap
{
    public class AwakenSwapContractTestBase : ContractTestBase<AwakenSwapContractTestModule>
    {
        internal readonly Address AwakenSwapContractAddress;
        
        internal readonly Address LpTokentContractAddress;
        
        internal readonly IBlockchainService blockChainService;
        internal ACS0Container.ACS0Stub ZeroContractStub { get; set; }

        private Address tokenContractAddress => GetAddress(TokenSmartContractAddressNameProvider.StringName);

        internal AwakenSwapContractContainer.AwakenSwapContractStub GetAwakenSwapContractStub(ECKeyPair senderKeyPair)
        {
            return GetTester<AwakenSwapContractContainer.AwakenSwapContractStub>(AwakenSwapContractAddress, senderKeyPair);
        }

        
        internal AwakenSwapContractContainer.AwakenSwapContractStub AwakenSwapContractStub =>
            GetAwakenSwapContractStub(SampleAccount.Accounts.First().KeyPair);
        
        internal AElf.Contracts.MultiToken.TokenContractContainer.TokenContractStub TokenContractStub =>
            GetTokenContractStub(SampleAccount.Accounts.First().KeyPair);

        internal AElf.Contracts.MultiToken.TokenContractContainer.TokenContractStub GetTokenContractStub(ECKeyPair senderKeyPair)
        {
            return Application.ServiceProvider.GetRequiredService<IContractTesterFactory>()
                .Create<AElf.Contracts.MultiToken.TokenContractContainer.TokenContractStub>(tokenContractAddress, senderKeyPair);
        }

        internal Awaken.Contracts.Token.TokenContractContainer.TokenContractStub GetLpContractStub(
            ECKeyPair senderKeyPair)
        {
            return Application.ServiceProvider.GetRequiredService<IContractTesterFactory>()
                .Create<Awaken.Contracts.Token.TokenContractContainer.TokenContractStub>(LpTokentContractAddress, senderKeyPair);
        }

        // You can get address of any contract via GetAddress method, for example:
        // internal Address DAppContractAddress => GetAddress(DAppSmartContractAddressNameProvider.StringName);
        public AwakenSwapContractTestBase()
        {
            ZeroContractStub = GetContractZeroTester(SampleAccount.Accounts[0].KeyPair);
            var result = AsyncHelper.RunSync(async () =>await ZeroContractStub.DeploySmartContract.SendAsync(new ContractDeploymentInput
            {   
                Category = KernelConstants.CodeCoverageRunnerCategory,
                Code = ByteString.CopyFrom(
                    File.ReadAllBytes(typeof(AwakenSwapContract).Assembly.Location))
            }));
            AwakenSwapContractAddress = Address.Parser.ParseFrom(result.TransactionResult.ReturnValue);
            result = AsyncHelper.RunSync(async () =>await ZeroContractStub.DeploySmartContract.SendAsync(new ContractDeploymentInput
            {   
                Category = KernelConstants.CodeCoverageRunnerCategory,
                Code = ByteString.CopyFrom(
                    File.ReadAllBytes(typeof(Token.TokenContract).Assembly.Location))
            }));
            LpTokentContractAddress = Address.Parser.ParseFrom(result.TransactionResult.ReturnValue);
            blockChainService = Application.ServiceProvider.GetRequiredService<IBlockchainService>();
        }

        private async Task<Address> DeployContractAsync(int category, byte[] code, ECKeyPair keyPair)
        {
            var addressService = Application.ServiceProvider.GetRequiredService<ISmartContractAddressService>();
            var stub = GetTester<ACS0Container.ACS0Stub>(addressService.GetZeroSmartContractAddress(),
                keyPair);
            var executionResult = await stub.DeploySmartContract.SendAsync(new ContractDeploymentInput
            {
                Category = category,
                Code = ByteString.CopyFrom(code)
            });
            return executionResult.Output;
        }

        private ECKeyPair AdminKeyPair { get; set; } = SampleAccount.Accounts[0].KeyPair;
        private ECKeyPair UserTomKeyPair { get; set; } = SampleAccount.Accounts.Last().KeyPair;
        private ECKeyPair UserLilyKeyPair { get; set; } = SampleAccount.Accounts.Reverse().Skip(1).First().KeyPair;

        internal Address UserTomAddress => Address.FromPublicKey(UserTomKeyPair.PublicKey);
        internal Address UserLilyAddress => Address.FromPublicKey(UserLilyKeyPair.PublicKey);

        internal Address AdminAddress => Address.FromPublicKey(AdminKeyPair.PublicKey);

        internal AwakenSwapContractContainer.AwakenSwapContractStub UserTomStub =>
            GetAwakenSwapContractStub(UserTomKeyPair);

        internal AwakenSwapContractContainer.AwakenSwapContractStub UserLilyStub =>
            GetAwakenSwapContractStub(UserLilyKeyPair);

        internal AElf.Contracts.MultiToken.TokenContractContainer.TokenContractStub UserTomTokenContractStub =>
            GetTokenContractStub(UserTomKeyPair);

        internal AElf.Contracts.MultiToken.TokenContractContainer.TokenContractStub UserLilyTokenContractStub =>
            GetTokenContractStub(UserLilyKeyPair);

        internal Awaken.Contracts.Token.TokenContractContainer.TokenContractStub AdminLpStub =>
            GetLpContractStub(AdminKeyPair);

        internal Awaken.Contracts.Token.TokenContractContainer.TokenContractStub TomLpStub =>
            GetLpContractStub(UserTomKeyPair);
        private Address GetAddress(string contractName)
        {
            var addressService = Application.ServiceProvider.GetRequiredService<ISmartContractAddressService>();
            var blockChainService = Application.ServiceProvider.GetRequiredService<IBlockchainService>();
            var chain = AsyncHelper.RunSync(blockChainService.GetChainAsync);
            var address = AsyncHelper.RunSync(() => addressService.GetSmartContractAddressAsync(new ChainContext()
            {
                BlockHash = chain.BestChainHash,
                BlockHeight = chain.BestChainHeight
            }, contractName)).SmartContractAddress.Address;
            return address;
        }
        internal ACS0Container.ACS0Stub GetContractZeroTester(
            ECKeyPair keyPair)
        {
            return GetTester<ACS0Container.ACS0Stub>(BasicContractZeroAddress,
                keyPair);
        }
    }
}