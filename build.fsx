// include Fake lib


#r @"tools\FAKE\tools\FakeLib.dll"

open Fake



// Properties
let buildDir = "./output/"

// Targets
Target "Clean" (fun _ ->
 CleanDir buildDir
)

Target "BuildApp" (fun _ ->
 !! "src/*.sln"
 |> MSBuildRelease buildDir "Build"
 |> Log "AppBuild-Output: "
)

Target "Default" (fun _ ->
 trace "SeaMist Build started"
)

// Dependencies
"Clean"
  ==> "BuildApp"
  ==> "Default"

// start build
RunTargetOrDefault "Default"