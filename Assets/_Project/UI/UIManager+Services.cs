using System;
using System.Collections.Generic;
using ENA.Services;
using UnityEngine;

namespace ENA.UI
{
    public partial class UIManager
    {
        #region Variables
        Dictionary<Type, IService> services = new Dictionary<Type, IService>();
        #endregion
        #region Methods
        public T Get<T>() where T: class, IService, new()
        {
            var type = typeof(T);
            if (services.TryGetValue(type, out IService service)) {
                return service as T;
            } else {
                T newInstance = new();
                services[type] = newInstance;
                return newInstance;
            }
        }

        public void Replace<T>(T service) where T: class, IService, new() => services[typeof(T)] = service;
        #endregion
    }
}