using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class SceneNamesGenerator
{
    private const string MenuItemName = "Editor Extensions/SceneNamesGenerator/Generate";

    private const string Indent = "    ";
    private const string GenerateClassName = "SceneNames";
    private const string GeneratePath = "Assets/Project/Scripts/Generate/" + GenerateClassName + ".cs";

    [MenuItem(MenuItemName)]
    public static void Generate()
    {
        if (!CanGenerate())
        {
            return;
        }

        string source = GenerateSoruce();

        string generateDirectoryName = Path.GetDirectoryName(GeneratePath);
        if (!Directory.Exists(generateDirectoryName))
        {
            Directory.CreateDirectory(generateDirectoryName);
        }
        File.WriteAllText(GeneratePath, source, Encoding.UTF8);

        AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

        EditorUtility.DisplayDialog(GeneratePath, "Generate completed.", "OK");
    }

    [MenuItem(MenuItemName, true)]
    public static bool CanGenerate() => !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;

    public static string GenerateSoruce()
    {
        var sceneNameConstants = EditorBuildSettings.scenes.
            Select(s => Path.GetFileNameWithoutExtension(s.path)).
            Select(s => $"public const string {s} = nameof({s});").
            Select(s => Indent + s);

        var source = @$"public static class {GenerateClassName}
{{
{string.Join(Environment.NewLine, sceneNameConstants)}
}}";

        return source;
    }
}