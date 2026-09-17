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

        private const int min_flip_count = 5;
        private const int max_flip_count = 10;

        private const double min_single_flip_duration = 150;

        private Drawable flipContainer;
        private Sprite headsSprite;
        private Sprite tailsSprite;

        public double HeadsWeight = 0.5;

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
            bool resultIsHeads = r.NextDouble() < HeadsWeight;

            int flipCount = r.Next() % (max_flip_count - min_flip_count + 1) + min_flip_count;
            double flipDuration = (max_flip_time - min_flip_time) + min_flip_time;

            double single_flip_duration = flipDuration / flipCount;

            if (single_flip_duration > min_single_flip_duration)
            {
                single_flip_duration = min_single_flip_duration;
                flipDuration = single_flip_duration * flipCount;
            }

            int flipDirection = (r.Next() % 2) * 2 - 1;

            this.FadeInFromZero(fade_duration);

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

            using (BeginDelayedSequence(fade_duration + pre_flip_linger_duration))
            {
                this.ScaleTo(1.3f, flipDuration * 0.5, Easing.OutQuad).MoveToOffset(new Vector2(0, -flip_height), flipDuration * 0.5, Easing.OutQuad)
                    .Then().ScaleTo(1, flipDuration * 0.5, Easing.InQuad).MoveToOffset(new Vector2(0, flip_height), flipDuration * 0.5, Easing.InQuad)
                    .Then().Delay(post_flip_linger_duration).Then().FadeOut(fade_duration).Then().Expire();

                flipContainer.RotateTo(360 * flipDirection, flipDuration);

                TransformSequence<Sprite> headsSpriteSequence = headsSprite.Delay(single_flip_duration / 2).Then();
                TransformSequence<Sprite> tailsSpriteSequence = tailsSprite.Delay(single_flip_duration / 2).Then();
                TransformSequence<Drawable> flipSequence = flipContainer.Delay(0).Then();

                for (int i = 0; i < flipCount; i++)
                {
                    flipSequence = flipSequence.ScaleTo(new Vector2(-1, 1), single_flip_duration, Easing.InOutSine).Then().ScaleTo(1).Then();

                    if (isHeads)
                    {
                        headsSpriteSequence = headsSpriteSequence.FadeOut().Delay(single_flip_duration).Then();
                        tailsSpriteSequence = tailsSpriteSequence.FadeIn().ScaleTo(new Vector2(-1, 1)).Delay(single_flip_duration / 2).Then().ScaleTo(1).Delay(single_flip_duration / 2).Then();
                    }
                    else
                    {
                        headsSpriteSequence = headsSpriteSequence.FadeIn().ScaleTo(new Vector2(-1, 1)).Delay(single_flip_duration / 2).Then().ScaleTo(1).Delay(single_flip_duration / 2).Then();
                        tailsSpriteSequence = tailsSpriteSequence.FadeOut().Delay(single_flip_duration).Then();
                    }

                    isHeads = !isHeads;
                }
            }

            base.LoadComplete();
        }

        //protected override void CheckForResult(bool userTriggered, double timeOffset)
        //{
        //    if (timeOffset >= linger_time)
        //        ApplyMaxResult();
        //}

        //protected override double InitialLifetimeOffset => TIME_PREEMPT;

    }
}
