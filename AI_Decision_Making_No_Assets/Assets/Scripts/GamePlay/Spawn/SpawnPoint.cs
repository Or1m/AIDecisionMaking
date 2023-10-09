using UnityEngine;
using Utils;

namespace GamePlay.Spawn
{
    public class SpawnPoint : MonoBehaviour
    {
        private void OnDrawGizmosSelected()
        {
            DrawArrow.ForGizmo(transform.position, transform.forward);
        }
    }
}
