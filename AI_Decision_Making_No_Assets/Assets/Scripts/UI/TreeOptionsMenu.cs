using Managers;
using Trees.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TreeOptionsMenu : SubMenu
    {
        [SerializeField]
        private Button id3Button;
        [SerializeField]
        private Button d45Button;
        [SerializeField]
        private Button cartButton;


        private void Awake()
        {
            id3Button.onClick.AddListener(OnID3ButtonClicked);
            d45Button.onClick.AddListener(OnD45ButtonClicked);
            cartButton.onClick.AddListener(OnCARTButtonClicked);
        }
        private void OnDestroy()
        {
            id3Button.onClick.RemoveAllListeners();
            d45Button.onClick.RemoveAllListeners();
            cartButton.onClick.RemoveAllListeners();
        }

        private void OnID3ButtonClicked()
        {
            MainManager.Instance.ConfigManager.TreeAlgorithm = ETreeAlgorithm.ID3;
        }
        private void OnD45ButtonClicked()
        {
            MainManager.Instance.ConfigManager.TreeAlgorithm = ETreeAlgorithm.D45;
        }
        private void OnCARTButtonClicked()
        {
            MainManager.Instance.ConfigManager.TreeAlgorithm = ETreeAlgorithm.CART;
        }
    }
}
