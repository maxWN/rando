using Rando.Common;
using Rando.Helpers;

public interface IInputEvaluator
{

    UserInput GetFormattedUserInput(ref string[]? args);

}