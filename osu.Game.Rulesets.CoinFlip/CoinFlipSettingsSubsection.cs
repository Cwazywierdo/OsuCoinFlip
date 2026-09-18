using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Overlays.Settings;

namespace osu.Game.Rulesets.CoinFlip
{
    internal partial class CoinFlipSettingsSubsection(Ruleset ruleset) : RulesetSettingsSubsection(ruleset)
    {
        protected override LocalisableString Header => "Coin Flip";

        private Bindable<bool> showResultCounter = new BindableBool(true);

        private SettingsItemV2 showResultCounterAfterNRollsSettings;

        [BackgroundDependencyLoader]
        private void load()
        {
            var config = (CoinFlipRulesetConfigManager)Config;

            showResultCounter = config.GetBindable<bool>(CoinFlipRulesetSetting.ShowResultCounter);

            Children =
            [
                new SettingsItemV2(new FormCheckBox
                {
                    Caption = "Show results counter",
                    Current = showResultCounter
                }),
                showResultCounterAfterNRollsSettings = new SettingsItemV2(new FormNumberBox
                {
                    Caption = "Show after N flips",
                    HintText = "Set this to 0 to make the counter always visible.",
                    Current = config.GetBindable<string>(CoinFlipRulesetSetting.ShowResultCounterAfterNRolls),

                }),
                new SettingsItemV2(new FormSliderBar<double>
                {
                    Caption = "Flip speed multiplier",
                    Current = config.GetBindable<double>(CoinFlipRulesetSetting.FlipSpeedMultiplier),
                }),
            ];

            showResultCounter.BindValueChanged(e =>
            {
                if (e.NewValue)
                    showResultCounterAfterNRollsSettings.Show();
                else
                    showResultCounterAfterNRollsSettings.Hide();
            }, true);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

        }
    }
}
