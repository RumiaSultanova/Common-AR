using System;
using Modules.ARService.Model;
using Modules.Core;
using Modules.InputService;
using Modules.Utils;
using UnityEngine;
using UniRx;
using UnityEngine.AddressableAssets;

namespace Modules.ARService
{
    public class PlacementController : IInject 
    {
        private GameObject _placeableGO;

        private CustomInput _input;
        
        private IDisposable _modeDisposable;
        private IDisposable _inputDisposable;

        public void Inject(SceneContainer container)
        {
            _input = container.GetService<CustomInput>();
            
            _modeDisposable = container.GetService<ARSessionManager>()
                .ModeChangedSubject
                .Subscribe(ModeChanged);
            
            Addressables.InstantiateAsync(AssetNames.ShovedReactionWithSpin).Completed += handle =>
            {
                _placeableGO = handle.Result;
                _placeableGO.SetActive(false);
            };
        }
        
        private void ModeChanged(ARMode mode)
        {
            switch (mode)
            {
                case ARMode.Plane:
                    Subscribe();
                    break;
                default:
                    Unsubscribe();
                    break;
            }
        }

        private void Subscribe()
        {
            _inputDisposable = _input
                .PhysicsEnterSubject
                .Subscribe(PlaceObject);
        }

        private void PlaceObject(Vector3 worldPoint)
        {
            _placeableGO.SetActive(true);
            _placeableGO.transform.position = worldPoint;
        }

        private void Unsubscribe()
        {
            _placeableGO.SetActive(false);

            _inputDisposable?.Dispose();
        }

        public void Destroy()
        {
            _inputDisposable?.Dispose();
            _modeDisposable?.Dispose();
        }
    }
}