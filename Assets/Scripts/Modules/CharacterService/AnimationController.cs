using Modules.Core;
using Modules.InputService;
using UniRx;
using UnityEngine;

namespace Modules.CharacterService
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private Animation _animation;

        private CompositeDisposable _disposables;
        
        private void Awake()
        {
            var input = SceneContainer.Instance.GetService<CustomInput>();           
            input.TouchEnterSubject.ObserveOnMainThread().Subscribe(ScreenTouched);
        }

        private void ScreenTouched(Vector2 touch)
        {
            if (_animation.isPlaying)
            {
                _animation.Stop();
            }
            
            _animation.Play();

            Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    if (_animation.isPlaying) { return; }

                    _animation.Play();

                    Observable
                        .NextFrame()
                        .Subscribe(_ =>
                        {
                            _animation.Stop();
                            _disposables?.Dispose();
                        })
                        .AddTo(_disposables);
                })
                .AddTo(_disposables);
        }
    }
}
