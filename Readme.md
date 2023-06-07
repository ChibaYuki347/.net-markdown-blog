# Summary
This repository is created for the purpose of creating a blog using Markdown on GitHub with .NET.

I am grateful to the author of the following article, which I am referring to for creating it.
https://zenn.dev/takunology/articles/github-docs-aspdotnet

The GitHub repository to be referenced is temporarily using the following.
https://github.com/takunology/MinecraftConnection

# Environment
.NET 7
There are console apps and Razor web applications available.

# Used library
Octokit.net Package that makes it easy to use the GitHub API
Markdig Package that converts Markdown to HTML

# How to use
When you run github-blog console, it retrieves Markdown from GitHub and generates HTML.
When you run github-blog-web, the web server starts and converts the retrieved Markdown to HTML and displays it.

Details of the mechanism
Taking the console app as an example, it takes the following steps.
In reality, you need to specify the target repository and authenticate if it is a private repository.

```csharp
var repo = "MinecraftConnection"; // repository name  
var author = "takunology";　// user name  
var brunch = "main";　// branch name  
  
var github = new GitHubClient(new ProductHeaderValue(repo)); // create a GitHub client  
var contents = await github.Repository.Content.GetAllContents(author, repo); // get a list of files in the repository  
var rawContent = await github.Repository.Content.GetRawContentByRef(author, repo, contents[5].Path, brunch); // get the contents of the file. Example: get the fifth file (Readme_Jp)  
var text = Encoding.UTF8.GetString(rawContent); // convert byte array to string  
  
Console.WriteLine(text);  
  
var html = Markdown.ToHtml(text);   // convert Markdown to HTML  
Console.WriteLine(html);// display HTML 
```

On the web app side, similar code to the console is written in Pages/index.cshtml and displayed.
```csharp
@page  
@using Markdig  
@using Octokit  
@using System.Text  
@model IndexModel  
@{  
    ViewData["Title"] = "Home page";  
}  
  
<div class="container">  
    @{  
        var repo = "MinecraftConnection";  
        var author = "takunology";  
        var brunch = "main";  
  
        var github = new GitHubClient(new ProductHeaderValue(repo));  
        var contents = await github.Repository.Content.GetAllContents(author, repo);  
        var rawContent = await github.Repository.Content.GetRawContentByRef(author, repo, contents[5].Path, brunch);  
        var text = Encoding.UTF8.GetString(rawContent);  
        var html = Markdown.ToHtml(text);  
    }  
    @Html.Raw(@html)  
</div>  
```
Regarding displaying images, it is possible to display images specified with relative paths by using the raw.githubusercontent.com via.

For syntax highlighting (when using source code, etc.), Visual Studio 2015 theme is used. It is enabled by adding the following to 
`Pagas/_Layout.cshtml.`

```html
 <link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/highlight.js/11.5.1/styles/vs2015.min.css">  

```
Scripts are also added.
```html
<script src="//cdnjs.cloudflare.com/ajax/libs/highlight.js/11.5.1/highlight.min.js"></script>  
<script>hljs.highlightAll();</script>  
```