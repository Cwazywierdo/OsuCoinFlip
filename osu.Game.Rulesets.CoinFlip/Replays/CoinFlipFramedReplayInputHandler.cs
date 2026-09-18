using osu.Game.Replays;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.CoinFlip.Replays
{
    public class CoinFlipFramedReplayInputHandler(Replay replay) : FramedReplayInputHandler<CoinFlipReplayFrame>(replay)
    {
    }
}
