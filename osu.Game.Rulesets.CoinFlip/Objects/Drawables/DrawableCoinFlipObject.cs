using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Transforms;
using osu.Game.Audio;
using osu.Game.Rulesets.Objects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.CoinFlip.Objects.Drawables
{
    public partial class DrawableCoinFlipObject : DrawableHitObject<CoinFlipObject>
    {
        public const double TIME_PREEMPT = max_single_flip_time * max_flip_count + 700;
        private const double time_fade = 400;
        private const double linger_time = 2000;

        private const float flip_height = 50;

        private const double min_single_flip_time = 150;
        private const double max_single_flip_time = 210;

        private const int min_flip_count = 5;
        private const int max_flip_count = 12;

        private Drawable flipContainer;
        private Sprite headsSprite;
        private Sprite tailsSprite;

        public double HeadsWeight = 0.5;

        private double flipTime;
        private int flipCount;
        private double flipSpeed;
        private bool resultIsHeads;

        public DrawableCoinFlipObject(CoinFlipObject hitObject)
            : base(hitObject)
        {
            Random r = new Random();

            resultIsHeads = r.NextDouble() < HeadsWeight;

            flipSpeed = r.NextDouble() * (max_single_flip_time - min_single_flip_time) + min_single_flip_time;
            flipCount = r.Next() % (max_flip_count - min_flip_count + 1) + min_flip_count;

            flipTime = flipCount * flipSpeed;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Size = new Vector2(360);
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
                        Texture = textures.Get("CoinHeads"),
                    },
                    tailsSprite = new Sprite
                    {
                        RelativeSizeAxes = Axes.Both,
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        Texture = textures.Get("CoinTails"),
                    },
                ]
            });
        }

        public override IEnumerable<HitSampleInfo> GetSamples() => [];

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset >= linger_time)
                ApplyMaxResult();
        }

        protected override double InitialLifetimeOffset => TIME_PREEMPT;

        protected override void UpdateInitialTransforms()
        {
            this.FadeInFromZero(time_fade);

            bool isHeads = resultIsHeads == (flipCount % 2 == 0);

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

            using (BeginDelayedSequence(TIME_PREEMPT - flipTime))
            {
                this.ScaleTo(1.3f, flipTime * 0.5, Easing.OutQuad).MoveToOffset(new Vector2(0, -flip_height), flipTime * 0.5, Easing.OutQuad)
                    .Then().ScaleTo(1, flipTime * 0.5, Easing.InQuad).MoveToOffset(new Vector2(0, flip_height), flipTime * 0.5, Easing.InQuad)
                    .Delay(2000).Then().FadeOut(time_fade);

                TransformSequence<Sprite> headsSpriteSequence = headsSprite.Delay(flipSpeed / 2).Then();
                TransformSequence<Sprite> tailsSpriteSequence = tailsSprite.Delay(flipSpeed / 2).Then();
                TransformSequence<Drawable> flipSequence = flipContainer.Delay(0).Then();

                for (int i = 0; i < flipCount; i++)
                {
                    flipSequence = flipSequence.ScaleTo(new Vector2(-1, 1), flipSpeed, Easing.InOutSine).Then().ScaleTo(1).Then();

                    if (isHeads)
                    {
                        headsSpriteSequence = headsSpriteSequence.FadeOut().Delay(flipSpeed).Then();
                        tailsSpriteSequence = tailsSpriteSequence.FadeIn().ScaleTo(new Vector2(-1, 1)).Delay(flipSpeed / 2).Then().ScaleTo(1).Delay(flipSpeed / 2).Then();
                    }
                    else
                    {
                        headsSpriteSequence = headsSpriteSequence.FadeIn().ScaleTo(new Vector2(-1, 1)).Delay(flipSpeed / 2).Then().ScaleTo(1).Delay(flipSpeed / 2).Then();
                        tailsSpriteSequence = tailsSpriteSequence.FadeOut().Delay(flipSpeed).Then();
                    }

                    isHeads = !isHeads;
                }
            }
        }
    }
}
