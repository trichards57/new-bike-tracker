(function e(t,n,r){function s(o,u){if(!n[o]){if(!t[o]){var a=typeof require=="function"&&require;if(!u&&a)return a(o,!0);if(i)return i(o,!0);var f=new Error("Cannot find module '"+o+"'");throw f.code="MODULE_NOT_FOUND",f}var l=n[o]={exports:{}};t[o][0].call(l.exports,function(e){var n=t[o][1][e];return s(n?n:e)},l,l.exports,e,t,n,r)}return n[o].exports}var i=typeof require=="function"&&require;for(var o=0;o<r.length;o++)s(r[o]);return s})({1:[function(require,module,exports){
/*!
 * JavaScript Cookie v2.1.4
 * https://github.com/js-cookie/js-cookie
 *
 * Copyright 2006, 2015 Klaus Hartl & Fagner Brack
 * Released under the MIT license
 */
;(function (factory) {
	var registeredInModuleLoader = false;
	if (typeof define === 'function' && define.amd) {
		define(factory);
		registeredInModuleLoader = true;
	}
	if (typeof exports === 'object') {
		module.exports = factory();
		registeredInModuleLoader = true;
	}
	if (!registeredInModuleLoader) {
		var OldCookies = window.Cookies;
		var api = window.Cookies = factory();
		api.noConflict = function () {
			window.Cookies = OldCookies;
			return api;
		};
	}
}(function () {
	function extend () {
		var i = 0;
		var result = {};
		for (; i < arguments.length; i++) {
			var attributes = arguments[ i ];
			for (var key in attributes) {
				result[key] = attributes[key];
			}
		}
		return result;
	}

	function init (converter) {
		function api (key, value, attributes) {
			var result;
			if (typeof document === 'undefined') {
				return;
			}

			// Write

			if (arguments.length > 1) {
				attributes = extend({
					path: '/'
				}, api.defaults, attributes);

				if (typeof attributes.expires === 'number') {
					var expires = new Date();
					expires.setMilliseconds(expires.getMilliseconds() + attributes.expires * 864e+5);
					attributes.expires = expires;
				}

				// We're using "expires" because "max-age" is not supported by IE
				attributes.expires = attributes.expires ? attributes.expires.toUTCString() : '';

				try {
					result = JSON.stringify(value);
					if (/^[\{\[]/.test(result)) {
						value = result;
					}
				} catch (e) {}

				if (!converter.write) {
					value = encodeURIComponent(String(value))
						.replace(/%(23|24|26|2B|3A|3C|3E|3D|2F|3F|40|5B|5D|5E|60|7B|7D|7C)/g, decodeURIComponent);
				} else {
					value = converter.write(value, key);
				}

				key = encodeURIComponent(String(key));
				key = key.replace(/%(23|24|26|2B|5E|60|7C)/g, decodeURIComponent);
				key = key.replace(/[\(\)]/g, escape);

				var stringifiedAttributes = '';

				for (var attributeName in attributes) {
					if (!attributes[attributeName]) {
						continue;
					}
					stringifiedAttributes += '; ' + attributeName;
					if (attributes[attributeName] === true) {
						continue;
					}
					stringifiedAttributes += '=' + attributes[attributeName];
				}
				return (document.cookie = key + '=' + value + stringifiedAttributes);
			}

			// Read

			if (!key) {
				result = {};
			}

			// To prevent the for loop in the first place assign an empty array
			// in case there are no cookies at all. Also prevents odd result when
			// calling "get()"
			var cookies = document.cookie ? document.cookie.split('; ') : [];
			var rdecode = /(%[0-9A-Z]{2})+/g;
			var i = 0;

			for (; i < cookies.length; i++) {
				var parts = cookies[i].split('=');
				var cookie = parts.slice(1).join('=');

				if (cookie.charAt(0) === '"') {
					cookie = cookie.slice(1, -1);
				}

				try {
					var name = parts[0].replace(rdecode, decodeURIComponent);
					cookie = converter.read ?
						converter.read(cookie, name) : converter(cookie, name) ||
						cookie.replace(rdecode, decodeURIComponent);

					if (this.json) {
						try {
							cookie = JSON.parse(cookie);
						} catch (e) {}
					}

					if (key === name) {
						result = cookie;
						break;
					}

					if (!key) {
						result[name] = cookie;
					}
				} catch (e) {}
			}

			return result;
		}

		api.set = api;
		api.get = function (key) {
			return api.call(api, key);
		};
		api.getJSON = function () {
			return api.apply({
				json: true
			}, [].slice.call(arguments));
		};
		api.defaults = {};

		api.remove = function (key, attributes) {
			api(key, '', extend(attributes, {
				expires: -1
			}));
		};

		api.withConverter = init;

		return api;
	}

	return init(function () {});
}));

},{}],2:[function(require,module,exports){
'use strict';

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _reactDom = require('react-dom');

var _reactDom2 = _interopRequireDefault(_reactDom);

var _root = require('./root.jsx');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

_reactDom2.default.render(_react2.default.createElement(_root.Root, null), document.getElementById('root'));

},{"./root.jsx":6,"react":"react","react-dom":"react-dom"}],3:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.Home = undefined;

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _reactstrap = require('reactstrap');

var _reactRouterDom = require('react-router-dom');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Home = exports.Home = function (_React$Component) {
    _inherits(Home, _React$Component);

    function Home() {
        _classCallCheck(this, Home);

        return _possibleConstructorReturn(this, (Home.__proto__ || Object.getPrototypeOf(Home)).apply(this, arguments));
    }

    _createClass(Home, [{
        key: 'render',
        value: function render() {
            return _react2.default.createElement(
                'div',
                { className: 'container' },
                _react2.default.createElement(
                    _reactstrap.Jumbotron,
                    null,
                    _react2.default.createElement(
                        'h1',
                        { className: 'display-3' },
                        'SJA Tracker'
                    ),
                    _react2.default.createElement(
                        'p',
                        { className: 'lead' },
                        'Welcome to the St Ambulance SWR Central District Tracker'
                    ),
                    _react2.default.createElement(
                        _reactRouterDom.Link,
                        { to: '/app/login', className: 'btn btn-lg btn-sja-dark-green' },
                        'Log in \xBB'
                    )
                ),
                _react2.default.createElement(
                    _reactstrap.CardDeck,
                    { className: 'front-page' },
                    _react2.default.createElement(
                        _reactstrap.Card,
                        null,
                        _react2.default.createElement(
                            _reactstrap.CardBlock,
                            null,
                            _react2.default.createElement(
                                _reactstrap.CardTitle,
                                null,
                                'New Users'
                            ),
                            _react2.default.createElement(
                                _reactstrap.CardText,
                                null,
                                'If you have never used this site before, you will need to be issued with a username and password.'
                            ),
                            _react2.default.createElement(
                                _reactRouterDom.Link,
                                { to: '/Contact', className: 'btn btn-secondary btn-block' },
                                'Request Account \xBB'
                            )
                        )
                    ),
                    _react2.default.createElement(
                        _reactstrap.Card,
                        null,
                        _react2.default.createElement(
                            _reactstrap.CardBlock,
                            null,
                            _react2.default.createElement(
                                _reactstrap.CardTitle,
                                null,
                                'Get the Trackers'
                            ),
                            _react2.default.createElement(
                                _reactstrap.CardText,
                                null,
                                'There are a set of apps to enable your phone to be tracked from this website.'
                            ),
                            _react2.default.createElement(
                                _reactRouterDom.Link,
                                { to: '', className: 'btn btn-secondary btn-block disabled', disabled: true },
                                'Coming Soon'
                            )
                        )
                    ),
                    _react2.default.createElement(
                        _reactstrap.Card,
                        null,
                        _react2.default.createElement(
                            _reactstrap.CardBlock,
                            null,
                            _react2.default.createElement(
                                _reactstrap.CardTitle,
                                null,
                                'Privacy Policy'
                            ),
                            _react2.default.createElement(
                                _reactstrap.CardText,
                                null,
                                'To get a better idea of how this website is used and how it can be made better, we collect some information about what you do with this site.'
                            ),
                            _react2.default.createElement(
                                _reactRouterDom.Link,
                                { to: '/Policies', className: 'btn btn-secondary btn-block' },
                                'Find out More \xBB'
                            )
                        )
                    )
                )
            );
        }
    }]);

    return Home;
}(_react2.default.Component);

},{"react":"react","react-router-dom":"react-router-dom","reactstrap":"reactstrap"}],4:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.Login = undefined;

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _reactstrap = require('reactstrap');

var _reactRouterDom = require('react-router-dom');

var _jsCookie = require('js-cookie');

var _jsCookie2 = _interopRequireDefault(_jsCookie);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Login = exports.Login = function (_React$Component) {
    _inherits(Login, _React$Component);

    function Login(props) {
        _classCallCheck(this, Login);

        var _this = _possibleConstructorReturn(this, (Login.__proto__ || Object.getPrototypeOf(Login)).call(this, props));

        _this.state = {
            xsrfToken: _jsCookie2.default.get('XSRF-TOKEN')
        };
        return _this;
    }

    _createClass(Login, [{
        key: 'render',
        value: function render() {
            return _react2.default.createElement(
                'div',
                { className: 'container' },
                _react2.default.createElement(
                    'div',
                    { className: 'd-flex justify-content-center' },
                    _react2.default.createElement(
                        _reactstrap.Card,
                        { id: 'login-card' },
                        _react2.default.createElement(
                            _reactstrap.CardBlock,
                            null,
                            _react2.default.createElement(
                                _reactstrap.CardTitle,
                                null,
                                'Log In'
                            ),
                            _react2.default.createElement(
                                _reactstrap.Form,
                                { action: '/Account/Login', method: 'post' },
                                _react2.default.createElement(_reactstrap.Input, { type: 'hidden', name: '__RequestVerificationToken', value: this.state.xsrfToken }),
                                _react2.default.createElement(
                                    _reactstrap.FormGroup,
                                    null,
                                    _react2.default.createElement(
                                        _reactstrap.InputGroup,
                                        null,
                                        _react2.default.createElement(
                                            _reactstrap.InputGroupAddon,
                                            null,
                                            _react2.default.createElement('span', { className: 'fa fa-fw fa-at' })
                                        ),
                                        _react2.default.createElement(_reactstrap.Input, { type: 'email', name: 'Email', id: 'email', placeholder: 'Email' })
                                    )
                                ),
                                _react2.default.createElement(
                                    _reactstrap.FormGroup,
                                    null,
                                    _react2.default.createElement(
                                        _reactstrap.InputGroup,
                                        null,
                                        _react2.default.createElement(
                                            _reactstrap.InputGroupAddon,
                                            null,
                                            _react2.default.createElement('span', { className: 'fa fa-fw fa-lock' })
                                        ),
                                        _react2.default.createElement(_reactstrap.Input, { type: 'password', name: 'Password', id: 'password', placeholder: 'Password' })
                                    )
                                ),
                                _react2.default.createElement(
                                    _reactstrap.FormGroup,
                                    { check: true },
                                    _react2.default.createElement(
                                        _reactstrap.Label,
                                        { check: true },
                                        _react2.default.createElement(_reactstrap.Input, { type: 'checkbox', name: 'RememberMe' }),
                                        ' ',
                                        'Remember me'
                                    )
                                ),
                                _react2.default.createElement(
                                    _reactstrap.Button,
                                    { type: 'submit', className: 'btn-block' },
                                    'Log in'
                                ),
                                _react2.default.createElement(
                                    _reactRouterDom.Link,
                                    { to: '/app/reset-password' },
                                    'Forgotten your password?'
                                )
                            )
                        )
                    )
                )
            );
        }
    }]);

    return Login;
}(_react2.default.Component);

},{"js-cookie":1,"react":"react","react-router-dom":"react-router-dom","reactstrap":"reactstrap"}],5:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.MainNav = undefined;

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _reactstrap = require('reactstrap');

var _reactRouterDom = require('react-router-dom');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var MainNav = exports.MainNav = function (_React$Component) {
    _inherits(MainNav, _React$Component);

    function MainNav(props) {
        _classCallCheck(this, MainNav);

        var _this = _possibleConstructorReturn(this, (MainNav.__proto__ || Object.getPrototypeOf(MainNav)).call(this, props));

        _this.state = {
            isOpen: false
        };
        return _this;
    }

    _createClass(MainNav, [{
        key: 'toggle',
        value: function toggle() {
            this.setState(function (prev, props) {
                return {
                    isOpen: !prev.isOpen
                };
            });
        }
    }, {
        key: 'render',
        value: function render() {
            var _this2 = this;

            return _react2.default.createElement(
                _reactstrap.Navbar,
                { color: 'sja-dark-green', inverse: true, toggleable: true },
                _react2.default.createElement(_reactstrap.NavbarToggler, { right: true, onClick: function onClick() {
                        return _this2.toggle();
                    } }),
                _react2.default.createElement(
                    _reactstrap.NavbarBrand,
                    { href: '/' },
                    'SJA Tracker'
                ),
                _react2.default.createElement(
                    _reactstrap.Collapse,
                    { isOpen: this.state.isOpen, navbar: true },
                    _react2.default.createElement(
                        _reactstrap.Nav,
                        { navbar: true },
                        _react2.default.createElement(
                            _reactstrap.NavItem,
                            null,
                            _react2.default.createElement(
                                _reactRouterDom.Link,
                                { className: 'nav-link', to: '/' },
                                'Home'
                            )
                        ),
                        _react2.default.createElement(
                            _reactstrap.NavItem,
                            null,
                            _react2.default.createElement(
                                _reactRouterDom.Link,
                                { className: 'nav-link', to: '/Map' },
                                'Map'
                            )
                        ),
                        _react2.default.createElement(
                            _reactstrap.NavItem,
                            null,
                            _react2.default.createElement(
                                _reactRouterDom.Link,
                                { className: 'nav-link', to: '/Contact' },
                                'Contact'
                            )
                        )
                    ),
                    _react2.default.createElement(
                        _reactstrap.Nav,
                        { navbar: true, className: 'ml-auto' },
                        _react2.default.createElement(
                            _reactstrap.NavItem,
                            null,
                            _react2.default.createElement(
                                _reactRouterDom.Link,
                                { className: 'nav-link', to: '/app/login' },
                                'Log in'
                            )
                        )
                    )
                )
            );
        }
    }]);

    return MainNav;
}(_react2.default.Component);

},{"react":"react","react-router-dom":"react-router-dom","reactstrap":"reactstrap"}],6:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.Root = undefined;

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _navbar = require('./navbar.jsx');

var _home = require('./home/home.jsx');

var _login = require('./login/login.jsx');

var _reactRouterDom = require('react-router-dom');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Root = exports.Root = function (_React$Component) {
    _inherits(Root, _React$Component);

    function Root() {
        _classCallCheck(this, Root);

        return _possibleConstructorReturn(this, (Root.__proto__ || Object.getPrototypeOf(Root)).apply(this, arguments));
    }

    _createClass(Root, [{
        key: 'render',
        value: function render() {
            return _react2.default.createElement(
                _reactRouterDom.BrowserRouter,
                null,
                _react2.default.createElement(
                    'div',
                    null,
                    _react2.default.createElement(_navbar.MainNav, null),
                    _react2.default.createElement(_reactRouterDom.Route, { exact: true, path: '/', component: _home.Home }),
                    _react2.default.createElement(_reactRouterDom.Route, { exact: true, path: '/app/login', component: _login.Login })
                )
            );
        }
    }]);

    return Root;
}(_react2.default.Component);

},{"./home/home.jsx":3,"./login/login.jsx":4,"./navbar.jsx":5,"react":"react","react-router-dom":"react-router-dom"}]},{},[2])
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm5vZGVfbW9kdWxlcy9icm93c2VyLXBhY2svX3ByZWx1ZGUuanMiLCJub2RlX21vZHVsZXMvanMtY29va2llL3NyYy9qcy5jb29raWUuanMiLCJzcmNcXGpzXFxhcHAuanN4Iiwic3JjXFxqc1xcaG9tZVxcaG9tZS5qc3giLCJzcmNcXGpzXFxsb2dpblxcbG9naW4uanN4Iiwic3JjXFxqc1xcbmF2YmFyLmpzeCIsInNyY1xcanNcXHJvb3QuanN4Il0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0FDQUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7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QSxzQkFBVSxRQUFRLEtBQUssS0FBTCxDQUFXLE1BQTdCLEVBQXFDLFlBQXJDO0FBQ0k7QUFBQTtBQUFBLDBCQUFLLFlBQUw7QUFDSTtBQUFBO0FBQUE7QUFDSTtBQUFBO0FBQUEsa0NBQU0sV0FBVSxVQUFoQixFQUEyQixJQUFHLEdBQTlCO0FBQUE7QUFBQTtBQURKLHlCQURKO0FBSUk7QUFBQTtBQUFBO0FBQ0k7QUFBQTtBQUFBLGtDQUFNLFdBQVUsVUFBaEIsRUFBMkIsSUFBRyxNQUE5QjtBQUFBO0FBQUE7QUFESix5QkFKSjtBQU9JO0FBQUE7QUFBQTtBQUNJO0FBQUE7QUFBQSxrQ0FBTSxXQUFVLFVBQWhCLEVBQTJCLElBQUcsVUFBOUI7QUFBQTtBQUFBO0FBREo7QUFQSixxQkFESjtBQVlJO0FBQUE7QUFBQSwwQkFBSyxZQUFMLEVBQVksV0FBVSxTQUF0QjtBQUNJO0FBQUE7QUFBQTtBQUNJO0FBQUE7QUFBQSxrQ0FBTSxXQUFVLFVBQWhCLEVBQTJCLElBQUcsWUFBOUI7QUFBQTtBQUFBO0FBREo7QUFESjtBQVpKO0FBSEosYUFESjtBQXVCSDs7OztFQXZDd0IsZ0JBQU0sUzs7Ozs7Ozs7Ozs7O0FDSmxDOzs7O0FBQ0Q7O0FBQ0E7O0FBQ0E7O0FBQ0E7Ozs7Ozs7Ozs7SUFFYSxJLFdBQUEsSTs7Ozs7Ozs7Ozs7aUNBQ0E7QUFDTCxtQkFDSTtBQUFBO0FBQUE7QUFDSTtBQUFBO0FBQUE7QUFDSSx3RUFESjtBQUVJLDJFQUFPLFdBQVAsRUFBYSxNQUFLLEdBQWxCLEVBQXNCLHFCQUF0QixHQUZKO0FBR0ksMkVBQU8sV0FBUCxFQUFhLE1BQUssWUFBbEIsRUFBK0IsdUJBQS9CO0FBSEo7QUFESixhQURKO0FBU0g7Ozs7RUFYcUIsZ0JBQU0sUyIsImZpbGUiOiJnZW5lcmF0ZWQuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlc0NvbnRlbnQiOlsiKGZ1bmN0aW9uIGUodCxuLHIpe2Z1bmN0aW9uIHMobyx1KXtpZighbltvXSl7aWYoIXRbb10pe3ZhciBhPXR5cGVvZiByZXF1aXJlPT1cImZ1bmN0aW9uXCImJnJlcXVpcmU7aWYoIXUmJmEpcmV0dXJuIGEobywhMCk7aWYoaSlyZXR1cm4gaShvLCEwKTt2YXIgZj1uZXcgRXJyb3IoXCJDYW5ub3QgZmluZCBtb2R1bGUgJ1wiK28rXCInXCIpO3Rocm93IGYuY29kZT1cIk1PRFVMRV9OT1RfRk9VTkRcIixmfXZhciBsPW5bb109e2V4cG9ydHM6e319O3Rbb11bMF0uY2FsbChsLmV4cG9ydHMsZnVuY3Rpb24oZSl7dmFyIG49dFtvXVsxXVtlXTtyZXR1cm4gcyhuP246ZSl9LGwsbC5leHBvcnRzLGUsdCxuLHIpfXJldHVybiBuW29dLmV4cG9ydHN9dmFyIGk9dHlwZW9mIHJlcXVpcmU9PVwiZnVuY3Rpb25cIiYmcmVxdWlyZTtmb3IodmFyIG89MDtvPHIubGVuZ3RoO28rKylzKHJbb10pO3JldHVybiBzfSkiLCIvKiFcbiAqIEphdmFTY3JpcHQgQ29va2llIHYyLjEuNFxuICogaHR0cHM6Ly9naXRodWIuY29tL2pzLWNvb2tpZS9qcy1jb29raWVcbiAqXG4gKiBDb3B5cmlnaHQgMjAwNiwgMjAxNSBLbGF1cyBIYXJ0bCAmIEZhZ25lciBCcmFja1xuICogUmVsZWFzZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlXG4gKi9cbjsoZnVuY3Rpb24gKGZhY3RvcnkpIHtcblx0dmFyIHJlZ2lzdGVyZWRJbk1vZHVsZUxvYWRlciA9IGZhbHNlO1xuXHRpZiAodHlwZW9mIGRlZmluZSA9PT0gJ2Z1bmN0aW9uJyAmJiBkZWZpbmUuYW1kKSB7XG5cdFx0ZGVmaW5lKGZhY3RvcnkpO1xuXHRcdHJlZ2lzdGVyZWRJbk1vZHVsZUxvYWRlciA9IHRydWU7XG5cdH1cblx0aWYgKHR5cGVvZiBleHBvcnRzID09PSAnb2JqZWN0Jykge1xuXHRcdG1vZHVsZS5leHBvcnRzID0gZmFjdG9yeSgpO1xuXHRcdHJlZ2lzdGVyZWRJbk1vZHVsZUxvYWRlciA9IHRydWU7XG5cdH1cblx0aWYgKCFyZWdpc3RlcmVkSW5Nb2R1bGVMb2FkZXIpIHtcblx0XHR2YXIgT2xkQ29va2llcyA9IHdpbmRvdy5Db29raWVzO1xuXHRcdHZhciBhcGkgPSB3aW5kb3cuQ29va2llcyA9IGZhY3RvcnkoKTtcblx0XHRhcGkubm9Db25mbGljdCA9IGZ1bmN0aW9uICgpIHtcblx0XHRcdHdpbmRvdy5Db29raWVzID0gT2xkQ29va2llcztcblx0XHRcdHJldHVybiBhcGk7XG5cdFx0fTtcblx0fVxufShmdW5jdGlvbiAoKSB7XG5cdGZ1bmN0aW9uIGV4dGVuZCAoKSB7XG5cdFx0dmFyIGkgPSAwO1xuXHRcdHZhciByZXN1bHQgPSB7fTtcblx0XHRmb3IgKDsgaSA8IGFyZ3VtZW50cy5sZW5ndGg7IGkrKykge1xuXHRcdFx0dmFyIGF0dHJpYnV0ZXMgPSBhcmd1bWVudHNbIGkgXTtcblx0XHRcdGZvciAodmFyIGtleSBpbiBhdHRyaWJ1dGVzKSB7XG5cdFx0XHRcdHJlc3VsdFtrZXldID0gYXR0cmlidXRlc1trZXldO1xuXHRcdFx0fVxuXHRcdH1cblx0XHRyZXR1cm4gcmVzdWx0O1xuXHR9XG5cblx0ZnVuY3Rpb24gaW5pdCAoY29udmVydGVyKSB7XG5cdFx0ZnVuY3Rpb24gYXBpIChrZXksIHZhbHVlLCBhdHRyaWJ1dGVzKSB7XG5cdFx0XHR2YXIgcmVzdWx0O1xuXHRcdFx0aWYgKHR5cGVvZiBkb2N1bWVudCA9PT0gJ3VuZGVmaW5lZCcpIHtcblx0XHRcdFx0cmV0dXJuO1xuXHRcdFx0fVxuXG5cdFx0XHQvLyBXcml0ZVxuXG5cdFx0XHRpZiAoYXJndW1lbnRzLmxlbmd0aCA+IDEpIHtcblx0XHRcdFx0YXR0cmlidXRlcyA9IGV4dGVuZCh7XG5cdFx0XHRcdFx0cGF0aDogJy8nXG5cdFx0XHRcdH0sIGFwaS5kZWZhdWx0cywgYXR0cmlidXRlcyk7XG5cblx0XHRcdFx0aWYgKHR5cGVvZiBhdHRyaWJ1dGVzLmV4cGlyZXMgPT09ICdudW1iZXInKSB7XG5cdFx0XHRcdFx0dmFyIGV4cGlyZXMgPSBuZXcgRGF0ZSgpO1xuXHRcdFx0XHRcdGV4cGlyZXMuc2V0TWlsbGlzZWNvbmRzKGV4cGlyZXMuZ2V0TWlsbGlzZWNvbmRzKCkgKyBhdHRyaWJ1dGVzLmV4cGlyZXMgKiA4NjRlKzUpO1xuXHRcdFx0XHRcdGF0dHJpYnV0ZXMuZXhwaXJlcyA9IGV4cGlyZXM7XG5cdFx0XHRcdH1cblxuXHRcdFx0XHQvLyBXZSdyZSB1c2luZyBcImV4cGlyZXNcIiBiZWNhdXNlIFwibWF4LWFnZVwiIGlzIG5vdCBzdXBwb3J0ZWQgYnkgSUVcblx0XHRcdFx0YXR0cmlidXRlcy5leHBpcmVzID0gYXR0cmlidXRlcy5leHBpcmVzID8gYXR0cmlidXRlcy5leHBpcmVzLnRvVVRDU3RyaW5nKCkgOiAnJztcblxuXHRcdFx0XHR0cnkge1xuXHRcdFx0XHRcdHJlc3VsdCA9IEpTT04uc3RyaW5naWZ5KHZhbHVlKTtcblx0XHRcdFx0XHRpZiAoL15bXFx7XFxbXS8udGVzdChyZXN1bHQpKSB7XG5cdFx0XHRcdFx0XHR2YWx1ZSA9IHJlc3VsdDtcblx0XHRcdFx0XHR9XG5cdFx0XHRcdH0gY2F0Y2ggKGUpIHt9XG5cblx0XHRcdFx0aWYgKCFjb252ZXJ0ZXIud3JpdGUpIHtcblx0XHRcdFx0XHR2YWx1ZSA9IGVuY29kZVVSSUNvbXBvbmVudChTdHJpbmcodmFsdWUpKVxuXHRcdFx0XHRcdFx0LnJlcGxhY2UoLyUoMjN8MjR8MjZ8MkJ8M0F8M0N8M0V8M0R8MkZ8M0Z8NDB8NUJ8NUR8NUV8NjB8N0J8N0R8N0MpL2csIGRlY29kZVVSSUNvbXBvbmVudCk7XG5cdFx0XHRcdH0gZWxzZSB7XG5cdFx0XHRcdFx0dmFsdWUgPSBjb252ZXJ0ZXIud3JpdGUodmFsdWUsIGtleSk7XG5cdFx0XHRcdH1cblxuXHRcdFx0XHRrZXkgPSBlbmNvZGVVUklDb21wb25lbnQoU3RyaW5nKGtleSkpO1xuXHRcdFx0XHRrZXkgPSBrZXkucmVwbGFjZSgvJSgyM3wyNHwyNnwyQnw1RXw2MHw3QykvZywgZGVjb2RlVVJJQ29tcG9uZW50KTtcblx0XHRcdFx0a2V5ID0ga2V5LnJlcGxhY2UoL1tcXChcXCldL2csIGVzY2FwZSk7XG5cblx0XHRcdFx0dmFyIHN0cmluZ2lmaWVkQXR0cmlidXRlcyA9ICcnO1xuXG5cdFx0XHRcdGZvciAodmFyIGF0dHJpYnV0ZU5hbWUgaW4gYXR0cmlidXRlcykge1xuXHRcdFx0XHRcdGlmICghYXR0cmlidXRlc1thdHRyaWJ1dGVOYW1lXSkge1xuXHRcdFx0XHRcdFx0Y29udGludWU7XG5cdFx0XHRcdFx0fVxuXHRcdFx0XHRcdHN0cmluZ2lmaWVkQXR0cmlidXRlcyArPSAnOyAnICsgYXR0cmlidXRlTmFtZTtcblx0XHRcdFx0XHRpZiAoYXR0cmlidXRlc1thdHRyaWJ1dGVOYW1lXSA9PT0gdHJ1ZSkge1xuXHRcdFx0XHRcdFx0Y29udGludWU7XG5cdFx0XHRcdFx0fVxuXHRcdFx0XHRcdHN0cmluZ2lmaWVkQXR0cmlidXRlcyArPSAnPScgKyBhdHRyaWJ1dGVzW2F0dHJpYnV0ZU5hbWVdO1xuXHRcdFx0XHR9XG5cdFx0XHRcdHJldHVybiAoZG9jdW1lbnQuY29va2llID0ga2V5ICsgJz0nICsgdmFsdWUgKyBzdHJpbmdpZmllZEF0dHJpYnV0ZXMpO1xuXHRcdFx0fVxuXG5cdFx0XHQvLyBSZWFkXG5cblx0XHRcdGlmICgha2V5KSB7XG5cdFx0XHRcdHJlc3VsdCA9IHt9O1xuXHRcdFx0fVxuXG5cdFx0XHQvLyBUbyBwcmV2ZW50IHRoZSBmb3IgbG9vcCBpbiB0aGUgZmlyc3QgcGxhY2UgYXNzaWduIGFuIGVtcHR5IGFycmF5XG5cdFx0XHQvLyBpbiBjYXNlIHRoZXJlIGFyZSBubyBjb29raWVzIGF0IGFsbC4gQWxzbyBwcmV2ZW50cyBvZGQgcmVzdWx0IHdoZW5cblx0XHRcdC8vIGNhbGxpbmcgXCJnZXQoKVwiXG5cdFx0XHR2YXIgY29va2llcyA9IGRvY3VtZW50LmNvb2tpZSA/IGRvY3VtZW50LmNvb2tpZS5zcGxpdCgnOyAnKSA6IFtdO1xuXHRcdFx0dmFyIHJkZWNvZGUgPSAvKCVbMC05QS1aXXsyfSkrL2c7XG5cdFx0XHR2YXIgaSA9IDA7XG5cblx0XHRcdGZvciAoOyBpIDwgY29va2llcy5sZW5ndGg7IGkrKykge1xuXHRcdFx0XHR2YXIgcGFydHMgPSBjb29raWVzW2ldLnNwbGl0KCc9Jyk7XG5cdFx0XHRcdHZhciBjb29raWUgPSBwYXJ0cy5zbGljZSgxKS5qb2luKCc9Jyk7XG5cblx0XHRcdFx0aWYgKGNvb2tpZS5jaGFyQXQoMCkgPT09ICdcIicpIHtcblx0XHRcdFx0XHRjb29raWUgPSBjb29raWUuc2xpY2UoMSwgLTEpO1xuXHRcdFx0XHR9XG5cblx0XHRcdFx0dHJ5IHtcblx0XHRcdFx0XHR2YXIgbmFtZSA9IHBhcnRzWzBdLnJlcGxhY2UocmRlY29kZSwgZGVjb2RlVVJJQ29tcG9uZW50KTtcblx0XHRcdFx0XHRjb29raWUgPSBjb252ZXJ0ZXIucmVhZCA/XG5cdFx0XHRcdFx0XHRjb252ZXJ0ZXIucmVhZChjb29raWUsIG5hbWUpIDogY29udmVydGVyKGNvb2tpZSwgbmFtZSkgfHxcblx0XHRcdFx0XHRcdGNvb2tpZS5yZXBsYWNlKHJkZWNvZGUsIGRlY29kZVVSSUNvbXBvbmVudCk7XG5cblx0XHRcdFx0XHRpZiAodGhpcy5qc29uKSB7XG5cdFx0XHRcdFx0XHR0cnkge1xuXHRcdFx0XHRcdFx0XHRjb29raWUgPSBKU09OLnBhcnNlKGNvb2tpZSk7XG5cdFx0XHRcdFx0XHR9IGNhdGNoIChlKSB7fVxuXHRcdFx0XHRcdH1cblxuXHRcdFx0XHRcdGlmIChrZXkgPT09IG5hbWUpIHtcblx0XHRcdFx0XHRcdHJlc3VsdCA9IGNvb2tpZTtcblx0XHRcdFx0XHRcdGJyZWFrO1xuXHRcdFx0XHRcdH1cblxuXHRcdFx0XHRcdGlmICgha2V5KSB7XG5cdFx0XHRcdFx0XHRyZXN1bHRbbmFtZV0gPSBjb29raWU7XG5cdFx0XHRcdFx0fVxuXHRcdFx0XHR9IGNhdGNoIChlKSB7fVxuXHRcdFx0fVxuXG5cdFx0XHRyZXR1cm4gcmVzdWx0O1xuXHRcdH1cblxuXHRcdGFwaS5zZXQgPSBhcGk7XG5cdFx0YXBpLmdldCA9IGZ1bmN0aW9uIChrZXkpIHtcblx0XHRcdHJldHVybiBhcGkuY2FsbChhcGksIGtleSk7XG5cdFx0fTtcblx0XHRhcGkuZ2V0SlNPTiA9IGZ1bmN0aW9uICgpIHtcblx0XHRcdHJldHVybiBhcGkuYXBwbHkoe1xuXHRcdFx0XHRqc29uOiB0cnVlXG5cdFx0XHR9LCBbXS5zbGljZS5jYWxsKGFyZ3VtZW50cykpO1xuXHRcdH07XG5cdFx0YXBpLmRlZmF1bHRzID0ge307XG5cblx0XHRhcGkucmVtb3ZlID0gZnVuY3Rpb24gKGtleSwgYXR0cmlidXRlcykge1xuXHRcdFx0YXBpKGtleSwgJycsIGV4dGVuZChhdHRyaWJ1dGVzLCB7XG5cdFx0XHRcdGV4cGlyZXM6IC0xXG5cdFx0XHR9KSk7XG5cdFx0fTtcblxuXHRcdGFwaS53aXRoQ29udmVydGVyID0gaW5pdDtcblxuXHRcdHJldHVybiBhcGk7XG5cdH1cblxuXHRyZXR1cm4gaW5pdChmdW5jdGlvbiAoKSB7fSk7XG59KSk7XG4iLCLvu79pbXBvcnQgUmVhY3QgZnJvbSAncmVhY3QnO1xyXG5pbXBvcnQgUmVhY3RET00gZnJvbSAncmVhY3QtZG9tJztcclxuaW1wb3J0IHsgUm9vdCB9IGZyb20gJy4vcm9vdC5qc3gnO1xyXG5cclxuUmVhY3RET00ucmVuZGVyKDxSb290IC8+LCBkb2N1bWVudC5nZXRFbGVtZW50QnlJZCgncm9vdCcpKTtcclxuIiwi77u/aW1wb3J0IFJlYWN0IGZyb20gJ3JlYWN0JztcclxuXHJcbmltcG9ydCB7IEp1bWJvdHJvbiwgQnV0dG9uLCBDYXJkLCBDYXJkQmxvY2ssIENhcmRUaXRsZSwgQ2FyZFRleHQsIENhcmREZWNrIH0gZnJvbSAncmVhY3RzdHJhcCc7XHJcbmltcG9ydCB7IExpbmsgfSBmcm9tICdyZWFjdC1yb3V0ZXItZG9tJztcclxuXHJcbmV4cG9ydCBjbGFzcyBIb21lIGV4dGVuZHMgUmVhY3QuQ29tcG9uZW50IHtcclxuICAgIHJlbmRlcigpIHtcclxuICAgICAgICByZXR1cm4gKFxyXG4gICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImNvbnRhaW5lclwiPlxyXG4gICAgICAgICAgICAgICAgPEp1bWJvdHJvbj5cclxuICAgICAgICAgICAgICAgICAgICA8aDEgY2xhc3NOYW1lPVwiZGlzcGxheS0zXCI+U0pBIFRyYWNrZXI8L2gxPlxyXG4gICAgICAgICAgICAgICAgICAgIDxwIGNsYXNzTmFtZT1cImxlYWRcIj5XZWxjb21lIHRvIHRoZSBTdCBBbWJ1bGFuY2UgU1dSIENlbnRyYWwgRGlzdHJpY3QgVHJhY2tlcjwvcD5cclxuICAgICAgICAgICAgICAgICAgICA8TGluayB0bz1cIi9hcHAvbG9naW5cIiBjbGFzc05hbWU9XCJidG4gYnRuLWxnIGJ0bi1zamEtZGFyay1ncmVlblwiPkxvZyBpbiDCuzwvTGluaz5cclxuICAgICAgICAgICAgICAgIDwvSnVtYm90cm9uPlxyXG5cclxuICAgICAgICAgICAgICAgIDxDYXJkRGVjayBjbGFzc05hbWU9XCJmcm9udC1wYWdlXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxDYXJkPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPENhcmRCbG9jaz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8Q2FyZFRpdGxlPk5ldyBVc2VyczwvQ2FyZFRpdGxlPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxDYXJkVGV4dD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgSWYgeW91IGhhdmUgbmV2ZXIgdXNlZCB0aGlzIHNpdGUgYmVmb3JlLCB5b3Ugd2lsbCBuZWVkIHRvIGJlIGlzc3VlZCB3aXRoIGEgdXNlcm5hbWUgYW5kIHBhc3N3b3JkLlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvQ2FyZFRleHQ+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPExpbmsgdG89XCIvQ29udGFjdFwiIGNsYXNzTmFtZT1cImJ0biBidG4tc2Vjb25kYXJ5IGJ0bi1ibG9ja1wiPlJlcXVlc3QgQWNjb3VudCDCuzwvTGluaz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvQ2FyZEJsb2NrPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8L0NhcmQ+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxDYXJkPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPENhcmRCbG9jaz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8Q2FyZFRpdGxlPkdldCB0aGUgVHJhY2tlcnM8L0NhcmRUaXRsZT5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8Q2FyZFRleHQ+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIFRoZXJlIGFyZSBhIHNldCBvZiBhcHBzIHRvIGVuYWJsZSB5b3VyIHBob25lIHRvIGJlIHRyYWNrZWQgZnJvbSB0aGlzIHdlYnNpdGUuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9DYXJkVGV4dD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8TGluayB0bz1cIlwiIGNsYXNzTmFtZT1cImJ0biBidG4tc2Vjb25kYXJ5IGJ0bi1ibG9jayBkaXNhYmxlZFwiIGRpc2FibGVkPkNvbWluZyBTb29uPC9MaW5rPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9DYXJkQmxvY2s+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDwvQ2FyZD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPENhcmQ+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8Q2FyZEJsb2NrPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxDYXJkVGl0bGU+UHJpdmFjeSBQb2xpY3k8L0NhcmRUaXRsZT5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8Q2FyZFRleHQ+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIFRvIGdldCBhIGJldHRlciBpZGVhIG9mIGhvdyB0aGlzIHdlYnNpdGUgaXMgdXNlZCBhbmQgaG93IGl0IGNhbiBiZSBtYWRlIGJldHRlciwgd2UgY29sbGVjdCBzb21lIGluZm9ybWF0aW9uIGFib3V0IHdoYXQgeW91IGRvIHdpdGggdGhpcyBzaXRlLlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvQ2FyZFRleHQ+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPExpbmsgdG89XCIvUG9saWNpZXNcIiBjbGFzc05hbWU9XCJidG4gYnRuLXNlY29uZGFyeSBidG4tYmxvY2tcIj5GaW5kIG91dCBNb3JlIMK7PC9MaW5rPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9DYXJkQmxvY2s+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDwvQ2FyZD5cclxuICAgICAgICAgICAgICAgIDwvQ2FyZERlY2s+XHJcbiAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICk7XHJcbiAgICB9XHJcbn1cclxuIiwiaW1wb3J0IFJlYWN0IGZyb20gJ3JlYWN0JztcclxuaW1wb3J0IHsgQ2FyZCwgQ2FyZEJsb2NrLCBDYXJkVGl0bGUsIEJ1dHRvbiwgRm9ybSwgRm9ybUdyb3VwLCBMYWJlbCwgSW5wdXQsIElucHV0R3JvdXAsIElucHV0R3JvdXBBZGRvbiB9IGZyb20gJ3JlYWN0c3RyYXAnO1xyXG5pbXBvcnQgeyBMaW5rIH0gZnJvbSAncmVhY3Qtcm91dGVyLWRvbSc7XHJcbmltcG9ydCBDb29raWVzIGZyb20gJ2pzLWNvb2tpZSc7XHJcblxyXG5leHBvcnQgY2xhc3MgTG9naW4gZXh0ZW5kcyBSZWFjdC5Db21wb25lbnQge1xyXG4gICAgY29uc3RydWN0b3IocHJvcHMpIHtcclxuICAgICAgICBzdXBlcihwcm9wcyk7XHJcbiAgICAgICAgdGhpcy5zdGF0ZSA9IHtcclxuICAgICAgICAgICAgeHNyZlRva2VuOiBDb29raWVzLmdldCgnWFNSRi1UT0tFTicpXHJcbiAgICAgICAgfTtcclxuICAgIH1cclxuXHJcbiAgICByZW5kZXIoKSB7XHJcbiAgICAgICAgcmV0dXJuIChcclxuICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJjb250YWluZXJcIj5cclxuICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiZC1mbGV4IGp1c3RpZnktY29udGVudC1jZW50ZXJcIj5cclxuICAgICAgICAgICAgICAgICAgICA8Q2FyZCBpZD1cImxvZ2luLWNhcmRcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPENhcmRCbG9jaz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxDYXJkVGl0bGU+TG9nIEluPC9DYXJkVGl0bGU+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8Rm9ybSBhY3Rpb249XCIvQWNjb3VudC9Mb2dpblwiIG1ldGhvZD1cInBvc3RcIiA+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPElucHV0IHR5cGU9XCJoaWRkZW5cIiBuYW1lPVwiX19SZXF1ZXN0VmVyaWZpY2F0aW9uVG9rZW5cIiB2YWx1ZT17dGhpcy5zdGF0ZS54c3JmVG9rZW59IC8+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPEZvcm1Hcm91cD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPElucHV0R3JvdXA+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8SW5wdXRHcm91cEFkZG9uPjxzcGFuIGNsYXNzTmFtZT1cImZhIGZhLWZ3IGZhLWF0XCI+PC9zcGFuPjwvSW5wdXRHcm91cEFkZG9uPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPElucHV0IHR5cGU9XCJlbWFpbFwiIG5hbWU9XCJFbWFpbFwiIGlkPVwiZW1haWxcIiBwbGFjZWhvbGRlcj1cIkVtYWlsXCIgLz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9JbnB1dEdyb3VwPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvRm9ybUdyb3VwPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxGb3JtR3JvdXA+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxJbnB1dEdyb3VwPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPElucHV0R3JvdXBBZGRvbj48c3BhbiBjbGFzc05hbWU9XCJmYSBmYS1mdyBmYS1sb2NrXCI+PC9zcGFuPjwvSW5wdXRHcm91cEFkZG9uPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPElucHV0IHR5cGU9XCJwYXNzd29yZFwiIG5hbWU9XCJQYXNzd29yZFwiIGlkPVwicGFzc3dvcmRcIiBwbGFjZWhvbGRlcj1cIlBhc3N3b3JkXCIgLz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9JbnB1dEdyb3VwPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvRm9ybUdyb3VwPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxGb3JtR3JvdXAgY2hlY2s+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxMYWJlbCBjaGVjaz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxJbnB1dCB0eXBlPVwiY2hlY2tib3hcIiBuYW1lPVwiUmVtZW1iZXJNZVwiIC8+eycgJ31cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIFJlbWVtYmVyIG1lXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvTGFiZWw+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9Gb3JtR3JvdXA+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPEJ1dHRvbiB0eXBlPVwic3VibWl0XCIgY2xhc3NOYW1lPVwiYnRuLWJsb2NrXCI+TG9nIGluPC9CdXR0b24+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPExpbmsgdG89XCIvYXBwL3Jlc2V0LXBhc3N3b3JkXCI+Rm9yZ290dGVuIHlvdXIgcGFzc3dvcmQ/PC9MaW5rPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9Gb3JtPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8L0NhcmRCbG9jaz5cclxuICAgICAgICAgICAgICAgICAgICA8L0NhcmQ+XHJcbiAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgKTtcclxuICAgIH1cclxufVxyXG4iLCLvu79pbXBvcnQgUmVhY3QgZnJvbSAncmVhY3QnO1xyXG5pbXBvcnQgeyBOYXZiYXIsIE5hdmJhclRvZ2dsZXIsIE5hdmJhckJyYW5kLCBDb2xsYXBzZSwgTmF2LCBOYXZJdGVtLCBOYXZMaW5rIH0gZnJvbSAncmVhY3RzdHJhcCc7XHJcbmltcG9ydCB7IExpbmsgfSBmcm9tICdyZWFjdC1yb3V0ZXItZG9tJztcclxuXHJcbmV4cG9ydCBjbGFzcyBNYWluTmF2IGV4dGVuZHMgUmVhY3QuQ29tcG9uZW50IHtcclxuICAgIGNvbnN0cnVjdG9yKHByb3BzKSB7XHJcbiAgICAgICAgc3VwZXIocHJvcHMpO1xyXG5cclxuICAgICAgICB0aGlzLnN0YXRlID0ge1xyXG4gICAgICAgICAgICBpc09wZW46IGZhbHNlXHJcbiAgICAgICAgfTtcclxuICAgIH1cclxuXHJcbiAgICB0b2dnbGUoKSB7XHJcbiAgICAgICAgdGhpcy5zZXRTdGF0ZSgocHJldiwgcHJvcHMpID0+ICh7XHJcbiAgICAgICAgICAgIGlzT3BlbjogIXByZXYuaXNPcGVuXHJcbiAgICAgICAgfSkpO1xyXG4gICAgfVxyXG5cclxuICAgIHJlbmRlcigpIHtcclxuICAgICAgICByZXR1cm4gKFxyXG4gICAgICAgICAgICA8TmF2YmFyIGNvbG9yPVwic2phLWRhcmstZ3JlZW5cIiBpbnZlcnNlIHRvZ2dsZWFibGU+XHJcbiAgICAgICAgICAgICAgICA8TmF2YmFyVG9nZ2xlciByaWdodCBvbkNsaWNrPXsoKSA9PiB0aGlzLnRvZ2dsZSgpfSAvPlxyXG4gICAgICAgICAgICAgICAgPE5hdmJhckJyYW5kIGhyZWY9XCIvXCI+U0pBIFRyYWNrZXI8L05hdmJhckJyYW5kPlxyXG4gICAgICAgICAgICAgICAgPENvbGxhcHNlIGlzT3Blbj17dGhpcy5zdGF0ZS5pc09wZW59IG5hdmJhcj5cclxuICAgICAgICAgICAgICAgICAgICA8TmF2IG5hdmJhcj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPE5hdkl0ZW0+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8TGluayBjbGFzc05hbWU9XCJuYXYtbGlua1wiIHRvPVwiL1wiPkhvbWU8L0xpbms+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDwvTmF2SXRlbT5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPE5hdkl0ZW0+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8TGluayBjbGFzc05hbWU9XCJuYXYtbGlua1wiIHRvPVwiL01hcFwiPk1hcDwvTGluaz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPC9OYXZJdGVtPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8TmF2SXRlbT5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxMaW5rIGNsYXNzTmFtZT1cIm5hdi1saW5rXCIgdG89XCIvQ29udGFjdFwiPkNvbnRhY3Q8L0xpbms+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDwvTmF2SXRlbT5cclxuICAgICAgICAgICAgICAgICAgICA8L05hdj5cclxuICAgICAgICAgICAgICAgICAgICA8TmF2IG5hdmJhciBjbGFzc05hbWU9XCJtbC1hdXRvXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxOYXZJdGVtPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPExpbmsgY2xhc3NOYW1lPVwibmF2LWxpbmtcIiB0bz1cIi9hcHAvbG9naW5cIj5Mb2cgaW48L0xpbms+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDwvTmF2SXRlbT5cclxuICAgICAgICAgICAgICAgICAgICA8L05hdj5cclxuICAgICAgICAgICAgICAgIDwvQ29sbGFwc2U+XHJcbiAgICAgICAgICAgIDwvTmF2YmFyPik7XHJcbiAgICB9XHJcbn1cclxuIiwi77u/aW1wb3J0IFJlYWN0IGZyb20gJ3JlYWN0JztcclxuaW1wb3J0IHsgTWFpbk5hdiB9IGZyb20gJy4vbmF2YmFyLmpzeCc7XHJcbmltcG9ydCB7IEhvbWUgfSBmcm9tICcuL2hvbWUvaG9tZS5qc3gnO1xyXG5pbXBvcnQgeyBMb2dpbiB9IGZyb20gJy4vbG9naW4vbG9naW4uanN4JztcclxuaW1wb3J0IHsgQnJvd3NlclJvdXRlciBhcyBSb3V0ZXIsIFJvdXRlIH0gZnJvbSAncmVhY3Qtcm91dGVyLWRvbSc7XHJcblxyXG5leHBvcnQgY2xhc3MgUm9vdCBleHRlbmRzIFJlYWN0LkNvbXBvbmVudCB7XHJcbiAgICByZW5kZXIoKSB7XHJcbiAgICAgICAgcmV0dXJuIChcclxuICAgICAgICAgICAgPFJvdXRlcj5cclxuICAgICAgICAgICAgICAgIDxkaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgPE1haW5OYXYgLz5cclxuICAgICAgICAgICAgICAgICAgICA8Um91dGUgZXhhY3QgcGF0aD0nLycgY29tcG9uZW50PXtIb21lfSAvPlxyXG4gICAgICAgICAgICAgICAgICAgIDxSb3V0ZSBleGFjdCBwYXRoPScvYXBwL2xvZ2luJyBjb21wb25lbnQ9e0xvZ2lufSAvPlxyXG4gICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgIDwvUm91dGVyPik7XHJcblxyXG4gICAgfVxyXG59XHJcbiJdfQ==
