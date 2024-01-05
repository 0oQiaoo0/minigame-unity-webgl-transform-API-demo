using LitJson;
using WeChatWASM;

public class ReaddirAndUnlink : Details
{
    private WXFileSystemManager _fileSystemManager;
    
    // 路径
    // 注意WX.env.USER_DATA_PATH后接字符串需要以/开头
    private static readonly string PathPrefix = WX.env.USER_DATA_PATH + "/ReaddirAndUnlink";
    private static readonly string DirPath = PathPrefix + "/dir";
    private static readonly string Path1 = PathPrefix + "/hello.txt";
    private static readonly string Path2 = PathPrefix + "/world.txt";
    private static readonly string Path3 = PathPrefix + "/dir/abandon.txt";

    // 当前路径
    private string _oldPath = "/hello.txt";
    
    private void Start()
    {
        // 获取全局唯一的文件管理器
        _fileSystemManager = WX.GetFileSystemManager();

        if (_fileSystemManager.AccessSync(DirPath) != "access:ok")
        {
            _fileSystemManager.MkdirSync(DirPath, true);
        }
        
        GameManager.Instance.detailsController.BindExtraButtonAction(0, Unlink);
        GameManager.Instance.detailsController.BindExtraButtonAction(1, ReadDir);
    }
    
    // 创建文件
    protected override void TestAPI(string[] args)
    {
        if (args[0] == "同步执行")
        {
            OpenSync(args[1]);
        }
        else
        {
            OpenAsync(args[1]);
        }
    }
    
    // 删除文件
    private void Unlink()
    {
        if (options[0] == "同步执行")
        {
            UnlinkSync(options[1]);
        }
        else
        {
            UnlinkAsync(options[1]);
        }
    }
    
    // 获取目录下的文件列表
    private void ReadDir()
    {
        if (options[0] == "同步执行")
        {
            ReaddirSync();
        }
        else
        {
            ReaddirAsync();
        }
    }
    
    private void OpenSync(string filePath)
    {
        _fileSystemManager.OpenSync(new OpenSyncOption()
        {
            filePath = PathPrefix + filePath,
            flag = "w+"
        });
        
        WX.ShowToast(new ShowToastOption()
        {
            title = "同步创建文件成功"
        });
    }
    
    private void OpenAsync(string filePath)
    {
        _fileSystemManager.Open(new OpenOption()
        {
            filePath = filePath,
            flag = "w+",
            success = (res) =>
            {
                WX.ShowToast(new ShowToastOption()
                {
                    title = "异步创建文件成功"
                });
            },
            fail = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "Open Fail, Result: " + JsonMapper.ToJson(res)
                });
            }
        });
    }
    
    private void UnlinkSync(string filePath)
    {
        var res = _fileSystemManager.UnlinkSync(PathPrefix + filePath);
        
        WX.ShowModal(new ShowModalOption()
        {
            content = "UnlinkSync Result: " + res
        });
    }
    
    private void UnlinkAsync(string filePath)
    {
        _fileSystemManager.Unlink(new UnlinkParam()
        {
            filePath = filePath,
            success = (res) =>
            {
                WX.ShowToast(new ShowToastOption()
                {
                    title = "Unlink Success, Result: " + JsonMapper.ToJson(res)
                });
            },
            fail = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "Unlink Fail, Result: " + JsonMapper.ToJson(res)
                });
            }
        });
    }
    
    private void ReaddirSync()
    {
        var res = _fileSystemManager.ReaddirSync(PathPrefix);
        
        UpdateResults(res);
        
        WX.ShowToast(new ShowToastOption()
        {
            title = "ReadDirSync Success"
        });
    }

    private void ReaddirAsync()
    {
        _fileSystemManager.Readdir(new ReaddirOption()
        {
            dirPath = PathPrefix,
            success = (res) =>
            {
                UpdateResults(res.files);

                WX.ShowModal(new ShowModalOption()
                {
                    content = "ReadDir Success, Result: " + res.errMsg
                });
            },
            fail = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "ReadDir Fail, Result: " + JsonMapper.ToJson(res)
                });
            }
        });
    }

    private void UpdateResults(string[] fileList)
    {
        GameManager.Instance.detailsController.KeepFirstNResults(1);

        foreach (var file in fileList)
        {
            GameManager.Instance.detailsController.AddResult(new ResultData()
            {
                initialContentText = file
            });
        }
    }
}
