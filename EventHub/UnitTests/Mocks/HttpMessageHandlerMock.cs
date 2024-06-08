namespace UnitTests.Mocks;

public class HttpMessageHandlerMock : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _sendAsync;

    public HttpMessageHandlerMock(Func<HttpRequestMessage, Task<HttpResponseMessage>> sendAsync)
    {
        _sendAsync = sendAsync;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return _sendAsync(request);
    }
}