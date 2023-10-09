using Characters.Enums;
using Characters.Player.Enums;
using System;
using System.IO;
using Trees.Enums;
using UnityEngine;

namespace Managers
{
    public class ConfigManager : MonoBehaviour
    {
        private const string settingsName = "/settings.txt";

        private string settingsPath;

        [SerializeField]
        private ETreeAlgorithm treeAlgorithm;
        public ETreeAlgorithm TreeAlgorithm { get { return treeAlgorithm; } set { treeAlgorithm = value; Serialize(); } }

        [SerializeField]
        private EPlayerControllMethod playerControll;
        public EPlayerControllMethod PlayerControll { get { return playerControll; } set { playerControll = value; } }

        [SerializeField]
        private ENPCControllMethod npcControll;
        public ENPCControllMethod NPCControll { get { return npcControll; } set { npcControll = value; Serialize(); } }

        public event Action OnDataChanged;


        public void OnAwake()
        {
            settingsPath = Application.persistentDataPath + settingsName;
            
            Deserialize();
            OnDataChanged?.Invoke();
        }
        public void OnStart() 
        {
            // Intentionally empty
        }

        private void Deserialize()
        {
            if (!File.Exists(settingsPath))
                return;

            var lines = File.ReadAllLines(settingsPath);

            var npcControllStr = lines[0].Split(": ")[1];
            var treeAlgorithmStr = lines[1].Split(": ")[1];

            if (Enum.TryParse<ENPCControllMethod>(npcControllStr, out var newNPCControll))
                npcControll = newNPCControll;

            if (Enum.TryParse<ETreeAlgorithm>(treeAlgorithmStr, out var newTreeAlgorithm))
                treeAlgorithm = newTreeAlgorithm;
        }
        private void Serialize()
        {
            string[] settings = new string[]
            {
                $"npcControll: {npcControll}",
                $"treeAlgorithm: {treeAlgorithm}"
            };

            OnDataChanged?.Invoke();
            File.WriteAllLines(settingsPath, settings);
        }
    }
}
