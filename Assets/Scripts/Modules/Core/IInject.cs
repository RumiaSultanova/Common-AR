namespace Modules.Core
{
    public interface IInject
    {
        void Inject(SceneContainer container);

        void Destroy();
    }
}