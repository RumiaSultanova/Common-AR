using Modules.ARService;
using Modules.Core;
using Modules.Utils;
using UnityEngine.AddressableAssets;

namespace Modules.UIService
{
    public class ARModeSwitcherUIService : IInject
    {
        private ARSessionManager _sessionManager;
        private ARModeSwitcherUI _ui;

        public void Inject(SceneContainer container)
        {
            _sessionManager = container.GetService<ARSessionManager>();
            
            Addressables.InstantiateAsync(AssetNames.ARModeSwitcherUI).Completed +=
                handle =>
                {
                    _ui = handle.Result.GetComponent<ARModeSwitcherUI>();
                    _ui.SwitchMode.onClick.AddListener(SwitchMode);
                };
        }
        
        private void SwitchMode()
        {
            var mode = _sessionManager.SwitchMode();
            _ui.SetLabel(mode.ToString());
        }
    
        public void Destroy() { }
    }
}