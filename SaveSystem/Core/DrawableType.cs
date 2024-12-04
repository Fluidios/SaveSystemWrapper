using System;
namespace Game.SaveSystem.Core
{
    [System.Serializable]
    public class DrawableType<T>
    {
        public string TypeName;
        public string TypeAssembly;
        public string ParentTypeName;
        public string ParentTypeAssembly;

        public DrawableType(Type type)
        {
            TypeName = type.FullName;
            ParentTypeName = typeof(T).FullName;
            ParentTypeAssembly = typeof(T).Assembly.FullName;
        }
    }
}
