public enum GameState
{
    Dialogue,
    Interaction
}

public static class GameFlow
{
    public static GameState State = GameState.Dialogue;
}