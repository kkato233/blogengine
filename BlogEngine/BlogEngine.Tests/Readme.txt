
To run tests:

1. Copy these files from "..\lib\watin" to "BlogEngine\BlogEngine.Tests\bin\Debug".
	Interop.SHDocVw.dll
	BlogEngine.Tests.dll.config

2. Click "BlogEngine\BlogEngine.sln" to load solution in VisualStudio 2010 or Visual Web Developer 2010.

3. Build and run solution (ctrl + F5 in Visual Studio)

4. If different, change root path in "BlogEngine\BlogEngine.Tests\Constants.cs" from 
	http://localhost:53265
	to point to website you've started in the previous step and re-build BlogEngine.Tests project.

5. Click "..\lib\nunit\runner\nunit.exe" to run NUnit UI.

6. File -> Open Project -> then select "BlogEngine\BlogEngine.Tests\bin\Debug\BlogEngine.Tests.dll"

7. Click "Run", all should pass.
