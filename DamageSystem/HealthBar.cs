using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.DamageSystem
{
    public class HealthBar : MonoBehaviour
    {

        [SerializeField] private Health health;
        [SerializeField] private GameObject healthCell;
        [SerializeField] private float cellWidth = 0.2f;
        [SerializeField] private float cellPadding = 0.1f;

        private List<GameObject> _bars = new List<GameObject>();

        private void Awake()
        {
            health.CurrentHealth.OnChanged += SetBar;
        }

        private void OnDestroy()
        {
            if(health != null)
                health.CurrentHealth.OnChanged -= SetBar;
        }

        public void SetBar(int cellCount)
        {
            while(_bars.Count < cellCount)
                _bars.Add(AddHealthCell(_bars.Count));

            int index = 0;
            foreach(GameObject bar in _bars)
            {
                if (index < cellCount)
                    bar.SetActive(true);
                else
                    bar.SetActive(false);

                index++;
            }
        }

        private GameObject AddHealthCell(int index)
        {
            GameObject newHealthCell = Instantiate(healthCell);
            newHealthCell.transform.SetParent(transform, true);
            newHealthCell.transform.localPosition = new Vector3(index * (cellWidth + cellPadding), 0f, 0f);
            newHealthCell.transform.localRotation = Quaternion.identity;

            return newHealthCell;
        }
    }
}
