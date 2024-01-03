using LitJson;
using WeChatWASM;

public class MkdirAndRmdir : Details
{
    private WXFileSystemManager _fileSystemManager;
    
    // 路径
    // 注意WX.env.USER_DATA_PATH后接字符串需要以/开头
    private static readonly string PathPrefix = WX.env.USER_DATA_PATH + "/MkDirAndRmDir";
    private static readonly string PathA = PathPrefix + "/a";
    private static readonly string PathB = PathPrefix + "/a/b";
    
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

        if (_fileSystemManager.AccessSync(PathA) == "access:ok")
        {
            _fileSystemManager.RmdirSync(PathA, true);
        }

        GameManager.Instance.detailsController.BindExtraButtonAction(0, RmDir);
    }
    
    // 创建目录
    protected override void TestAPI(string[] args)
    {
        if (args[0] == "同步执行")
        {
            MkdirSync(args[1], args[2]);
        }
        else
        {
            MkdirAsync(args[1], args[2]);
        }
    }
    
    // 删除目录
    private void RmDir()
    {
        if (options[0] == "同步执行")
        {
            RmdirSync(options[1], options[2]);
        }
        else
        {
            RmdirAsync(options[1], options[2]);
        }
    }

    private void MkdirSync(string dirPath, string recursive)
    {
        WX.ShowModal(new ShowModalOption()
        {
           content = "MkdirSync Result: " + _fileSystemManager.MkdirSync(PathPrefix + dirPath, recursive == "true")
        });
        UpdateResult();
    }

    private void MkdirAsync(string dirPath, string recursive)
    {
        _fileSystemManager.Mkdir(new MkdirParam()
        {
            dirPath = PathPrefix + dirPath,
            recursive = recursive == "true",
            success = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "MkdirAsync Success, Result: " + JsonMapper.ToJson(res)
                });
                UpdateResult();
            },
            fail = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "MkdirAsync Fail, Result: " + JsonMapper.ToJson(res)
                });
            }
        });
    }
    
    private void RmdirSync(string dirPath, string recursive)
    {
        WX.ShowModal(new ShowModalOption()
        {
            content = "RmdirSync Result: " + _fileSystemManager.RmdirSync(PathPrefix + dirPath, recursive == "true")
        });
        
        UpdateResult();
    }
    
    private void RmdirAsync(string dirPath, string recursive)
    {
        _fileSystemManager.Rmdir(new RmdirParam()
        {
            dirPath = PathPrefix + dirPath,
            recursive = recursive == "true",
            success = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "RmdirAsync Success, Result: " + JsonMapper.ToJson(res)
                });
                UpdateResult();
            },
            fail = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "RmdirAsync Fail, Result: " + JsonMapper.ToJson(res)
                });
            }
        });
    }

    private void UpdateResult()
    {
        GameManager.Instance.detailsController.resultObjects[0]
            .SetActive(_fileSystemManager.AccessSync(PathA) == "access:ok");
        GameManager.Instance.detailsController.resultObjects[1]
            .SetActive(_fileSystemManager.AccessSync(PathB) == "access:ok");
    }
}