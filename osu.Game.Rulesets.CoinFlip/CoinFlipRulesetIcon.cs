using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Game.Overlays.Toolbar;
using osu.Game.Rulesets.CoinFlip.UI;

namespace osu.Game.Rulesets.CoinFlip
{
    public partial class CoinFlipIcon(Ruleset ruleset) : Sprite
    {
        private readonly Ruleset ruleset = ruleset;

        [Resolved]
        private OsuGame game { get; set; }

        [Resolved]
        private Bindable<RulesetInfo> activeRuleset { get; set; }

        private DrawableCoin coin;

        [BackgroundDependencyLoader]
        private void load(IRenderer renderer)
        {
            Texture = new TextureStore(renderer, new TextureLoaderStore(ruleset.CreateResourceStore()), false).Get("Textures/Icon");
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            if (isChildOf<ToolbarRulesetSelector>())
            {
                activeRuleset.ValueChanged += r =>
                {
                    string coinFlipName = new CoinFlipRuleset().ShortName;
                    if (r.NewValue.ShortName == coinFlipName)
                    {
                        if (r.OldValue.ShortName != coinFlipName)
                            activeRuleset.Value = r.OldValue;

                        createCoin();
                    }
                };
            }
        }

#if DEBUG
        protected override bool OnClick(ClickEvent e)
        {
            createCoin();
            return base.OnClick(e);
        }
#endif

        private void createCoin()
        {
            if (!coin?.IsAlive ?? true)
                game.Add(coin = new DrawableCoin(ruleset));
        }

        private bool isChildOf<T>() where T : Drawable
        {
            Drawable d = Parent;
            while (d != null)
            {
                if (d.GetType() == typeof(T))
                    return true;

                d = d.Parent;
            }
            return false;
        }
    }
}
