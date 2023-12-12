using System.Collections.Generic;
using Unity.Services.Analytics;

public class UI_Scene : UI_Base
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        if (Managers.AnalyticsCollected)
        {
            AnalyticsService.Instance.CustomData(
                "Scene",
                new Dictionary<string, object> { { "SceneName", $"{GetType().Name}" }, }
            );
        }

        Managers.UI.SetCanvas(gameObject, false);
        return true;
    }
}