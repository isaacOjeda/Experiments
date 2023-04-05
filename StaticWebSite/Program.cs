using StatiCSharp;

var myAwesomeWebsite = new Website(
    url: "https://yourdomain.com",
    name: "My Awesome Website",
    description: @"Description of your website",
    language: "en-US",
    sections: "posts, about"            // Select which folders should be treated as sections.
);

var manager = new WebsiteManager(
    website: myAwesomeWebsite,
    source: "."    // Path to the folder of your website project.
);

await manager.Make();