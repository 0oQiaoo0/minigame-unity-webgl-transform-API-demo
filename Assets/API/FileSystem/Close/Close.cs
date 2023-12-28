using System;
using LitJson;
using WeChatWASM;

public class Close : Details
{
    private static WXFileSystemManager _fileSystemManager;
    
    // 路径
    // 注意WX.env.USER_DATA_PATH后接字符串需要以/开头
    private static readonly string PathPrefix = WX.env.USER_DATA_PATH + "/Close";
    private static readonly string Path = PathPrefix + "/hello.txt";
    
    // 回调函数
    private static Action<FileError> onSuccess = (res) =>
    {
        WX.ShowModal(new ShowModalOption()
        {
            content = "Close Success, Result: " + JsonMapper.ToJson(res)
        });
    };
    private static Action<FileError> onFail = (res) =>
    {
        WX.ShowModal(new ShowModalOption()
        {
            content = "Close Fail, Result: " + JsonMapper.ToJson(res)
        });
    };

    // 文件描述符
    private string _fd;
    
    private void Start()
    {
        _fileSystemManager = WX.GetFileSystemManager();
            
        if (_fileSystemManager.AccessSync(PathPrefix) != "access:ok")
        {
            _fileSystemManager.MkdirSync(PathPrefix, true);
        }
    }
    
    protected override void TestAPI(params string[] args)
    {
        _fd = _fileSystemManager.OpenSync(new OpenSyncOption()
        {
            filePath = Path,
            flag = "w+"
        });
        
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
        _fileSystemManager.Close(new FileSystemManagerCloseOption()
        {
            fd = _fd,
            success = onSuccess,
            fail = onFail
        });
    }

    private void RunSync()
    {
        _fileSystemManager.CloseSync(new CloseSyncOption()
        {
            fd = _fd
        });
        
        WX.ShowModal(new ShowModalOption()
        {
            content = "CloseSync Success"
        });
    }
}