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
        private Bindable<RulesetInfo> activeRuleset { get; set; }

        private CoinSpawner coinSpawner;

        [BackgroundDependencyLoader]
        private void load(IRenderer renderer)
        {
            Texture = new TextureStore(renderer, new TextureLoaderStore(ruleset.CreateResourceStore()), false).Get("Textures/Icon");
        }

        protected override void LoadComplete()
        {
            if (getParent<ToolbarRulesetTabButton>() is ToolbarRulesetTabButton b)
            {
                b.Add(coinSpawner = new CoinSpawner(ruleset)
                {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                });

                // In case they switch using keyboard shortcuts
                activeRuleset.ValueChanged += r =>
                {
                    string coinFlipName = new CoinFlipRuleset().ShortName;
                    if (r.NewValue.ShortName == coinFlipName)
                    {
                        if (r.OldValue.ShortName != coinFlipName)
                            activeRuleset.Value = r.OldValue;

                        coinSpawner.createCoin();
                    }
                };
            }

            base.LoadComplete();
        }

        private T getParent<T>() where T : Drawable
        {
            Drawable d = Parent;
            while (d != null)
            {
                if (d.GetType() == typeof(T))
                    return d as T;

                d = d.Parent;
            }
            return null;
        }

        private partial class CoinSpawner(Ruleset ruleset) : Drawable
        {
            [Resolved]
            private OsuGame game { get; set; }

            private DrawableCoin coin;

            private TextureStore textures;

            [BackgroundDependencyLoader]
            private void load(IRenderer renderer)
            {
                textures = new TextureStore(renderer, new TextureLoaderStore(ruleset.CreateResourceStore()), false);
            }

            protected override bool OnClick(ClickEvent e)
            {
                createCoin();
                return true;
            }

            public void createCoin()
            {
                if (coin?.IsAlive ?? false)
                {
                    if (coin.FlipComplete)
                        coin.Flip();
                }
                else
                    game.Add(coin = new DrawableCoin(textures));
            }
        }
    }
}
