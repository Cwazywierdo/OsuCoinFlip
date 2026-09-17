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
    public partial class DrawableCoin(TextureStore textures) : Container
    {
        private const float coin_size = 280;
        private const double fade_duration = 100;
        private const double pre_flip_linger_duration = 300;
        private const double post_flip_linger_duration = 500;

        private const float flip_height = 50;

        private const double min_flip_time = 1100;
        private const double max_flip_time = 1700;

        private const int min_flip_count = 3;
        private const int max_flip_count = 5;

        private const double min_half_flip_duration = 150;

        private Drawable scaleContainer;
        private Drawable flipContainer;
        private Sprite headsSprite;
        private Sprite tailsSprite;

        public bool FlipComplete { get; set; } = false;

        public double HeadsWeight = 0.5;

        private bool isHeads = false;

        [BackgroundDependencyLoader]
        private void load(IRenderer renderer)
        {
            Size = new Vector2(coin_size);
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;

            AddInternal(new Container
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                Padding = new MarginPadding(5),
                EdgeEffect = new Framework.Graphics.Effects.EdgeEffectParameters
                {
                    Colour = Colour4.Black.Opacity(0.3f),
                    Type = Framework.Graphics.Effects.EdgeEffectType.Shadow,
                    Radius = coin_size / 2,
                    Roundness = coin_size / 2,
                    Offset = new Vector2(0, coin_size / 40),
                }
            });

            AddInternal(scaleContainer = new Container
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,

                Child = flipContainer = new Container
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
                }
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

            ClearTransforms(true);

            this.FadeIn((1 - Alpha) * fade_duration).Delay(coinFlipDuration + post_flip_linger_duration).Then().FadeOut(fade_duration).Expire();

            scaleContainer.ScaleTo(1.3f, coinFlipDuration * 0.5, Easing.OutQuad).MoveToOffset(new Vector2(0, -flip_height), coinFlipDuration * 0.5, Easing.OutQuad)
                .Then().ScaleTo(1, coinFlipDuration * 0.5, Easing.InQuad).MoveToOffset(new Vector2(0, flip_height), coinFlipDuration * 0.5, Easing.InQuad)
                .Then().Schedule(() => FlipComplete = true);

            flipContainer.Spin(coinFlipDuration, (RotationDirection)(r.Next() % 2), 0, 1);

            TransformSequence<Sprite> headsSpriteSequence = headsSprite.Delay(halfFlipDuration / 2);
            TransformSequence<Sprite> tailsSpriteSequence = tailsSprite.Delay(halfFlipDuration / 2);
            TransformSequence<Drawable> flipSequence = flipContainer.Delay(0);

            for (int i = 0; i < halfFlips; i++)
            {
                flipSequence = flipSequence.ScaleTo(new Vector2(-1, 1), halfFlipDuration, Easing.InOutSine).Then().ScaleTo(1);

                if (isHeads)
                {
                    headsSpriteSequence = headsSpriteSequence.FadeOut().Delay(halfFlipDuration);
                    tailsSpriteSequence = tailsSpriteSequence.FadeIn().ScaleTo(new Vector2(-1, 1)).Delay(halfFlipDuration / 2).Then().ScaleTo(1).Delay(halfFlipDuration / 2);
                }
                else
                {
                    headsSpriteSequence = headsSpriteSequence.FadeIn().ScaleTo(new Vector2(-1, 1)).Delay(halfFlipDuration / 2).Then().ScaleTo(1).Delay(halfFlipDuration / 2);
                    tailsSpriteSequence = tailsSpriteSequence.FadeOut().Delay(halfFlipDuration);
                }

                isHeads = !isHeads;
            }
        }
    }
}
