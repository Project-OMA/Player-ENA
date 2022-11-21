using UnityEngine;

namespace ENA
{
    public static partial class CharacterControllerExtensions
    {
        public static void MoveTowards(this CharacterController controller, Vector3 target, float maxDistanceDelta)
        {
            var controllerPosition = controller.transform.position;
            var targetPosition = Vector3.MoveTowards(controllerPosition, target, maxDistanceDelta);
            controller.Move(targetPosition - controllerPosition);
        }
    }
}