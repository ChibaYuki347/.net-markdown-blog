# 概要

本レポジトリは.NETを使ってGitHubにあるMarkdownを使ってBlogを作成する用途で作成しています。

下記記事を参考に作っているため作者の方に感謝を申し上げます。
https://zenn.dev/takunology/articles/github-docs-aspdotnet

なお参照するGitHubレポジトリは暫定的に下記を使用させていただいております。
https://github.com/takunology/MinecraftConnection


# 環境

- .NET 7

コンソールアプリとRazorでのWebアプリケーションの用意があります。

# 使用ライブラリ

Octokit.net GitHub APIを簡単に利用できるようにしたパッケージ  
Markdig MarkdownからHTMLへの変換を行うパッケージ

# 使い方

github-blog consoleを実行すると、GitHubからMarkdownを取得してHTMLを生成します。
github-blog-web 実行すると、Webサーバーが起動し取得したMarkdownをHTMLに変換して表示します。

# 詳細の仕組み

consoleアプリを例にとると下記のような流れをとっています。
実際には対象のレポジトリを指定したり、プライベートレポの場合は認証も必要です。

```csharp
var repo = "MinecraftConnection"; // レポジトリ名
var author = "takunology";　// ユーザー名
var brunch = "main";　// ブランチ名

var github = new GitHubClient(new ProductHeaderValue(repo)); // GitHubクライアントの作成
var contents = await github.Repository.Content.GetAllContents(author, repo); // レポジトリ内のファイル一覧を取得
var rawContent = await github.Repository.Content.GetRawContentByRef(author, repo, contents[5].Path, brunch); // ファイルの中身を取得例として5番目のファイル(Readme_Jp)を取得
var text = Encoding.UTF8.GetString(rawContent); // バイト配列を文字列に変換

Console.WriteLine(text);

var html = Markdown.ToHtml(text);   // MarkdownをHTMLに変換
Console.WriteLine(html);// HTMLを表示
```


Webアプリ側ではPages/index.cshtmlにコンソールと似たようなコードを記述して表示しています。

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

画像の表示に関してはraw.githubusercontent.comを経由を使うことで相対パスで指定された画像も表示可能。

シンタックスハイライト（ソースコードなどを使う場合）Visual Studio 2015のテーマを使用 Pagas/_Layout.cshtmlに下記を追加することで有効化しています。

```html
 <link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/highlight.js/11.5.1/styles/vs2015.min.css">
```

スクリプトについても追加をしています。
```html
<script src="//cdnjs.cloudflare.com/ajax/libs/highlight.js/11.5.1/highlight.min.js"></script>
<script>hljs.highlightAll();</script>
```