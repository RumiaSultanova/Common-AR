using System;
using System.Collections.Generic;
using Modules.ARService;
using Modules.Core;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

namespace Modules.InputService
{
    public class CustomInput : MonoBehaviour, IInject
    {
        public readonly Subject<Vector2> TouchEnterSubject = new Subject<Vector2>();
        public readonly Subject<Vector3> PhysicsEnterSubject = new Subject<Vector3>();

        private ARSessionManager _arSession;
        
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public void Inject(SceneContainer container)
        {
            _arSession = container.GetService<ARSessionManager>();
        }
        
        private void Awake()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ => CheckTouch())
                .AddTo(_disposables);
        }

        private void CheckTouch()
        {
            if (Input.touchCount <= 0) { return; }

            if (EventSystem.current.IsPointerOverGameObject()) { return; }
            
            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    TouchEnterSubject?.OnNext(touch.position);
                    CheckPhysics(touch.position);
                    break;
                case TouchPhase.Moved:
                    break;
                case TouchPhase.Ended:
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CheckPhysics(Vector2 touch)
        {
            var hits = new List<ARRaycastHit>();
            if (_arSession.RaycastManager.Raycast(touch, hits))
            {
                PhysicsEnterSubject.OnNext(hits[0].pose.position);
            }
        }

        public void Destroy()
        {
            _disposables.Dispose();
        }
    }
}
