namespace DiagnosticsSample;

public static partial class Log
{
    [LoggerMessage(
        EventId = 4000,
        Level = LogLevel.Information,
        Message = "Started a game `{Game}`")]
    public static partial void GameStarted(this ILogger logger, string game);

    [LoggerMessage(
        EventId = 4001,
        Level = LogLevel.Information,
        Message = "Game `{Game}` has ended")]
    public static partial void GameEnded(this ILogger logger, string game);

    [LoggerMessage(
        EventId = 4002,
        Level = LogLevel.Information,
        Message = "Received a move with {Move}, returing {Result}")]
    public static partial void SetMove(this ILogger logger, string move, string result);

    [LoggerMessage(
        EventId = 4003,
        Level = LogLevel.Information,
        Message = "New game cached, currently {Count} games active")]
    public static partial void GameCached(this ILogger logger, int count);

    [LoggerMessage(
        EventId = 4004,
        Level = LogLevel.Warning,
        Message = "Game {Id} not retrieved from cache")]
    public static partial void GameNotCached(this ILogger logger, string id);

    [LoggerMessage(
        EventId = 4005,
        Level = LogLevel.Error,
        Message = "Game {Id} not found in database")]
    public static partial void GameIdNotFound(this ILogger logger, string id);
}
