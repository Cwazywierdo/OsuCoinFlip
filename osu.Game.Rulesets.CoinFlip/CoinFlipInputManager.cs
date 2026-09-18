using System;
using osu.Framework.Allocation;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.CoinFlip
{
    [Cached]
    public partial class CoinFlipInputManager(RulesetInfo ruleset) : RulesetInputManager<CoinFlipAction>(ruleset, 0, SimultaneousBindingMode.Unique)
    {
        public event Action<MouseDownEvent> mouseDown;

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            mouseDown.Invoke(e);
            return true;
        }
    }

    public enum CoinFlipAction
    {
        FlipButton
    }
}
