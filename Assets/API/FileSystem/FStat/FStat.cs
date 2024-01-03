using LitJson;
using WeChatWASM;

public class Fstat : Details
{
    private WXFileSystemManager _fileSystemManager;
    
    // 路径
    // 注意WX.env.USER_DATA_PATH后接字符串需要以/开头
    private static readonly string PathPrefix = WX.env.USER_DATA_PATH + "/Fstat";
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
    }
    
    protected override void TestAPI(string[] args)
    {
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
        _fileSystemManager.Fstat(new FstatOption()
        {
            fd = _fd,
            success = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "Fstat Success, Result: " + JsonMapper.ToJson(res)
                });
            },
            fail = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "Fstat Fail, Result: " + JsonMapper.ToJson(res)
                });
            }
        });
    }
    
    private void RunSync()
    {
        WX.ShowModal(new ShowModalOption()
        {
            content = "FstatSync Result: " + JsonMapper.ToJson(_fileSystemManager.FstatSync(new FstatSyncOption()
            {
                fd = _fd
            }))
        });
    }
}
