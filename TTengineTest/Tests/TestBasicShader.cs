// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

namespace TTengineTest
{
    /// <summary>
    /// Basic shader test that creates an Fx layer and renders sprites to it
    /// </summary>
    class TestBasicShader : Test
    {

        public override void BuildAll()
        {
            var fx = CreateFx(New(), "FixedColor");
            using (BuildTo(fx))
            {
                var t = new TestRotation();
                t.BuildLike(this);
                t.BuildAll();
            }

            
        }

    }
}
