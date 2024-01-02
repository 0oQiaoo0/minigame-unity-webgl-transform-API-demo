using LitJson;
using WeChatWASM;

public class CopyFile : Details
{
    private WXFileSystemManager _fileSystemManager;
    
    // 路径
    // 注意WX.env.USER_DATA_PATH后接字符串需要以/开头
    private static readonly string PathPrefix = WX.env.USER_DATA_PATH + "/AppendFile";
    private static readonly string Path = PathPrefix + "/hello.txt";
    private static readonly string SyncPath = PathPrefix + "/copyFileSync.txt";
    private static readonly string AsyncPath = PathPrefix + "/copyFileAsync.txt";
    
    private void Start()
    {
        // 获取全局唯一的文件管理器
        _fileSystemManager = WX.GetFileSystemManager();
            
        if (_fileSystemManager.AccessSync(PathPrefix) != "access:ok")
        {
            _fileSystemManager.MkdirSync(PathPrefix, true);
        }
        
        var fd = _fileSystemManager.OpenSync(new OpenSyncOption()
        {
            filePath = Path,
            flag = "w+"
        });
        
        _fileSystemManager.WriteSync(new WriteSyncStringOption()
        {
            fd = fd,
            data = "Original Data "
        });
        
        if (_fileSystemManager.AccessSync(SyncPath) == "access:ok")
        {
            _fileSystemManager.UnlinkSync(SyncPath);
        }
        if (_fileSystemManager.AccessSync(AsyncPath) == "access:ok")
        {
            _fileSystemManager.UnlinkSync(AsyncPath);
        }

        GameManager.Instance.detailsController.BindExtraButtonAction(0, ClearCopyFile);
    }
    
    protected override void TestAPI(string[] args)
    {
        if (args[0] == null) args[0] = "同步执行";
        
        if (args[0] == "同步执行")
        {
            RunSync();
        }
        else
        {
            RunAsync();
        }
    }
    
    private void RunAsync()
    {
        _fileSystemManager.CopyFile(new CopyFileParam()
        {
            srcPath = Path,
            destPath = AsyncPath,
            success = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "CopeFile Success, Result: " + JsonMapper.ToJson(res)
                                                           + "\nCopied File Content: " + _fileSystemManager.ReadFileSync(AsyncPath, "utf8")
                });
                GameManager.Instance.detailsController.EnableResult(2);
            },
            fail = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "CopyFile Fail, Result: " + JsonMapper.ToJson(res)
                });
            }
        });
    }

    private void RunSync()
    {
        WX.ShowModal(new ShowModalOption()
        {
            content = "CopyFileSync Result: "  + _fileSystemManager.CopyFileSync(Path, SyncPath)
            + "\nCopied File Content: " + _fileSystemManager.ReadFileSync(Path, "utf8")
        });
        GameManager.Instance.detailsController.EnableResult(1);
    }
    
    private void ClearCopyFile()
    {
        if (_fileSystemManager.AccessSync(SyncPath) == "access:ok")
        {
            _fileSystemManager.UnlinkSync(SyncPath);
        }
        if (_fileSystemManager.AccessSync(AsyncPath) == "access:ok")
        {
            _fileSystemManager.UnlinkSync(AsyncPath);
        }
        
        GameManager.Instance.detailsController.DisableResult(1);
        GameManager.Instance.detailsController.DisableResult(2);
        
        WX.ShowToast(new ShowToastOption()
        {
            title = "已清除复制文件"
        });
    }
}