using ENA.Maps;
using UnityEngine;

namespace ENA.Props
{
    [CreateAssetMenu(fileName="New Theme", menuName="ENA/Prop Theme")]
    public class PropTheme: ScriptableObject
    {
        #region Variables
        [SerializeField] PropGroup floorObjects;
        [SerializeField] PropGroup wallObjects;
        [SerializeField] PropGroup doorWindowObjects;
        [SerializeField] PropGroup furnitureObjects;
        [SerializeField] PropGroup eletronicsObjects;
        [SerializeField] PropGroup utensilsObjects;
        [SerializeField] PropGroup interactiveElementsObjects;
        #endregion
        #region Methods
        private PropGroup GetGroupFor(MapCategory category)
        {
            switch (category) {
                case MapCategory.Floor:
                case MapCategory.Ceiling:
                    return floorObjects;
                case MapCategory.Wall:
                    return wallObjects;
                case MapCategory.DoorWindow:
                    return doorWindowObjects;               
                case MapCategory.Furniture:
                    return furnitureObjects;
                case MapCategory.Electronics:
                    return eletronicsObjects;
                case MapCategory.Utensils:
                    return utensilsObjects;
                case MapCategory.Interactive:
                    return interactiveElementsObjects;
                default:
                    return default;
            }
        }

        public bool GetPrefab(MapCategory category, string inputCode, out Prop.Spawn spawn)
        {
            var group = GetGroupFor(category);
            spawn = default;

            if (group?.FetchProp(inputCode, out spawn) ?? false) {
                return true;
            } else {
                return false;
            }
        }
        #endregion
    }
}