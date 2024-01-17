using UnityEngine;
using WeChatWASM;

public class BannerAd : Details
{
    private WXBannerAd _bannerAd;
    
    private bool _isShow = false;
    
    private void Start()
    {
        GameManager.Instance.detailsController.BindExtraButtonAction(0, SwitchAdState);
        GameManager.Instance.detailsController.BindExtraButtonAction(1, DestroyAd);
    }

    // 创建预设的 Banner 广告组件并挂载事件
    // 如需自定义style请调用WX.CreateBannerAd(WXCreateBannerAdParam param)接口
    protected override void TestAPI(string[] args)
    {
        // adUnitId 请填写自己的广告位 ID
        _bannerAd = WX.CreateFixedBottomMiddleBannerAd("adunit-xxxxxxxxxxxxxxxx", 30, 200);
        
        _bannerAd.OnLoad((res) =>
        {
            WX.ShowModal(new ShowModalOption()
            {
                content = "BannerAd OnLoad Result:" + JsonUtility.ToJson(res)
            });
        });
        _bannerAd.OnError((res) =>
        {
            WX.ShowModal(new ShowModalOption()
            {
                content = "BannerAd onError Result:" + JsonUtility.ToJson(res)
            });
        });
        _bannerAd.OnResize((res) =>
        {
            WX.ShowModal(new ShowModalOption()
            {
                content = "BannerAd onResize Result:" + JsonUtility.ToJson(res)
            });
        });
    }

    // 切换广告显示状态
    private void SwitchAdState()
    {
        if (_isShow)
        {
            _bannerAd.Hide();
            WX.ShowToast(new ShowToastOption()
            {
                title = "Hide BannerAd Complete"
            });
        }
        else
        {
            _bannerAd.Show();
            WX.ShowToast(new ShowToastOption()
            {
                title = "Show BannerAd Complete"
            });
        }
    }

    private void DestroyAd()
    {
        _bannerAd.Destroy();
        WX.ShowToast(new ShowToastOption()
        {
            title = "Destroy BannerAd Complete"
        });
    }

    private void OnDestroy()
    {
        _bannerAd.Destroy();
    }
}
