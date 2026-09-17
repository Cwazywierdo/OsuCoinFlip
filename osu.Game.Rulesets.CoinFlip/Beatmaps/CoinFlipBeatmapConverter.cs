using osu.Game.Beatmaps;
using osu.Game.Rulesets.CoinFlip.Objects;

namespace osu.Game.Rulesets.CoinFlip.Beatmaps
{
    public class CoinFlipBeatmapConverter(IBeatmap beatmap, Ruleset ruleset) : BeatmapConverter<CoinFlipObject>(beatmap, ruleset)
    {
        public override bool CanConvert() => true;
    }
}
