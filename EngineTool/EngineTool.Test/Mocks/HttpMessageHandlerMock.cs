namespace EngineTool.Test.Mocks;

internal class HttpMessageHandlerMock(HttpResponseMessage? httpResponse = null, Exception? exception = null) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (exception != null)
        {
            throw exception;
        }

        httpResponse ??= new HttpResponseMessage();

        return Task.FromResult(httpResponse);
    }
}