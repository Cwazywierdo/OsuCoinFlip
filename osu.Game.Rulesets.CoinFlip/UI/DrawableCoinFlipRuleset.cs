using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.CoinFlip.Objects;
using osu.Game.Rulesets.CoinFlip.Objects.Drawables;
using osu.Game.Rulesets.CoinFlip.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.CoinFlip.UI
{
    [Cached]
    public partial class DrawableCoinFlip(CoinFlip ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null) : DrawableRuleset<CoinFlipObject>(ruleset, beatmap, mods)
    {
        public override bool AllowGameplayOverlays => false;

        [Resolved]
        private GameplayClockContainer clock { get; set; }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            clock.Seek(Objects.First().StartTime - DrawableCoinFlipObject.TIME_PREEMPT);
        }

        protected override Playfield CreatePlayfield() => new CoinFlipPlayfield();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new CoinFlipFramedReplayInputHandler(replay);

        public override DrawableHitObject<CoinFlipObject> CreateDrawableRepresentation(CoinFlipObject h) => new DrawableCoinFlipObject(h);

        protected override PassThroughInputManager CreateInputManager() => new CoinFlipInputManager(Ruleset?.RulesetInfo);
    }
}
