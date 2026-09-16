using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.CoinFlip
{
    public partial class CoinFlipInputManager(RulesetInfo ruleset) : RulesetInputManager<CoinFlipAction>(ruleset, 0, SimultaneousBindingMode.Unique)
    {
    }

    public enum CoinFlipAction
    {

    }
}
