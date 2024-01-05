using LitJson;
using WeChatWASM;

public class Rename : Details
{
    private WXFileSystemManager _fileSystemManager;
    
    // 路径
    // 注意WX.env.USER_DATA_PATH后接字符串需要以/开头
    private static readonly string PathPrefix = WX.env.USER_DATA_PATH + "/Rename";
    private static readonly string Path1 = PathPrefix + "/hello.txt";
    private static readonly string Path2 = PathPrefix + "/world.txt";
    private static readonly string Path3 = PathPrefix + "/dir/hello.txt";

    // 当前路径
    private string _oldPath = "/hello.txt";
    
    private void Start()
    {
        // 获取全局唯一的文件管理器
        _fileSystemManager = WX.GetFileSystemManager();

        if (_fileSystemManager.AccessSync(PathPrefix + "/dir") != "access:ok")
        {
            _fileSystemManager.MkdirSync(PathPrefix + "/dir", true);
        }
            
        var fd = _fileSystemManager.OpenSync(new OpenSyncOption()
        {
            filePath = Path1,
            flag = "w+"
        });
        _fileSystemManager.WriteSync(new WriteSyncStringOption()
        {
            fd = fd,
            data = "Original Data "
        });
    }
    
    protected override void TestAPI(string[] args)
    {
        if (args[0] == "同步执行")
        {
            RunSync(args[1]);
        }
        else
        {
            RunAsync(args[1]);
        }
    }
    
    private void RunAsync(string newPath)
    {   
       _fileSystemManager.Rename(new RenameOption()
       {
           newPath = PathPrefix + newPath,
           oldPath = PathPrefix + _oldPath,
           success = (res) =>
           {
               _oldPath = newPath;
               UpdateResults();
               WX.ShowModal(new ShowModalOption()
               {
                   content = "Rename Success, Result: " + JsonMapper.ToJson(res)
               });
           },
           fail = (res) =>
           {
               WX.ShowModal(new ShowModalOption()
               {
                   content = "Rename Fail, Result: " + JsonMapper.ToJson(res)
               });
           }
       });
    }
    
    private void RunSync(string newPath)
    {
        _fileSystemManager.RenameSync(PathPrefix + _oldPath, PathPrefix + newPath);

        _oldPath = newPath;
        UpdateResults();
        WX.ShowToast(new ShowToastOption()
        {
            title = "Rename Success"
        });
    }

    private void UpdateResults()
    {
        GameManager.Instance.detailsController.SetResultActive(0, _fileSystemManager.AccessSync(Path1) == "access:ok");
        GameManager.Instance.detailsController.SetResultActive(1, _fileSystemManager.AccessSync(Path2) == "access:ok");
        GameManager.Instance.detailsController.SetResultActive(2, _fileSystemManager.AccessSync(Path3) == "access:ok");
    }
}
