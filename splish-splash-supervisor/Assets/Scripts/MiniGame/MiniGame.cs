public enum MiniGameStatus
{
    Win,
    Lose,
    Complete,
    Playing,
}
public interface IMiniGame
{
    public MiniGameStatus GetStatus();
    public void SetStatus(MiniGameStatus result);
}

public class MiniGame : IMiniGame
{
    private MiniGameStatus status;
    public MiniGame()
    {
        status = MiniGameStatus.Playing;
    }
    public MiniGameStatus GetStatus()
    {
        return status;
    }
    public void SetStatus(MiniGameStatus status)
    {
        this.status = status;
    }
}