// include Fake lib
#r "packages/build/FAKE/tools/FakeLib.dll"
open Fake

// Properties
let buildDir = FullName "./build/"

let apikey = getBuildParam "apikey"
let version = getBuildParam "version"

Target "Clean" (fun _ ->
   CleanDir buildDir
)

Target "Build" (fun _ -> 
   XMLHelper.XmlPokeInnerText "./src/TelegramClient.Core/TelegramClient.Core.csproj" "/Project/PropertyGroup/Version" version
   XMLHelper.XmlPokeInnerText "./src/TelegramClient.Entities/TelegramClient.Entities.csproj" "/Project/PropertyGroup/Version" version

   DotNetCli.Restore (fun p -> p)

   DotNetCli.Build (fun p -> 
   { p with
      Configuration = "Release"
   })

   ()

   DotNetCli.Pack (fun p -> 
   { p with
      OutputPath = buildDir
      Project = "./src/TelegramClient.Core/TelegramClient.Core.csproj"
   })
   DotNetCli.Pack (fun p -> 
   { p with
      OutputPath = buildDir
      Project = "./src/TelegramClient.Entities/TelegramClient.Entities.csproj"
   })
)

Target "PublishNuget" (fun _ -> 
   Paket.Push (fun nugetParams -> 
    { nugetParams with
        ApiKey = apikey
        WorkingDir = buildDir
    }
   )
)

Target "Default" (fun _ ->
   trace "Hello World from FAKE"
)

// Dependencies
"Clean"
   ==> "Build"
   ==> "PublishNuget"
   ==> "Default"

// start build
RunTargetOrDefault "Default"