/**PokeIn Comet Ajax Library 0.86 (pokein.codeplex.com) (GPL v2) Copyright © 2010 Oguz Bastemur (info@pokein.com)*/
function PokeIn() {
}

PokeIn.clid = '[$$ClientId$$]';
PokeIn.OnError = null;
PokeIn.Request = 0;
PokeIn.RequestList = [];
PokeIn.ListenUrl = '[$$Listen$$]';
PokeIn.SendUrl = '[$$Send$$]';
PokeIn.ListenCounter = new Date().getTime();
PokeIn.IsConnected = true;
PokeIn.isMozilla = /Firefox/i.test(navigator.userAgent);
PokeIn.isIE = /MSIE/i.test(navigator.userAgent);
PokeIn.isOpera = /Opera/i.test(navigator.userAgent);
PokeIn.isSafari = /Safari/i.test(navigator.userAgent);
PokeIn.ForcePokeInAjax = false;
PokeIn.CometEnabled = true;
PokeIn.FormMethod = "POST";
PokeIn.Secure = true;
PokeIn.ListenActive = false;

PokeIn.SetText = function (e, t) {
    if (e.innerText != null) {
        e.innerText = t;
    }
    else 
    {
        e.textContent = t;
    }
};

PokeIn.ToXML = function (js_class) {
    var XMLString = "";
    if (typeof js_class == "object") {
        if (js_class instanceof Array) {
            for (i = 0, ln = js_class.length; i < ln; i++) {
                XMLString += "<item>" + js_class[i] + "</item>";
            }
        }
        else {
            for (o in js_class) {
                var _end = "</" + o + ">";
                XMLString += "<" + o + ">" + ToXML(js_class[o]) + _end;
            }
        }
    }
    else if (typeof js_class == "string") {
        XMLString = js_class;
    }
    else if (js_class.toString) {
        XMLString = js_class.toString();
    }

    return XMLString;
};

PokeIn.AddEvent = function (_e, _n, _h) {
    if (window.attachEvent) {
        _e.attachEvent("on" + _n, _h);
    }
    else {
        _e.addEventListener(_n, _h, false);
    }
};

PokeIn.GetClientId = function () {
    return PokeIn.clid;
};

PokeIn.Listen = function () {
    if (!PokeIn.IsConnected) {
        return;
    }
    PokeIn.ListenActive = true;
    PokeIn.Request++;
    PokeIn.RequestList[PokeIn.Request] = { status: true, message: "", connector: PokeIn.CreateAjax(PokeIn.Request), is_send: false };
    PokeIn._Send(PokeIn.Request);
};

PokeIn.UnfinishedMessageReceived = function () {
    if (PokeIn.OnError != null) {
        PokeIn.OnError('Unfinished Message Received', false);
    }
}; var _______ = ".(){},@? ][{};&\"'#";

PokeIn.UnfinishedMessageSent = function () {
    if (PokeIn.OnError != null) {
        PokeIn.OnError('Unfinished Message Sent', false);
    }
};

PokeIn.CompilerError = function (message) {
    if (PokeIn.OnError != null) {
        PokeIn.OnError('Compiler Error Received: ' + message, false);
    }
};

PokeIn.ClientObjectsDoesntExist = function () {
    if (PokeIn.OnError != null) {
        PokeIn.OnError('Client Objects Doesnt Exist', true);
    }
};

PokeIn.RepHelper = function (s1, s2, s3) {
    while (s1.indexOf(s2) >= 0) {
        s1 = s1.replace(s2, s3);
    }
    return s1;
};

PokeIn.CreateText = function (mess, _in) {
    var len = PokeIn.clid.length - 1;
    var clide = PokeIn.clid.substr(1, len);

    var le = _______.length, ve = _______;

    if (_in) { 
        for (var i = 0; i < le; i++) {
            mess = PokeIn.RepHelper(mess, ":" + clide + i.toString() + ":", ve.charAt(i));
        }
        mess = PokeIn.RepHelper(mess, '&quot;', '&');
        mess = PokeIn.RepHelper(mess, '&#92;', '\\');
    }
    else {
        mess = PokeIn.RepHelper(mess, '\\', '&#92;');
        for (var i = 0; i < le; i++) {
            mess = PokeIn.RepHelper(mess, ve.charAt(i), ":" + clide + i.toString() + ":");
        }
    }
    if (_in && PokeIn.ForcePokeInAjax) {
        try { eval(mess); } catch (e) { }
    }
    else {
        return mess;
    }
};

PokeIn.StrFix = function (str) {
    str = str.replace(/[&]/g, '&quot;'); 
    str = str.replace(/["]/g, '\\"');
    str = str.replace(/[\\]/g, '&#92;');
    str = '"' + str + '"';
    return str;
};

PokeIn.Send = function (mess) {
    if (!PokeIn.IsConnected) {
        return;
    }
    PokeIn.Request++;
    if (PokeIn.Secure) {
        mess = PokeIn.CreateText(mess, false);
    }

    PokeIn.RequestList[PokeIn.Request] = { status: true, message: mess, connector: PokeIn.CreateAjax(PokeIn.Request), is_send: true };
    PokeIn._Send(PokeIn.Request);
};

PokeIn.Close = function () {
    PokeIn.Send(PokeIn.GetClientId() + '.CometBase.Close();');
};

PokeIn.Closed = function () {
    PokeIn.OnError = null;
    PokeIn.IsConnected = false;
    PokeIn.Started = false;
};

PokeIn.Started = false;
PokeIn.Start = function (_callback_) {
    setTimeout(function () {
        if (PokeIn.Started) {
            return;
        }
        if (!PokeIn.IsConnected) {
            var conn_str = "?";
            if (self.location.href.indexOf("?") > 0) {
                conn_str = "&";
            }
            self.location = self.location + conn_str + "rt=" + PokeIn.ListenCounter;

            if (_callback_ != null) {
                _callback_(false);
            }
            return;
        }
        PokeIn.Started = true;

        if (PokeIn.CometEnabled) {
            PokeIn.Listen();
        }

        if (_callback_ != null) {
            _callback_(true);
        }
    }, 10);
};

PokeIn.HttpRequest = function (id) {
    this.Headers = {};
    this.method = "";
    this.url = "";
    this.async = "";
    this.onreadystatechange = null;
    this.readystate = 0;
    this.status = 0;
    this.parameters = "";
    this.id = id;
};

PokeIn.HttpRequest.prototype.setRequestHeader = function (_type, _value) {
    this.Headers[_type] = _value;
};

PokeIn.HttpRequest.prototype.open = function (_method, _url, _async) {
    this.method = _method;
    this.url = _url;
    this.async = _async;
};

PokeIn.HttpRequest.prototype.send = function (_parameters) {
    this.parameters = _parameters;
    this._element = document.createElement("script");
    this._element.defer = true;
    this._element.id = "s" + this.id;
    var _this = this;
    this._element.onload = function (ev) {
        if (_this.onreadystatechange != null) {
            _this.readystate = 4;
            _this.status = 200;
            _this.responseText = "";
            _this.onreadystatechange();
            delete _this._element.parentNode.removeChild(_this._element);
        }
    };
    this._element.src = this.url + "?ij=1&" + _parameters;
    document.getElementsByTagName("head")[0].appendChild(this._element);
};

PokeIn.CreateAjax = function (id) {
    if (PokeIn.ForcePokeInAjax) {
        return new PokeIn.HttpRequest(id);
    }
    var xmlHttp = null;
    try {
        xmlHttp = new XMLHttpRequest();
    }
    catch (e) {
        try {
            xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
        }
        catch (e) {
            try {
                xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
            }
            catch (e) {
                xmlHttp = new PokeIn.HttpRequest(id);
            }
        }
    }
    return xmlHttp;
};

PokeIn._Send = function (call_id) {
    var txt = [];
    txt.push('c=' + PokeIn.GetClientId());
    var _url = PokeIn.SendUrl;
    if (PokeIn.RequestList[call_id].is_send) {
        txt.push('ms=' + PokeIn.RequestList[call_id].message);
    }
    else {
        _url = PokeIn.ListenUrl;
    }
    txt.push('ce=' + (PokeIn.CometEnabled));
    txt.push('co=' + (PokeIn.ListenCounter++));
    txt.push('sc=' + PokeIn.Secure);
    txt = txt.join('&');
    var xmlHttp = PokeIn.RequestList[call_id].connector;

    xmlHttp.open(PokeIn.FormMethod, _url, true);
    xmlHttp.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
    xmlHttp.setRequestHeader('If-Modified-Since', 'Thu, 6 Mar 1980 00:00:00 GMT');
    xmlHttp.setRequestHeader('Connection', 'close');
    xmlHttp.onreadystatechange = function () {
        if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
            PokeIn.RequestList[call_id].status = false;
            try {
                if (xmlHttp.responseText != "") { 
                    if (PokeIn.Secure) { 
                        eval(PokeIn.CreateText(xmlHttp.responseText, true)); 
                    }
                    else { 
                        eval(xmlHttp.responseText); 
                    } 
                }
            }
            catch (e) {
                if (!PokeIn.ListenActive) {
                    PokeIn.Listen();
                }
                if (PokeIn.OnError != null) {
                    PokeIn.OnError('Ajax Error: ' + xmlHttp.responseText, true);
                    return;
                }
            }
            delete (PokeIn.RequestList[call_id].connector);
            PokeIn.RequestList[call_id].connector = null;
            xmlHttp = null;
        }
    };
    xmlHttp.send(txt);
};