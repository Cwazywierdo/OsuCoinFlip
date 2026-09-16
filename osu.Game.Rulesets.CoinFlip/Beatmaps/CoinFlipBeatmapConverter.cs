using System.Linq;
using System.Threading;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.CoinFlip.Objects;
using osu.Game.Rulesets.Objects.Types;

namespace osu.Game.Rulesets.CoinFlip.Beatmaps
{
    public class CoinFlipBeatmapConverter(IBeatmap beatmap, Ruleset ruleset) : BeatmapConverter<CoinFlipObject>(beatmap, ruleset)
    {
        public override bool CanConvert() => Beatmap.HitObjects.All(h => h is IHasPosition);

        protected override Beatmap<CoinFlipObject> ConvertBeatmap(IBeatmap original, CancellationToken cancellationToken)
        {
            Beatmap<CoinFlipObject> beatmap = base.ConvertBeatmap(original, cancellationToken);

            double startTime;

            if (beatmap.ControlPointInfo.EffectPoints.LastOrDefault(point => point.KiaiMode) is ControlPoint effectPoint)
                startTime = effectPoint.Time;
            else
            {
                double previewTime = beatmap.BeatmapInfo.Metadata.PreviewTime;
                if (previewTime == -1)
                    previewTime = beatmap.BeatmapInfo.Length * 0.4d;

                TimingControlPoint timingPoint = beatmap.ControlPointInfo.TimingPointAt(previewTime);

                // Testing a few of my beatmaps, it seems that the preview point is often about a measure before some kind of drop.
                // Not very reliable, but better than nothing.
                startTime = previewTime + timingPoint.TimeSignature.Numerator * timingPoint.BeatLength;
            }

            beatmap.HitObjects = [new CoinFlipObject() { StartTime = startTime }];

            return beatmap;
        }
    }
}
