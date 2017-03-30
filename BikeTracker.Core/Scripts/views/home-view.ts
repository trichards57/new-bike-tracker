class HomeView implements ng.IDirective {
    public link: (scope: ng.IScope, element: ng.IAugmentedJQuery, attr: ng.IAttributes) => void;
    public templateUrl = "/html/home.html";
    public scope = {};

    public static Factory() {
        let directive = () => new HomeView();

        directive.$inject = [''];

        return directive;
    }
}

ng.module("app").directive("homeView", HomeView.Factory());
