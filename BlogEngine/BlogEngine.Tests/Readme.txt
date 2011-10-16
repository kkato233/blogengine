BlogEngine.Tests.Account.Login (TestFixtureSetUp):
SetUp : System.IO.FileNotFoundException : Could not load file or assembly 'Interop.SHDocVw, Version=1.1.0.0, Culture=neutral, PublicKeyToken=db7cfd3acb5ad44e' or one of its dependencies. The system cannot find the file specified.

To run tests:

2. Copy these files from "BlogEngine\packages\WatiN.2.1.0\lib\net40\" to "BlogEngine\BlogEngine.Tests\bin\Debug".
    Interop.SHDocVw.dll
	AjaxMin.dll
	BlogEngine.Tests.dll.config

2. Click "BlogEngine\BlogEngine.sln" to load solution in VisualStudio 2010 or Visual Web Developer 2010.

3. Build and run solution (ctrl + F5 in Visual Studio etc.)

4. Change root path in "BlogEngine\BlogEngine.Tests\Constants.cs" from 
	http://localhost:53265
	to point to website you've started in the previous step and re-build BlogEngine.Tests project.

5. Go to "C:\Projects\hg\be\BlogEngine\packages\NUnit.2.5.10.11092\tools" and click "nunit.exe".

6. File -> Open Project -> then select "BlogEngine\BlogEngine.Tests\bin\Debug\BlogEngine.Tests.dll"

7. Click "Run", all should pass.


