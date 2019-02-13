1. Update the version field in "Rpi.SenseHat.nuspec" (next to the csproj-file).

2. Open a CMD in this folder.

3. Run the build-nuget-package.cmd

4. Execute:
nuget setApiKey {API KEY}

Where the {API KEY} is gotten from https://www.nuget.org/account

5. Execute:
nuget push Emmellsoft.IoT.Rpi.SenseHat.{#.#.#.#}.nupkg

Where {#.#.#.#} is the current version.