using Characters.Enums;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OptionsMenu : SubMenu
    {
        [SerializeField]
        private Button treeButton;
        [SerializeField]
        private Button mlButton;

        private void Awake()
        {
            treeButton.onClick.AddListener(OnTreeButtonClicked);
            mlButton.onClick.AddListener(OnMLButtonClicked);
        }
        private void OnDestroy()
        {
            if (treeButton)
                treeButton.onClick.RemoveAllListeners();

            if (mlButton)
                mlButton.onClick.RemoveAllListeners();
        }

        private void OnTreeButtonClicked()
        {
            MainManager.Instance.ConfigManager.NPCControll = ENPCControllMethod.DecisionTrees;
        }
        private void OnMLButtonClicked()
        {
            MainManager.Instance.ConfigManager.NPCControll = ENPCControllMethod.ReinforcementLearning;
        }
    }
}
