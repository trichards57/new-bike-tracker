ng.module("app", []);
var VehicleType;
(function (VehicleType) {
})(VehicleType || (VehicleType = {}));
var HomeView = (function () {
    function HomeView() {
        this.templateUrl = "/html/home.html";
        this.scope = {};
    }
    HomeView.Factory = function () {
        var directive = function () { return new HomeView(); };
        directive.$inject = [''];
        return directive;
    };
    return HomeView;
}());
ng.module("app").directive("homeView", HomeView.Factory());
