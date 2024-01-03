using LitJson;
using WeChatWASM;

public class FtruncateAndTruncate : Details
{
    private WXFileSystemManager _fileSystemManager;
    
    // 路径
    // 注意WX.env.USER_DATA_PATH后接字符串需要以/开头
    private static readonly string PathPrefix = WX.env.USER_DATA_PATH + "/FtruncateAndTruncate";
    private static readonly string Path = PathPrefix + "/hello.txt";
    
    // 文件描述符
    private string _fd;
    
    private void Start()
    {
        // 获取全局唯一的文件管理器
        _fileSystemManager = WX.GetFileSystemManager();

        if (_fileSystemManager.AccessSync(PathPrefix) != "access:ok")
        {
            _fileSystemManager.MkdirSync(PathPrefix, true);
        }
            
        _fd = _fileSystemManager.OpenSync(new OpenSyncOption()
        {
            filePath = Path,
            flag = "w+"
        });
        _fileSystemManager.WriteSync(new WriteSyncStringOption()
        {
            fd = _fd,
            data = "Original Data "
        });
        
        // 绑定还原按钮
        GameManager.Instance.detailsController.BindExtraButtonAction(0, ResetFile);
    }
    
    protected override void TestAPI(string[] args)
    {
        if (args[0] == "文件描述符")
        {
            RunFtruncate(args[1], args[2]);
        }
        else
        {
            RunTruncate(args[1], args[2]);
        }
    }

    private void RunFtruncate(string mode, string length)
    {
        if(mode == "同步执行")
        {
            RunFtruncateSync(length);
        }
        else
        {
            RunFtruncateAsync(length);
        }
    }
    
    private void RunFtruncateAsync(string length)
    {   
        _fileSystemManager.Ftruncate(new FtruncateOption()
        {
            fd = _fd,
            length = int.Parse(length),
            success = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "Ftruncate Success, Result: " + JsonMapper.ToJson(res)
                });
        
                UpdateResult();
            },
            fail = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "Ftruncate Fail, Result: " + JsonMapper.ToJson(res)
                });
            }
        });
    }
    
    private void RunFtruncateSync(string length)
    {
        _fileSystemManager.FtruncateSync(new FtruncateSyncOption()
        {
            fd = _fd,
            length = int.Parse(length)
        });
        
        UpdateResult();
        
        WX.ShowToast(new ShowToastOption()
        {
            title = "FtruncateSync Success"
        });
    }
    
    private void RunTruncate(string mode, string length)
    {
        if(mode == "同步执行")
        {
            RunTruncateSync(length);
        }
        else
        {
            RunTruncateAsync(length);
        }
    }
    
    private void RunTruncateAsync(string length)
    {   
        _fileSystemManager.Truncate(new TruncateOption()
        {
            filePath = Path,
            length = int.Parse(length),
            success = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "Truncate Success, Result: " + JsonMapper.ToJson(res)
                });
        
                UpdateResult();
            },
            fail = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "Truncate Fail, Result: " + JsonMapper.ToJson(res)
                });
            }
        });
    }
    
    private void RunTruncateSync(string length)
    {
        _fileSystemManager.TruncateSync(new TruncateSyncOption()
        {
            filePath = Path,
            length = int.Parse(length)
        });
        
        UpdateResult();
        
        WX.ShowToast(new ShowToastOption()
        {
            title = "TruncateSync Success"
        });
    }

    private void UpdateResult()
    {
        GameManager.Instance.detailsController.resultObjects[0].GetComponent<ResultController>()
            .ChangeContent(_fileSystemManager.ReadFileSync(Path, "utf8"));
    }

    private void ResetFile()
    {
        _fd = _fileSystemManager.OpenSync(new OpenSyncOption()
        {
            filePath = Path,
            flag = "w+"
        });
        _fileSystemManager.WriteSync(new WriteSyncStringOption()
        {
            fd = _fd,
            data = "Original Data "
        });
        UpdateResult();
        
        WX.ShowToast(new ShowToastOption()
        {
            title = "已重置文件"
        });
    }
}
