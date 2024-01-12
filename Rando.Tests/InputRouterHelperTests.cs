using Microsoft.Extensions.Logging;
using Moq;
using Rando.Common;
using Rando.Helpers;

namespace Rando.Tests;

public class InputRouterHelperTests
{
    private InputRouterHelper inputRouterHelperSut;
    private Mock<ILogger<InputRouterHelper>> mockLogger;
    private Mock<IHttpClientFactory> mockhttpClientFactory;
    private Mock<IFileCreatorHelper> mockfileCreatorHelper;
    private Mock<IInputEvaluatorHelper> mockinputEvaluatorHelper;

    public InputRouterHelperTests()
    {
        mockLogger = new Mock<ILogger<InputRouterHelper>>();
        mockhttpClientFactory = new Mock<IHttpClientFactory>();
        mockinputEvaluatorHelper = new Mock<IInputEvaluatorHelper>();
        mockfileCreatorHelper = new Mock<IFileCreatorHelper>();
        inputRouterHelperSut = new InputRouterHelper(mockLogger.Object, mockhttpClientFactory.Object,
            mockfileCreatorHelper.Object, mockinputEvaluatorHelper.Object);
    }

    [Test]
    public void GetMockDataAsync_Success() 
    {
    }

    [TearDown]
    public void Teardown()
    {
    }
}