namespace Rando.Common;

public static class AppConstants {
    
    /// <summary>
    /// The base URL to make requests for randomized data
    /// </summary>
    public const string RANDOM_DATA_API_BASE_URL = "https://random-data-api.com/api/v2/";

    /// <summary>
    /// Instructions shown when user either uses help command or makes a mistake
    /// </summary>
    public const string USER_DIRECTIONS = "Usage: rando.exe [arguments] [options] \n\nArguments: \n* Data Type\tRepresents types in Random API docs\n* Quantity (OPTIONAL)\tAmount of entries you want Random API to return \n\nOptions: \n* --help\t\t\t\t\tPrint list of rando commands with details on proper usage\n* --file-output <file-path> <file-name.extension>\tIndicates that a file will be created containing the randomized data\n* --db-output <table-name>\t\t\tIndicates randomized data will be inserted into table of choice (requires SQL Dialect, Connection String, and Database Name in config file)";

}