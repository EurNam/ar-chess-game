using UnityEngine;
using UnityEngine.UI;
using System.Collections;

    public class ToggleVirtualCursor : MonoBehaviour
    {
        public Button toggleButton;

        void Start()
        {
            toggleButton.onClick.AddListener(ToggleVirtualMouse);
        }

        void ToggleVirtualMouse()
        {
            if (VirtualMouse.Instance != null)
            {
                VirtualMouse.Instance.ToggleVirtualMouse();
            }
        }
    }