using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace BearStarsTest.Systems 
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct PressedButtonComponent : IComponent
    {
        public string WindowName;
    }
}