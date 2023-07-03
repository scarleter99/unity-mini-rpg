/**
 * @brief enum 정의
 */
public class Define
{
    public enum Layer
    {
        Ground = 6,
        Block = 7,
        Monster = 8,
    }
    
    public enum Scene
    {
        UnknownScene,
        TitleScene,
        GameScene,
    }
    
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }
    
    public enum UIEvent
    {
        Click,
        Drag,
    }
    
    public enum MouseEvent
    {
        Press,
        Click,
    }
    
    public enum CameraMode
    {
        QuarterView,
    }
}