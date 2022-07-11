using System.Collections.Generic;
using ENA.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace ENA.UI
{
    [RequireComponent(typeof(LayoutGroup))]
    public class UIList: MonoBehaviour
    {
        #region Variables
        [SerializeField] GameObject prefab;
        #endregion
        #region Methods
        public GameObject Instance()
        {
            var obj = prefab.Instance();
            obj.SetParent(transform);
            return obj;
        }

        public void Resize(int newAmount)
        {
            if (newAmount > transform.childCount) {
                AddInstances(newAmount - transform.childCount);
            } else if (newAmount < transform.childCount) {
                RemoveInstances(transform.childCount - newAmount);
            }
        }

        private void RemoveInstances(int newAmount)
        {
            var deletionList = new List<GameObject>();
            foreach (Transform child in transform) {
                deletionList.Add(child.gameObject);
                if (--newAmount <= 0) break;
            }

            deletionList.ForEach(GameObject.DestroyImmediate);
        }

        private void AddInstances(int newAmount)
        {
            for(int i = 0; i < newAmount; i++) {
                Instance();
            }
        }
        #endregion
    }
}