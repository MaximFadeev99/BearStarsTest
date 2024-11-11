using BearStarsTest.Data;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace BearStarsTest.UI.MainView
{
    public interface IViewPanel
    {
        public GameObject GameObject { get; }
        public void Initialize(IReadOnlyList<WindowData> windowData, IObjectResolver objectResolver);
        public void Dispose();
    }
}