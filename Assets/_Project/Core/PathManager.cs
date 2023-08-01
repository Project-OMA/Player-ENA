using UnityEngine;

namespace ENA
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
            NewPath(attach, out _);
        }

        public void NewPath(GameObject attach, out TrailRenderer trailRenderer)
        {
            currentTrail?.transform.SetParent(null);

            Transform attachTransform = attach.transform;
            currentTrail = Instantiate(trailModels[counter], attachTransform);
            currentTrail.transform.SetPositionAndRotation(attachTransform.position, Quaternion.identity);
            currentTrail.Clear();
            currentTrail.gameObject.SetActive(true);

            counter = (counter + 1) % trailModels.Length;

            trailRenderer = currentTrail;
        }
        #endregion
    }
}