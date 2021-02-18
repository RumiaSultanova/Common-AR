using System;
using Modules.ARService.Model;
using Modules.Core;
using Modules.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Modules.ARService
{
    public class ARModeSwitcher : IInject
    {
        private ARSessionOrigin _arSessionOrigin;

        private ARPlaneManager _planeManager;
        private ARTrackedImageManager _imageManager;
        
        private ARMode _mode;
        private const int ModesCount = 2;

        public void Inject(SceneContainer container)
        {
            _arSessionOrigin = container.ARSessionOrigin;
            
            _planeManager = _arSessionOrigin.gameObject.AddComponent<ARPlaneManager>();
            _planeManager.enabled = false;
            Addressables.LoadAssetAsync<GameObject>(AssetNames.ARPlane).Completed += handle => _planeManager.planePrefab = handle.Result;

            _imageManager = _arSessionOrigin.gameObject.AddComponent<ARTrackedImageManager>();
            _imageManager.enabled = false;
            Addressables.LoadAssetAsync<XRReferenceImageLibrary>(AssetNames.ReferenceImageLibrary).Completed +=
                handle => _imageManager.referenceLibrary = handle.Result;
            Addressables.LoadAssetAsync<GameObject>(AssetNames.ShovedReactionWithSpin).Completed +=
                handle => _imageManager.trackedImagePrefab = handle.Result;
        }
        
        public void SwitchMode()
        {
            SetActive((int) _mode, false);
            
            _mode++;
            if ((int) _mode >= ModesCount)
            {
                _mode -= ModesCount;
            }

            SetActive((int) _mode, true);
        }

        private void SetActive(int id, bool state)
        {
            switch ((ARMode) id)
            {
                case ARMode.Marker:
                    _planeManager.enabled = state;
                    break;
                case ARMode.Plane:
                    _imageManager.enabled = state;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
