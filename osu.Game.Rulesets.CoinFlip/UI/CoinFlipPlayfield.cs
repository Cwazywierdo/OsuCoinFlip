using osu.Framework.Allocation;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.CoinFlip.UI
{
    [Cached]
    public partial class CoinFlipPlayfield : Playfield
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            AddRangeInternal(
            [
                HitObjectContainer,
            ]);
        }
    }
}
