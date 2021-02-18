using Modules.Core;
using Modules.InputService;
using UniRx;
using UnityEngine;

namespace Modules.CharacterService
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private Animation _animation;

        private void Awake()
        {
            var input = SceneContainer.Instance.GetService<CustomInput>();           
            input.TouchEnterSubject.Subscribe(ScreenTouched);
        }

        private void ScreenTouched(Vector2 touch)
        {
            _animation.Play();
        }
    }
}
