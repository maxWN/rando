using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Rando.Common;
using Rando.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Moq.Protected;
using Microsoft.Extensions.Configuration;

namespace Rando.Tests;

public class InputRouterHelperTests
{
    private InputRouterHelper inputRouterHelperSut;
    private Mock<ILogger<InputRouterHelper>> mockLogger;
    private Mock<IHttpClientFactory> mockhttpClientFactory;
    private Mock<IFileCreatorHelper> mockfileCreatorHelper;
    private Mock<IInputEvaluatorHelper> mockinputEvaluatorHelper;
    private Mock<HttpClient> mockHttpClient;
    private Mock<IConfiguration> mockConfiguration;
    private Mock<ISqlDbBuilder> mockSqlDbBuilder;

    public InputRouterHelperTests()
    {
        mockLogger = new Mock<ILogger<InputRouterHelper>>();
        mockhttpClientFactory = new Mock<IHttpClientFactory>();
        mockinputEvaluatorHelper = new Mock<IInputEvaluatorHelper>();
        mockfileCreatorHelper = new Mock<IFileCreatorHelper>();
        mockHttpClient = new Mock<HttpClient>();
        mockConfiguration = new Mock<IConfiguration>();
        mockSqlDbBuilder = new Mock<ISqlDbBuilder>();
        inputRouterHelperSut = new InputRouterHelper(mockLogger.Object, mockhttpClientFactory.Object,
            mockfileCreatorHelper.Object, mockinputEvaluatorHelper.Object, mockConfiguration.Object, mockSqlDbBuilder.Object);
    }

    [Test]
    public void GetMockDataAsync_Success()
    {
        // arrange
        UserInput userInput = new UserInput
        {
            DataType = "USERS",
            Quantity = 10,
        };
        string responseJsonString = "{ \"id\":7138,\"uid\":\"84fb2bc1-ccc8-41c7-98c9-dfa2e1a33ffb\",\"brand\":\"Tsingtao\",\"name\":\"Sublimely Self-Righteous Ale\",\"style\":\"Bock\",\"hop\":\"Citra\",\"yeast\":\"1469 - West Yorkshire Ale\",\"malts\":\"Caramel\",\"ibu\":\"67 IBU\",\"alcohol\":\"4.6%\",\"blg\":\"18.6°Blg\"}";

        // requestUri: $"{userInput.DataType.ToLowerInvariant()}?size={userInput.Quantity}&is_xml=true"

        // https://stackoverflow.com/questions/57091410/unable-to-mock-httpclient-postasync-in-unit-tests
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("GetAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(responseJsonString) });

        var client = new HttpClient(mockHttpMessageHandler.Object);

        // act
        //var response = mockHttpClient.Setup(x => x.GetAsync(It.IsAny<string>()))
        //    .Returns(Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)));

        var response = inputRouterHelperSut.GetMockDataAsync(userInput);

        // assert
        Assert.IsNotNull(response);
    }

    [TearDown]
    public void Teardown()
    {
    }
}