using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Extensions
{
    public static class ButtonExtensions
    {
        public static void Reset(this Button button)
        {
            button.interactable = false;
            button.onClick.RemoveAllListeners();
        }

        public static void AddListeners(this Button button, params UnityAction[] listners)
        {
            foreach (var action in listners) 
                button.onClick.AddListener(action);
        }

        public static void AddListenersAndEnable(this Button button, params UnityAction[] listeners)
        {
            button.interactable = true;

            button.AddListeners(listeners);
        }
    }
}
