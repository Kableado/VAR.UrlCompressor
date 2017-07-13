# .Net library for compressing URLs

## Instalation

You can install the NuGet package using this command on the package manager:
	
	Install-Package VAR.UrlCompressor 

Alternativelly you can copy and reference the assembly resulting of the project VAR.UrlCompressor.

## Usage

### VAR.UrlCompressor
Add the resulting assembly as reference in your projects, and this line on code:

	using VAR.UrlCompressor;

Compress an URL with:

	string compressedUrl = UrlCompressor.Compress("https:\\google.com");
	// compressedUrl = "Hk30TGDxt8jOOW6"

Decompress an URL with:

	string decompressedUrl = UrlCompressor.Decompress("Hk30TGDxt8jOOW6");
	// decompressedUrl = "https:\\google.com";
	
For extra compression use host conversions. For example:
	
	Dictionary<string, string> hostConversions = new Dictionary<string, string> {
		{ "google", "G" }
		{ "com", "C" }
	}
	string compressedUrl = UrlCompressor.Compress("https:\\google.com", );
	// compressedUrl = "oMyuFVR41"
	string decompressedUrl = UrlCompressor.Decompress("oMyuFVR41");
	// decompressedUrl = "https:\\google.com";


### UrlCompressor.Tests
It is a simple console application, to test basic funcitionallity of the library.

## Building
A Visual Studio 2017 solution is provided. Simply, click build on the IDE.

A .nuget package can be build using:
	VAR.UrlCompressor\Build.NuGet.cmd

## Contributing
1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D

## Credits
* Valeriano Alfonso Rodriguez.

## License

    The MIT License (MIT)

    Copyright (c) 2016-2017 Valeriano Alfonso Rodriguez

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
