using System;
using Modules.ARService.Model;
using Modules.Core;
using Modules.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Modules.ARService
{
    public class ARSessionManager : IInject
    {
        private ARSessionOrigin _arSessionOrigin;

        private ARPlaneManager _planeManager;
        private PlacementController _placementController;
        
        private ARTrackedImageManager _imageManager;

        public ARRaycastManager RaycastManager { get; private set; }

        private ARMode _mode;
        private const int ModesCount = 2;

        public readonly Subject<ARMode> ModeChangedSubject = new Subject<ARMode>();

        public void Inject(SceneContainer container)
        {
            _arSessionOrigin = container.ARSessionOrigin;

            RaycastManager = _arSessionOrigin.gameObject.AddComponent<ARRaycastManager>();
            
            _planeManager = _arSessionOrigin.GetComponent<ARPlaneManager>() ?? _arSessionOrigin.gameObject.AddComponent<ARPlaneManager>();
            _planeManager.enabled = false;
            Addressables.LoadAssetAsync<GameObject>(AssetNames.ARPlane).Completed += handle => _planeManager.planePrefab = handle.Result;

            _imageManager = _arSessionOrigin.GetComponent<ARTrackedImageManager>() ?? _arSessionOrigin.gameObject.AddComponent<ARTrackedImageManager>();
            _imageManager.enabled = false;
            Addressables.LoadAssetAsync<XRReferenceImageLibrary>(AssetNames.ReferenceImageLibrary).Completed +=
                handle => _imageManager.referenceLibrary = handle.Result;
            Addressables.LoadAssetAsync<GameObject>(AssetNames.ShovedReactionWithSpin).Completed +=
                handle =>
                {
                    _imageManager.trackedImagePrefab = handle.Result;
                };
        }

        public ARMode SwitchMode()
        {
            SetActive(_mode, false);
   
            _mode++;
            if ((int) _mode >= ModesCount)
            {
                _mode -= ModesCount;
            }

            SetActive(_mode, true);
            ModeChangedSubject.OnNext(_mode);
            
            return _mode;
        }

        private void SetActive(ARMode id, bool state)
        {
            switch (id)
            {
                case ARMode.Marker:
                    _imageManager.enabled = state;
                    _imageManager.SetTrackablesActive(state);
                    break;
                case ARMode.Plane:
                    _planeManager.enabled = state;
                    _planeManager.SetTrackablesActive(state);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void Destroy() { }
    }
}
