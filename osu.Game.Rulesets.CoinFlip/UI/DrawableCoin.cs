using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Transforms;
using osuTK;

namespace osu.Game.Rulesets.CoinFlip.UI
{
    public partial class DrawableCoin(Ruleset ruleset) : Container
    {
        private const double fade_duration = 100;
        private const double pre_flip_linger_duration = 300;
        private const double post_flip_linger_duration = 500;

        private const float flip_height = 50;

        private const double min_flip_time = 1100;
        private const double max_flip_time = 1700;

        private const int min_flip_count = 3;
        private const int max_flip_count = 5;

        private const double min_half_flip_duration = 150;

        private Drawable flipContainer;
        private Sprite headsSprite;
        private Sprite tailsSprite;

        public bool FlipComplete { get; set; } = false;

        public double HeadsWeight = 0.5;

        private bool isHeads = false;

        [BackgroundDependencyLoader]
        private void load(IRenderer renderer)
        {
            TextureStore textures = new TextureStore(renderer, new TextureLoaderStore(ruleset.CreateResourceStore()), false);

            Size = new Vector2(280);
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;

            AddInternal(flipContainer = new Container
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,

                Children =
                [
                    headsSprite = new Sprite
                    {
                        RelativeSizeAxes = Axes.Both,
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        Texture = textures.Get("Textures/CoinHeads"),
                    },
                    tailsSprite = new Sprite
                    {
                        RelativeSizeAxes = Axes.Both,
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        Texture = textures.Get("Textures/CoinTails"),
                    },
                ]
            });
        }

        protected override void LoadComplete()
        {
            Random r = new Random();
            isHeads = r.NextDouble() < 0.5;

            if (isHeads)
            {
                headsSprite.FadeIn();
                tailsSprite.FadeOut();
            }
            else
            {
                headsSprite.FadeOut();
                tailsSprite.FadeIn();
            }

            this.FadeInFromZero(fade_duration);

            this.Delay(fade_duration + pre_flip_linger_duration).Then().Schedule(Flip);

            base.LoadComplete();
        }

        public void Flip()
        {
            FlipComplete = false;

            ClearTransforms(true);

            this.FadeIn((1 - Alpha) * fade_duration);

            Random r = new Random();

            bool resultIsHeads = r.NextDouble() < HeadsWeight;

            int fullFlips = r.Next() % (max_flip_count - min_flip_count + 1) + min_flip_count;
            int halfFlips = 2 * fullFlips + (resultIsHeads == isHeads ? 0 : 1);

            double coinFlipDuration = r.NextDouble() * (max_flip_time - min_flip_time) + min_flip_time;

            double halfFlipDuration = coinFlipDuration / halfFlips;

            if (halfFlipDuration > min_half_flip_duration)
            {
                halfFlipDuration = min_half_flip_duration;
                coinFlipDuration = halfFlipDuration * halfFlips;
            }

            this.ScaleTo(1.3f, coinFlipDuration * 0.5, Easing.OutQuad).MoveToOffset(new Vector2(0, -flip_height), coinFlipDuration * 0.5, Easing.OutQuad)
                .Then().ScaleTo(1, coinFlipDuration * 0.5, Easing.InQuad).MoveToOffset(new Vector2(0, flip_height), coinFlipDuration * 0.5, Easing.InQuad)
                .Then().Schedule(() => FlipComplete = true).Delay(post_flip_linger_duration).Then().FadeOut(fade_duration).Then().Expire();

            flipContainer.Spin(coinFlipDuration, (RotationDirection)(r.Next() % 2), 0, 1);

            TransformSequence<Sprite> headsSpriteSequence = headsSprite.Delay(halfFlipDuration / 2).Then();
            TransformSequence<Sprite> tailsSpriteSequence = tailsSprite.Delay(halfFlipDuration / 2).Then();
            TransformSequence<Drawable> flipSequence = flipContainer.Delay(0).Then();

            for (int i = 0; i < halfFlips; i++)
            {
                flipSequence = flipSequence.ScaleTo(new Vector2(-1, 1), halfFlipDuration, Easing.InOutSine).Then().ScaleTo(1).Then();

                if (isHeads)
                {
                    headsSpriteSequence = headsSpriteSequence.FadeOut().Delay(halfFlipDuration).Then();
                    tailsSpriteSequence = tailsSpriteSequence.FadeIn().ScaleTo(new Vector2(-1, 1)).Delay(halfFlipDuration / 2).Then().ScaleTo(1).Delay(halfFlipDuration / 2).Then();
                }
                else
                {
                    headsSpriteSequence = headsSpriteSequence.FadeIn().ScaleTo(new Vector2(-1, 1)).Delay(halfFlipDuration / 2).Then().ScaleTo(1).Delay(halfFlipDuration / 2).Then();
                    tailsSpriteSequence = tailsSpriteSequence.FadeOut().Delay(halfFlipDuration).Then();
                }

                isHeads = !isHeads;
            }
        }
    }
}
