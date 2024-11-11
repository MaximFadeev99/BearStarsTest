using BearStarsTest.Systems;
using BearStarsTest.UI.MainView;
using BearStarsTest.UI.MainView;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BearStarsTest.DI
{
    internal class GameSceneScope : LifetimeScope
    {
        [SerializeField] private Transform _canvasTransform;
        [SerializeField] private BasicViewPanel _viewPanelPrefab;
        [SerializeField] private UIInitializer _uiInitializerSO;
        [SerializeField] private ButtonHandlingSystem _buttonHandlingSystemSO;
        [SerializeField] private WindowHandlingSystem _windowHandlingSystemSO;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();
            builder.RegisterInstance(_canvasTransform);
            builder.RegisterComponent<IViewPanel>(_viewPanelPrefab);

            RegisterTypesForAutoInjection(builder);
        }

        private void RegisterTypesForAutoInjection(IContainerBuilder builder) 
        {
            builder.RegisterComponent(_uiInitializerSO);
            builder.RegisterComponent(_buttonHandlingSystemSO);
            builder.RegisterComponent(_windowHandlingSystemSO);
        }
    }
}