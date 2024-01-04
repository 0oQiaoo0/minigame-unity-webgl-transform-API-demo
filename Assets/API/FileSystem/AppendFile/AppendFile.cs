using System;
using LitJson;
using WeChatWASM;

public class AppendFile : Details
{
    private static WXFileSystemManager _fileSystemManager;
    
    // 路径
    // 注意WX.env.USER_DATA_PATH后接字符串需要以/开头
    private static readonly string PathPrefix = WX.env.USER_DATA_PATH + "/AppendFile";
    private static readonly string Path = PathPrefix + "/hello.txt";
    
    // 数据
    private string _stringData = "String Data ";
    private byte[] _bufferData = {66, 117, 102, 102, 101, 114, 32, 68, 97, 116, 97, 32};
    
    // 回调函数
    private Action<WXTextResponse> onSuccess = (res) =>
    {
        WX.ShowModal(new ShowModalOption()
        {
            content = "AppendFile Success, Result: " + JsonMapper.ToJson(res)
        });
        UpdateResult();
    };
    private Action<WXTextResponse> onFail = (res) =>
    {
        WX.ShowModal(new ShowModalOption()
        {
            content = "AppendFile Fail, Result: " + JsonMapper.ToJson(res)
        });
    };
    
    private void Start()
    {
        // 获取全局唯一的文件管理器
        _fileSystemManager = WX.GetFileSystemManager();
            
        if (_fileSystemManager.AccessSync(PathPrefix) != "access:ok")
        {
            _fileSystemManager.MkdirSync(PathPrefix, true);
        }
        
        var fd = _fileSystemManager.OpenSync(new OpenSyncOption()
        {
            filePath = Path,
            flag = "w+"
        });
        
        _fileSystemManager.WriteSync(new WriteSyncStringOption()
        {
            fd = fd,
            data = "Original Data "
        });

        GameManager.Instance.detailsController.BindExtraButtonAction(0, ResetFile);
    }
    
    protected override void TestAPI(string[] args)
    {
        if (args[0] == "同步执行")
        {
            RunSync(args[1], args[2]);
        }
        else
        {
            RunAsync(args[1], args[2]);
        }
    }
    
    private void RunAsync(string dataType, string encoding)
    {
        if (dataType == "byte[]")
        {
            if (encoding == "null")
            {
                _fileSystemManager.AppendFile(new WriteFileParam()
                {
                    filePath = Path,
                    data = _bufferData,
                    success = onSuccess,
                    fail = onFail
                });
            }
            else
            {
                _fileSystemManager.AppendFile(new WriteFileParam()
                {
                    filePath = Path,
                    data = _bufferData,
                    encoding = encoding,
                    success = onSuccess,
                    fail = onFail
                });
            }
        }
        else
        {
            if (encoding == "null")
            {
                _fileSystemManager.AppendFile(new WriteFileStringParam()
                {
                    filePath = Path,
                    data = _stringData,
                    success = onSuccess,
                    fail = onFail
                });
            }
            else
            {
                _fileSystemManager.AppendFile(new WriteFileStringParam()
                {
                    filePath = Path,
                    data = _stringData,
                    encoding = encoding,
                    success = onSuccess,
                    fail = onFail
                });
            }
        }
    }

    private void RunSync(string dataType, string encoding)
    {
        if (dataType == "byte[]")
        {
            if (encoding == "null")
            {
                _fileSystemManager.AppendFileSync(Path, _bufferData);
            }
            else
            {
                _fileSystemManager.AppendFileSync(Path, _bufferData, encoding);
            }
        }
        else
        {
            if (encoding == "null")
            {
                _fileSystemManager.AppendFileSync(Path, _stringData);
            }
            else
            {
                _fileSystemManager.AppendFileSync(Path, _stringData, encoding);
            }
        }
        
        WX.ShowToast(new ShowToastOption()
        {
            title = "AppendFileSync Success"
        });
        
        UpdateResult();
    }

    private static void UpdateResult()
    {
        GameManager.Instance.detailsController.ChangeResultContent(0, _fileSystemManager.ReadFileSync(Path, "utf8"));
    }

    private void ResetFile()
    {
        var fd = _fileSystemManager.OpenSync(new OpenSyncOption()
        {
            filePath = Path,
            flag = "w+"
        });
        _fileSystemManager.WriteSync(new WriteSyncStringOption()
        {
            fd = fd,
            data = "Original Data "
        });
        UpdateResult();
        
        WX.ShowToast(new ShowToastOption()
        {
            title = "已重置文件"
        });
    }
}