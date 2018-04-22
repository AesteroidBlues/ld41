using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[Tiled2Unity.CustomTiledImporter]
public class MyCustomImporter : Tiled2Unity.ICustomTiledImporter
{

    public void HandleCustomProperties(UnityEngine.GameObject gameObject, IDictionary<string, string> props)
    {
        CustomProperties gameObjectProps = gameObject.AddComponent<CustomProperties>();
        if (props.ContainsKey("isVertical"))
        {
            gameObjectProps.IsVertical = bool.Parse(props["isVertical"]);
            gameObjectProps.IsLaser = true;
        }

        if (props.ContainsKey("isOpen"))
        {
            gameObjectProps.IsOpen = bool.Parse(props["isOpen"]);
            gameObjectProps.IsLaser = true;
        }

        if (props.ContainsKey("onDeathToggle"))
        {
            gameObjectProps.OnDeathToggle = int.Parse(props["onDeathToggle"]);
        }

        if (props.ContainsKey("onDeathToggleAll"))
        {
            gameObjectProps.OnDeathToggleAll = bool.Parse(props["onDeathToggleAll"]);
        }

        if (props.ContainsKey("behavior"))
        {
            gameObjectProps.PatrolBehavior = props["behavior"];
        }
    }

    public void CustomizePrefab(UnityEngine.GameObject prefab)
    {
        // Do nothing (at least yet)
    }
}