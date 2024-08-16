namespace JKTechnologies.SeensioGo.Scene
{
    public interface ISeensioGoSceneUtilities
    {
        public static ISeensioGoSceneUtilities Instance;
        public void InviteOtherUserToRoom();
        public void ExitRoom();
        public void VibratePop();
        public bool IsBuildingMode();
        public void ReloadScene();
    }
}