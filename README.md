# .Net library for compressing URLs

## Instalation

You can install the NuGet package using this command on the package manager:
	
	Install-Package VAR.UrlCompressor 

Alternativelly you can copy and reference the assembly resulting of the project VAR.UrlCompressor.

## Usage

### VAR.UrlCompressor
Add the resulting assembly as reference in your projects, and this line on code:

```csharp
using VAR.UrlCompressor;
```

Compress an URL with:

```csharp
	string compressedUrl = UrlCompressor.Compress("https:\\google.com");
	// compressedUrl = "Hk30TGDxt8jOOW6"
```

Decompress an URL with:

```csharp
	string decompressedUrl = UrlCompressor.Decompress("Hk30TGDxt8jOOW6");
	// decompressedUrl = "https:\\google.com";
```

For extra compression use host conversions. For example:

```csharp
	Dictionary<string, string> hostConversions = new Dictionary<string, string> {
		{ "google", "G" }
		{ "com", "C" }
	}
	string compressedUrl = UrlCompressor.Compress("https:\\google.com", );
	// compressedUrl = "oMyuFVR41"
	string decompressedUrl = UrlCompressor.Decompress("oMyuFVR41");
	// decompressedUrl = "https:\\google.com";
```


### UrlCompressor.Tests
It is a simple console application, to test basic funcitionallity of the library.

## Building
A Visual Studio solution is provided. Simply, click build on the IDE.

The build generates a DLL and a Nuget package.

## Contributing
1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D

## Credits
* Valeriano Alfonso Rodriguez.
