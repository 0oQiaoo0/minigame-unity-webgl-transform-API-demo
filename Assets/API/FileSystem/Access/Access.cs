using WeChatWASM;

public class Access : Details
{
    private WXFileSystemManager _fileSystemManager;
    
    private string _pathPrefix = WX.env.USER_DATA_PATH + "/Access";
    
    private void Start()
    {
        _fileSystemManager = WX.GetFileSystemManager();
        
        _fileSystemManager.MkdirSync(_pathPrefix + "/exist", true);
        _fileSystemManager.OpenSync(new OpenSyncOption()
        {
            filePath = _pathPrefix + "/exist/exist.txt",
            flag = "w+"
        });
        _fileSystemManager.WriteFileSync(_pathPrefix + "/exist/exist.txt", "String Data");
    }
    
    protected override void TestAPI(params string[] args)
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
    
    private void RunAsync(string path)
    {
        _fileSystemManager.Access(new AccessParam()
        {
            path = _pathPrefix + path,
            success = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "Access Success: " + res
                });
            },
            fail = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "Access Fail: " + res
                });
            }
        });
    }
    
    private void RunSync(string path)
    {
        WX.ShowToast(new ShowToastOption()
        {
            title = _fileSystemManager.AccessSync(_pathPrefix + path)
        });
    }
}
