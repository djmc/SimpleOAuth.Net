# Simple OAuth .Net

OAuth libraries come in all shapes and sizes, however in the .Net land, they only come in one - extra large. This library is made to rectify that. It's a small (~15kb) library, with no dependencies that lets you create WebRequest's and sign them to your hearts content. Some of the features:

* Tiny library,
* No dependencies,
* Works in .Net 3.0+,
* Extends off WebRequest objects,
* Create and sign a request in ~5 lines of code.

Please note, this is for client applications only. You'll need to find something else if you're looking for a server, for example, DevDefined.OAuth.

## You have me so far, give me an example!

Well, OK then!

	var tokens = new Tokens() { ConsumerKey = "key", ConsumerSecret = "secret",
		AccessKey = "key", AccessKeySecret = "secret" };
	var request = WebRequest.Create("https://api.twitter.com/1/statuses/home_timeline.json?count=5");
	request.SignRequest()
		.WithTokens(tokens)
		.InHeader();

Or, you can condense your <code>WithTokens</code> call into the <code>SignRequest</code>

	request.SignRequest(tokens).InHeader();

## By Joe, what have you done!?

Made OAuth simple, and I hope you find it really does.

## What's in here?

You'll find the source above and a solution that's fine for Visual Studio 2010. It is targetting to .Net version 3.0, so if you're using a version of Visual Studio 2008 you can create a new solution, import the project, and you should be good to go.

There are also two examples,

1. SimpleOAuthTester is a console app to test against the OAuth server at http://term.ie/oauth/example/. Rather unexciting.
2. SimpleOAuthTwitter is a console app to test against Twitter itself. You can sign in, post API requests and more to get a feel for what happens on the inside.

Finally, you'll find a Compiled Help file in the Help directory that documents it.

## License

Simple OAuth .Net is is licensed under the terms of the MIT License, see the included LICENSE file.