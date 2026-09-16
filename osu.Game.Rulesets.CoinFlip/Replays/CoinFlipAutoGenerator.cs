using osu.Game.Beatmaps;
using osu.Game.Rulesets.CoinFlip.Objects;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.CoinFlip.Replays
{
    public class CoinFlipAutoGenerator(IBeatmap beatmap) : AutoGenerator<CoinFlipReplayFrame>(beatmap)
    {
        public new Beatmap<CoinFlipObject> Beatmap => (Beatmap<CoinFlipObject>)base.Beatmap;

        protected override void GenerateFrames()
        {

        }
    }
}
