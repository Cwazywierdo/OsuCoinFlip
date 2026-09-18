using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.Sprites;
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
                new OsuSpriteText {
                    Origin = Anchor.BottomCentre,
                    Anchor = Anchor.BottomCentre,
                    Text = "Press Escape to exit",
                    Font = new Framework.Graphics.Sprites.FontUsage(size: 35),
                    Position = new osuTK.Vector2(0, -20)
                }
            ]);
        }
    }
}
