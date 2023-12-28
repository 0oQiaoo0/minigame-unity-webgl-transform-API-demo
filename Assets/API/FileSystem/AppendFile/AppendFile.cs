using System;
using LitJson;
using UnityEngine;
using WeChatWASM;

public class AppendFile : Details
{
    private static WXFileSystemManager _fileSystemManager;
    
    // 路径
    // 注意WX.env.USER_DATA_PATH后接字符串需要以/开头
    private static readonly string PathPrefix = WX.env.USER_DATA_PATH + "/AppendFile";
    private static readonly string Path = PathPrefix + "/hello.txt";
    
    // 数据
    private static string _stringData = "String Data\n";
    private static byte[] _bufferData = {66, 117, 102, 102, 101, 114, 32, 68, 97, 116, 97, 10};
    
    // 回调函数
    private static Action<WXTextResponse> onSuccess = (res) =>
    {
        WX.ShowModal(new ShowModalOption()
        {
            content = "AppendFile Success: " + JsonMapper.ToJson(res) 
                                             + "\nFile Content: " + _fileSystemManager.ReadFileSync(Path, "utf8")
        });
    };
    private static Action<WXTextResponse> onFail = (res) =>
    {
        WX.ShowModal(new ShowModalOption()
        {
            content = "AppendFile Fail: " + JsonMapper.ToJson(res)
        });
    };
    
    private void Start()
    {
        _fileSystemManager = WX.GetFileSystemManager();
            
        if (_fileSystemManager.AccessSync(PathPrefix) != "access:ok")
        {
            _fileSystemManager.MkdirSync(PathPrefix, true);
        }
        
        _fileSystemManager.OpenSync(new OpenSyncOption()
        {
            filePath = Path,
            flag = "w+"
        });
        
        _fileSystemManager.WriteFileSync(Path, "Original Data\n");
    }
    
    protected override void TestAPI(params string[] args)
    {
        if (args[0] == null) args[0] = "同步执行";
        if (args[1] == null) args[1] = "string";
        if (args[2] == null) args[2] = "null";
        
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
        
        WX.ShowModal(new ShowModalOption()
        {
            content = "File Content: " + _fileSystemManager.ReadFileSync(Path, "utf8")
        });
    }
}