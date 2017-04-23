(function e(t,n,r){function s(o,u){if(!n[o]){if(!t[o]){var a=typeof require=="function"&&require;if(!u&&a)return a(o,!0);if(i)return i(o,!0);var f=new Error("Cannot find module '"+o+"'");throw f.code="MODULE_NOT_FOUND",f}var l=n[o]={exports:{}};t[o][0].call(l.exports,function(e){var n=t[o][1][e];return s(n?n:e)},l,l.exports,e,t,n,r)}return n[o].exports}var i=typeof require=="function"&&require;for(var o=0;o<r.length;o++)s(r[o]);return s})({1:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var ReactDOM = require("react-dom");
var root_1 = require("./root");
ReactDOM.render(React.createElement(root_1.Root, null), document.getElementById('root'));
},{"./root":6,"react":"react","react-dom":"react-dom"}],2:[function(require,module,exports){
"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var reactstrap_1 = require("reactstrap");
var react_router_dom_1 = require("react-router-dom");
var Home = (function (_super) {
    __extends(Home, _super);
    function Home() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Home.prototype.render = function () {
        return (React.createElement("div", { className: "container" },
            React.createElement(reactstrap_1.Jumbotron, null,
                React.createElement("h1", { className: "display-3" }, "SJA Tracker"),
                React.createElement("p", { className: "lead" }, "Welcome to the St Ambulance SWR Central District Tracker"),
                React.createElement(react_router_dom_1.Link, { to: "/app/login", className: "btn btn-lg btn-sja-dark-green" }, "Log in \u00BB")),
            React.createElement(reactstrap_1.CardDeck, { className: "front-page" },
                React.createElement(reactstrap_1.Card, null,
                    React.createElement(reactstrap_1.CardBlock, null,
                        React.createElement(reactstrap_1.CardTitle, null, "New Users"),
                        React.createElement(reactstrap_1.CardText, null, "If you have never used this site before, you will need to be issued with a username and password."),
                        React.createElement(react_router_dom_1.Link, { to: "/Contact", className: "btn btn-secondary btn-block" }, "Request Account \u00BB"))),
                React.createElement(reactstrap_1.Card, null,
                    React.createElement(reactstrap_1.CardBlock, null,
                        React.createElement(reactstrap_1.CardTitle, null, "Get the Trackers"),
                        React.createElement(reactstrap_1.CardText, null, "There are a set of apps to enable your phone to be tracked from this website."),
                        React.createElement(react_router_dom_1.Link, { to: "", className: "btn btn-secondary btn-block disabled", disabled: true }, "Coming Soon"))),
                React.createElement(reactstrap_1.Card, null,
                    React.createElement(reactstrap_1.CardBlock, null,
                        React.createElement(reactstrap_1.CardTitle, null, "Privacy Policy"),
                        React.createElement(reactstrap_1.CardText, null, "To get a better idea of how this website is used and how it can be made better, we collect some information about what you do with this site."),
                        React.createElement(react_router_dom_1.Link, { to: "/Policies", className: "btn btn-secondary btn-block" }, "Find out More \u00BB"))))));
    };
    return Home;
}(React.Component));
exports.Home = Home;
},{"react":"react","react-router-dom":"react-router-dom","reactstrap":"reactstrap"}],3:[function(require,module,exports){
"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var reactstrap_1 = require("reactstrap");
var react_router_dom_1 = require("react-router-dom");
var LoginBox = (function (_super) {
    __extends(LoginBox, _super);
    function LoginBox(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            username: "",
            password: ""
        };
        return _this;
    }
    LoginBox.prototype.updateUsername = function (event) {
        this.setState({ username: event.target.value });
    };
    LoginBox.prototype.updatePassword = function (event) {
        this.setState({ password: event.target.value });
    };
    LoginBox.prototype.submitForm = function (event) {
        event.preventDefault();
        this.props.onLogin(this.state.username, this.state.password);
    };
    LoginBox.prototype.render = function () {
        var _this = this;
        return (React.createElement("div", { className: "container" },
            React.createElement("div", { className: "d-flex justify-content-center" },
                React.createElement(reactstrap_1.Card, { className: "login-card" },
                    React.createElement(reactstrap_1.CardBlock, null,
                        React.createElement(reactstrap_1.CardTitle, null, "Log In"),
                        React.createElement(reactstrap_1.Form, null,
                            React.createElement(reactstrap_1.FormGroup, null,
                                React.createElement(reactstrap_1.InputGroup, null,
                                    React.createElement(reactstrap_1.InputGroupAddon, null,
                                        React.createElement("span", { className: "fa fa-fw fa-at" })),
                                    React.createElement(reactstrap_1.Input, { type: "email", name: "Email", id: "email", placeholder: "Email", value: this.state.username, onChange: function (e) { return _this.updateUsername(e); } }))),
                            React.createElement(reactstrap_1.FormGroup, null,
                                React.createElement(reactstrap_1.InputGroup, null,
                                    React.createElement(reactstrap_1.InputGroupAddon, null,
                                        React.createElement("span", { className: "fa fa-fw fa-lock" })),
                                    React.createElement(reactstrap_1.Input, { type: "password", name: "Password", id: "password", placeholder: "Password", value: this.state.password, onChange: function (e) { return _this.updatePassword(e); } }))),
                            React.createElement(reactstrap_1.Button, { type: "submit", className: "btn-block", onClick: function (e) { return _this.submitForm(e); } }, "Log in"),
                            React.createElement(react_router_dom_1.Link, { to: "/app/reset-password" }, "Forgotten your password?")))))));
    };
    return LoginBox;
}(React.Component));
exports.LoginBox = LoginBox;
},{"react":"react","react-router-dom":"react-router-dom","reactstrap":"reactstrap"}],4:[function(require,module,exports){
"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t;
    return { next: verb(0), "throw": verb(1), "return": verb(2) };
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = y[op[0] & 2 ? "return" : op[0] ? "throw" : "next"]) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [0, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var login_box_1 = require("./login-box");
var Login = (function (_super) {
    __extends(Login, _super);
    function Login() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Login.prototype.render = function () {
        var _this = this;
        return (React.createElement(login_box_1.LoginBox, { onLogin: function (u, s) { return _this.login(u, s); } }));
    };
    Login.prototype.login = function (username, password) {
        return __awaiter(this, void 0, void 0, function () {
            var headers, result;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        headers = new Headers();
                        headers.append("Content-Type", "application/json");
                        return [4 /*yield*/, fetch("/token", {
                                method: 'POST',
                                body: JSON.stringify({
                                    username: username, password: password
                                }),
                                headers: headers
                            })];
                    case 1:
                        result = _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    return Login;
}(React.Component));
exports.Login = Login;
},{"./login-box":3,"react":"react"}],5:[function(require,module,exports){
"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var reactstrap_1 = require("reactstrap");
var react_router_dom_1 = require("react-router-dom");
var MainNav = (function (_super) {
    __extends(MainNav, _super);
    function MainNav(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            isOpen: false
        };
        return _this;
    }
    MainNav.prototype.toggle = function () {
        this.setState(function (prev, props) { return ({
            isOpen: !prev.isOpen
        }); });
    };
    MainNav.prototype.render = function () {
        var _this = this;
        var loginElement;
        if (this.props.authenticated) {
            loginElement = [
                React.createElement(reactstrap_1.NavItem, { key: "hello" },
                    React.createElement(react_router_dom_1.Link, { className: "nav-link", to: "/profile" },
                        "Hello ",
                        this.props.name)),
                React.createElement(reactstrap_1.NavItem, { key: "logout" },
                    React.createElement(reactstrap_1.NavLink, { href: "/", onClick: function () { return _this.props.onLogout(); } }, "Logout"))
            ];
        }
        else {
            loginElement = React.createElement(reactstrap_1.NavItem, null,
                React.createElement(react_router_dom_1.Link, { className: "nav-link", to: "/app/login" }, "Log in"));
        }
        return (React.createElement(reactstrap_1.Navbar, { color: "sja-dark-green", inverse: true, toggleable: true },
            React.createElement(reactstrap_1.NavbarToggler, { right: true, onClick: function () { return _this.toggle(); } }),
            React.createElement(reactstrap_1.NavbarBrand, { href: "/" }, "SJA Tracker"),
            React.createElement(reactstrap_1.Collapse, { isOpen: this.state.isOpen, navbar: true },
                React.createElement(reactstrap_1.Nav, { navbar: true },
                    React.createElement(reactstrap_1.NavItem, null,
                        React.createElement(react_router_dom_1.Link, { className: "nav-link", to: "/" }, "Home")),
                    React.createElement(reactstrap_1.NavItem, null,
                        React.createElement(react_router_dom_1.Link, { className: "nav-link", to: "/Map" }, "Map")),
                    React.createElement(reactstrap_1.NavItem, null,
                        React.createElement(react_router_dom_1.Link, { className: "nav-link", to: "/Contact" }, "Contact"))),
                React.createElement(reactstrap_1.Nav, { navbar: true, className: "ml-auto" }, loginElement))));
    };
    return MainNav;
}(React.Component));
exports.MainNav = MainNav;
},{"react":"react","react-router-dom":"react-router-dom","reactstrap":"reactstrap"}],6:[function(require,module,exports){
"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t;
    return { next: verb(0), "throw": verb(1), "return": verb(2) };
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = y[op[0] & 2 ? "return" : op[0] ? "throw" : "next"]) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [0, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var navbar_1 = require("./navbar");
var home_1 = require("./home/home");
var login_1 = require("./login/login");
var react_router_dom_1 = require("react-router-dom");
var Cookies = require("js-cookie");
var Root = (function (_super) {
    __extends(Root, _super);
    function Root(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            authenticated: true
        };
        _this.checkAuthentication();
        return _this;
    }
    Root.prototype.checkAuthentication = function () {
        return __awaiter(this, void 0, void 0, function () {
            var result, whoAmI;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, fetch('/api/account/whoami', {
                            credentials: 'same-origin'
                        })];
                    case 1:
                        result = _a.sent();
                        return [4 /*yield*/, result.json()];
                    case 2:
                        whoAmI = _a.sent();
                        this.setState({
                            authenticated: whoAmI.authenticated,
                            name: whoAmI.realName
                        });
                        return [2 /*return*/];
                }
            });
        });
    };
    Root.prototype.logOut = function () {
        return __awaiter(this, void 0, void 0, function () {
            var headers, result;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        headers = new Headers();
                        headers.append("RequestVerificationToken", Cookies.get('XSRF-TOKEN'));
                        return [4 /*yield*/, fetch('/account/logout', {
                                credentials: 'same-origin',
                                method: 'POST',
                                headers: headers
                            })];
                    case 1:
                        result = _a.sent();
                        location.reload(true);
                        return [2 /*return*/];
                }
            });
        });
    };
    Root.prototype.render = function () {
        var _this = this;
        return (React.createElement(react_router_dom_1.BrowserRouter, null,
            React.createElement("div", null,
                React.createElement(navbar_1.MainNav, { name: this.state.name, authenticated: this.state.authenticated, onLogout: function () { return _this.logOut(); } }),
                React.createElement(react_router_dom_1.Route, { exact: true, path: '/', component: home_1.Home }),
                React.createElement(react_router_dom_1.Route, { exact: true, path: '/app/login', component: login_1.Login }))));
    };
    return Root;
}(React.Component));
exports.Root = Root;
},{"./home/home":2,"./login/login":4,"./navbar":5,"js-cookie":"js-cookie","react":"react","react-router-dom":"react-router-dom"}]},{},[1])
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm5vZGVfbW9kdWxlcy9icm93c2VyLXBhY2svX3ByZWx1ZGUuanMiLCJzcmMvanMvYXBwLnRzeCIsInNyYy9qcy9ob21lL2hvbWUudHN4Iiwic3JjL2pzL2xvZ2luL2xvZ2luLWJveC50c3giLCJzcmMvanMvbG9naW4vbG9naW4udHN4Iiwic3JjL2pzL25hdmJhci50c3giLCJzcmMvanMvcm9vdC50c3giXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7OztBQ0FDLDZCQUErQjtBQUNoQyxvQ0FBc0M7QUFDdEMsK0JBQThCO0FBRTlCLFFBQVEsQ0FBQyxNQUFNLENBQUMsb0JBQUMsV0FBSSxPQUFHLEVBQUUsUUFBUSxDQUFDLGNBQWMsQ0FBQyxNQUFNLENBQUMsQ0FBQyxDQUFDOzs7Ozs7Ozs7Ozs7OztBQ0oxRCw2QkFBK0I7QUFFaEMseUNBQStGO0FBQy9GLHFEQUE2RDtBQUs3RDtJQUEwQix3QkFBc0M7SUFBaEU7O0lBMENBLENBQUM7SUF6Q0cscUJBQU0sR0FBTjtRQUNJLE1BQU0sQ0FBQyxDQUNILDZCQUFLLFNBQVMsRUFBQyxXQUFXO1lBQ3RCLG9CQUFDLHNCQUFTO2dCQUNOLDRCQUFJLFNBQVMsRUFBQyxXQUFXLGtCQUFpQjtnQkFDMUMsMkJBQUcsU0FBUyxFQUFDLE1BQU0sK0RBQTZEO2dCQUNoRixvQkFBQyx1QkFBSSxJQUFDLEVBQUUsRUFBQyxZQUFZLEVBQUMsU0FBUyxFQUFDLCtCQUErQixvQkFBZ0IsQ0FDdkU7WUFFWixvQkFBQyxxQkFBUSxJQUFDLFNBQVMsRUFBQyxZQUFZO2dCQUM1QixvQkFBQyxpQkFBSTtvQkFDRCxvQkFBQyxzQkFBUzt3QkFDTixvQkFBQyxzQkFBUyxvQkFBc0I7d0JBQ2hDLG9CQUFDLHFCQUFRLDRHQUVNO3dCQUNmLG9CQUFDLHVCQUFJLElBQUMsRUFBRSxFQUFDLFVBQVUsRUFBQyxTQUFTLEVBQUMsNkJBQTZCLDZCQUF5QixDQUM1RSxDQUNUO2dCQUNQLG9CQUFDLGlCQUFJO29CQUNELG9CQUFDLHNCQUFTO3dC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QUEsaUJBU0M7UUFSRyxNQUFNLENBQUMsQ0FDSCxvQkFBQyxnQ0FBTTtZQUNIO2dCQUNJLG9CQUFDLGdCQUFPLElBQUMsSUFBSSxFQUFFLElBQUksQ0FBQyxLQUFLLENBQUMsSUFBSSxFQUFFLGFBQWEsRUFBRSxJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsRUFBRSxRQUFRLEVBQUUsY0FBTSxPQUFBLEtBQUksQ0FBQyxNQUFNLEVBQUUsRUFBYixDQUFhLEdBQUk7Z0JBQzFHLG9CQUFDLHdCQUFLLElBQUMsS0FBSyxRQUFDLElBQUksRUFBQyxHQUFHLEVBQUMsU0FBUyxFQUFFLFdBQUksR0FBSTtnQkFDekMsb0JBQUMsd0JBQUssSUFBQyxLQUFLLFFBQUMsSUFBSSxFQUFDLFlBQVksRUFBQyxTQUFTLEVBQUUsYUFBSyxHQUFJLENBQ2pELENBQ0QsQ0FBQyxDQUFDO0lBQ25CLENBQUM7SUFDTCxXQUFDO0FBQUQsQ0FoREEsQUFnREMsQ0FoRHlCLEtBQUssQ0FBQyxTQUFTLEdBZ0R4QztBQWhEWSxvQkFBSSIsImZpbGUiOiJnZW5lcmF0ZWQuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlc0NvbnRlbnQiOlsiKGZ1bmN0aW9uIGUodCxuLHIpe2Z1bmN0aW9uIHMobyx1KXtpZighbltvXSl7aWYoIXRbb10pe3ZhciBhPXR5cGVvZiByZXF1aXJlPT1cImZ1bmN0aW9uXCImJnJlcXVpcmU7aWYoIXUmJmEpcmV0dXJuIGEobywhMCk7aWYoaSlyZXR1cm4gaShvLCEwKTt2YXIgZj1uZXcgRXJyb3IoXCJDYW5ub3QgZmluZCBtb2R1bGUgJ1wiK28rXCInXCIpO3Rocm93IGYuY29kZT1cIk1PRFVMRV9OT1RfRk9VTkRcIixmfXZhciBsPW5bb109e2V4cG9ydHM6e319O3Rbb11bMF0uY2FsbChsLmV4cG9ydHMsZnVuY3Rpb24oZSl7dmFyIG49dFtvXVsxXVtlXTtyZXR1cm4gcyhuP246ZSl9LGwsbC5leHBvcnRzLGUsdCxuLHIpfXJldHVybiBuW29dLmV4cG9ydHN9dmFyIGk9dHlwZW9mIHJlcXVpcmU9PVwiZnVuY3Rpb25cIiYmcmVxdWlyZTtmb3IodmFyIG89MDtvPHIubGVuZ3RoO28rKylzKHJbb10pO3JldHVybiBzfSkiLCLvu79pbXBvcnQgKiBhcyBSZWFjdCBmcm9tICdyZWFjdCc7XHJcbmltcG9ydCAqIGFzIFJlYWN0RE9NIGZyb20gJ3JlYWN0LWRvbSc7XHJcbmltcG9ydCB7IFJvb3QgfSBmcm9tICcuL3Jvb3QnO1xyXG5cclxuUmVhY3RET00ucmVuZGVyKDxSb290IC8+LCBkb2N1bWVudC5nZXRFbGVtZW50QnlJZCgncm9vdCcpKTtcclxuIiwi77u/aW1wb3J0ICogYXMgUmVhY3QgZnJvbSAncmVhY3QnO1xyXG5cclxuaW1wb3J0IHsgSnVtYm90cm9uLCBCdXR0b24sIENhcmQsIENhcmRCbG9jaywgQ2FyZFRpdGxlLCBDYXJkVGV4dCwgQ2FyZERlY2sgfSBmcm9tICdyZWFjdHN0cmFwJztcclxuaW1wb3J0IHsgTGluaywgUm91dGVDb21wb25lbnRQcm9wcyB9IGZyb20gJ3JlYWN0LXJvdXRlci1kb20nO1xyXG5cclxuaW50ZXJmYWNlIElIb21lUHJvcHMgZXh0ZW5kcyBSb3V0ZUNvbXBvbmVudFByb3BzPGFueT4ge1xyXG59XHJcblxyXG5leHBvcnQgY2xhc3MgSG9tZSBleHRlbmRzIFJlYWN0LkNvbXBvbmVudDxJSG9tZVByb3BzLCB1bmRlZmluZWQ+IHtcclxuICAgIHJlbmRlcigpIHtcclxuICAgICAgICByZXR1cm4gKFxyXG4gICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImNvbnRhaW5lclwiPlxyXG4gICAgICAgICAgICAgICAgPEp1bWJvdHJvbj5cclxuICAgICAgICAgICAgICAgICAgICA8aDEgY2xhc3NOYW1lPVwiZGlzcGxheS0zXCI+U0pBIFRyYWNrZXI8L2gxPlxyXG4gICAgICAgICAgICAgICAgICAgIDxwIGNsYXNzTmFtZT1cImxlYWRcIj5XZWxjb21lIHRvIHRoZSBTdCBBbWJ1bGFuY2UgU1dSIENlbnRyYWwgRGlzdHJpY3QgVHJhY2tlcjwvcD5cclxuICAgICAgICAgICAgICAgICAgICA8TGluayB0bz1cIi9hcHAvbG9naW5cIiBjbGFzc05hbWU9XCJidG4gYnRuLWxnIGJ0bi1zamEtZGFyay1ncmVlblwiPkxvZyBpbiDCuzwvTGluaz5cclxuICAgICAgICAgICAgICAgIDwvSnVtYm90cm9uPlxyXG5cclxuICAgICAgICAgICAgICAgIDxDYXJkRGVjayBjbGFzc05hbWU9XCJmcm9udC1wYWdlXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgPENhcmQ+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxDYXJkQmxvY2s+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8Q2FyZFRpdGxlPk5ldyBVc2VyczwvQ2FyZFRpdGxlPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPENhcmRUZXh0PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIElmIHlvdSBoYXZlIG5ldmVyIHVzZWQgdGhpcyBzaXRlIGJlZm9yZSwgeW91IHdpbGwgbmVlZCB0byBiZSBpc3N1ZWQgd2l0aCBhIHVzZXJuYW1lIGFuZCBwYXNzd29yZC5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L0NhcmRUZXh0PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPExpbmsgdG89XCIvQ29udGFjdFwiIGNsYXNzTmFtZT1cImJ0biBidG4tc2Vjb25kYXJ5IGJ0bi1ibG9ja1wiPlJlcXVlc3QgQWNjb3VudCDCuzwvTGluaz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPC9DYXJkQmxvY2s+XHJcbiAgICAgICAgICAgICAgICAgICAgPC9DYXJkPlxyXG4gICAgICAgICAgICAgICAgICAgIDxDYXJkPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8Q2FyZEJsb2NrPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPENhcmRUaXRsZT5HZXQgdGhlIFRyYWNrZXJzPC9DYXJkVGl0bGU+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8Q2FyZFRleHQ+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgVGhlcmUgYXJlIGEgc2V0IG9mIGFwcHMgdG8gZW5hYmxlIHlvdXIgcGhvbmUgdG8gYmUgdHJhY2tlZCBmcm9tIHRoaXMgd2Vic2l0ZS5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L0NhcmRUZXh0PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPExpbmsgdG89XCJcIiBjbGFzc05hbWU9XCJidG4gYnRuLXNlY29uZGFyeSBidG4tYmxvY2sgZGlzYWJsZWRcIiBkaXNhYmxlZD5Db21pbmcgU29vbjwvTGluaz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPC9DYXJkQmxvY2s+XHJcbiAgICAgICAgICAgICAgICAgICAgPC9DYXJkPlxyXG4gICAgICAgICAgICAgICAgICAgIDxDYXJkPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8Q2FyZEJsb2NrPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPENhcmRUaXRsZT5Qcml2YWN5IFBvbGljeTwvQ2FyZFRpdGxlPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPENhcmRUZXh0PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIFRvIGdldCBhIGJldHRlciBpZGVhIG9mIGhvdyB0aGlzIHdlYnNpdGUgaXMgdXNlZCBhbmQgaG93IGl0IGNhbiBiZSBtYWRlIGJldHRlciwgd2UgY29sbGVjdCBzb21lIGluZm9ybWF0aW9uIGFib3V0IHdoYXQgeW91IGRvIHdpdGggdGhpcyBzaXRlLlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvQ2FyZFRleHQ+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8TGluayB0bz1cIi9Qb2xpY2llc1wiIGNsYXNzTmFtZT1cImJ0biBidG4tc2Vjb25kYXJ5IGJ0bi1ibG9ja1wiPkZpbmQgb3V0IE1vcmUgwrs8L0xpbms+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDwvQ2FyZEJsb2NrPlxyXG4gICAgICAgICAgICAgICAgICAgIDwvQ2FyZD5cclxuICAgICAgICAgICAgICAgIDwvQ2FyZERlY2s+XHJcbiAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICk7XHJcbiAgICB9XHJcbn1cclxuIiwiaW1wb3J0ICogYXMgUmVhY3QgZnJvbSAncmVhY3QnO1xyXG5pbXBvcnQgeyBDYXJkLCBDYXJkQmxvY2ssIENhcmRUaXRsZSwgQnV0dG9uLCBGb3JtLCBGb3JtR3JvdXAsIExhYmVsLCBJbnB1dCwgSW5wdXRHcm91cCwgSW5wdXRHcm91cEFkZG9uLCB9IGZyb20gJ3JlYWN0c3RyYXAnO1xyXG5pbXBvcnQgeyBMaW5rIH0gZnJvbSAncmVhY3Qtcm91dGVyLWRvbSc7XHJcblxyXG5pbnRlcmZhY2UgSUxvZ2luQm94U3RhdGUge1xyXG4gICAgdXNlcm5hbWU6IHN0cmluZztcclxuICAgIHBhc3N3b3JkOiBzdHJpbmc7XHJcbn1cclxuXHJcbmludGVyZmFjZSBJTG9naW5Cb3hQcm9wcyB7XHJcbiAgICBvbkxvZ2luOiAodXNlcm5hbWU6IHN0cmluZywgcGFzc3dvcmQ6IHN0cmluZykgPT4gdm9pZDtcclxufVxyXG5cclxuZXhwb3J0IGNsYXNzIExvZ2luQm94IGV4dGVuZHMgUmVhY3QuQ29tcG9uZW50PElMb2dpbkJveFByb3BzLCBJTG9naW5Cb3hTdGF0ZT4ge1xyXG4gICAgY29uc3RydWN0b3IocHJvcHM6IElMb2dpbkJveFByb3BzKSB7XHJcbiAgICAgICAgc3VwZXIocHJvcHMpO1xyXG4gICAgICAgIHRoaXMuc3RhdGUgPSB7XHJcbiAgICAgICAgICAgIHVzZXJuYW1lOiBcIlwiLFxyXG4gICAgICAgICAgICBwYXNzd29yZDogXCJcIlxyXG4gICAgICAgIH07XHJcbiAgICB9XHJcblxyXG4gICAgcHJpdmF0ZSB1cGRhdGVVc2VybmFtZShldmVudDogUmVhY3QuQ2hhbmdlRXZlbnQ8SFRNTElucHV0RWxlbWVudD4pIHtcclxuICAgICAgICB0aGlzLnNldFN0YXRlKHsgdXNlcm5hbWU6IGV2ZW50LnRhcmdldC52YWx1ZSB9KTtcclxuICAgIH1cclxuXHJcbiAgICBwcml2YXRlIHVwZGF0ZVBhc3N3b3JkKGV2ZW50OiBSZWFjdC5DaGFuZ2VFdmVudDxIVE1MSW5wdXRFbGVtZW50Pikge1xyXG4gICAgICAgIHRoaXMuc2V0U3RhdGUoeyBwYXNzd29yZDogZXZlbnQudGFyZ2V0LnZhbHVlIH0pO1xyXG4gICAgfVxyXG5cclxuICAgIHByaXZhdGUgc3VibWl0Rm9ybShldmVudDogUmVhY3QuU3ludGhldGljRXZlbnQ8SFRNTEZvcm1FbGVtZW50Pikge1xyXG4gICAgICAgIGV2ZW50LnByZXZlbnREZWZhdWx0KCk7XHJcblxyXG4gICAgICAgIHRoaXMucHJvcHMub25Mb2dpbih0aGlzLnN0YXRlLnVzZXJuYW1lLCB0aGlzLnN0YXRlLnBhc3N3b3JkKTtcclxuICAgIH1cclxuXHJcbiAgICBwdWJsaWMgcmVuZGVyKCkge1xyXG4gICAgICAgIHJldHVybiAoXHJcbiAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiY29udGFpbmVyXCI+XHJcbiAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImQtZmxleCBqdXN0aWZ5LWNvbnRlbnQtY2VudGVyXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgPENhcmQgY2xhc3NOYW1lPVwibG9naW4tY2FyZFwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8Q2FyZEJsb2NrPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPENhcmRUaXRsZT5Mb2cgSW48L0NhcmRUaXRsZT5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxGb3JtPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxGb3JtR3JvdXA+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxJbnB1dEdyb3VwPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPElucHV0R3JvdXBBZGRvbj48c3BhbiBjbGFzc05hbWU9XCJmYSBmYS1mdyBmYS1hdFwiPjwvc3Bhbj48L0lucHV0R3JvdXBBZGRvbj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxJbnB1dCB0eXBlPVwiZW1haWxcIiBuYW1lPVwiRW1haWxcIiBpZD1cImVtYWlsXCIgcGxhY2Vob2xkZXI9XCJFbWFpbFwiIHZhbHVlPXt0aGlzLnN0YXRlLnVzZXJuYW1lfSBvbkNoYW5nZT17KGUpID0+IHRoaXMudXBkYXRlVXNlcm5hbWUoZSl9IC8+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvSW5wdXRHcm91cD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L0Zvcm1Hcm91cD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8Rm9ybUdyb3VwPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8SW5wdXRHcm91cD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxJbnB1dEdyb3VwQWRkb24+PHNwYW4gY2xhc3NOYW1lPVwiZmEgZmEtZncgZmEtbG9ja1wiPjwvc3Bhbj48L0lucHV0R3JvdXBBZGRvbj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxJbnB1dCB0eXBlPVwicGFzc3dvcmRcIiBuYW1lPVwiUGFzc3dvcmRcIiBpZD1cInBhc3N3b3JkXCIgcGxhY2Vob2xkZXI9XCJQYXNzd29yZFwiIHZhbHVlPXt0aGlzLnN0YXRlLnBhc3N3b3JkfSBvbkNoYW5nZT17KGUpID0+IHRoaXMudXBkYXRlUGFzc3dvcmQoZSl9IC8+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvSW5wdXRHcm91cD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L0Zvcm1Hcm91cD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8QnV0dG9uIHR5cGU9XCJzdWJtaXRcIiBjbGFzc05hbWU9XCJidG4tYmxvY2tcIiBvbkNsaWNrPXsoZSkgPT4gdGhpcy5zdWJtaXRGb3JtKGUpfT5Mb2cgaW48L0J1dHRvbj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8TGluayB0bz1cIi9hcHAvcmVzZXQtcGFzc3dvcmRcIj5Gb3Jnb3R0ZW4geW91ciBwYXNzd29yZD88L0xpbms+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L0Zvcm0+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDwvQ2FyZEJsb2NrPlxyXG4gICAgICAgICAgICAgICAgICAgIDwvQ2FyZD5cclxuICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICApO1xyXG4gICAgfVxyXG59XHJcbiIsImltcG9ydCAqIGFzIFJlYWN0IGZyb20gJ3JlYWN0JztcclxuaW1wb3J0IHsgTG9naW5Cb3ggfSBmcm9tICcuL2xvZ2luLWJveCc7XHJcbmltcG9ydCB7IFJvdXRlQ29tcG9uZW50UHJvcHMgfSBmcm9tICdyZWFjdC1yb3V0ZXItZG9tJztcclxuXHJcbmV4cG9ydCBjbGFzcyBMb2dpbiBleHRlbmRzIFJlYWN0LkNvbXBvbmVudDxSb3V0ZUNvbXBvbmVudFByb3BzPGFueT4sIHVuZGVmaW5lZD4ge1xyXG4gICAgcHVibGljIHJlbmRlcigpIHtcclxuICAgICAgICByZXR1cm4gKFxyXG4gICAgICAgICAgICA8TG9naW5Cb3ggb25Mb2dpbj17KHUsIHMpID0+IHRoaXMubG9naW4odSwgcyl9IC8+XHJcbiAgICAgICAgKTtcclxuICAgIH1cclxuXHJcbiAgICBwcml2YXRlIGFzeW5jIGxvZ2luKHVzZXJuYW1lOiBzdHJpbmcsIHBhc3N3b3JkOiBzdHJpbmcpIHtcclxuICAgICAgICBsZXQgaGVhZGVycyA9IG5ldyBIZWFkZXJzKCk7XHJcbiAgICAgICAgaGVhZGVycy5hcHBlbmQoXCJDb250ZW50LVR5cGVcIiwgXCJhcHBsaWNhdGlvbi9qc29uXCIpO1xyXG5cclxuICAgICAgICBjb25zdCByZXN1bHQgPSBhd2FpdCBmZXRjaChcIi90b2tlblwiLCB7XHJcbiAgICAgICAgICAgIG1ldGhvZDogJ1BPU1QnLFxyXG4gICAgICAgICAgICBib2R5OiBKU09OLnN0cmluZ2lmeSh7XHJcbiAgICAgICAgICAgICAgICB1c2VybmFtZSwgcGFzc3dvcmRcclxuICAgICAgICAgICAgfSksXHJcbiAgICAgICAgICAgIGhlYWRlcnNcclxuICAgICAgICB9KTtcclxuICAgIH1cclxufVxyXG4iLCLvu79pbXBvcnQgKiBhcyBSZWFjdCBmcm9tICdyZWFjdCc7XHJcbmltcG9ydCB7IE5hdmJhciwgTmF2YmFyVG9nZ2xlciwgTmF2YmFyQnJhbmQsIENvbGxhcHNlLCBOYXYsIE5hdkl0ZW0sIE5hdkxpbmsgfSBmcm9tICdyZWFjdHN0cmFwJztcclxuaW1wb3J0IHsgTGluayB9IGZyb20gJ3JlYWN0LXJvdXRlci1kb20nO1xyXG5cclxuaW50ZXJmYWNlIElNYWluTmF2UHJvcHMge1xyXG4gICAgbmFtZTogc3RyaW5nO1xyXG4gICAgYXV0aGVudGljYXRlZDogYm9vbGVhbjtcclxuICAgIG9uTG9nb3V0PzogKCkgPT4gdm9pZDtcclxufVxyXG5cclxuaW50ZXJmYWNlIElNYWluTmF2U3RhdGUge1xyXG4gICAgaXNPcGVuOiBib29sZWFuO1xyXG59XHJcblxyXG5leHBvcnQgY2xhc3MgTWFpbk5hdiBleHRlbmRzIFJlYWN0LkNvbXBvbmVudDxJTWFpbk5hdlByb3BzLCBJTWFpbk5hdlN0YXRlPiB7XHJcbiAgICBjb25zdHJ1Y3Rvcihwcm9wczogSU1haW5OYXZQcm9wcykge1xyXG4gICAgICAgIHN1cGVyKHByb3BzKTtcclxuXHJcbiAgICAgICAgdGhpcy5zdGF0ZSA9IHtcclxuICAgICAgICAgICAgaXNPcGVuOiBmYWxzZVxyXG4gICAgICAgIH07XHJcbiAgICB9XHJcblxyXG4gICAgdG9nZ2xlKCkge1xyXG4gICAgICAgIHRoaXMuc2V0U3RhdGUoKHByZXYsIHByb3BzKSA9PiAoe1xyXG4gICAgICAgICAgICBpc09wZW46ICFwcmV2LmlzT3BlblxyXG4gICAgICAgIH0pKTtcclxuICAgIH1cclxuXHJcbiAgICByZW5kZXIoKSB7XHJcbiAgICAgICAgbGV0IGxvZ2luRWxlbWVudDtcclxuXHJcbiAgICAgICAgaWYgKHRoaXMucHJvcHMuYXV0aGVudGljYXRlZCkge1xyXG4gICAgICAgICAgICBsb2dpbkVsZW1lbnQgPSBbXHJcbiAgICAgICAgICAgICAgICA8TmF2SXRlbSBrZXk9XCJoZWxsb1wiPlxyXG4gICAgICAgICAgICAgICAgICAgIDxMaW5rIGNsYXNzTmFtZT1cIm5hdi1saW5rXCIgdG89XCIvcHJvZmlsZVwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICBIZWxsbyB7dGhpcy5wcm9wcy5uYW1lfVxyXG4gICAgICAgICAgICAgICAgICAgIDwvTGluaz5cclxuICAgICAgICAgICAgICAgIDwvTmF2SXRlbT4sXHJcbiAgICAgICAgICAgICAgICA8TmF2SXRlbSBrZXk9XCJsb2dvdXRcIj5cclxuICAgICAgICAgICAgICAgICAgICA8TmF2TGluayBocmVmPVwiL1wiIG9uQ2xpY2s9eygpID0+IHRoaXMucHJvcHMub25Mb2dvdXQoKX0+TG9nb3V0PC9OYXZMaW5rPlxyXG4gICAgICAgICAgICAgICAgPC9OYXZJdGVtPlxyXG4gICAgICAgICAgICBdO1xyXG4gICAgICAgIH1cclxuICAgICAgICBlbHNlIHtcclxuICAgICAgICAgICAgbG9naW5FbGVtZW50ID0gPE5hdkl0ZW0+XHJcbiAgICAgICAgICAgICAgICA8TGluayBjbGFzc05hbWU9XCJuYXYtbGlua1wiIHRvPVwiL2FwcC9sb2dpblwiPkxvZyBpbjwvTGluaz5cclxuICAgICAgICAgICAgPC9OYXZJdGVtPjtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIHJldHVybiAoXHJcbiAgICAgICAgICAgIDxOYXZiYXIgY29sb3I9XCJzamEtZGFyay1ncmVlblwiIGludmVyc2UgdG9nZ2xlYWJsZT5cclxuICAgICAgICAgICAgICAgIDxOYXZiYXJUb2dnbGVyIHJpZ2h0IG9uQ2xpY2s9eygpID0+IHRoaXMudG9nZ2xlKCl9IC8+XHJcbiAgICAgICAgICAgICAgICA8TmF2YmFyQnJhbmQgaHJlZj1cIi9cIj5TSkEgVHJhY2tlcjwvTmF2YmFyQnJhbmQ+XHJcbiAgICAgICAgICAgICAgICA8Q29sbGFwc2UgaXNPcGVuPXt0aGlzLnN0YXRlLmlzT3Blbn0gbmF2YmFyPlxyXG4gICAgICAgICAgICAgICAgICAgIDxOYXYgbmF2YmFyPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8TmF2SXRlbT5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxMaW5rIGNsYXNzTmFtZT1cIm5hdi1saW5rXCIgdG89XCIvXCI+SG9tZTwvTGluaz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPC9OYXZJdGVtPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8TmF2SXRlbT5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxMaW5rIGNsYXNzTmFtZT1cIm5hdi1saW5rXCIgdG89XCIvTWFwXCI+TWFwPC9MaW5rPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8L05hdkl0ZW0+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxOYXZJdGVtPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPExpbmsgY2xhc3NOYW1lPVwibmF2LWxpbmtcIiB0bz1cIi9Db250YWN0XCI+Q29udGFjdDwvTGluaz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPC9OYXZJdGVtPlxyXG4gICAgICAgICAgICAgICAgICAgIDwvTmF2PlxyXG4gICAgICAgICAgICAgICAgICAgIDxOYXYgbmF2YmFyIGNsYXNzTmFtZT1cIm1sLWF1dG9cIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAge2xvZ2luRWxlbWVudH1cclxuICAgICAgICAgICAgICAgICAgICA8L05hdj5cclxuICAgICAgICAgICAgICAgIDwvQ29sbGFwc2U+XHJcbiAgICAgICAgICAgIDwvTmF2YmFyPik7XHJcbiAgICB9XHJcbn1cclxuIiwi77u/aW1wb3J0ICogYXMgUmVhY3QgZnJvbSAncmVhY3QnO1xyXG5pbXBvcnQgeyBNYWluTmF2IH0gZnJvbSAnLi9uYXZiYXInO1xyXG5pbXBvcnQgeyBIb21lIH0gZnJvbSAnLi9ob21lL2hvbWUnO1xyXG5pbXBvcnQgeyBMb2dpbiB9IGZyb20gJy4vbG9naW4vbG9naW4nO1xyXG5pbXBvcnQgeyBCcm93c2VyUm91dGVyIGFzIFJvdXRlciwgUm91dGUgfSBmcm9tICdyZWFjdC1yb3V0ZXItZG9tJztcclxuaW1wb3J0ICogYXMgQ29va2llcyBmcm9tICdqcy1jb29raWUnO1xyXG5cclxuaW50ZXJmYWNlIElSb290UHJvcHMge1xyXG59XHJcblxyXG5pbnRlcmZhY2UgSVJvb3RTdGF0ZSB7XHJcbiAgICBhdXRoZW50aWNhdGVkOiBib29sZWFuO1xyXG4gICAgbmFtZT86IHN0cmluZztcclxufVxyXG5cclxuaW50ZXJmYWNlIElXaG9BbUlSZXN1bHQge1xyXG4gICAgYXV0aGVudGljYXRlZDogYm9vbGVhbjtcclxuICAgIHJlYWxOYW1lOiBzdHJpbmc7XHJcbiAgICByb2xlOiBzdHJpbmdbXTtcclxuICAgIHVzZXJOYW1lOiBzdHJpbmc7XHJcbn1cclxuXHJcbmV4cG9ydCBjbGFzcyBSb290IGV4dGVuZHMgUmVhY3QuQ29tcG9uZW50PElSb290UHJvcHMsIElSb290U3RhdGU+IHtcclxuICAgIGNvbnN0cnVjdG9yKHByb3BzOiBJUm9vdFByb3BzKSB7XHJcbiAgICAgICAgc3VwZXIocHJvcHMpO1xyXG5cclxuICAgICAgICB0aGlzLnN0YXRlID0ge1xyXG4gICAgICAgICAgICBhdXRoZW50aWNhdGVkOiB0cnVlXHJcbiAgICAgICAgfTtcclxuXHJcbiAgICAgICAgdGhpcy5jaGVja0F1dGhlbnRpY2F0aW9uKCk7XHJcbiAgICB9XHJcblxyXG4gICAgYXN5bmMgY2hlY2tBdXRoZW50aWNhdGlvbigpIHtcclxuICAgICAgICBsZXQgcmVzdWx0ID0gYXdhaXQgZmV0Y2goJy9hcGkvYWNjb3VudC93aG9hbWknLCB7XHJcbiAgICAgICAgICAgIGNyZWRlbnRpYWxzOiAnc2FtZS1vcmlnaW4nXHJcbiAgICAgICAgfSk7XHJcblxyXG4gICAgICAgIGxldCB3aG9BbUkgPSBhd2FpdCByZXN1bHQuanNvbigpIGFzIElXaG9BbUlSZXN1bHQ7XHJcblxyXG4gICAgICAgIHRoaXMuc2V0U3RhdGUoe1xyXG4gICAgICAgICAgICBhdXRoZW50aWNhdGVkOiB3aG9BbUkuYXV0aGVudGljYXRlZCxcclxuICAgICAgICAgICAgbmFtZTogd2hvQW1JLnJlYWxOYW1lXHJcbiAgICAgICAgfSk7XHJcbiAgICB9XHJcblxyXG4gICAgYXN5bmMgbG9nT3V0KCkge1xyXG4gICAgICAgIGxldCBoZWFkZXJzID0gbmV3IEhlYWRlcnMoKTtcclxuXHJcbiAgICAgICAgaGVhZGVycy5hcHBlbmQoXCJSZXF1ZXN0VmVyaWZpY2F0aW9uVG9rZW5cIiwgQ29va2llcy5nZXQoJ1hTUkYtVE9LRU4nKSk7XHJcblxyXG4gICAgICAgIGxldCByZXN1bHQgPSBhd2FpdCBmZXRjaCgnL2FjY291bnQvbG9nb3V0Jywge1xyXG4gICAgICAgICAgICBjcmVkZW50aWFsczogJ3NhbWUtb3JpZ2luJyxcclxuICAgICAgICAgICAgbWV0aG9kOiAnUE9TVCcsXHJcbiAgICAgICAgICAgIGhlYWRlcnNcclxuICAgICAgICB9KTtcclxuXHJcbiAgICAgICAgbG9jYXRpb24ucmVsb2FkKHRydWUpO1xyXG4gICAgfVxyXG5cclxuICAgIHJlbmRlcigpIHtcclxuICAgICAgICByZXR1cm4gKFxyXG4gICAgICAgICAgICA8Um91dGVyPlxyXG4gICAgICAgICAgICAgICAgPGRpdj5cclxuICAgICAgICAgICAgICAgICAgICA8TWFpbk5hdiBuYW1lPXt0aGlzLnN0YXRlLm5hbWV9IGF1dGhlbnRpY2F0ZWQ9e3RoaXMuc3RhdGUuYXV0aGVudGljYXRlZH0gb25Mb2dvdXQ9eygpID0+IHRoaXMubG9nT3V0KCl9IC8+XHJcbiAgICAgICAgICAgICAgICAgICAgPFJvdXRlIGV4YWN0IHBhdGg9Jy8nIGNvbXBvbmVudD17SG9tZX0gLz5cclxuICAgICAgICAgICAgICAgICAgICA8Um91dGUgZXhhY3QgcGF0aD0nL2FwcC9sb2dpbicgY29tcG9uZW50PXtMb2dpbn0gLz5cclxuICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICA8L1JvdXRlcj4pO1xyXG4gICAgfVxyXG59XHJcbiJdfQ==
