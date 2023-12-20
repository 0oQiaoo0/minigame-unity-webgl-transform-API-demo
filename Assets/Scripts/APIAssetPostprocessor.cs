#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Linq;

public class APIAssetPostprocessor : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string assetPath in importedAssets)
        {
            if (Path.GetExtension(assetPath) == ".asset")
            {
                APISO apiSO = AssetDatabase.LoadAssetAtPath<APISO>(assetPath);
                CategorySO categorySO = AssetDatabase.LoadAssetAtPath<CategorySO>(assetPath);
                EntrySO entrySO = AssetDatabase.LoadAssetAtPath<EntrySO>(assetPath);

                if (apiSO != null)
                {
                    UpdateAPISO(apiSO);
                }
                else if (categorySO != null)
                {
                    UpdateCategorySO(categorySO);
                    UpdateAPISO(GetAPISOFromCategorySOPath(assetPath));
                }
                else if (entrySO != null)
                {
                    UpdateCategorySO(GetCategorySOFromEntrySOPath(assetPath));
                }
            }
        }

        foreach (var assetPath in deletedAssets)
        {
            APISO apiSO = GetAPISOFromCategorySOPath(assetPath);
            CategorySO categorySO = GetCategorySOFromEntrySOPath(assetPath);
            
            if(apiSO) UpdateAPISO(apiSO);
            if(categorySO) UpdateCategorySO(categorySO);
        }
    }
    
    public static void UpdateAPISO(APISO apiSO)
    {
        string assetDirectory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(apiSO));
        string[] categorySO_GUIDs = AssetDatabase.FindAssets("t:CategorySO", new[] { assetDirectory });

        apiSO.categoryList = categorySO_GUIDs.Select(guid => AssetDatabase.LoadAssetAtPath<CategorySO>(AssetDatabase.GUIDToAssetPath(guid))).ToList();
        EditorUtility.SetDirty(apiSO);
    }

    public static void UpdateCategorySO(CategorySO categorySO)
    {
        string assetDirectory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(categorySO));
        string[] entrySO_GUIDs = AssetDatabase.FindAssets("t:EntrySO", new[] { assetDirectory });

        categorySO.entryList = entrySO_GUIDs.Select(guid => AssetDatabase.LoadAssetAtPath<EntrySO>(AssetDatabase.GUIDToAssetPath(guid))).ToList();
        EditorUtility.SetDirty(categorySO);
    }
    
    public static APISO GetAPISOFromCategorySOPath(string path)
    {
        string parentDirectory = Path.GetDirectoryName(Path.GetDirectoryName(path));
        string[] apiSO_GUIDs = AssetDatabase.FindAssets("t:APISO", new[] { parentDirectory });

        if (apiSO_GUIDs.Length > 0)
        {
            string apiSOPath = AssetDatabase.GUIDToAssetPath(apiSO_GUIDs[0]);
            APISO apiSO = AssetDatabase.LoadAssetAtPath<APISO>(apiSOPath);
            return apiSO;
        }

        return null;
    }
    
    public static CategorySO GetCategorySOFromEntrySOPath(string path)
    {
        string parentDirectory = Path.GetDirectoryName(Path.GetDirectoryName(path));
        string[] categorySO_GUIDs = AssetDatabase.FindAssets("t:CategorySO", new[] { parentDirectory });

        if (categorySO_GUIDs.Length > 0)
        {
            string categorySOPath = AssetDatabase.GUIDToAssetPath(categorySO_GUIDs[0]);
            CategorySO categorySO = AssetDatabase.LoadAssetAtPath<CategorySO>(categorySOPath);
            return categorySO;
        }

        return null;
    }
}
#endif