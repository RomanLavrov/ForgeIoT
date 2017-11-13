var viewerApp;
var viewables;
var indexViewable;

var options = {
    env: 'AutodeskProduction',
    getAccessToken: getAccessToken,
    refreshToken: getAccessToken
};

function getAccessToken() {
    var xmlHttp = null;
    xmlHttp = new XMLHttpRequest();
    xmlHttp.open("Get", 'api/forge/token', false);
    xmlHttp.send(null);
    return xmlHttp.responseText;
}

function initApplication(urn) {    
    var documentId = 'urn:' + urn;
    var config3d = { extensions: ['SmokeDetectorsExtension'] };
    Autodesk.Viewing.Initializer(options, function onInitialized() {
        viewerApp = new Autodesk.Viewing.ViewingApplication('MyViewerDiv');
        viewerApp.registerViewer(viewerApp.k3D, Autodesk.Viewing.Private.GuiViewer3D, config3d);
        viewerApp.loadDocument(documentId, onDocumentLoadSuccess, onDocumentLoadFailure);
    });
}

function onDocumentLoadSuccess(doc) {

    // We could still make use of Document.getSubItemsWithProperties()
    // However, when using a ViewingApplication, we have access to the **bubble** attribute,
    // which references the root node of a graph that wraps each object from the Manifest JSON.
    viewables = viewerApp.bubble.search({ 'type': 'geometry' });
    if (viewables.length === 0) {
        console.error('Document contains no viewables.');
        return;
    }

    // Choose any of the avialble viewables
    viewerApp.selectItem(viewables[0].data, onItemLoadSuccess, onItemLoadFail);
}

function onDocumentLoadFailure(viewerErrorCode) {
    console.error('onDocumentLoadFailure() - errorCode:' + viewerErrorCode);
}

function onItemLoadSuccess(viewer, item) {
    console.log('onItemLoadSuccess()!');
    console.log(viewer);
    console.log(item);

    // Congratulations! The viewer is now ready to be used.
    console.log('Viewers are equal: ' + (viewer === viewerApp.getCurrentViewer()));
}

function onItemLoadFail(errorCode) {
    console.error('onItemLoadFail() - errorCode:' + errorCode);
}

function loadNextModel() {
    // Next viewable index. Loop back to 0 when overflown.
    indexViewable = (indexViewable + 1) % viewables.length;
    viewerApp.selectItem(viewables[indexViewable].data, onItemLoadSuccess, onItemLoadFail);
}


