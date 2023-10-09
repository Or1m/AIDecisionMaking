using Characters.Enums;
using Managers;
using System.Collections;
using Trees;
using UnityEngine;
#if !UNITY_EDITOR
using System.IO;
#endif

namespace Characters
{
    public abstract class NPCBrain : MonoBehaviour
    {
        private readonly ENPCAction defaultAction = ENPCAction.Idle;
#if UNITY_EDITOR
        private readonly string dataPath = Application.dataPath + "/Data";
#else
        private readonly string dataPath = Directory.GetCurrentDirectory() + "/Data";
#endif

        [SerializeField]
        protected NPCBody body;

        private bool isActive;
        private DecisionTree tree;
        
        protected abstract string TreeFileName { get; }
        protected abstract StateMachine StateMachine { get; }


        void Start()
        {
            isActive = true;
            tree = DecisionTree.FromFile($"{dataPath}/{TreeFileName}_{MainManager.Instance.ConfigManager.TreeAlgorithm}.txt");

            StateMachine.ChangeState(defaultAction, body);

            StartCoroutine(UpdateStateCoroutine());
        }

        IEnumerator UpdateStateCoroutine()
        {
            float delay = 0.2f;
            var wait = new WaitForSeconds(delay);

            while (isActive)
            {
                var currentAction = tree.Traverse(body);
                StateMachine.ChangeState(currentAction, body);

                yield return wait;
            }
        }

        private void Update()
        {
            StateMachine.OnUpdate(body);
        }

        private void OnDestroy()
        {
            isActive = false;
        }
    }
}
