using UnityEngine;

namespace ENA.Maps
{
    public interface IPropDataSource
    {
        #region Methods
        bool FetchProp(string inputCode, out Prop.Spawn preset);
        #endregion
    }
}