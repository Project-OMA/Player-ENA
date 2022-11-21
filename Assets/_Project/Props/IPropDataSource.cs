using UnityEngine;

namespace ENA.Props
{
    public interface IPropDataSource
    {
        #region Methods
        bool FetchProp(string inputCode, out Prop.Spawn preset);
        #endregion
    }
}