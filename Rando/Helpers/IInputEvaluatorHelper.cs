using Rando.Common;

namespace Rando.Helpers;

public interface IInputEvaluatorHelper
{
    UserInput GetUserInputObject(string[]? args);

}