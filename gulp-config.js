module.exports = function () {
    var instanceRoot = "C:\\Deploy";
    var config = {
        websiteRoot: instanceRoot + "\\Website",
        sitecoreLibraries: instanceRoot +  "\\Website\\bin",
        solutionName: "CodeTest",
        buildConfiguration: "Debug",
        runCleanBuilds: false,
        deploymentFolder: "c:\\temp"
    }
    return config;
}