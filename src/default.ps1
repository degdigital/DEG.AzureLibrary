properties { 
  $base_dir = resolve-path .
  $parent_dir = resolve-path ..
  $build_dir = "$base_dir\_build"
  $tools_dir = "$parent_dir\tools"
  $sln_file = "$base_dir\DEG.AzureLibrary.sln"
  $run_tests = $false
  $msbuild = "C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild"
  $dotnet = "C:\Program Files\dotnet\dotnet.exe"
}
Framework "4.0"
	
task default -depends Package

task Clean {
	remove-item -force -recurse $build_dir -ErrorAction SilentlyContinue
}

task Init -depends Clean {
	new-item $build_dir -itemType directory
}

task Compile -depends Init {
	& $dotnet build "$sln_file" -c Debug -o "$build_dir\4.61"
}

task Dependency {
	$package_files = @(Get-ChildItem $base_dir -include *packages.config -recurse)
	foreach ($package in $package_files)
	{
		& $tools_dir\NuGet.exe install $package.FullName -o packages
	}
}

task Package -depends Dependency, Compile {
	$spec_files = @(Get-ChildItem $base_dir -include *.nuspec -recurse)
	foreach ($spec in $spec_files)
	{
		& $tools_dir\NuGet.exe pack $spec.FullName -o $build_dir -Symbols -BasePath $base_dir
	}
	& $tools_dir\NuGet.exe locals all -clear
}

task Push -depends Package {
	$spec_files = @(Get-ChildItem $build_dir -include *.nupkg -recurse)
	foreach ($spec in $spec_files)
	{
		& $tools_dir\NuGet.exe push $spec.FullName -source "https://www.nuget.org"
	}
}

