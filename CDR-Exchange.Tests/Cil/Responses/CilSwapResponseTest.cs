using CDR_Exchange.Services.Cil.Responses;

namespace CDR_Exchange.Tests.Cil.Responses;

public class CilSwapResponseTest
{
    [Fact]
    public void CilSwapResponse_Test()
    {
        var response = @"{
                'result': 'wallet',
            }";

        var instance = new CilSwapResponse(response);
        Assert.Equal("wallet", instance.Wallet);
    }
}