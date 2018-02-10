// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

namespace TTengineTest
{
    /// <summary>
    /// Shader test with texture sampling, applied to entire screen
    /// </summary>
    class TestTextureSamplingShader : Test
    {

        public override void BuildAll()
        {
            var fx1 = CreateFx(New(), "Grayscale");
            using (BuildTo(fx1))
            {
                var t = new TestRotation();
                t.BuildLike(this);
                t.BuildAll();
            }
        }

    }
}
