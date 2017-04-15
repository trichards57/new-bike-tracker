(function e(t,n,r){function s(o,u){if(!n[o]){if(!t[o]){var a=typeof require=="function"&&require;if(!u&&a)return a(o,!0);if(i)return i(o,!0);var f=new Error("Cannot find module '"+o+"'");throw f.code="MODULE_NOT_FOUND",f}var l=n[o]={exports:{}};t[o][0].call(l.exports,function(e){var n=t[o][1][e];return s(n?n:e)},l,l.exports,e,t,n,r)}return n[o].exports}var i=typeof require=="function"&&require;for(var o=0;o<r.length;o++)s(r[o]);return s})({1:[function(require,module,exports){
'use strict';

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _reactDom = require('react-dom');

var _reactDom2 = _interopRequireDefault(_reactDom);

var _root = require('./root.jsx');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

_reactDom2.default.render(_react2.default.createElement(_root.Root, null), document.getElementById('root'));

},{"./root.jsx":4,"react":"react","react-dom":"react-dom"}],2:[function(require,module,exports){
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
                        { to: '/Login', className: 'btn btn-lg btn-sja-dark-green' },
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

},{"react":"react","react-router-dom":"react-router-dom","reactstrap":"reactstrap"}],3:[function(require,module,exports){
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
                                { className: 'nav-link', to: '/Login' },
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

},{"react":"react","react-router-dom":"react-router-dom","reactstrap":"reactstrap"}],4:[function(require,module,exports){
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
                    _react2.default.createElement(_reactRouterDom.Route, { path: '/', component: _home.Home })
                )
            );
        }
    }]);

    return Root;
}(_react2.default.Component);

},{"./home/home.jsx":2,"./navbar.jsx":3,"react":"react","react-router-dom":"react-router-dom"}]},{},[1])
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm5vZGVfbW9kdWxlcy9icm93c2VyLXBhY2svX3ByZWx1ZGUuanMiLCJzcmNcXGpzXFxhcHAuanN4Iiwic3JjXFxqc1xcaG9tZVxcaG9tZS5qc3giLCJzcmNcXGpzXFxuYXZiYXIuanN4Iiwic3JjXFxqc1xccm9vdC5qc3giXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7OztBQ0FDOzs7O0FBQ0Q7Ozs7QUFDQTs7OztBQUVBLG1CQUFTLE1BQVQsQ0FBZ0IsK0NBQWhCLEVBQTBCLFNBQVMsY0FBVCxDQUF3QixNQUF4QixDQUExQjs7Ozs7Ozs7Ozs7O0FDSkM7Ozs7QUFFRDs7QUFDQTs7Ozs7Ozs7OztJQUVhLEksV0FBQSxJOzs7Ozs7Ozs7OztpQ0FDQTtBQUNMLG1CQUNJO0FBQUE7QUFBQSxrQkFBSyxXQUFVLFdBQWY7QUFDSTtBQUFBO0FBQUE7QUFDSTtBQUFBO0FBQUEsMEJBQUksV0FBVSxXQUFkO0FBQUE7QUFBQSxxQkFESjtBQUVJO0FBQUE7QUFBQSwwQkFBRyxXQUFVLE1BQWI7QUFBQTtBQUFBLHFCQUZKO0FBR0k7QUFBQTtBQUFBLDBCQUFNLElBQUcsUUFBVCxFQUFrQixXQUFVLCtCQUE1QjtBQUFBO0FBQUE7QUFISixpQkFESjtBQU9JO0FBQUE7QUFBQSxzQkFBVSxXQUFVLFlBQXBCO0FBQ1E7QUFBQTtBQUFBO0FBQ0k7QUFBQTtBQUFBO0FBQ0k7QUFBQTtBQUFBO0FBQUE7QUFBQSw2QkFESjtBQUVJO0FBQUE7QUFBQTtBQUFBO0FBQUEsNkJBRko7QUFLSTtBQUFBO0FBQUEsa0NBQU0sSUFBRyxVQUFULEVBQW9CLFdBQVUsNkJBQTlCO0FBQUE7QUFBQTtBQUxKO0FBREoscUJBRFI7QUFVUTtBQUFBO0FBQUE7QUFDSTtBQUFBO0FBQUE7QUFDSTtBQUFBO0FBQUE7QUFBQTtBQUFBLDZCQURKO0FBRUk7QUFBQTtBQUFBO0FBQUE7QUFBQSw2QkFGSjtBQUtJO0FBQUE7QUFBQSxrQ0FBTSxJQUFHLEVBQVQsRUFBWSxXQUFVLHNDQUF0QixFQUE2RCxjQUE3RDtBQUFBO0FBQUE7QUFMSjtBQURKLHFCQVZSO0FBbUJRO0FBQUE7QUFBQTtBQUNJO0FBQUE7QUFBQTtBQUNJO0FBQUE7QUFBQTtBQUFBO0FBQUEsNkJBREo7QUFFSTtBQUFBO0FBQUE7QUFBQTtBQUFBLDZCQUZKO0FBS0k7QUFBQTtBQUFBLGtDQUFNLElBQUcsV0FBVCxFQUFxQixXQUFVLDZCQUEvQjtBQUFBO0FBQUE7QUFMSjtBQURKO0FBbkJSO0FBUEosYUFESjtBQXVDSDs7OztFQXpDcUIsZ0JBQU0sUzs7Ozs7Ozs7Ozs7O0FDTC9COzs7O0FBQ0Q7O0FBQ0E7Ozs7Ozs7Ozs7SUFFYSxPLFdBQUEsTzs7O0FBQ1QscUJBQVksS0FBWixFQUFtQjtBQUFBOztBQUFBLHNIQUNULEtBRFM7O0FBR2YsY0FBSyxLQUFMLEdBQWE7QUFDVCxvQkFBUTtBQURDLFNBQWI7QUFIZTtBQU1sQjs7OztpQ0FFUTtBQUNMLGlCQUFLLFFBQUwsQ0FBYyxVQUFDLElBQUQsRUFBTyxLQUFQO0FBQUEsdUJBQWtCO0FBQzVCLDRCQUFRLENBQUMsS0FBSztBQURjLGlCQUFsQjtBQUFBLGFBQWQ7QUFHSDs7O2lDQUVRO0FBQUE7O0FBQ0wsbUJBQ0k7QUFBQTtBQUFBLGtCQUFRLE9BQU0sZ0JBQWQsRUFBK0IsYUFBL0IsRUFBdUMsZ0JBQXZDO0FBQ0ksMkVBQWUsV0FBZixFQUFxQixTQUFTO0FBQUEsK0JBQU0sT0FBSyxNQUFMLEVBQU47QUFBQSxxQkFBOUIsR0FESjtBQUVJO0FBQUE7QUFBQSxzQkFBYSxNQUFLLEdBQWxCO0FBQUE7QUFBQSxpQkFGSjtBQUdJO0FBQUE7QUFBQSxzQkFBVSxRQUFRLEtBQUssS0FBTCxDQUFXLE1BQTdCLEVBQXFDLFlBQXJDO0FBQ0k7QUFBQTtBQUFBLDBCQUFLLFlBQUw7QUFDSTtBQUFBO0FBQUE7QUFDSTtBQUFBO0FBQUEsa0NBQU0sV0FBVSxVQUFoQixFQUEyQixJQUFHLEdBQTlCO0FBQUE7QUFBQTtBQURKLHlCQURKO0FBSUk7QUFBQTtBQUFBO0FBQ0k7QUFBQTtBQUFBLGtDQUFNLFdBQVUsVUFBaEIsRUFBMkIsSUFBRyxNQUE5QjtBQUFBO0FBQUE7QUFESix5QkFKSjtBQU9JO0FBQUE7QUFBQTtBQUNJO0FBQUE7QUFBQSxrQ0FBTSxXQUFVLFVBQWhCLEVBQTJCLElBQUcsVUFBOUI7QUFBQTtBQUFBO0FBREo7QUFQSixxQkFESjtBQVlJO0FBQUE7QUFBQSwwQkFBSyxZQUFMLEVBQVksV0FBVSxTQUF0QjtBQUNJO0FBQUE7QUFBQTtBQUNJO0FBQUE7QUFBQSxrQ0FBTSxXQUFVLFVBQWhCLEVBQTJCLElBQUcsUUFBOUI7QUFBQTtBQUFBO0FBREo7QUFESjtBQVpKO0FBSEosYUFESjtBQXVCSDs7OztFQXZDd0IsZ0JBQU0sUzs7Ozs7Ozs7Ozs7O0FDSmxDOzs7O0FBQ0Q7O0FBQ0E7O0FBQ0E7Ozs7Ozs7Ozs7SUFFYSxJLFdBQUEsSTs7Ozs7Ozs7Ozs7aUNBQ0E7QUFDTCxtQkFDSTtBQUFBO0FBQUE7QUFDSTtBQUFBO0FBQUE7QUFDSSx3RUFESjtBQUVJLDJFQUFPLE1BQUssR0FBWixFQUFnQixxQkFBaEI7QUFGSjtBQURKLGFBREo7QUFRSDs7OztFQVZxQixnQkFBTSxTIiwiZmlsZSI6ImdlbmVyYXRlZC5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzQ29udGVudCI6WyIoZnVuY3Rpb24gZSh0LG4scil7ZnVuY3Rpb24gcyhvLHUpe2lmKCFuW29dKXtpZighdFtvXSl7dmFyIGE9dHlwZW9mIHJlcXVpcmU9PVwiZnVuY3Rpb25cIiYmcmVxdWlyZTtpZighdSYmYSlyZXR1cm4gYShvLCEwKTtpZihpKXJldHVybiBpKG8sITApO3ZhciBmPW5ldyBFcnJvcihcIkNhbm5vdCBmaW5kIG1vZHVsZSAnXCIrbytcIidcIik7dGhyb3cgZi5jb2RlPVwiTU9EVUxFX05PVF9GT1VORFwiLGZ9dmFyIGw9bltvXT17ZXhwb3J0czp7fX07dFtvXVswXS5jYWxsKGwuZXhwb3J0cyxmdW5jdGlvbihlKXt2YXIgbj10W29dWzFdW2VdO3JldHVybiBzKG4/bjplKX0sbCxsLmV4cG9ydHMsZSx0LG4scil9cmV0dXJuIG5bb10uZXhwb3J0c312YXIgaT10eXBlb2YgcmVxdWlyZT09XCJmdW5jdGlvblwiJiZyZXF1aXJlO2Zvcih2YXIgbz0wO288ci5sZW5ndGg7bysrKXMocltvXSk7cmV0dXJuIHN9KSIsIu+7v2ltcG9ydCBSZWFjdCBmcm9tICdyZWFjdCc7XHJcbmltcG9ydCBSZWFjdERPTSBmcm9tICdyZWFjdC1kb20nO1xyXG5pbXBvcnQgeyBSb290IH0gZnJvbSAnLi9yb290LmpzeCc7XHJcblxyXG5SZWFjdERPTS5yZW5kZXIoPFJvb3QgLz4sIGRvY3VtZW50LmdldEVsZW1lbnRCeUlkKCdyb290JykpO1xyXG4iLCLvu79pbXBvcnQgUmVhY3QgZnJvbSAncmVhY3QnO1xyXG5cclxuaW1wb3J0IHsgSnVtYm90cm9uLCBCdXR0b24sIENhcmQsIENhcmRCbG9jaywgQ2FyZFRpdGxlLCBDYXJkVGV4dCwgQ2FyZERlY2sgfSBmcm9tICdyZWFjdHN0cmFwJztcclxuaW1wb3J0IHsgTGluayB9IGZyb20gJ3JlYWN0LXJvdXRlci1kb20nO1xyXG5cclxuZXhwb3J0IGNsYXNzIEhvbWUgZXh0ZW5kcyBSZWFjdC5Db21wb25lbnQge1xyXG4gICAgcmVuZGVyKCkge1xyXG4gICAgICAgIHJldHVybiAoXHJcbiAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiY29udGFpbmVyXCI+XHJcbiAgICAgICAgICAgICAgICA8SnVtYm90cm9uPlxyXG4gICAgICAgICAgICAgICAgICAgIDxoMSBjbGFzc05hbWU9XCJkaXNwbGF5LTNcIj5TSkEgVHJhY2tlcjwvaDE+XHJcbiAgICAgICAgICAgICAgICAgICAgPHAgY2xhc3NOYW1lPVwibGVhZFwiPldlbGNvbWUgdG8gdGhlIFN0IEFtYnVsYW5jZSBTV1IgQ2VudHJhbCBEaXN0cmljdCBUcmFja2VyPC9wPlxyXG4gICAgICAgICAgICAgICAgICAgIDxMaW5rIHRvPVwiL0xvZ2luXCIgY2xhc3NOYW1lPVwiYnRuIGJ0bi1sZyBidG4tc2phLWRhcmstZ3JlZW5cIj5Mb2cgaW4gwrs8L0xpbms+XHJcbiAgICAgICAgICAgICAgICA8L0p1bWJvdHJvbj5cclxuXHJcbiAgICAgICAgICAgICAgICA8Q2FyZERlY2sgY2xhc3NOYW1lPVwiZnJvbnQtcGFnZVwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8Q2FyZD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxDYXJkQmxvY2s+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPENhcmRUaXRsZT5OZXcgVXNlcnM8L0NhcmRUaXRsZT5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8Q2FyZFRleHQ+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIElmIHlvdSBoYXZlIG5ldmVyIHVzZWQgdGhpcyBzaXRlIGJlZm9yZSwgeW91IHdpbGwgbmVlZCB0byBiZSBpc3N1ZWQgd2l0aCBhIHVzZXJuYW1lIGFuZCBwYXNzd29yZC5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L0NhcmRUZXh0PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxMaW5rIHRvPVwiL0NvbnRhY3RcIiBjbGFzc05hbWU9XCJidG4gYnRuLXNlY29uZGFyeSBidG4tYmxvY2tcIj5SZXF1ZXN0IEFjY291bnQgwrs8L0xpbms+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L0NhcmRCbG9jaz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPC9DYXJkPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8Q2FyZD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxDYXJkQmxvY2s+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPENhcmRUaXRsZT5HZXQgdGhlIFRyYWNrZXJzPC9DYXJkVGl0bGU+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPENhcmRUZXh0PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBUaGVyZSBhcmUgYSBzZXQgb2YgYXBwcyB0byBlbmFibGUgeW91ciBwaG9uZSB0byBiZSB0cmFja2VkIGZyb20gdGhpcyB3ZWJzaXRlLlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvQ2FyZFRleHQ+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPExpbmsgdG89XCJcIiBjbGFzc05hbWU9XCJidG4gYnRuLXNlY29uZGFyeSBidG4tYmxvY2sgZGlzYWJsZWRcIiBkaXNhYmxlZD5Db21pbmcgU29vbjwvTGluaz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvQ2FyZEJsb2NrPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8L0NhcmQ+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxDYXJkPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPENhcmRCbG9jaz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8Q2FyZFRpdGxlPlByaXZhY3kgUG9saWN5PC9DYXJkVGl0bGU+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPENhcmRUZXh0PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBUbyBnZXQgYSBiZXR0ZXIgaWRlYSBvZiBob3cgdGhpcyB3ZWJzaXRlIGlzIHVzZWQgYW5kIGhvdyBpdCBjYW4gYmUgbWFkZSBiZXR0ZXIsIHdlIGNvbGxlY3Qgc29tZSBpbmZvcm1hdGlvbiBhYm91dCB3aGF0IHlvdSBkbyB3aXRoIHRoaXMgc2l0ZS5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L0NhcmRUZXh0PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxMaW5rIHRvPVwiL1BvbGljaWVzXCIgY2xhc3NOYW1lPVwiYnRuIGJ0bi1zZWNvbmRhcnkgYnRuLWJsb2NrXCI+RmluZCBvdXQgTW9yZSDCuzwvTGluaz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvQ2FyZEJsb2NrPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8L0NhcmQ+XHJcbiAgICAgICAgICAgICAgICA8L0NhcmREZWNrPlxyXG4gICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICApO1xyXG4gICAgfVxyXG59XHJcbiIsIu+7v2ltcG9ydCBSZWFjdCBmcm9tICdyZWFjdCc7XHJcbmltcG9ydCB7IE5hdmJhciwgTmF2YmFyVG9nZ2xlciwgTmF2YmFyQnJhbmQsIENvbGxhcHNlLCBOYXYsIE5hdkl0ZW0sIE5hdkxpbmsgfSBmcm9tICdyZWFjdHN0cmFwJztcclxuaW1wb3J0IHsgTGluayB9IGZyb20gJ3JlYWN0LXJvdXRlci1kb20nO1xyXG5cclxuZXhwb3J0IGNsYXNzIE1haW5OYXYgZXh0ZW5kcyBSZWFjdC5Db21wb25lbnQge1xyXG4gICAgY29uc3RydWN0b3IocHJvcHMpIHtcclxuICAgICAgICBzdXBlcihwcm9wcyk7XHJcblxyXG4gICAgICAgIHRoaXMuc3RhdGUgPSB7XHJcbiAgICAgICAgICAgIGlzT3BlbjogZmFsc2VcclxuICAgICAgICB9O1xyXG4gICAgfVxyXG5cclxuICAgIHRvZ2dsZSgpIHtcclxuICAgICAgICB0aGlzLnNldFN0YXRlKChwcmV2LCBwcm9wcykgPT4gKHtcclxuICAgICAgICAgICAgaXNPcGVuOiAhcHJldi5pc09wZW5cclxuICAgICAgICB9KSk7XHJcbiAgICB9XHJcblxyXG4gICAgcmVuZGVyKCkge1xyXG4gICAgICAgIHJldHVybiAoXHJcbiAgICAgICAgICAgIDxOYXZiYXIgY29sb3I9XCJzamEtZGFyay1ncmVlblwiIGludmVyc2UgdG9nZ2xlYWJsZT5cclxuICAgICAgICAgICAgICAgIDxOYXZiYXJUb2dnbGVyIHJpZ2h0IG9uQ2xpY2s9eygpID0+IHRoaXMudG9nZ2xlKCl9IC8+XHJcbiAgICAgICAgICAgICAgICA8TmF2YmFyQnJhbmQgaHJlZj1cIi9cIj5TSkEgVHJhY2tlcjwvTmF2YmFyQnJhbmQ+XHJcbiAgICAgICAgICAgICAgICA8Q29sbGFwc2UgaXNPcGVuPXt0aGlzLnN0YXRlLmlzT3Blbn0gbmF2YmFyPlxyXG4gICAgICAgICAgICAgICAgICAgIDxOYXYgbmF2YmFyPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8TmF2SXRlbT5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxMaW5rIGNsYXNzTmFtZT1cIm5hdi1saW5rXCIgdG89XCIvXCI+SG9tZTwvTGluaz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPC9OYXZJdGVtPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8TmF2SXRlbT5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxMaW5rIGNsYXNzTmFtZT1cIm5hdi1saW5rXCIgdG89XCIvTWFwXCI+TWFwPC9MaW5rPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8L05hdkl0ZW0+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxOYXZJdGVtPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPExpbmsgY2xhc3NOYW1lPVwibmF2LWxpbmtcIiB0bz1cIi9Db250YWN0XCI+Q29udGFjdDwvTGluaz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPC9OYXZJdGVtPlxyXG4gICAgICAgICAgICAgICAgICAgIDwvTmF2PlxyXG4gICAgICAgICAgICAgICAgICAgIDxOYXYgbmF2YmFyIGNsYXNzTmFtZT1cIm1sLWF1dG9cIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPE5hdkl0ZW0+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8TGluayBjbGFzc05hbWU9XCJuYXYtbGlua1wiIHRvPVwiL0xvZ2luXCI+TG9nIGluPC9MaW5rPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8L05hdkl0ZW0+XHJcbiAgICAgICAgICAgICAgICAgICAgPC9OYXY+XHJcbiAgICAgICAgICAgICAgICA8L0NvbGxhcHNlPlxyXG4gICAgICAgICAgICA8L05hdmJhcj4pO1xyXG4gICAgfVxyXG59XHJcbiIsIu+7v2ltcG9ydCBSZWFjdCBmcm9tICdyZWFjdCc7XHJcbmltcG9ydCB7IE1haW5OYXYgfSBmcm9tICcuL25hdmJhci5qc3gnO1xyXG5pbXBvcnQgeyBIb21lIH0gZnJvbSAnLi9ob21lL2hvbWUuanN4JztcclxuaW1wb3J0IHsgQnJvd3NlclJvdXRlciBhcyBSb3V0ZXIsIFJvdXRlIH0gZnJvbSAncmVhY3Qtcm91dGVyLWRvbSc7XHJcblxyXG5leHBvcnQgY2xhc3MgUm9vdCBleHRlbmRzIFJlYWN0LkNvbXBvbmVudCB7XHJcbiAgICByZW5kZXIoKSB7XHJcbiAgICAgICAgcmV0dXJuIChcclxuICAgICAgICAgICAgPFJvdXRlcj5cclxuICAgICAgICAgICAgICAgIDxkaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgPE1haW5OYXYgLz5cclxuICAgICAgICAgICAgICAgICAgICA8Um91dGUgcGF0aD0nLycgY29tcG9uZW50PXtIb21lfSAvPlxyXG4gICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgIDwvUm91dGVyPik7XHJcblxyXG4gICAgfVxyXG59XHJcbiJdfQ==
