using NUnit.Framework;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.CoinFlip.Tests
{
    [TestFixture]
    public partial class TestSceneOsuPlayer : PlayerTestScene
    {
        protected override Ruleset CreatePlayerRuleset() => new CoinFlip();
    }
}
