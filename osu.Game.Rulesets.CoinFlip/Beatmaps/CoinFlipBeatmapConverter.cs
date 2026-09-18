using System.Threading;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.CoinFlip.Objects;

namespace osu.Game.Rulesets.CoinFlip.Beatmaps
{
    public class CoinFlipBeatmapConverter(IBeatmap beatmap, Ruleset ruleset) : BeatmapConverter<CoinFlipObject>(beatmap, ruleset)
    {
        public override bool CanConvert() => true;

        protected override Beatmap<CoinFlipObject> ConvertBeatmap(IBeatmap original, CancellationToken cancellationToken)
        {
            Beatmap<CoinFlipObject> beatmap = base.ConvertBeatmap(original, cancellationToken);
            beatmap.HitObjects.Add(new CoinFlipObject());
            return beatmap;
        }
    }
}
