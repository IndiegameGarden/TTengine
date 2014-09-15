@echo off
rem
rem Build file to build Effect (.fx) using MonoGame content builder tool
rem Use this with post-build command:
rem $(ProjectDir)\MonoGame-Effect-build.bat $(ProjectDir) $(OutDir)
rem
cd %1
"..\..\MGCB\MGCB.exe"^
 /outputDir:..\TTengineTest\%2\Content^
 /intermediateDir:obj^
 /importer:EffectImporter^
 /processor:EffectProcessor^
 /build:Grayscale.fx^
 /build:FixedColor.fx^
 /build:RandomColor.fx
