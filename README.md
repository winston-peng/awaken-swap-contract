# awaken-swap-contract

BRANCH | AZURE PIPELINES                                                                                                                                                                                                                                                                | TESTS                                                                                                                                                                                                                        | CODE COVERAGE
-------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------
MASTER   | [![Build Status](https://dev.azure.com/Awaken-Finance/awaken-swap-contract/_apis/build/status/awaken-swap-contract?branchName=master)](https://dev.azure.com/Awaken-Finance/awaken-swap-contract/_build/latest?definitionId=8&branchName=master) | [![Test Status](https://img.shields.io/azure-devops/tests/Awaken-Finance/awaken-swap-contract/8/master)](https://dev.azure.com/Awaken-Finance/awaken-swap-contract/_build/latest?definitionId=8&branchName=master) | [![codecov](https://codecov.io/gh/Awaken-Finance/awaken-swap-contract/branch/master/graph/badge.svg?token=eiF4Wb65pJ)](https://codecov.io/gh/Awaken-Finance/awaken-swap-contract)
DEV    | [![Build Status](https://dev.azure.com/Awaken-Finance/awaken-swap-contract/_apis/build/status/awaken-swap-contract?branchName=dev)](https://dev.azure.com/Awaken-Finance/awaken-swap-contract/_build/latest?definitionId=8&branchName=dev) | [![Test Status](https://img.shields.io/azure-devops/tests/Awaken-Finance/awaken-swap-contract/8/dev)](https://dev.azure.com/Awaken-Finance/awaken-swap-contract/_build/latest?definitionId=8&branchName=dev) | [![codecov](https://codecov.io/gh/Awaken-Finance/awaken-swap-contract/branch/dev/graph/badge.svg?token=eiF4Wb65pJ)](https://codecov.io/gh/Awaken-Finance/awaken-swap-contract)

AwakenSwap is a decentralized exchange (DEX) based on the Automated Market Maker (AMM) algorithm. Thriving on aelf chain, AwakenSwap supports swapping between two arbitrary tokens.

## Installation

Before cloning the code and deploying the contract, command dependencies and development tools are needed. You can follow:

- [Common dependencies](https://aelf-boilerplate-docs.readthedocs.io/en/latest/overview/dependencies.html)
- [Building sources and development tools](https://aelf-boilerplate-docs.readthedocs.io/en/latest/overview/tools.html)

The following command will clone Awaken Swap Contract into a folder. Please open a terminal and enter the following command:

```Bash
git clone https://github.com/Awaken-Finance/awaken-swap-contract
```

The next step is to build the contract to ensure everything is working correctly. Once everything is built, you can run as follows:

```Bash
# enter the Launcher folder and build 
cd src/AElf.Boilerplate.SwapContract.Launcher

# build
dotnet build

# run the node 
dotnet run
```

It will run a local temporary aelf node and automatically deploy the Awaken Swap Contract on it. You can access the node from `localhost:1235`.

This temporary aelf node runs on a framework called Boilerplate for deploying smart contract easily. When running it, you might see errors showing incorrect password. To solve this, you need to back up your `aelf/keys`folder and start with an empty keys folder. Once you have cleaned the keys folder, stop and restart the node with `dotnet run`command shown above. It will automatically generate a new aelf account for you. This account will be used for running the aelf node and deploying the Awaken Swap Contract.

### Test

You can easily run unit tests on Awaken Swap Contracts. Navigate to the Awaken.Contracts.Swap.Tests and run:

```Bash
cd ../../test/Awaken.Contracts.Swap.Tests
dotnet test
```