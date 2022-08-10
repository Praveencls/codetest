var replace = require('gulp-replace');
var gulp = require("gulp");
var nopt = require("nopt");
var msbuild = require("gulp-msbuild");
var debug = require("gulp-debug");
var foreach = require("gulp-foreach");
var rename = require("gulp-rename");
var newer = require("gulp-newer");
var util = require("gulp-util");
var runSequence = require("run-sequence");
var path = require("path");
var config = require("./gulp-config.js")();
var nugetRestore = require('gulp-nuget-restore');
var fs = require('fs');
var del = require('del');
var cleanCSS = require('gulp-clean-css');
var plumber = require('gulp-plumber');
var concat = require('gulp-concat');
var sourcemaps = require('gulp-sourcemaps');
var sass = require('gulp-sass');
var autoprefixer = require('gulp-autoprefixer');
var browserify = require('browserify');
var source = require('vinyl-source-stream');
var uglify = require('gulp-uglify');
var gutil = require('gulp-util');

module.exports.config = config;

var args = nopt({
    "env": [String, null],
    "path": [String, null]
});


gulp.task("deploy-all-to-folder", ["set-temp-output-folder", "03-Publish-All-Projects"], function (callback) {

});

gulp.task("default", function (callback) {
    config.runCleanBuilds = true;
    return runSequence(
        "01-Copy-Sitecore-Lib",
        "02-Nuget-Restore",
        "03-Publish-All-Projects",
        "04-Apply-Xml-Transform",
        "05-Deploy-Transforms",
        callback);
});

gulp.task("set-temp-output-folder", function (callback) {
    args.path = config.deploymentFolder;
    var tmpDir = args.path;
    if (!fs.existsSync(tmpDir)) {
        fs.mkdirSync(tmpDir);
    }
    if (!fs.existsSync(tmpDir + "/Website")) {
        fs.mkdirSync(tmpDir + "/Website");
    }
    config.rootFolder = path.resolve(tmpDir);
    config.websiteRoot = path.resolve(tmpDir);
    callback();
});


/*****************************
  Initial setup
*****************************/
gulp.task("01-Copy-Sitecore-Lib", function () {
    console.log("Copying Sitecore Libraries");

    fs.statSync(config.sitecoreLibraries);

    var files = config.sitecoreLibraries + "/**/*";

    return gulp.src(files).pipe(gulp.dest("./lib/Sitecore"));
});

gulp.task("02-Nuget-Restore", function (callback) {
    var solution = "./" + config.solutionName + ".sln";
    return gulp.src(solution).pipe(nugetRestore());
});

/* --------------------------------------------------------------
Start GULP TASK Frontend (Serve)
--------------------------------------------------------------- */
/* --------------------------------------------------------------
IMAGES (Optimize Images)
--------------------------------------------------------------- */
gulp.task('images', function() {
    return gulp.src('./src/Project/Website/code/assets/client/images/src/**/*')
        .pipe(gulp.dest('./src/Project/Website/code/dist/images'))
		
        //.pipe(notify({ message: "Images compressed."}) )
});

gulp.task('vendor', function () {    
    return gulp.src(['./node_modules/jquery/dist/jquery.min.js','./node_modules/bootstrap/dist/js/bootstrap.min.js', './node_modules/popper.js/dist/popper.js', 'node_modules/tether/dist/js/tether.min.js'])
    .pipe(gulp.dest('./src/Project/Website/code/dist/scripts'));
});

/* --------------------------------------------------------------
CSS (Minify CSS)
--------------------------------------------------------------- */
gulp.task('minify-css', ['font-awesome:fonts', 'font-awesome:css', 'css'], function() {
    return gulp.src('./src/Project/Website/code/dist/styles/main.css')
        .pipe(cleanCSS({ compatibility: '*' }))
        .pipe(concat('main.min.css'))
        //.pipe(notify({ message: "CSS Minified."}) )
        .pipe(gulp.dest('./src/Project/Website/code/dist/styles/'));
});

/* --------------------------------------------------------------
Font-Awesome (Copy)
--------------------------------------------------------------- */
gulp.task('font-awesome:fonts', function() {
    return gulp.src('./src/Project/Website/code/assets/client/css/libs/font-awesome/fonts/**/*')
        .pipe(gulp.dest('./src/Project/Website/code/dist/fonts'));
});

gulp.task('font-awesome:css', function() {
    return gulp.src('./src/Project/Website/code/assets/client/css/libs/font-awesome/css/font-awesome.min.css')
        .pipe(gulp.dest('./src/Project/Website/code/dist/styles'));
});

/* --------------------------------------------------------------
CSS (Compile CSS, Add Sourcemaps, Autoprefix)
--------------------------------------------------------------- */
gulp.task('css', ['font-awesome:fonts', 'font-awesome:css'], function() {
    return gulp.src('./src/Project/Website/code/assets/client/css/**/*.scss')
        .pipe(plumber())
        .pipe(sourcemaps.init())
        .pipe(sass().on('error', sass.logError))
        .pipe(plumber())
        .pipe(autoprefixer({
            browsers: ['last 2 versions', 'ie >= 9']
        }))
        .pipe(concat('main.css'))
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest('./src/Project/Website/code/dist/styles'));
        
});

// Compile sass into CSS & auto-inject into browsers
gulp.task('sass', function() {
    return gulp.src(['node_modules/bootstrap/scss/bootstrap.scss', 'src/scss/*.scss'])
        .pipe(sass())
        .pipe(gulp.dest("src/css"));
        
});

gulp.task('js', function() {
    browserify('./src/Project/Website/code/assets/client/js/main.js')
        .bundle()
        .on('error', function(e) {
            gutil.log(e);
        })
        .pipe(source('client.bundle.js'))
        .pipe(gulp.dest('./src/Project/Website/code/dist/scripts'));
        //.pipe(notify({ message: "JS bundled."}) )
       
});


gulp.task('compress', ['js'], function() {
    return gulp.src('./src/Project/Website/code/assets/client/js/dist/client.bundle.js')
        .pipe(uglify())
        .pipe(concat('client.bundle.min.js'))
        .pipe(gulp.dest('./src/Project/Website/code/dist/js'))
        //.pipe(notify({ message: "JS compressed."}) )
});
/* --------------------------------------------------------------
Clean
--------------------------------------------------------------- */
gulp.task('clean', function() {
    return del(["./src/Project/Website/code/dist/"])
});

gulp.task('serve', ['clean'], function() {
    gulp.start('images', 'vendor', 'minify-css', 'compress');
});

/* --------------------------------------------------------------
End GULP TASK Frontend (Serve)
--------------------------------------------------------------- */

gulp.task("03-Publish-All-Projects", function (callback) {
    return runSequence(
        "Publish-Foundation-Projects",
        "Publish-Feature-Projects",
        "Publish-Project-Projects", callback);
});

gulp.task("04-Apply-Xml-Transform", function () {
    var layerPathFilters = ["./src/Foundation/**/*.transform", "./src/Feature/**/*.transform", "./src/Project/**/*.transform", "!./src/**/obj/**/*.transform", "!./src/**/bin/**/*.transform"];
    return gulp.src(layerPathFilters)
        .pipe(foreach(function (stream, file) {
            var fileToTransform = file.path.replace(/.+code\\(.+)\.transform/, "$1");
            util.log("Applying configuration transform: " + file.path);
            return gulp.src("./applytransform.targets")
                .pipe(msbuild({
                    targets: ["ApplyTransform"],
                    configuration: config.buildConfiguration,
                    logCommand: false,
                    verbosity: "normal",
                    maxcpucount: 0,
                    toolsVersion: 15.0,
                    properties: {
                        WebConfigToTransform: config.websiteRoot,
                        TransformFile: file.path,
                        FileToTransform: fileToTransform,
                        VisualStudioVersion: "15.0"
                    }
                }));
        }));
});

//gulp.task("05-Sync-Unicorn", function (callback) {
//    var options = {};
//    options.siteHostName = habitat.getSiteUrl();
//    options.authenticationConfigFile = config.websiteRoot + "/App_config/Include/Unicorn/Unicorn.UI.config";

//    unicorn(function () { return callback() }, options);
//});


gulp.task("05-Deploy-Transforms", function () {
    return gulp.src("./src/**/code/**/*.transform")
        .pipe(gulp.dest(config.websiteRoot + "/temp/transforms"));
});

/*****************************
  Copy assemblies to all local projects
*****************************/
gulp.task("Copy-Local-Assemblies", function () {
    console.log("Copying site assemblies to all local projects");
    var files = config.sitecoreLibraries + "/**/*";

    var root = "./src";
    var projects = root + "/**/code/bin";
    return gulp.src(projects, { base: root })
        .pipe(foreach(function (stream, file) {
            console.log("copying to " + file.path);
            gulp.src(files)
                .pipe(gulp.dest(file.path));
            return stream;
        }));
});

/*****************************
  Publish
*****************************/
var publishProjects = function (location, dest) {
    dest = dest || config.websiteRoot;
    var targets = ["Build"];
    if (config.runCleanBuilds) {
        targets = ["Clean", "Build"]
    }
    console.log("publish to " + dest + " folder");
    return gulp.src([location + "/**/code/*.csproj"])
        .pipe(foreach(function (stream, file) {
            return stream
                .pipe(debug({ title: "Building project:" }))
                .pipe(msbuild({
                    targets: targets,
                    configuration: config.buildConfiguration,
                    logCommand: false,
                    verbosity: "minimal",
                    stdout: true,
                    maxcpucount: 0,
                    toolsVersion: 15.0,
                    properties: {
                        DeployOnBuild: "true",
                        DeployDefaultTarget: "WebPublish",
                        WebPublishMethod: "FileSystem",
                        DeleteExistingFiles: "false",
                        publishUrl: dest,
                        _FindDependencies: "false",
                        MSDeployUseChecksum: "true"
                    }
                }));
        }));
};

gulp.task("Publish-Foundation-Projects", function () {
    return publishProjects("./src/Foundation");
});

gulp.task("Publish-Feature-Projects", function () {
    return publishProjects("./src/Feature");
});

gulp.task("Publish-Project-Projects", function () {
    return publishProjects("./src/Project");
});

gulp.task("Publish-Assemblies", function () {
    var root = "./src";
    var binFiles = root + "/**/code/**/bin/Sitecore.{Feature,Foundation}.*.{dll,pdb}";
    var destination = config.websiteRoot + "/bin/";
    return gulp.src(binFiles, { base: root })
        .pipe(rename({ dirname: "" }))
        .pipe(newer(destination))
        .pipe(debug({ title: "Copying " }))
        .pipe(gulp.dest(destination));
});

gulp.task("Publish-All-Views", function () {
    var root = "./src";
    var roots = [root + "/**/Views", "!" + root + "/**/obj/**/Views"];
    var files = "/**/*.cshtml";
    var destination = config.websiteRoot + "\\Views";
    return gulp.src(roots, { base: root }).pipe(
        foreach(function (stream, file) {
            console.log("Publishing from " + file.path);
            gulp.src(file.path + files, { base: file.path })
                .pipe(newer(destination))
                .pipe(debug({ title: "Copying " }))
                .pipe(gulp.dest(destination));
            return stream;
        })
    );
});

gulp.task("Publish-All-Configs", function () {
    var root = "./src";
    var roots = [root + "/**/App_Config", "!" + root + "/**/obj/**/App_Config"];
    var files = "/**/*.config";
    var destination = config.websiteRoot + "\\App_Config";
    return gulp.src(roots, { base: root }).pipe(
        foreach(function (stream, file) {
            console.log("Publishing from " + file.path);
            gulp.src(file.path + files, { base: file.path })
                .pipe(newer(destination))
                .pipe(debug({ title: "Copying " }))
                .pipe(gulp.dest(destination));
            return stream;
        })
    );
});

/*****************************
 Watchers
*****************************/
gulp.task("Auto-Publish-Css", function () {
    var root = "./src";
    var roots = [root + "/**/styles", "!" + root + "/**/obj/**/styles"];
    var files = "/**/*.css";
    var destination = config.websiteRoot + "\\styles";
    gulp.src(roots, { base: root }).pipe(
        foreach(function (stream, rootFolder) {
            gulp.watch(rootFolder.path + files, function (event) {
                if (event.type === "changed") {
                    console.log("publish this file " + event.path);
                    gulp.src(event.path, { base: rootFolder.path }).pipe(gulp.dest(destination));
                }
                console.log("published " + event.path);
            });
            return stream;
        })
    );
});

gulp.task("Auto-Publish-Views", function () {
    var root = "./src";
    var roots = [root + "/**/Views", "!" + root + "/**/obj/**/Views"];
    var files = "/**/*.cshtml";
    var destination = config.websiteRoot + "\\Views";
    gulp.src(roots, { base: root }).pipe(
        foreach(function (stream, rootFolder) {
            gulp.watch(rootFolder.path + files, function (event) {
                if (event.type === "changed") {
                    console.log("publish this file " + event.path);
                    gulp.src(event.path, { base: rootFolder.path }).pipe(gulp.dest(destination));
                }
                console.log("published " + event.path);
            });
            return stream;
        })
    );
});

gulp.task("Auto-Publish-Fonts", function () {
    var root = "./src";
    var roots = [root + "/**/fonts", "!" + root + "/**/obj/**/fonts"];
    var files = "/**/*.*";
    var destination = config.websiteRoot + "\\dist\fonts";
    gulp.src(roots, { base: root }).pipe(
        foreach(function (stream, rootFolder) {
            gulp.watch(rootFolder.path + files, function (event) {
                if (event.type === "changed") {
                    console.log("publish this file " + event.path);
                    gulp.src(event.path, { base: rootFolder.path }).pipe(gulp.dest(destination));
                }
                console.log("published " + event.path);
            });
            return stream;
        })
    );
});


gulp.task("Auto-Publish-Assemblies", function () {
    var root = "./src";
    var roots = [root + "/**/code/**/bin"];
    var files = "/**/Sitecore.{Feature,Foundation}.*.{dll,pdb}";
    var destination = config.websiteRoot + "/bin/";
    gulp.src(roots, { base: root }).pipe(
        foreach(function (stream, rootFolder) {
            gulp.watch(rootFolder.path + files, function (event) {
                if (event.type === "changed") {
                    console.log("publish this file " + event.path);
                    gulp.src(event.path, { base: rootFolder.path }).pipe(gulp.dest(destination));
                }
                console.log("published " + event.path);
            });
            return stream;
        })
    );
});