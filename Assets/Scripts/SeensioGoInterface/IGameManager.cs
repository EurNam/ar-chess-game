namespace JKTechnologies.SeensioGo.GameEngine
{
    public interface IGameManager
    {
        // Call if invoke start game
        public object GetGameSettings();
        public void SetGameSettings(object gameSettings);
        public void SwitchTurn();
        public bool IsMyTurn();
    }
}