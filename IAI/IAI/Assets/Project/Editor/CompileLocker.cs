using UnityEngine;
using UnityEditor;

// https://raspberly.hateblo.jp/entry/InvalidateUnityCompile
// https://gist.github.com/decoc/bde047ac7ad8c9bfce7eb408f2712424
public static class CompileLocker
{
    private const string MenuItemName = "Editor Extensions/CompileLocker/Lock";

    [MenuItem(MenuItemName, false, 1)]
    static void Lock()
    {
        bool isLocked = Menu.GetChecked(MenuItemName);
        if (isLocked)
        {
            Debug.Log("Compile Unlocked");
            EditorApplication.UnlockReloadAssemblies();
            Menu.SetChecked(MenuItemName, false);
        }
        else
        {
            Debug.Log("Compile Locked");
            EditorApplication.LockReloadAssemblies();
            Menu.SetChecked(MenuItemName, true);
        }
    }
}