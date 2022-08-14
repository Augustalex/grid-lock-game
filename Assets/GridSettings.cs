using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "GridSettings", menuName = "GridSettings", order = 0)]
    public class GridSettings : ScriptableObject
    {
        public int gridSize = 8;
        public GameObject gridTemplate;
        
        public GameObject blockerBlock;
        public GameObject basicBlock;
        public GameObject factoryBlock;
        
        public float moveSpeed = 4f;
        public float timeScale = 1f;

        public AnimationCurve basicMovement;
        public float spawnTime = 4f;
    }
}