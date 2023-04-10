using UnityEngine;

namespace ENA.Utilities
{
    public class PathManager: MonoBehaviour
    {
        #region Variables
        [SerializeField] TrailRenderer currentTrail;
        [Header("Trails")]
        [SerializeField] TrailRenderer[] trailModels;
        int counter;
        #endregion
        #region Methods
        public void NewPath(GameObject attach)
        {
            NewPath(attach, out var renderer);
        }

        public void NewPath(GameObject attach, out TrailRenderer trailRenderer)
        {
            if (currentTrail != null) {
                currentTrail.transform.parent = null;
            }

            Transform attachTransform = attach.transform;
            currentTrail = Instantiate(trailModels[counter], attachTransform);
            currentTrail.transform.SetPositionAndRotation(attachTransform.position, Quaternion.identity);
            currentTrail.Clear();
            currentTrail.gameObject.SetActive(true);

            counter = (counter + 1) % 12;

            trailRenderer = currentTrail;
        }
        #endregion
    }
}