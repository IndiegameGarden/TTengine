@echo off
rem
rem Build file to build Effect (.fx) using MonoGame content builder tool
rem
cd %1
rem "C:\Program Files (x86)\MSBuild\MonoGame\v3.0\MGCB\MGCB.exe"
"..\..\TTengine\MGCB\MGCB.exe"^
 /outputDir:..\TTengineTest\%2\Content^
 /intermediateDir:obj^
 /importer:EffectImporter^
 /processor:EffectProcessor^
 /build:Grayscale.fx^
 /build:FixedColor.fx^
 /build:RandomColor.fx
