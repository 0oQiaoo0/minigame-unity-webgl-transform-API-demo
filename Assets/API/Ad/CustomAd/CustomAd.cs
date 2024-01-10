using UnityEngine;
using WeChatWASM;

public class CustomAd : Details
{
    private WXCustomAd _customAd;

    // SDK未支持CustomAd.IsShow()接口，使用自定义变量记录广告显示状态
    private bool _isShow;
    
    private void Start()
    {
        GameManager.Instance.detailsController.BindExtraButtonAction(0, SwitchCustomAdState);
        GameManager.Instance.detailsController.BindExtraButtonAction(1, DestroyCustomAd);
    }

    // 创建原生模板广告组件并挂载事件
    protected override void TestAPI(string[] args)
    {
        _customAd = WX.CreateCustomAd(new WXCreateCustomAdParam()
        {
            // adUnitId 请填写自己的广告位 ID
            adUnitId = "adunit-xxxxxxxxxxxxxxxx",
            adIntervals = 30,
            style = {
                left = 0,
                top = 100,
            },
        });
        _customAd.OnLoad((res) =>
        {
            WX.ShowModal(new ShowModalOption()
            {
                content = "CustomAd OnLoad Result:" + JsonUtility.ToJson(res)
            });
        });
        _customAd.OnError((res) =>
        {
            WX.ShowModal(new ShowModalOption()
            {
                content = "CustomAd onError Result:" + JsonUtility.ToJson(res)
            });
        });
        _customAd.OnHide(() =>
        {
            WX.ShowModal(new ShowModalOption()
            {
                content = "CustomAd onHide"
            });
        });
        _customAd.OnClose(() =>
        {
            WX.ShowModal(new ShowModalOption()
            {
                content = "CustomAd onClose"
            });
        });
    }

    // 切换广告显示状态
    private void SwitchCustomAdState()
    {
        if (_isShow)
        {
            _customAd.Hide();
            WX.ShowToast(new ShowToastOption()
            {
                title = "Hide CustomAd Complete"
            });
        }
        else
        {
            _customAd.Show();
            WX.ShowToast(new ShowToastOption()
            {
                title = "Show CustomAd Complete"
            });
        }
    }

    private void DestroyCustomAd()
    {
        _customAd.Destroy();
        WX.ShowToast(new ShowToastOption()
        {
            title = "Destroy CustomAd Complete"
        });
    }

    private void OnDestroy()
    {
        _customAd.Destroy();
    }
}
