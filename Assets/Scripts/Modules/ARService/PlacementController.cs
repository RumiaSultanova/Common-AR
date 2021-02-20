using System;
using Modules.ARService.Model;
using Modules.Core;
using Modules.InputService;
using UnityEngine;
using UniRx;

namespace Modules.ARService
{
    public class PlacementController
    {
        private GameObject _placeableGO;

        public PlacementController(GameObject placeableGO, SceneContainer container)
        {
            _placeableGO = UnityEngine.Object.Instantiate(placeableGO);
            _placeableGO.SetActive(false);

            container.GetService<CustomInput>().PhysicsEnterSubject.Subscribe(PlaceObject);
            container.GetService<ARSessionManager>().ModeChangedSubject.Subscribe(ModeChanged);
        }

        private void ModeChanged(ARMode mode)
        {
            switch (mode)
            {
                case ARMode.Plane:
                    _placeableGO.SetActive(false);
                    break;
                case ARMode.Marker:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        private void PlaceObject(Vector3 worldPoint)
        {
            _placeableGO.SetActive(true);
            _placeableGO.transform.position = worldPoint;
        }
    }
}