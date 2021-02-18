using System;
using System.Collections.Generic;
using Modules.ARService;
using Modules.InputService;
using Modules.UIService;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Modules.Core
{
    public class SceneContainer : MonoBehaviour
    {
        private static SceneContainer _instance;
        public static SceneContainer Instance => _instance;
        
        public ARSessionOrigin ARSessionOrigin;
        
        private readonly Dictionary<Type, IInject> _services = new Dictionary<Type, IInject>();

        private void Awake()
        {
            _instance = this;
            
            AddService(new ARModeSwitcher());
            AddService(new ARModeSwitcherUIService());
            AddService(gameObject.AddComponent<CustomInput>());
        }

        private void Start()
        {
            foreach (var service in _services)
            {
                service.Value.Inject(this);
            }
        }

        private void AddService(IInject service)
        {
            _services.Add(service.GetType(), service);
        }

        public T GetService<T>() where T : class
        {
            _services.TryGetValue(typeof(T), out var service);
            return service as T;
        }
    }
}