using UnityEngine;
using UnityEngine.UI;

namespace Modules.UIService
{
    public class ARModeSwitcherUI : MonoBehaviour
    {
        public Button SwitchMode;
        public Text Label;

        public void SetLabel(string text)
        {
            Label.text = text;
        }
    }
}