using System.Collections.Generic;
using ENA;
using UnityEngine;
using UnityEngine.UI;

namespace ENA.UI
{
    [RequireComponent(typeof(LayoutGroup))]
    public class UIList: ExtendedMonoBehaviour
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
            if (newAmount > Transform.childCount) {
                AddInstances(newAmount - Transform.childCount);
            } else if (newAmount < Transform.childCount) {
                RemoveInstances(Transform.childCount - newAmount);
            }
        }

        private void RemoveInstances(int newAmount)
        {
            List<GameObject> deletionList = new();
            foreach (Transform child in transform) {
                deletionList.Add(child.gameObject);
                if (--newAmount <= 0) break;
            }

            deletionList.ForEach(DestroyImmediate);
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