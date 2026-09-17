using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.CoinFlip.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.CoinFlip
{
    public class CoinFlipRuleset : Ruleset
    {
        public override string Description => "Flip a Coin";

        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) => null;

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) =>
            new CoinFlipBeatmapConverter(beatmap, this);

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) =>
            new CoinFlipDifficultyCalculator(RulesetInfo, beatmap);

        public override IEnumerable<Mod> GetModsFor(ModType type) => [];

        public override string ShortName => "coinflip";

        public override string PlayingVerb => "Flipping coins";

        public override Drawable CreateIcon() => new CoinFlipIcon(this);

        // Leave this line intact. It will bake the correct version into the ruleset on each build/release.
        public override string RulesetAPIVersionSupported => CURRENT_RULESET_API_VERSION;
    }
}
