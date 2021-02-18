using Modules.ARService;
using Modules.Core;
using Modules.Utils;
using UnityEngine.AddressableAssets;

namespace Modules.UIService
{
    public class ARModeSwitcherUIService : IInject
    {
        private ARModeSwitcher _modeSwitcher;
        private ARModeSwitcherUI _ui;

        public void Inject(SceneContainer container)
        {
            _modeSwitcher = container.GetService<ARModeSwitcher>();
            
            Addressables.InstantiateAsync(AssetNames.ARModeSwitcherUI).Completed +=
                handle =>
                {
                    _ui = handle.Result.GetComponent<ARModeSwitcherUI>();
                    _ui.SwitchMode.onClick.AddListener(() => _modeSwitcher.SwitchMode());
                };
        }
    }
}