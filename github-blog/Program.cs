using Octokit;
using System.Text;
using Markdig;

var repo = "MinecraftConnection";
var author = "takunology";
var brunch = "main";

var github = new GitHubClient(new ProductHeaderValue(repo));
var contents = await github.Repository.Content.GetAllContents(author, repo);
var rawContent = await github.Repository.Content.GetRawContentByRef(author, repo, contents[5].Path, brunch);
var text = Encoding.UTF8.GetString(rawContent);

Console.WriteLine(text);

var html = Markdown.ToHtml(text);
Console.WriteLine(html);


Console.ReadLine(); // ユーザーが何かを入力するまでアプリケーションの終了を遅らせます