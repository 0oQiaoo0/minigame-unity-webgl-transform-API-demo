#if UNITY_EDITOR
using UnityEditor;
using System.IO;

public class FolderRenameProcessor : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (var movedAsset in movedAssets)
        {
            if (!AssetDatabase.IsValidFolder(movedAsset)) continue; // 跳过非文件夹
            
            var newFolderName = Path.GetFileName(movedAsset);
                
            var filePaths = Directory.GetFiles(movedAsset);

            foreach (var filePath in filePaths)
            {
                var fileExtension = Path.GetExtension(filePath);
                string newFileName;

                switch (fileExtension)
                {
                    case ".cs":
                        newFileName = newFolderName + fileExtension;
                        break;
                    case ".asset":
                        newFileName = newFolderName + "SO" + fileExtension;
                        break;
                    default:
                        continue; // 跳过其他文件类型
                }

                var newFilePath = Path.Combine(movedAsset, newFileName);

                // 使用 AssetDatabase.MoveAsset 重命名文件
                AssetDatabase.MoveAsset(filePath, newFilePath);
            }
        }
    }
}
#endif