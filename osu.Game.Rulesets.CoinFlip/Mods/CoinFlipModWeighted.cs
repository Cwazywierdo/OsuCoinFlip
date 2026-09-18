using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Configuration;
using osu.Game.Rulesets.CoinFlip.Objects.Drawables;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.CoinFlip.Mods
{
    internal class CoinFlipModWeighted : Mod, IApplicableToDrawableHitObject
    {
        public override string Name => "Weighted";

        public override string Acronym => "WT";

        public override LocalisableString Description => "Rig the bet";

        public override ModType Type => ModType.Conversion;

        public override string ExtendedIconInformation => $"{double.Round(HeadsProbability.Value * 100)}/{100 - double.Round(HeadsProbability.Value * 100)}";

        [SettingSource("Heads propability")]
        public BindableNumber<double> HeadsProbability { get; } = new BindableNumber<double>(0.5)
        {
            MinValue = 0,
            MaxValue = 1,
            Precision = 0.01,
        };

        public void ApplyToDrawableHitObject(DrawableHitObject drawable)
        {
            if (drawable is DrawableCoinFlipObject o)
                o.HeadsWeight = HeadsProbability.Value;
        }
    }
}
