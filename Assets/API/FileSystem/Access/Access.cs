using System;
using LitJson;
using WeChatWASM;

public class Access : Details
{
    private static WXFileSystemManager _fileSystemManager;
    
    // 路径
    // 注意WX.env.USER_DATA_PATH后接字符串需要以/开头
    private static readonly string PathPrefix = WX.env.USER_DATA_PATH + "/Access";
    
    // 回调函数
    private static Action<WXTextResponse> onSuccess = (res) =>
    {
        WX.ShowModal(new ShowModalOption()
        {
            content = "Access Success, Result: " + JsonMapper.ToJson(res)
        });
    };
    private static Action<WXTextResponse> onFail = (res) =>
    {
        WX.ShowModal(new ShowModalOption()
        {
            content = "Access Fail, Result: " + JsonMapper.ToJson(res)
        });
    };
    
    private void Start()
    {
        _fileSystemManager = WX.GetFileSystemManager();

        if (_fileSystemManager.AccessSync(PathPrefix + "/exist") != "access:ok")
        {
            _fileSystemManager.MkdirSync(PathPrefix + "/exist", true);
        }
            
        _fileSystemManager.OpenSync(new OpenSyncOption()
        {
            filePath = PathPrefix + "/exist/exist.txt",
            flag = "w+"
        });
        _fileSystemManager.WriteFileSync(PathPrefix + "/exist/exist.txt", "String Data");
    }
    
    protected override void TestAPI(params string[] args)
    {
        if (args[0] == null) args[0] = "同步执行";
        if (args[1] == null) args[1] = "/exist";
        
        if (args[0] == "同步执行")
        {
            RunSync(args[1]);
        }
        else
        {
            RunAsync(args[1]);
        }
    }
    
    private void RunAsync(string path)
    {   
        _fileSystemManager.Access(new AccessParam()
        {
            path = PathPrefix + path,
            success = onSuccess,
            fail = onFail
        });
    }
    
    private void RunSync(string path)
    {
        WX.ShowModal(new ShowModalOption()
        {
            content = "AccessSync Result: " + _fileSystemManager.AccessSync(PathPrefix + path)
        });
    }
}
