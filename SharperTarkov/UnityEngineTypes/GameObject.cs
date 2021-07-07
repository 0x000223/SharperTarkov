using System;
using System.Collections.Generic;
using SharpMemory.Ioctl;

namespace SharperTarkov.UnityEngineTypes
{
    public class GameObject : Object
    {
        public GameObject() { }

        public GameObject(ulong address) : base(address)
        {
            Name = MemoryHelper.ReadNarrowString(Address + Offsets.GameObject.Name);

            Tag = Memory.Read<short>(Address + Offsets.GameObject.Tag);
        }

        public string Name { get; }

        public short Tag { get; }

        public Component GetComponentByName(string componentName)
        {
            var componentArray = Memory.Read<ulong>(Address + Offsets.GameObject.ComponentArray);

            var arrayCount = Memory.Read<int>(Address + Offsets.GameObject.ComponentCount);

            for (uint index = 0; index < arrayCount; ++index)
            {
                var temp = Memory.Read<ulong>(componentArray + 0x10 * index + 8);

                var retComponent = new Component(temp);

                if (retComponent.Name.Contains(componentName))
                {
                    return retComponent;
                }
            }

            return default;
        }

        public List<Component> GetComponents()
        {
            var componentArray = Memory.Read<ulong>(Address + Offsets.GameObject.ComponentArray);

            var arrayCount = Memory.Read<int>(Address + Offsets.GameObject.ComponentCount);

            var components = new List<Component>();

            for (uint index = 0; index < arrayCount; ++index)
            {
                var component = new Component(Memory.Read<ulong>(componentArray + 0x10 * index + 0x8));

                components.Add(component);
            }

            return components;
        }
        
        public static List<GameObject> GetActiveObjects(int limit = 0)
        {
            if (MemoryContext.ModuleAddress == 0)
            {
                return new List<GameObject>();
            }

            var objectManager = Memory.Read<ulong>(MemoryContext.ModuleAddress + Offsets.Global.GameObjectManager);

            var ret = new List<GameObject>();

            var activeObject = Memory.Read<ulong>(objectManager + Offsets.GameObjectManager.ActiveObjects);

            var lastActiveObject = objectManager + Offsets.GameObjectManager.LastActiveObject;

            while (true)
            {
                var objectAddress = Memory.Read<ulong>(activeObject + 0x10);

                ret.Add(new GameObject(objectAddress));

                activeObject = Memory.Read<ulong>(activeObject + 0x8);

                if (activeObject == lastActiveObject || ret.Count == limit)
                {
                    break;
                }
            }
            
            return ret;
        }

        public static List<GameObject> GetTaggedObjects(int limit = 0)
        {
            if (MemoryContext.ModuleAddress == 0)
            {
                return new List<GameObject>();
            }

            var objectManager = Memory.Read<ulong>(MemoryContext.ModuleAddress + Offsets.Global.GameObjectManager);

            var ret = new List<GameObject>();

            var taggedObject = Memory.Read<ulong>(objectManager + Offsets.GameObjectManager.TaggedObjects);

            while (taggedObject != objectManager)
            {
                var objectAddress = Memory.Read<ulong>(taggedObject + 0x10);

                var tempObject = new GameObject(objectAddress);

                ret.Add(tempObject);

                taggedObject = Memory.Read<ulong>(taggedObject + Offsets.GameObjectManager.NextObject);

                if (limit != 0 && ret.Count == limit)
                {
                    return ret;
                }
            }

            return ret;
        }

        public static GameObject GetActiveObjectByName(string name)
        {
            var moduleAddress = MemoryContext.ModuleAddress;

            var objectManager = Memory.Read<ulong>(moduleAddress + Offsets.Global.GameObjectManager);

            if (objectManager == 0)
            {
                return new GameObject();
            }

            var activeObject = Memory.Read<ulong>(objectManager + Offsets.GameObjectManager.ActiveObjects);

            var lastActiveObject = objectManager + Offsets.GameObjectManager.LastActiveObject;

            do
            {
                var objectAddress = Memory.Read<ulong>(activeObject + 0x10);

                var currentObject = new GameObject(objectAddress);

                if (currentObject.Name.Contains(name))
                {
                    return currentObject;
                }

                activeObject = Memory.Read<ulong>(activeObject + 0x8);

            } while (activeObject != lastActiveObject);

            return default;
        }
    }
}
