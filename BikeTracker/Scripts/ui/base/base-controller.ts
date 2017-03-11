class BaseController {
    constructor($scope: angular.IScope, $uibModal: angular.ui.bootstrap.IModalService,
        $window: angular.IWindowService, title: string) {
        this.$uibModal = $uibModal;
        this.previousTitle = $window.document.title;
        $window.document.title = title;

        let vm = this;

        $scope.$on("$destroy", function () {
            $window.document.title = vm.previousTitle;
        });
    }

    protected $uibModal: angular.ui.bootstrap.IModalService;
    private previousTitle: string;

    showError(title: string, message: string) {
        this.$uibModal.open({
            animation: true,
            templateUrl: "/Dialog/ErrorForm",
            controller: "ErrorFormCtrl as vm",
            resolve: {
                title: function () {
                    return title;
                },
                message: function () {
                    return message;
                }
            }
        });
    };
}