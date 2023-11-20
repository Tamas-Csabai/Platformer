using UnityEngine;

namespace Main
{
    public class Setup : MonoBehaviour
    {

        [SerializeField] private GameObject[] findOnObjects;

        private void Awake()
        {
            for (int i = 0; i < findOnObjects.Length; i++)
            {
                ISetup[] setups = findOnObjects[i].GetComponents<ISetup>();

                for (int j = 0; j < setups.Length; j++)
                    setups[j].Setup();
            }
        }

    }
}
