using System;
using LitJson;
using WeChatWASM;

public class OpenAndClose : Details
{
    private WXFileSystemManager _fileSystemManager;
    
    // 路径
    // 注意WX.env.USER_DATA_PATH后接字符串需要以/开头
    private static readonly string PathPrefix = WX.env.USER_DATA_PATH + "/OpenAndClose";
    private static readonly string Path = PathPrefix + "/hello.txt";
    
    // 文件描述符
    private static string _fd;
    
    // 回调函数
    private Action<OpenSuccessCallbackResult> onOpenSuccess = (res) =>
    {
        _fd = res.fd;
        WX.ShowModal(new ShowModalOption()
        {
            content = "Open Success, Result: " + JsonMapper.ToJson(res)
        });
    };
    private Action<FileError> onOpenFail = (res) =>
    {
        WX.ShowModal(new ShowModalOption()
        {
            content = "Open Fail, Result: " + JsonMapper.ToJson(res)
        });
    };
    private Action<FileError> onCloseSuccess = (res) =>
    {
        WX.ShowModal(new ShowModalOption()
        {
            content = "Close Success, Result: " + JsonMapper.ToJson(res)
        });
    };
    private Action<FileError> onCloseFail = (res) =>
    {
        WX.ShowModal(new ShowModalOption()
        {
            content = "Close Fail, Result: " + JsonMapper.ToJson(res)
        });
    };
    
    private bool _isOpened = false;
    
    private void Start()
    {
        // 获取全局唯一的文件管理器
        _fileSystemManager = WX.GetFileSystemManager();
            
        if (_fileSystemManager.AccessSync(PathPrefix) != "access:ok")
        {
            _fileSystemManager.MkdirSync(PathPrefix, true);
        }

        if (_fileSystemManager.AccessSync(Path) == "access:ok")
        {
            _fileSystemManager.UnlinkSync(Path);
        }
        
        GameManager.Instance.detailsController.ChangeButtonText("打开文件");
    }
    
    protected override void TestAPI(string[] args)
    {
        if (args[0] == null) args[0] = "同步执行";

        if (_isOpened)
        {
            Close(args[0]);
            
            GameManager.Instance.detailsController.ChangeButtonText("打开文件");
            GameManager.Instance.detailsController.DisableResult(0);
        }
        else
        {
            Open(args[0]);
            
            GameManager.Instance.detailsController.ChangeButtonText("关闭文件");
            GameManager.Instance.detailsController.EnableResult(0);
        }
        _isOpened = !_isOpened;
    }
    
    private void Open(string mode)
    {
        if (mode == "同步执行")
        {
            OpenSync();
        }
        else
        {
            OpenAsync();
        }
    }

    private void OpenSync()
    {
        _fd = _fileSystemManager.OpenSync(new OpenSyncOption()
        {
            filePath = Path,
            flag = "w+"
        });
        WX.ShowModal(new ShowModalOption()
        {
            content = "OpenSync Success, fd: " + _fd
        });
    }
    
    private void OpenAsync()
    {
        _fileSystemManager.Open(new OpenOption()
        {
            filePath = Path,
            flag = "w+",
            success = onOpenSuccess,
            fail = onOpenFail
        });
    }
    
    private void Close(string mode)
    {
        if (mode == "同步执行")
        {
            CloseSync();
        }
        else
        {
            CloseAsync();
        }
    }
    
    private void CloseSync()
    {
        _fileSystemManager.CloseSync(new CloseSyncOption()
        {
            fd = _fd
        });
        
        WX.ShowToast(new ShowToastOption()
        {
            title = "CloseSync Success"
        });
    }
    
    private void CloseAsync()
    {
        _fileSystemManager.Close(new FileSystemManagerCloseOption()
        {
            fd = _fd,
            success = onCloseSuccess,
            fail = onCloseFail
        });
    }
}