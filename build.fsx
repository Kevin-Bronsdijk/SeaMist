// --------------------------------------------------------------------------------------
// FAKE build script 
// --------------------------------------------------------------------------------------

#r @"tools\FAKE\tools\FakeLib.dll"
open Fake
open Fake.AssemblyInfoFile

// --------------------------------------------------------------------------------------
// Information about the project to be used at NuGet and in AssemblyInfo files
// --------------------------------------------------------------------------------------

let project = "SeaMist"
let authors = ["Kevin Bronsdijk"]
let summary = "A client SDK for Kraken.io."
let version = "0.1.1.5"
let description = """
The SeaMist library interacts with the Kraken.io REST API allowing you to utilize Kraken’s features using a .NET interface.
"""
let notes = "Added Direct Upload, Direct Upload external data store, Image Orientation, Enhancing Resized Images. For more information and documentation, please visit the project site on GitHub."
let nugetVersion = "1.1.5"
let tags = "kraken.io C# API image optimization"
let gitHome = "https://github.com/Kevin-Bronsdijk"
let gitName = "SeaMist"

// --------------------------------------------------------------------------------------
// Build script 
// --------------------------------------------------------------------------------------

let buildDir = "./output/"

// --------------------------------------------------------------------------------------

Target "Clean" (fun _ ->
 CleanDir buildDir
)

// --------------------------------------------------------------------------------------

Target "AssemblyInfo" (fun _ ->
    let attributes =
        [ 
            Attribute.Title project
            Attribute.Product project
            Attribute.Description summary
            Attribute.Version version
            Attribute.FileVersion version
        ]

    CreateCSharpAssemblyInfo "src/SeaMist/Properties/AssemblyInfo.cs" attributes
)

// --------------------------------------------------------------------------------------

Target "Build" (fun _ ->
 !! "src/*.sln"
 |> MSBuildRelease buildDir "Build"
 |> Log "AppBuild-Output: "
)

// --------------------------------------------------------------------------------------

Target "All" DoNothing

"Clean"
  ==> "AssemblyInfo"
  ==> "Build"
  ==> "All"

RunTargetOrDefault "All"