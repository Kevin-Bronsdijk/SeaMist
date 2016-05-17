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
let summary = "SeaMist a .NET library for the Kraken.io REST API"
let version = "0.1.1.7"
let description = "The SeaMist library interacts with the Kraken.io REST API allowing you to utilize Krakens features using a .NET interface."
let notes = "Added file path support for uploads, Chroma Subsampling and crop_mode. For more information and documentation, please visit the project site on GitHub."
let nugetVersion = "1.1.7"
let tags = "kraken.io C# API image optimization"
let gitHome = "https://github.com/Kevin-Bronsdijk"
let gitName = "SeaMist"

// --------------------------------------------------------------------------------------
// Build script 
// --------------------------------------------------------------------------------------

let buildDir = "./output/"
let packagingOutputPath = "./nuGet/"
let packagingWorkingDir = "./inputNuget/"
let nugetDependencies = getDependencies "./src/SeaMist/packages.config"

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

Target "CreatePackage" (fun _ ->

    CreateDir packagingWorkingDir
    CleanDir packagingWorkingDir
    CopyFile packagingWorkingDir "./output/SeaMist.dll"

    NuGet (fun p -> 
        {p with
            Authors = authors
            Dependencies = nugetDependencies
            Files = [@"SeaMist.dll", Some @"lib/net452", None]
            Project = project
            Description = description
            OutputPath = packagingOutputPath
            Summary = summary
            WorkingDir = packagingWorkingDir
            Version = nugetVersion
            ReleaseNotes = notes
            Publish = false }) 
            "scanr.nuspec"
            
    DeleteDir packagingWorkingDir
)

// --------------------------------------------------------------------------------------

Target "All" DoNothing

"Clean"
  ==> "AssemblyInfo"
  ==> "Build"
  ==> "CreatePackage"
  ==> "All"

RunTargetOrDefault "All"