using osu.Framework.Allocation;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace osu.Game.Rulesets.CoinFlip
{
    public partial class CoinFlipIcon(Ruleset ruleset) : Sprite
    {
        private readonly Ruleset ruleset = ruleset;

        [BackgroundDependencyLoader]
        private void load(IRenderer renderer)
        {
            Texture = new TextureStore(renderer, new TextureLoaderStore(ruleset.CreateResourceStore()), false).Get("Textures/Icon");
        }
    }
}
