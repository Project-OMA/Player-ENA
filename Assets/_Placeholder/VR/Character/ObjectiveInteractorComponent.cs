using ENA.Goals;
using ENA.Physics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ENA.VR
{
    public partial class ObjectiveInteractorComponent: MonoBehaviour
    {
        #region Variables
        [SerializeField] CollisionTracker tracker;
        #endregion
        #region Properties
        #endregion
        #region Static Properties
        #endregion
        #region Events
        #endregion
        #region MonoBehaviour Lifecycle
        #endregion
        #region Methods
        public void SelectProp(SelectEnterEventArgs args)
        {
            tracker.HandleCollision(args.interactableObject.transform.gameObject);
        }

        public void ReturnProp(SelectExitEventArgs args)
        {
            if (!args.interactableObject.transform.TryGetComponent(out CollidableProp prop)) return;

            Debug.Log("Returned prop!");
        }

        public void InteractWithProp(ActivateEventArgs args)
        {
            if (!args.interactableObject.transform.TryGetComponent(out ObjectiveComponent objective)) return;

            Debug.Log($"Interacted with {objective.name}");
        }

        public void DeactivateProp(DeactivateEventArgs args)
        {
            if (!args.interactableObject.transform.TryGetComponent(out ObjectiveComponent objective)) return;

            Debug.Log($"Deactivated {objective.name}");
        }
        #endregion
    }
}