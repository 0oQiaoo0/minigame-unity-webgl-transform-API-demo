using WeChatWASM;

public class Access : Details
{
    private WXFileSystemManager _fileSystemManager;
    
    private string _pathPrefix = WX.env.USER_DATA_PATH + "/Access";

    public bool isSync;
    public string path;

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
    
    public void SetSync(int index)
    {
        isSync = index != 0;
    }
    
    public void SetPath(int index)
    {
        path = entrySO.optionList[1].availableOptions[index];
    }

    public override void Run()
    {
        if (isSync)
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
        _fileSystemManager.Access(new AccessParam()
        {
            path = _pathPrefix + path,
            success = (res) =>
            {
                WX.ShowToast(new ShowToastOption()
                {
                    title = "Access Success: " + res
                });
            },
            fail = (res) =>
            {
                WX.ShowToast(new ShowToastOption()
                {
                    title = "Access Fail: " + res
                });
            }
        });
    }

    private void RunSync()
    {
        WX.ShowToast(new ShowToastOption()
        {
            title = _fileSystemManager.AccessSync(_pathPrefix + path)
        });
    }
}
