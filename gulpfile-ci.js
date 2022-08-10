var gulp = require("gulp");
var gutil = require("gulp-util");
var foreach = require("gulp-foreach");
var rimrafDir = require("rimraf");
var rimraf = require("gulp-rimraf");
var runSequence = require("run-sequence");
var fs = require("fs");
var path = require("path");
var xmlpoke = require("xmlpoke");
var nugetRestore = require('gulp-nuget-restore');
var config = require("./gulpfile.js").config;
var websiteRootBackup = config.websiteRoot;

gulp.task("CI-Nuget-Restore", function (callback) {
    var solution = "./" + config.solutionName + ".sln";
    return gulp.src(solution).pipe(nugetRestore());
});

gulp.task("CI-Frontend", function (callback) {
    runSequence(
         "serve", callback);
});


gulp.task("CI-Publish", function (callback) {
    config.websiteRoot = path.resolve("./Website");
    config.buildConfiguration = "Release";
    fs.mkdirSync(config.websiteRoot);

    runSequence(
        "Publish-Foundation-Projects",
        "Publish-Feature-Projects",
        "Publish-Project-Projects", callback);

});

gulp.task("CI-Prepare-Package-Files", function (callback) {
    var excludeList = [
        config.websiteRoot + "\\bin\\{Sitecore,Lucene,Newtonsoft,System,Microsoft.Web.Infrastructure}*dll",
        config.websiteRoot + "\\compilerconfig.json.defaults",
        config.websiteRoot + "\\packages.config",
        config.websiteRoot + "\\App_Config\\Include\\{Feature,Foundation,Project}\\*Serialization.config",
        config.websiteRoot + "\\App_Config\\Include\\{Feature,Foundation,Project}\\z.*DevSettings.config",
        "!" + config.websiteRoot + "\\bin\\Sitecore.Support*dll",
        "!" + config.websiteRoot + "\\bin\\Sitecore.{Feature,Foundation,Habitat,Demo,Common}*dll"
    ];
    console.log(excludeList);

    return gulp.src(excludeList, { read: false }).pipe(rimraf({ force: true }));
});



gulp.task("CI-Clean", function (callback) {
    rimrafDir.sync(path.resolve("./Website"));
    callback();
});

gulp.task("CI-Do-magic", function (callback) {
    runSequence(
        "CI-Clean",
        "CI-Frontend",
        "CI-Nuget-Restore",
        "CI-Publish",
        "CI-Prepare-Package-Files",
        callback);
});
