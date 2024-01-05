using System;
using LitJson;
using WeChatWASM;

public class OpenAndClose : Details
{
    private WXFileSystemManager _fileSystemManager;
    
    // 路径
    // 注意WX.env.USER_DATA_PATH后接字符串需要以/开头
    private static readonly string PathPrefix = WX.env.USER_DATA_PATH + "/OpenAndClose";
    private static readonly string Path1 = PathPrefix + "/exist.txt";
    private static readonly string Path2 = PathPrefix + "/notExist.txt";
    
    // 文件描述符
    private string[] _fd;
    
    private void Start()
    {
        // 获取全局唯一的文件管理器
        _fileSystemManager = WX.GetFileSystemManager();
            
        if (_fileSystemManager.AccessSync(PathPrefix) != "access:ok")
        {
            _fileSystemManager.MkdirSync(PathPrefix, true);
        }

        _fd = new string[2];
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
        
        if (_fileSystemManager.AccessSync(Path2) == "access:ok")
        {
            _fileSystemManager.UnlinkSync(Path2);
        }
        
        GameManager.Instance.detailsController.BindExtraButtonAction(0, Close);
        GameManager.Instance.detailsController.BindExtraButtonAction(1, ResetDetails);
    }
    
    // 打开文件
    protected override void TestAPI(string[] args)
    {
        if(args[0] == "同步执行")
        {
            OpenSync(args[1], args[2]);
        }
        else
        {
            OpenAsync(args[1], args[2]);
        }
    }
    
    // 关闭文件
    private void Close()
    {
        if (options[0] == "同步执行")
        {
            CloseSync(options[1]);
        }
        else
        {
            CloseAsync(options[1]);
        }
    }
    
    // 重置
    private void ResetDetails()
    {
        _fd = new string[2];
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
        
        if (_fileSystemManager.AccessSync(Path2) == "access:ok")
        {
            _fileSystemManager.UnlinkSync(Path2);
        }
        
        UpdateResults();
        GameManager.Instance.detailsController.SetResultActive(3, false);
        GameManager.Instance.detailsController.SetResultActive(4, false);
        
        WX.ShowToast(new ShowToastOption()
        {
            title = "已重置"
        });
    }

    private void OpenSync(string filePath, string flag)
    {
        var index = filePath == "/exist.txt" ? 0 : 1;

        try
        {
            _fd[index] = _fileSystemManager.OpenSync(new OpenSyncOption()
            {
                filePath = PathPrefix + filePath,
                flag = flag == "null" ? null : flag
            });
        }
        catch (Exception e)
        {
            WX.ShowModal(new ShowModalOption()
            {
                content = "OpenSync Fail, Exception: " + e.Message
            });
            return;
        }

        GameManager.Instance.detailsController.SetResultActive(index + 3, true);
        
        WX.ShowModal(new ShowModalOption()
        {
            content = "OpenSync Success, fd: " + _fd[index]
        });
    }
    
    private void OpenAsync(string filePath, string flag)
    {
        var index = filePath == "/exist.txt" ? 0 : 1;
        
        _fileSystemManager.Open(new OpenOption()
        {
            filePath = filePath,
            flag = flag == "null" ? null : flag,
            success = (res) =>
            {
                _fd[index] = res.fd;
                GameManager.Instance.detailsController.SetResultActive(index + 3, true);
                WX.ShowModal(new ShowModalOption()
                {
                    content = "Open Success, Result: " + JsonMapper.ToJson(res)
                });
            },
            fail = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "Open Fail, Result: " + JsonMapper.ToJson(res)
                });
            }
        });
    }
    
    private void CloseSync(string filePath)
    {
        var index = filePath == "/exist.txt" ? 0 : 1;
        
        _fileSystemManager.CloseSync(new CloseSyncOption()
        {
            fd = _fd[index]
        });
        
        GameManager.Instance.detailsController.SetResultActive(index + 3, false);
        
        WX.ShowToast(new ShowToastOption()
        {
            title = "CloseSync Success"
        });
    }
    
    private void CloseAsync(string filePath)
    {
        var index = filePath == "/exist.txt" ? 0 : 1;
        
        _fileSystemManager.Close(new FileSystemManagerCloseOption()
        {
            fd = _fd[index],
            success = (res) =>
            {
                GameManager.Instance.detailsController.SetResultActive(index + 3, false);
                WX.ShowModal(new ShowModalOption()
                {
                    content = "Close Success, Result: " + JsonMapper.ToJson(res)
                });
            },
            fail = (res) =>
            {
                WX.ShowModal(new ShowModalOption()
                {
                    content = "Close Fail, Result: " + JsonMapper.ToJson(res)
                });
            }
        });
    }

    private void UpdateResults()
    {
        GameManager.Instance.detailsController.SetResultActive(0, _fileSystemManager.AccessSync(Path1) == "access:ok");
        GameManager.Instance.detailsController.SetResultActive(1, _fileSystemManager.AccessSync(Path1) == "access:ok");
    }
}