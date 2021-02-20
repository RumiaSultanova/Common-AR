using Modules.Core;
using Modules.InputService;
using UniRx;
using UnityEngine;

namespace Modules.CharacterService
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private string touchState = "Shoved Reaction With Spin";
        private bool IsToushState => _animator.GetCurrentAnimatorStateInfo(0).IsName(touchState);
        private static readonly int Play = Animator.StringToHash("Play");
        private static readonly int ForceStop = Animator.StringToHash("ForceStop");

        private void Awake()
        {
            var input = SceneContainer.Instance.GetService<CustomInput>();           
            input.TouchEnterSubject.Subscribe(ScreenTouched);
        }

        private void ScreenTouched(Vector2 touch)
        {
            if (IsToushState)
            {
                _animator.SetTrigger(ForceStop);
            }
            
            _animator.SetTrigger(Play);
        }
    }
}
