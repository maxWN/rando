// using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Rando.Common;
using Rando.Helpers;

namespace Rando.Tests;

public class InputEvaluatorHelperTests 
{
    private readonly ILogger<InputRouterHelper> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IFileCreatorHelper _fileCreatorHelper;
    private readonly IInputEvaluatorHelper _inputEvaluatorHelper;
    // private readonly IConfiguration _configuration;
    // private readonly ISqlDbBuilder _sqlDbBuilder;
    private readonly IInputRouterHelper _inputRouterHelper;

    public InputEvaluatorHelperTests() {
        _logger = Substitute.For<ILogger<InputRouterHelper>>();
        _httpClientFactory = Substitute.For<IHttpClientFactory>();
        _fileCreatorHelper = Substitute.For<IFileCreatorHelper>();
        // _sqlDbBuilder = Substitute.For<ISqlDbBuilder>();
        _inputEvaluatorHelper = Substitute.For<IInputEvaluatorHelper>();
        // _configuration = Substitute.For<IConfiguration>();
        _inputRouterHelper = new InputRouterHelper(_logger, _httpClientFactory, _fileCreatorHelper, _inputEvaluatorHelper/*, _configuration, _sqlDbBuilder*/);
    }

    [Fact]
    public void HandleUserInput_ThrowsArgumentNullException_WithNullInput()
    {
        // Arrange
        UserInput input = null;

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() => _inputRouterHelper.HandleUserInputAsync(input));
    }

    [Fact]
    public void HandleAdditionalUserInput_ThrowsArgumentNullException_WithNullInput()
    {
        // Arrange
        UserInput input = new UserInput { DataType = DataType.BEERS, Quantity = 1, FlagType = "asdfasfa" };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _inputRouterHelper.HandleAdditionalUserInput(input, "asdfadf"));
        Assert.Equal("Invalid file type provided!", ex.Message);
    }
}