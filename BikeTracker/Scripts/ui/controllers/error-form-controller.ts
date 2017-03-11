class ErrorFormController {
    constructor($modalInstance: angular.ui.bootstrap.IModalServiceInstance, title: string,
        message: string) {
        this.$modalInstance = $modalInstance;
        this.title = title;
        this.message = message;
    }

    static $inject = ["$uibModalInstance", "title", "message"];

    private $modalInstance: angular.ui.bootstrap.IModalServiceInstance;
    title: string;
    message: string;

    close() {
        this.$modalInstance.close();
    }
}

angular.module("app").controller("ErrorFormCtrl", ErrorFormController);