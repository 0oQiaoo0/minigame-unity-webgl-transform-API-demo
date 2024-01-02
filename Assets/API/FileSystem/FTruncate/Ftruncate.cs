using System;
using LitJson;
using WeChatWASM;

public class Ftruncate : Details
{
    private static WXFileSystemManager _fileSystemManager;
    
    // 路径
    // 注意WX.env.USER_DATA_PATH后接字符串需要以/开头
    private static readonly string PathPrefix = WX.env.USER_DATA_PATH + "/Ftruncate";
    private static readonly string Path = PathPrefix + "/hello.txt";
    
    // 回调函数
    private Action<FileError> onSuccess = (res) =>
    {
        WX.ShowModal(new ShowModalOption()
        {
            content = "Ftruncate Success, Result: " + JsonMapper.ToJson(res)
        });
        
        UpdateResult();
    };
    private Action<FileError> onFail = (res) =>
    {
        WX.ShowModal(new ShowModalOption()
        {
            content = "Ftruncate Fail, Result: " + JsonMapper.ToJson(res)
        });
    };
    
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
        GameManager.Instance.detailsController.extraButtonObjects[0].GetComponent<ButtonController>()
            .AddButtonListener(() =>
            {
                _fileSystemManager.WriteSync(new WriteSyncStringOption()
                {
                    fd = _fd,
                    data = "Original Data "
                });
            });
    }
    
    protected override void TestAPI(string[] args)
    {
        if (args[0] == null) args[0] = "同步执行";
        if (args[1] == null) args[1] = "4";
        
        if (args[0] == "同步执行")
        {
            RunSync(args[1]);
        }
        else
        {
            RunAsync(args[1]);
        }
    }
    
    private void RunAsync(string length)
    {   
        _fileSystemManager.Ftruncate(new FtruncateOption()
        {
            fd = _fd,
            length = int.Parse(length),
            success = onSuccess,
            fail = onFail
        });
    }
    
    private void RunSync(string length)
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

    private static void UpdateResult()
    {
        GameManager.Instance.detailsController.resultObjects[0].GetComponent<ResultController>()
            .ChangeContent(_fileSystemManager.ReadFileSync(Path, "utf8"));
    }
}
