module.exports = function () {
    var instanceRoot = "C:\\inetpub\\wwwroot\\AllegroMicroSystems.local";
    var config = {
        websiteRoot: instanceRoot + "\\Website",
        sitecoreLibraries: instanceRoot +  "\\Website\\bin",
        solutionName: "AllegroMicroSystems",
        buildConfiguration: "Debug",
        runCleanBuilds: false,
        deploymentFolder: "c:\\temp"
    }
    return config;
}