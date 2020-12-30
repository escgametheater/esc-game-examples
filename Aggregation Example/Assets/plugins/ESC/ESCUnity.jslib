mergeInto(LibraryManager.library, {

    JslibVersion: function() {
        console.log("ESCUnity.jslib version 0.0.5");
    },

    JavaScriptAlert: function (str) {
        window.alert(Pointer_stringify(str));
    },

    JavaScriptConsoleLog: function (str) {
        console.log(Pointer_stringify(str));
    },

    JavaScriptSend: function (ConnectionId, ChannelId, sendbuffer, sendbufferLength) {
        if (RelayMessageFromUnity) {
            RelayMessageFromUnity(ConnectionId, ChannelId, Pointer_stringify(sendbuffer, sendbufferLength) );
        } else {
            console.log("RelayMessageFromUnity() not present",ConnectionId, ChannelId, sendbuffer, sendbufferLength);
        }
    },

    JavaScriptStartWebsocketRelay: function() {
        if (StartWebsocketRelay) {
            StartWebsocketRelay();
        } else {
            console.log("StartWebsocketRelay() not present.");
        }
    },

    JavaScriptMessageToHost: function (channelName,message) {
        if (RelayHostMessageToMainProcess) {
            RelayHostMessageToMainProcess(Pointer_stringify(channelName),Pointer_stringify(message));
        }
        else {
            console.log("RelayHostMessageToMainProcess not present",Pointer_stringify(channelName),Pointer_stringify(message));
        }
    },

    JavaScriptDebugger: function () {
        if (window.debugOn)
            debugger;
    },
    JavaScriptDebugOn: function () {
        window.debugOn = true ;
    },
    JavaScriptDebugOff: function () {
        window.debugOn = false ;
    },

    JavaScriptReturnNumber(number) {
        if (!window.unity) window.unity = {};
        window.unity.returnNumber = number ;
    },
    JavaScriptReturnString(ptr) {
        if (!window.unity) window.unity = {};
        window.unity.returnString = Pointer_stringify(ptr) ;
    },
    JavaScriptReturnJsonObject(json) {
        if (!window.unity) window.unity = {};
        window.unity.returnObject = JSON.parse(Pointer_stringify(ptr)) ;
    },

    JavaScriptLoadImage(slug,id) {
        if (LoadImageWithSlug) {
            return LoadImageWithSlug(Pointer_stringify(slug),Pointer_stringify(id));
        }
        else {
            console.log("JavaScriptLoadImage not present", Pointer_stringify(slug), Pointer_stringify(id));
            return "ERROR";
        }
    },
    JavaScriptLoadImageInTexture: function (texture,slug,width,height) {
        if (LoadImageInUnityTexture) {
            return LoadImageInUnityTexture(GLctx,GL.textures[texture],Pointer_stringify(slug),width,height);
        }
        else {
            console.log("LoadImageInUnityTexture not present",GLctx,texture,Pointer_stringify(slug));
            return false ;
        }
    },
    JavaScriptCheckTextureProgress: function (texture) {
        if (CheckTextureProgress) {
            return ReturnString(CheckTextureProgress(GL.textures[texture]));
        } else {
            console.log("CheckTextureProgress not present",texture);
            return ReturnString("unknown");
        }
    },

    ReturnString: function (str) {
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(str, buffer, bufferSize);
        return buffer;
    },

    JavaScriptGetTextureFromElement: function (texture, selector) {
        if (TextureFromElement) {
            TextureFromElement(GLctx,GL.textures[texture],Pointer_stringify(selector));
        }
        else {
            console.log("TextureFromElement not present",GLctx,texture,Pointer_stringify(selector));
        }
    }
});
