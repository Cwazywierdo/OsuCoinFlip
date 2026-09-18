using osu.Game.Configuration;
using osu.Game.Rulesets.Configuration;

namespace osu.Game.Rulesets.CoinFlip
{
    internal class CoinFlipRulesetConfigManager(SettingsStore store, RulesetInfo ruleset, int? variant = null) : RulesetConfigManager<CoinFlipRulesetSetting>(store, ruleset, variant)
    {
        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();
            SetDefault(CoinFlipRulesetSetting.ShowResultCounter, true);
            SetDefault(CoinFlipRulesetSetting.ShowResultCounterAfterNRolls, "5");
            SetDefault(CoinFlipRulesetSetting.FlipSpeedMultiplier, 1, 0.1, 2, 0.1);
        }
    }

    public enum CoinFlipRulesetSetting
    {
        ShowResultCounter,
        ShowResultCounterAfterNRolls,
        FlipSpeedMultiplier,
    }
}
