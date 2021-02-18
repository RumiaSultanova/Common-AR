using System;
using Modules.Core;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Modules.InputService
{
    public class CustomInput : MonoBehaviour, IInject
    {
        public readonly Subject<Vector2> TouchEnterSubject = new Subject<Vector2>();

        public void Inject(SceneContainer container) { }
        
        void Update()
        {
            CheckTouch();
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
    }
}
