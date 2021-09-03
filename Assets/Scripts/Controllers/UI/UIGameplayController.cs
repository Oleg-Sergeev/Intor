namespace Assets.Scripts.Controllers.UI
{
    public class UIGameplayController : UIBaseController
    {
        public void Pause(UIPauseController uiPause)
        {
            uiPause.Toggle();
            Disable();
        }
    }
}
