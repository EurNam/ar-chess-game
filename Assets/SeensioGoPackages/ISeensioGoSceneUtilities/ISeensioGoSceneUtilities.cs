namespace JKTechnologies.SeensioGo.Scene
{
    public interface ISeensioGoSceneUtilities
    {
        public static ISeensioGoSceneUtilities Instance;
        public void InviteOtherUserToRoom();
        public void ExitRoom();
        public void VibratePop();
    }
}