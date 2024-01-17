using UnityEngine;
using WeChatWASM;

public class CustomAd : Details
{
    private WXCustomAd _customAd;

    // SDK未支持CustomAd.IsShow()接口，使用自定义变量记录广告显示状态
    private bool _isShow;
    
    private void Start()
    {
        GameManager.Instance.detailsController.BindExtraButtonAction(0, SwitchAdState);
        GameManager.Instance.detailsController.BindExtraButtonAction(1, DestroyAd);
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
    private void SwitchAdState()
    {
        if (_isShow)
        {
            _customAd.Hide();
            WX.ShowToast(new ShowToastOption()
            {
                title = "已隐藏广告"
            });
        }
        else
        {
            _customAd.Show();
            WX.ShowToast(new ShowToastOption()
            {
                title = "已展示广告"
            });
        }
    }

    private void DestroyAd()
    {
        _customAd.Destroy();
        WX.ShowToast(new ShowToastOption()
        {
            title = "已销毁广告"
        });
    }

    private void OnDestroy()
    {
        _customAd.Destroy();
    }
}
