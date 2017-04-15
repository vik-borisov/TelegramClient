// include Fake lib
#r "packages/FAKE/tools/FakeLib.dll"
open Fake

// Properties
let buildDir = FullName "./build/"

let version = "0.0.1" //getBuildParam "version"

let mainProject = "./src/TelegramClient.Core/TelegramClient.Core.csproj"

Target "Clean" (fun _ ->
   CleanDir buildDir
)

Target "Build" (fun _ -> 
   DotNetCli.Restore (fun p -> p)

   DotNetCli.Build (fun p -> 
      { p with
            Configuration = "Release"
            Project = mainProject
      })

   ()

   DotNetCli.Pack (fun p -> 
      { p with
            OutputPath = buildDir
            Project = mainProject
      })
)
Target "Default" (fun _ ->
   trace "Hello World from FAKE"
)

// Dependencies
"Clean"
   ==> "Build"
   ==> "Default"

// start build
RunTargetOrDefault "Default"