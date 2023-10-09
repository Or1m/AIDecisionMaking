using Managers;
using UnityEngine;

namespace UI
{
    public class SubMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject currentSubMenu;
        [SerializeField]
        private GameObject previousSubMenu;


        private void Update()
        {
            if (MainManager.Instance.InputManager.WasCancelledLastFrame)
            {
                currentSubMenu.SetActive(false);
                previousSubMenu.SetActive(true);
            }
        }
    }
}
